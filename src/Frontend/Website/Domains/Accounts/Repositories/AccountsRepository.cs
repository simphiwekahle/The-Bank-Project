﻿using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Shared.Domains.Accounts.Models;
using System.Data;
using Website.Configurations;

namespace Website.Domains.Accounts.Repositories;

public class AccountsRepository(
ILogger<AccountsRepository> logger,
IOptionsSnapshot<ConnectionStringOptions> connectionStrings,
IOptionsSnapshot<StoredProcedureOptions> storedProcedures) : IAccountsRepository
{
    public async Task<AccountsModel?> CreateAsync(AccountsModel account)
    {
        logger.LogInformation("Repository => Attempting to create a new account");

        var dynamicParams = new DynamicParameters();

        dynamicParams.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
        dynamicParams.Add(name: "@person_code", value: account.Person_Code, dbType: DbType.String, direction: ParameterDirection.Input);
        dynamicParams.Add(name: "@account_number", value: account.Account_Number, dbType: DbType.String, direction: ParameterDirection.Input);
        dynamicParams.Add(name: "@outstanding_balance", value: account.Outstanding_Balance, dbType: DbType.String, direction: ParameterDirection.Input);
        dynamicParams.Add(name: "@isActive", value: account.IsActive, dbType: DbType.Boolean, direction: ParameterDirection.Input);

        using var sqlConnection = new SqlConnection(connectionStrings.Value.TransactionsDB);

        try
        {
            await sqlConnection.ExecuteAsync(
                sql: storedProcedures.Value.InsertNewAccount,
                param: dynamicParams,
                commandType: CommandType.StoredProcedure);

            account.Code = dynamicParams.Get<int>("@code");

            logger.LogInformation(
                "{Announcement}: Attempt to create a new account completed successfully with id {Person}",
                "SUCCEEDED", account.Code);

            return account;
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
            "Repository => Attempting to delete an account code {Code}",
            code);

        using var sqlConnection = new SqlConnection(connectionStrings.Value.TransactionsDB);

        try
        {
            await sqlConnection.ExecuteAsync(
                sql: storedProcedures.Value.DeleteAccountByCode,
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

    public async Task<List<AccountsModel>> RetrieveAllAsync()
    {
        logger.LogInformation("Repository => Attempting to retrieve all accounts");

        using var sqlConnection = new SqlConnection(connectionStrings.Value.TransactionsDB);

        var accounts = new List<AccountsModel>();

        try
        {
            accounts =
                (await sqlConnection.QueryAsync<AccountsModel>(
                    sql: storedProcedures.Value.GetAllAccounts,
                    commandType: CommandType.StoredProcedure))
                    .ToList();

            logger.LogInformation(
                "{Announcement}: Attempt to retrieve all accounts completed successfully",
                "SUCCEEDED");
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "{Announcement}: Attempt to retrieve all accounts was unsuccessful",
                "FAILED");
        }

        return accounts;
    }

    public async Task<AccountsModel?> RetrieveSingleAsync(int code)
    {
        logger.LogInformation(
        "Repository => Attempting to retrieve an account {Account}",
        code);

        using var sqlConnection = new SqlConnection(connectionStrings.Value.TransactionsDB);

        AccountsModel? account = null;

        try
        {
            account =
                await sqlConnection.QuerySingleOrDefaultAsync<AccountsModel>(
                    sql: storedProcedures.Value.GetAccountByCode,
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

            account = null;
        }

        return account;
    }

    public async Task<bool> UpdateAsync(int code, AccountsModel account)
    {
        logger.LogInformation(
         "Repository => Attempting to update account {Account}",
         code);

        using var sqlConnection = new SqlConnection(connectionStrings.Value.TransactionsDB);

        try
        {
            await sqlConnection.ExecuteAsync(
                sql: storedProcedures.Value.UpdateAccountById,
                param: new
                {
                    code,
                    account.Person_Code,
                    account.Account_Number,
                    Id_Number = account.Outstanding_Balance
                },
                commandType: CommandType.StoredProcedure);

            logger.LogInformation(
                "{Announcement}: Attempt to update account {Account} completed successfully",
                "SUCCEEDED", code);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "{Announcement}: Attempt to update account {Account} was unsuccessful",
                "FAILED", code);

            return false;
        }
    }

    public async Task<bool> UpdateBalanceAsync(int code, decimal amount)
    {
        logger.LogInformation("Repository => Attempting to create a new person");

        var dynamicParams = new DynamicParameters();

        dynamicParams.Add(name: "@code", value: code, dbType: DbType.String, direction: ParameterDirection.Input);
        dynamicParams.Add(name: "@account_number", value: amount, dbType: DbType.String, direction: ParameterDirection.Input);

        using var sqlConnection = new SqlConnection(connectionStrings.Value.TransactionsDB);

        try
        {
            await sqlConnection.ExecuteAsync(
                sql: storedProcedures.Value.UpdateAccountBalance,
                param: new
                {
                    code,
                    amount
                },
                commandType: CommandType.StoredProcedure);

            logger.LogInformation(
                "{Announcement}: Attempt to update account {Account} completed successfully",
                "SUCCEEDED", code);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "{Announcement}: Attempt to update account {Account} was unsuccessful",
                "FAILED", code);

            return false;
        }
    }

    public async Task<bool> UpdateStatusAsync(int code, bool status)
    {
        logger.LogInformation("Repository => Attempting to create a new person");

        var dynamicParams = new DynamicParameters();

        dynamicParams.Add(name: "@code", value: code, dbType: DbType.String, direction: ParameterDirection.Input);
        dynamicParams.Add(name: "@status", value: status, dbType: DbType.Boolean, direction: ParameterDirection.Input);

        using var sqlConnection = new SqlConnection(connectionStrings.Value.TransactionsDB);

        try
        {
            await sqlConnection.ExecuteAsync(
                sql: storedProcedures.Value.UpdateAccountStatus,
                param: new
                {
                    code,
                    status
                },
                commandType: CommandType.StoredProcedure);

            logger.LogInformation(
                "{Announcement}: Attempt to update account {Account} completed successfully",
                "SUCCEEDED", code);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "{Announcement}: Attempt to update account {Account} was unsuccessful",
                "FAILED", code);

            return false;
        }
    }
}

