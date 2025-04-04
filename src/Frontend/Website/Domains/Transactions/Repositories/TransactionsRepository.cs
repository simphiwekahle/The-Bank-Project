﻿using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Shared.Domains.Transactions.Models;
using System.Data;
using Website.Configurations;

namespace Website.Domains.Transactions.Repositories;

public class TransactionsRepository(
	ILogger<TransactionsRepository> logger,
	IOptionsSnapshot<ConnectionStringOptions> connectionStrings,
	IOptionsSnapshot<StoredProcedureOptions> storedProcedures) : ITransactionsRepository
{
	public async Task<TransactionsModel?> CreateAsync(TransactionsModel transaction)
	{
		logger.LogInformation("Repository => Attempting to create a new transaction");

		var dynamicParams = new DynamicParameters();

		dynamicParams.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
		dynamicParams.Add(name: "@account_code", value: transaction.Account_Code, dbType: DbType.Int32, direction: ParameterDirection.Input);
		dynamicParams.Add(name: "@transaction_date", value: transaction.Transaction_Date, dbType: DbType.DateTime, direction: ParameterDirection.Input);
		dynamicParams.Add(name: "@capture_date", value: transaction.Capture_Date, dbType: DbType.DateTime, direction: ParameterDirection.Input);
		dynamicParams.Add(name: "@amount", value: transaction.Amount, dbType: DbType.Decimal, direction: ParameterDirection.Input);
		dynamicParams.Add(name: "@description", value: transaction.Description, dbType: DbType.String, direction: ParameterDirection.Input);

		using var sqlConnection = new SqlConnection(connectionStrings.Value.TransactionsDB);

		try
		{
			await sqlConnection.ExecuteAsync(
				sql: storedProcedures.Value.InsertNewTransaction,
				param: dynamicParams,
				commandType: CommandType.StoredProcedure);

			transaction.Code = dynamicParams.Get<int>("@code");

			logger.LogInformation(
				"{Announcement}: Attempt to create a new transaction completed successfully with code {Transaction}",
				"SUCCEEDED", transaction.Code);

			return transaction;
		}
		catch (Exception ex)
		{
			logger.LogError(
				ex,
				"{Announcement}: Attempt to create a new transaction was unsuccessful",
				"FAILED");

			return null;
		}
	}

	public async Task<List<TransactionsModel>> RetrieveAllAsync()
	{
		logger.LogInformation("Repository => Attempting to retrieve all persons");

		using var sqlConnection = new SqlConnection(connectionStrings.Value.TransactionsDB);

		var transactions = new List<TransactionsModel>();

		try
		{
			transactions =
				(await sqlConnection.QueryAsync<TransactionsModel>(
					sql: storedProcedures.Value.GetAllTransactions,
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

		return transactions;
	}

    public async Task<List<TransactionTypesModel>> RetrieveAllTypesAsync()
	{
        logger.LogInformation("Repository => Attempting to retrieve all persons");

        using var sqlConnection = new SqlConnection(connectionStrings.Value.TransactionsDB);

        var transactionTypes = new List<TransactionTypesModel>();

        try
        {
            transactionTypes = (await sqlConnection.QueryAsync<TransactionTypesModel>(
                    sql: storedProcedures.Value.GetAllTransactionTypes,
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

        return transactionTypes;
    }

    public async Task<TransactionsModel?> RetrieveSingleAsync(int code)
	{
		logger.LogInformation(
		"Repository => Attempting to retrieve person {Person}",
		code);

		using var sqlConnection = new SqlConnection(connectionStrings.Value.TransactionsDB);

		TransactionsModel? transaction = null;

		try
		{
			transaction =
				await sqlConnection.QuerySingleOrDefaultAsync<TransactionsModel>(
					sql: storedProcedures.Value.GetTransactionByCode,
					param: new { code },
					commandType: CommandType.StoredProcedure);

			logger.LogInformation(
				"{Announcement}: Attempt to retrieve account {Account} completed successfully",
				"SUCCEEDED", code);
		}
		catch (Exception ex)
		{
			logger.LogError(
				ex,
				"{Announcement}: Attempt to retrieve account {Account} was unsuccessful",
				"FAILED", code);

			transaction = null;
		}

		return transaction;
	}

	public Task<bool> UpdateAsync(int code, TransactionsModel transaction)
	{
		throw new NotImplementedException();
	}

    public async Task<bool> DeleteAsync(int code)
    {
        logger.LogInformation(
            "Repository => Attempting to delete an account code {Code}",
            code);

        using var sqlConnection = new SqlConnection(connectionStrings.Value.TransactionsDB);

        try
        {
            await sqlConnection.ExecuteAsync(
                sql: storedProcedures.Value.DeleteTransactionByCode,
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
}
