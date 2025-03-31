using Shared.Domains.Transactions.Models;

namespace Website.Domains.Transactions.ViewModels;

public class TransactionsViewModel
{
    public TransactionsModel transactions { get; set; } = new TransactionsModel();
    public List<TransactionTypesModel> transactionsTypes { get; set; } = [];
}
