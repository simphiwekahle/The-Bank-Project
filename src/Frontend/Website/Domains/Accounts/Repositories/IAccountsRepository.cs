﻿using Shared.Domains.Accounts.Models;

namespace Website.Domains.Accounts.Repositories;

public interface IAccountsRepository
{
    Task<AccountsModel?> CreateAsync(AccountsModel account);

    Task<List<AccountsModel>> RetrieveAllAsync();

    Task<AccountsModel?> RetrieveSingleAsync(int code);

    Task<bool> UpdateAsync(int code, AccountsModel account);

    Task<bool> UpdateBalanceAsync(int code, decimal amount);

    Task<bool> UpdateStatusAsync(int code, bool status);

    Task<bool> DeleteAsync(int code);
}
