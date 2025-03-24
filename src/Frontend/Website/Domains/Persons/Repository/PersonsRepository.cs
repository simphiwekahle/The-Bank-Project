using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Shared.Domains.Persons.Models;
using System.Data;
using Website.Configurations;
using Website.Domains.Entities.Persons.Repositories;

namespace ManagePeopleAPI.Domains.Entities.Persons.Repositories;

public class PersonsRepository(
ILogger<PersonsRepository> logger,
IOptionsSnapshot<ConnectionStringOptions> connectionStrings,
IOptionsSnapshot<StoredProcedureOptions> storedProcedures) : IPersonsRepository
{
	public async Task<PersonsModel?> CreateAsync(PersonsModel person)
	{
		logger.LogInformation("Repository => Attempting to create a new person");

		var dynamicParams = new DynamicParameters();

		dynamicParams.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
		dynamicParams.Add(name: "@name", value: person.Name, dbType: DbType.String, direction: ParameterDirection.Input);
		dynamicParams.Add(name: "@surname", value: person.Surname, dbType: DbType.String, direction: ParameterDirection.Input);
		dynamicParams.Add(name: "@id_number", value: person.Id_Number, dbType: DbType.String, direction: ParameterDirection.Input);

		using var sqlConnection = new SqlConnection(connectionStrings.Value.TransactionsDB);

		try
		{
			await sqlConnection.ExecuteAsync(
				sql: storedProcedures.Value.InsertNewPerson,
				param: dynamicParams,
				commandType: CommandType.StoredProcedure);

			person.Code = dynamicParams.Get<int>("@code");

			logger.LogInformation(
				"{Announcement}: Attempt to create a new person completed successfully with id {Person}",
				"SUCCEEDED", person.Code);

			return person;
		}
		catch (Exception ex)
		{
			logger.LogError(
				ex,
				"{Announcement}: Attempt to create a new person was unsuccessful",
				"FAILED");

			return null;
		}
	}

	public async Task<bool> DeleteAsync(int code)
	{
		logger.LogInformation(
			"Repository => Attempting to delete team {Team}",
			code);

		using var sqlConnection = new SqlConnection(connectionStrings.Value.TransactionsDB);

		try
		{
			await sqlConnection.ExecuteAsync(
				sql: storedProcedures.Value.DeletePersonByCode,
				param: new { Code = code },
				commandType: CommandType.StoredProcedure);

			logger.LogInformation(
				"{Announcement}: Attempt to delete team {Team} completed successfully",
				"SUCCEEDED", code);

			return true;
		}
		catch (Exception ex)
		{
			logger.LogError(
				ex,
				"{Announcement}: Attempt to delete team {Team} was unsuccessful",
				"FAILED", code);

			return false;
		}
	}

	public async Task<List<PersonsModel>> RetrieveAllAsync()
	{
		logger.LogInformation("Repository => Attempting to retrieve all persons");

		using var sqlConnection = new SqlConnection(connectionStrings.Value.TransactionsDB);

		var persons = new List<PersonsModel>();

		try
		{
			persons =
				(await sqlConnection.QueryAsync<PersonsModel>(
					sql: storedProcedures.Value.GetAllPersons,
					commandType: CommandType.StoredProcedure))
					.ToList();

			logger.LogInformation(
				"{Announcement}: Attempt to retrieve all persons completed successfully",
				"SUCCEEDED");
		}
		catch (Exception ex)
		{
			logger.LogError(
				ex,
				"{Announcement}: Attempt to retrieve all persons was unsuccessful",
				"FAILED");
		}

		return persons;
	}

	public async Task<PersonsModel?> RetrieveSingleAsync(int code)
	{
		logger.LogInformation(
		"Repository => Attempting to retrieve person {Person}",
		code);

		using var sqlConnection = new SqlConnection(connectionStrings.Value.TransactionsDB);

		PersonsModel? person = null;

		try
		{
			person =
				await sqlConnection.QuerySingleOrDefaultAsync<PersonsModel>(
					sql: storedProcedures.Value.GetPersonByCode,
					param: new { code },
					commandType: CommandType.StoredProcedure);

			logger.LogInformation(
				"{Announcement}: Attempt to retrieve person {Person} completed successfully",
				"SUCCEEDED", code);
		}
		catch (Exception ex)
		{
			logger.LogError(
				ex,
				"{Announcement}: Attempt to retrieve person {Person} was unsuccessful",
				"FAILED", code);

			person = null;
		}

		return person;
	}

	public async Task<bool> UpdateAsync(int code, PersonsModel person)
	{
		logger.LogInformation(
		 "Repository => Attempting to update team {Team}",
		 code);

		using var sqlConnection = new SqlConnection(connectionStrings.Value.TransactionsDB);

		try
		{
			await sqlConnection.ExecuteAsync(
				sql: storedProcedures.Value.UpdatePersonByCode,
				param: new
				{
					//code,
					person.Name,
					person.Surname,
					person.Id_Number
				},
				commandType: CommandType.StoredProcedure);

			logger.LogInformation(
				"{Announcement}: Attempt to update person {Person} completed successfully",
				"SUCCEEDED", code);

			return true;
		}
		catch (Exception ex)
		{
			logger.LogError(
				ex,
				"{Announcement}: Attempt to update person {Person} was unsuccessful",
				"FAILED", code);

			return false;
		}
	}
}

