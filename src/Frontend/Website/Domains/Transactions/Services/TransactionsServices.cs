using Shared.Domains.Transactions.Models;
using Website.Domains.Accounts.Repositories;
using Website.Domains.Transactions.Repositories;
using Website.Domains.Transactions.ViewModels;

namespace Website.Domains.Transactions.Services;

public class TransactionsServices(
    ITransactionsRepository transactionsRepository,
    IAccountsRepository accountsRepository) : ITransactionsServices
{
    public async Task<TransactionsModel?> AddTransactionAsync(TransactionsViewModel transaction)
    {
        var account = await accountsRepository.RetrieveSingleAsync(transaction!.transactions.Account_Code);

        if (account is null)
            return null;

        if (!account!.IsActive)
            return null;

        if (transaction is not null)
        {
            if (transaction.transactions.Transaction_Type_Id == 1)
            {
                transaction.transactions.Description = "New Debit transaction added";
            }
            else
            {
                transaction.transactions.Description = "New Credit transaction added";
            }

            transaction.transactions.Transaction_Date = DateTime.Now;
            transaction.transactions.Capture_Date = DateTime.Now;
        }

        var newTransaction = await transactionsRepository.CreateAsync(transaction!.transactions);

        if (newTransaction is null ||
            newTransaction.Transaction_Date > DateTime.Now ||
            newTransaction.Amount == 0)
        {
            return null;
        }

        var newBalance = account.Outstanding_Balance;

        switch (transaction.transactions.Transaction_Type_Id)
        {
            case 1:
                newBalance += transaction.transactions.Amount;
                break;
            case 2:
                newBalance -= transaction.transactions.Amount;
                break;
            default:
                return null;
        }

        await accountsRepository.UpdateBalanceAsync(transaction.transactions.Account_Code, newBalance);

        return newTransaction;
    }

    public async Task<TransactionsModel?> GetSingleTransactionAsync(int code)
    {
        var transaction = await transactionsRepository.RetrieveSingleAsync(code);

        if (transaction is null)
            return null;

        return transaction;
    }

    public async Task<List<TransactionsModel>?> GetTransactionsAsync()
    {
        var transactions = await transactionsRepository.RetrieveAllAsync();

        if (transactions is null)
            return null;

        return transactions;
    }

    public Task<bool> UpdateTransactionAsync(int code, TransactionsModel transaction)
    {
        throw new NotImplementedException();
    }
}
