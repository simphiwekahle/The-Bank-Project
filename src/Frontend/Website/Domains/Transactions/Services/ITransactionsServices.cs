using Shared.Domains.Transactions.Models;

namespace Website.Domains.Transactions.Services;

public interface ITransactionsServices
{
    Task<TransactionsModel?> AddTransactionAsync(TransactionsModel transaction);
    Task<List<TransactionsModel>?> GetTransactionsAsync();
    Task<TransactionsModel?> GetSingleTransactionAsync(int code);
    Task<bool> UpdateTransactionAsync(int code, TransactionsModel transaction);
    Task<bool> RemoveTransactionAsync(int code);
}
