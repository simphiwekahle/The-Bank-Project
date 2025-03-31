using Shared.Domains.Transactions.Models;

namespace Website.Domains.Transactions.Repositories;

public interface ITransactionsRepository
{
    Task<TransactionsModel?> CreateAsync(TransactionsModel transaction);

    Task<List<TransactionsModel>> RetrieveAllAsync();
    Task<List<TransactionTypesModel>> RetrieveAllTypesAsync();

    Task<TransactionsModel?> RetrieveSingleAsync(int code);

    Task<bool> UpdateAsync(int code, TransactionsModel transaction);

    Task<bool> DeleteAsync(int code);
}
