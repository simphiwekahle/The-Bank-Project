using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Domains.Transactions.Models;
using Website.Domains.Accounts.Services;
using Website.Domains.Transactions.Repositories;
using Website.Domains.Transactions.Services;
using Website.Domains.Transactions.ViewModels;

namespace Website.Domains.Transactions.Controllers;

[Authorize]
public class TransactionsController(
    ITransactionsServices transactionsServices,
    ITransactionsRepository transactionsRepository,
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

    public async Task<IActionResult> Create(int id, TransactionsViewModel model)
    {
        if (!ModelState.IsValid)
            return View();

        var getAccount = await accountsServices.GetSingleAccountAsync(id);

        model.transactions.Account_Code = getAccount!.account.Code;
        model.transactionsTypes = await transactionsRepository.RetrieveAllTypesAsync();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TransactionsViewModel transaction)
    {
        if (ModelState.IsValid)
        {
            var addedTransaction = await transactionsServices.AddTransactionAsync(transaction);

            TempData["SuccessMessage"] = "Transaction has been created successfully.";
            if (addedTransaction is not null)
                return RedirectToAction("Details", "Accounts", new { id = transaction.transactions.Account_Code });
        }
        return View(transaction);
    }
}
