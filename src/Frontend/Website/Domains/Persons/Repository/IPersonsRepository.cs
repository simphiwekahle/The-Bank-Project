using Shared.Domains.Persons.Models;

namespace Website.Domains.Entities.Persons.Repositories;

public interface IPersonsRepository
{
    Task<PersonsModel?> CreateAsync(PersonsModel person);

    Task<List<PersonsModel>> RetrieveAllAsync();

    Task<PersonsModel?> RetrieveSingleAsync(int code);

    Task<bool> UpdateAsync(int code, PersonsModel person);

    Task<bool> DeleteAsync(int code);
}
