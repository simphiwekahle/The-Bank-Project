using Shared.Domains.Accounts.Models;
using Shared.Domains.Persons.Models;

namespace Website.Domains.Persons.ViewModels;

public class PersonsViewModel
{
    public PersonsModel persons { get; set; } = new PersonsModel();
    public List<AccountsModel> accounts { get; set; } = [];
}
