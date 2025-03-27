using Shared.Domains.Accounts.Models;
using Shared.Domains.Transactions.Models;

namespace Website.Domains.Accounts.ViewModels;

public class AccountsViewModel
{
    public AccountsModel account { get; set; } = new AccountsModel();
    public List<TransactionsModel> transactions { get; set; } = [];
}
