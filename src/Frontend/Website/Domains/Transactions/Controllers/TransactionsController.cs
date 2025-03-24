using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Domains.Transactions.Models;
using Website.Domains.Transactions.Repositories;
using Website.Domains.Transactions.Services;

namespace Website.Domains.Transactions.Controllers;

[Authorize]
public class TransactionsController(
    ITransactionsServices transactionsServices,
    ITransactionsRepository transactionsRepository) : Controller
{
    public async Task<IActionResult> Index()
    {
        var transactions = await transactionsServices.GetTransactionsAsync();
        return View(transactions);
    }

    public async Task<IActionResult> Details(int id)
    {
        var transaction = await transactionsRepository.RetrieveSingleAsync(id);
        if (transaction is null)
            return NotFound();

        return View(transaction);
    }

	public IActionResult Create()
	{
		return View();
	}

	[HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TransactionsModel transaction)
    {
        if (ModelState.IsValid)
        {
            var addedTransaction = await transactionsServices.AddTransactionAsync(transaction);

            if (addedTransaction is not null)
                return RedirectToAction(nameof(Index));
        }
        return View(transaction);
    }
}
