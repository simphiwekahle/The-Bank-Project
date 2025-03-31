using Shared.Domains.Transactions.Models;
using Website.Domains.Transactions.ViewModels;

namespace Website.Domains.Transactions.Services;

public interface ITransactionsServices
{
    Task<TransactionsModel?> AddTransactionAsync(TransactionsViewModel transaction);
    Task<List<TransactionsModel>?> GetTransactionsAsync();
    Task<TransactionsModel?> GetSingleTransactionAsync(int code);
    Task<bool> UpdateTransactionAsync(int code, TransactionsModel transaction);
}
