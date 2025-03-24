using Shared.Domains.Persons.Models;
using Website.Domains.Persons.ViewModels;

namespace Website.Domains.Persons.Services;

public interface IPersonsServices
{
	Task<PersonsModel?> AddPersonAsync(PersonsModel person);
	Task<List<PersonsModel>?> GetPersonsAsync();
	Task<PersonsViewModel?> GetSinglePersonAsync(int code);
	Task<bool> UpdatePersonAsync(int code, PersonsModel person);
	Task<bool> RemovePersonAsync(int code);
}
