using Shared.Domains.Persons.Models;
using Website.Domains.Accounts.Repositories;
using Website.Domains.Entities.Persons.Repositories;
using Website.Domains.Persons.ViewModels;

namespace Website.Domains.Persons.Services;

public class PersonsServices(
	IPersonsRepository personsRepository,
    IAccountsRepository accountsRepository) : IPersonsServices
{
    public async Task<PersonsModel?> AddPersonAsync(PersonsModel person)
    {
        if (person is null)
            return null;
        
        var individual = (await personsRepository.RetrieveAllAsync())
            .Find(p => p.Id_Number.Equals(person!.Id_Number));

        if (individual is null) 
        {
            var newPerson = await personsRepository.CreateAsync(person);

            return newPerson;
        }
        
        return null;
    }

    public async Task<List<PersonsModel>?> GetPersonsAsync()
    {
        var people = await personsRepository.RetrieveAllAsync();

        if (people is null)
            return null;

        return people;
    }

    public async Task<PersonsViewModel?> GetSinglePersonAsync(int code)
    {
        PersonsViewModel personsView = new();

        personsView.persons = await personsRepository.RetrieveSingleAsync(code);
        personsView.accounts = (await accountsRepository.RetrieveAllAsync())
            .FindAll(a => a.Person_Code == code);

        if (personsView is null)
            return null;

        return personsView;
    }

    public async Task<bool> RemovePersonAsync(int code)
    {
        var personExists = await personsRepository.RetrieveSingleAsync(code);
        var personsAccounts = (await accountsRepository.RetrieveAllAsync())
            .FindAll(a => a.Person_Code == code);

        if (personExists is null ||
            personsAccounts.Count > 0)
            return false;

        var deletePerson = await personsRepository.DeleteAsync(code);

        return deletePerson;
    }

    public async Task<bool> UpdatePersonAsync(int code, PersonsModel person)
    {
        var exists = await personsRepository.RetrieveSingleAsync(code);

        if (exists is null)
            return false;

        var updatePerson = await personsRepository.UpdateAsync(code, person);
        
        return updatePerson;
    }
}
