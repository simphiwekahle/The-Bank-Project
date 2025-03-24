using ManagePeopleAPI.Domains.Entities.Persons.Repositories;
using Website.Domains.Entities.Persons.Repositories;
using Website.Domains.Persons.Services;

namespace Website.Domains.Persons;

public static class DependencyInjection
{
	public static IServiceCollection AddPersonsServices(this IServiceCollection services)
	{
		services.AddScoped<IPersonsRepository, PersonsRepository>();
		services.AddScoped<IPersonsServices, PersonsServices>();
		return services;
	}
}
