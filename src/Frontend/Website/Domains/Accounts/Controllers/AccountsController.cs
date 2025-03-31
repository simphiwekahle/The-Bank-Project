using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Domains.Accounts.Models;
using System.Reflection;
using System.Transactions;
using Website.Domains.Accounts.Repositories;
using Website.Domains.Accounts.Services;
using Website.Domains.Accounts.ViewModels;
using Website.Domains.Persons.Services;
using Website.Domains.Transactions.Repositories;

namespace Website.Controllers;

[Authorize]
public class AccountsController(
    ILogger<AccountsController> logger,
    IAccountsRepository accountsRepository,
    IAccountsService accountsService,
    IPersonsServices personsServices) : Controller
{
    public async Task<IActionResult> Index()
    {
        var accounts = await accountsRepository.RetrieveAllAsync();
        return View(accounts);
    }

    public async Task<IActionResult> Details(int id)
    {
        if (!ModelState.IsValid)
            return View();

        var account = await accountsService.GetSingleAccountAsync(id);

        if (account is null)
            return NotFound();

        return View(account);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int id)
    {
        try
        {
            var person = await personsServices.GetSinglePersonAsync(id);
            if (person == null)
            {
                return Json(new { success = false, message = "Person not found." });
            }

            var newAccount = new AccountsModel
            {
                Person_Code = person.persons.Code
            };

            var createdAccount = await accountsService.AddAccountAsync(newAccount);
            if (createdAccount != null)
            {
                TempData["SuccessMessage"] = $"{createdAccount.Account_Number} created successfully.";
                return Json(new { success = true, message = "Account created successfully." });
            }

            return Json(new { success = false, message = "Failed to create an account. Account might already exist." });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating account.");
            return Json(new { success = false, message = $"Server error: {ex.Message}" });
        }
    }



    public async Task<IActionResult> Edit(int id)
    {
        if (!ModelState.IsValid)
            return View();

        var account = await accountsRepository.RetrieveSingleAsync(id);

        if (account == null) 
            return NotFound();

        return View(account);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AccountsModel account)
    {
        if (ModelState.IsValid)
        {
            var success = await accountsRepository.UpdateAsync(id, account);
            if (success) return RedirectToAction(nameof(Index));
        }

        return View(account);
    }

    public async Task<IActionResult> Delete(int id)
    {
        if (!ModelState.IsValid)
            return View();

        var account = await accountsRepository.RetrieveSingleAsync(id);

        if (account == null) 
            return NotFound();

        return View(account);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var success = await accountsRepository.DeleteAsync(id);
        if (success) return RedirectToAction(nameof(Index));
        return View();
    }
}
