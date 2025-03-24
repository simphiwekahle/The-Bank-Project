using Shared.Domains.Accounts.Models;
using Website.Domains.Accounts.Repositories;
using Website.Domains.Accounts.ViewModels;
using Website.Domains.Entities.Persons.Repositories;
using Website.Domains.Transactions.Repositories;

namespace Website.Domains.Accounts.Services
{
	public class AccountsService(
		IAccountsRepository accountsRepository,
		IPersonsRepository personsRepository,
		ITransactionsRepository transactionsRepository) : IAccountsService
	{
		public async Task<AccountsModel?> AddAccountAsync(AccountsModel account)
		{
			if (account is null)
				return null;

			var accountCheck = (await accountsRepository.RetrieveAllAsync())
				.Find(a => a.Account_Number.Equals(account!.Account_Number));

			var personCheck = await personsRepository.RetrieveSingleAsync(account.Person_Code);

			if (accountCheck is null && personCheck is not null)
			{
				var newAccount = await accountsRepository.CreateAsync(account);

				return newAccount;
			}

			return null;
		}

		public Task<List<AccountsModel>?> GetAccountsAsync()
		{
			throw new NotImplementedException();
		}

		public async Task<AccountsViewModel?> GetSingleAccountAsync(int code)
		{
			AccountsViewModel accountsView = new();

            accountsView.account = await accountsRepository.RetrieveSingleAsync(code);
			accountsView.transactions = (await transactionsRepository.RetrieveAllAsync())
				.FindAll(t => t.Account_Code == code);

			if (accountsView is null)
				return null;

            return accountsView;
		}

		public Task<bool> RemoveAccountAsync(int code)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UpdateAccountAsync(int code, AccountsModel account)
		{
			throw new NotImplementedException();
		}
	}
}
