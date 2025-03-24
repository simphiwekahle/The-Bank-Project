using Website.Domains.Accounts.Repositories;
using Website.Domains.Accounts.Services;

namespace Website.Domains.Accounts;

public static class DependencyInjection
{
	public static IServiceCollection AddAccountsServices(this IServiceCollection services)
	{
		services.AddScoped<IAccountsRepository, AccountsRepository>();
		services.AddScoped<IAccountsService, AccountsService>();
		return services;
	}
}
