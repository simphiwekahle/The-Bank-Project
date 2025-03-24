using Website.Domains.Transactions.Repositories;
using Website.Domains.Transactions.Services;

namespace Website.Domains.Transactions;

public static class DependencyInjection
{
	public static IServiceCollection AddTransactionsServices(this IServiceCollection services)
	{
		services.AddScoped<ITransactionsRepository, TransactionsRepository>();
		services.AddScoped<ITransactionsServices, TransactionsServices>();
		return services;
	}
}
