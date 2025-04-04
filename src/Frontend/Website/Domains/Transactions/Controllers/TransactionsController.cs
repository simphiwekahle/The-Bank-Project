﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Domains.Transactions.Models;
using System.Globalization;
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

            if (addedTransaction is null)
            {
                TempData["ErrorMessage"] = $"Attempt to create transaction for account {transaction.transactions.Account_Code} has failed, Please Try again";
                return RedirectToAction("Create", new { id = transaction.transactions.Account_Code });
            }

            TempData["SuccessMessage"] = $"{addedTransaction!.Transaction_Type_Id} transaction of amount {transaction.transactions.Amount.ToString("C", new CultureInfo("en-ZA"))} has been added to account code {transaction.transactions.Account_Code} successfully.";
            return RedirectToAction("Details", "Accounts", new { id = transaction.transactions.Account_Code });
        }
        return View(transaction);
    }
}
