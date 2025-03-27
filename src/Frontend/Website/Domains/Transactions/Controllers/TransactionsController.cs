using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Domains.Transactions.Models;
using Website.Domains.Accounts.Services;
using Website.Domains.Transactions.Services;

namespace Website.Domains.Transactions.Controllers;

[Authorize]
public class TransactionsController(
    ITransactionsServices transactionsServices,
    IAccountsService accountsServices) : Controller
{
    public async Task<IActionResult> Index()
    {
        var transactions = await transactionsServices.GetTransactionsAsync();
        return View(transactions);
    }

    public async Task<IActionResult> Details(int id)
    {
        if (!ModelState.IsValid)
            return View();

        var transaction = await transactionsServices.GetSingleTransactionAsync(id);

        if (transaction is null)
            return NotFound();

        return View(transaction);
    }

    public async Task<IActionResult> Create(int id, TransactionsModel model)
    {
        if (!ModelState.IsValid)
            return View();

        var getAccount = await accountsServices.GetSingleAccountAsync(id);

        model.Account_Code = getAccount!.account.Code;

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TransactionsModel transaction)
    {
        if (ModelState.IsValid)
        {
            var addedTransaction = await transactionsServices.AddTransactionAsync(transaction);

            if (addedTransaction is not null)
                return RedirectToAction("Details", "Accounts", new { id = transaction.Account_Code });
        }
        return View(transaction);
    }
}
