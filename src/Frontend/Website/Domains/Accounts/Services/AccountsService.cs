using Shared.Domains.Accounts.Models;
using System.Text;
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

			var accountNumber = GenerarteAccountNumber();

			var accountCheck = (await accountsRepository.RetrieveAllAsync())
				.Find(a => a.Account_Number.Equals(accountNumber));

			if (accountCheck is not null)
			{
				return null;
			}

			var personCheck = await personsRepository.RetrieveSingleAsync(account.Person_Code);

			if (personCheck is not null)
			{
				account.Account_Number = accountNumber;
				account.IsActive = true;
				account.Outstanding_Balance = 0;

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

			var account = await accountsRepository.RetrieveSingleAsync(code);

			if (account is null)
				return null;

			accountsView.account = account;
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

		private static string GenerarteAccountNumber()
		{
            Random random = new Random();
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < 8; i++)
            {
                result.Append(random.Next(0, 10));
            }

			return result.ToString();
        }

        public Task<bool> UpdateAccountStatusAsync(int code, AccountsModel account)
        {
            throw new NotImplementedException();
        }
    }
}
