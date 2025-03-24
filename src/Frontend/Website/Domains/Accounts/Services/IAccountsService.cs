using Shared.Domains.Accounts.Models;
using Website.Domains.Accounts.ViewModels;

namespace Website.Domains.Accounts.Services;

public interface IAccountsService
{
	Task<AccountsModel?> AddAccountAsync(AccountsModel account);
	Task<List<AccountsModel>?> GetAccountsAsync();
	Task<AccountsViewModel?> GetSingleAccountAsync(int code);
	Task<bool> UpdateAccountAsync(int code, AccountsModel account);
	Task<bool> RemoveAccountAsync(int code);
}
