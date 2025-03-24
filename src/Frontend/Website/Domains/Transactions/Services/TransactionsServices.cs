using Shared.Domains.Transactions.Models;
using Website.Domains.Accounts.Repositories;
using Website.Domains.Transactions.Repositories;

namespace Website.Domains.Transactions.Services;

public class TransactionsServices(
    ITransactionsRepository transactionsRepository,
    IAccountsRepository accountsRepository) : ITransactionsServices
{
    public async Task<TransactionsModel?> AddTransactionAsync(TransactionsModel transaction)
    {
        var account = await accountsRepository.RetrieveSingleAsync(transaction!.Account_Code);

        if (transaction is null)
            return null;

        if (transaction is not null)
        {
            transaction.Description = "New transaction added";
        }

        var newTransaction = await transactionsRepository.CreateAsync(transaction!);

        if (newTransaction is null ||
            newTransaction.Transaction_Date > DateTime.Now || 
            newTransaction.Amount == 0)
        {
            return null;
        }

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

    public Task<bool> RemoveTransactionAsync(int code)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateTransactionAsync(int code, TransactionsModel transaction)
    {
        throw new NotImplementedException();
    }
}
