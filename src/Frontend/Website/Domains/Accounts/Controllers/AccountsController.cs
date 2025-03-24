using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Domains.Accounts.Models;
using Website.Domains.Accounts.Repositories;
using Website.Domains.Accounts.Services;

namespace Website.Controllers;

[Authorize]
public class AccountsController(
    ILogger<AccountsController> logger,
    IAccountsRepository accountsRepository,
    IAccountsService accountsService) : Controller
{
    public async Task<IActionResult> Index()
    {
        //var accounts = await accountsRepository.RetrieveAllAsync();
        //return View(accounts);
        return null;
    }

    public async Task<IActionResult> Details(int id)
    {
        var account = await accountsService.GetSingleAccountAsync(id);
        if (account == null) return NotFound();
        return View(account);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AccountsModel account)
    {
        if (ModelState.IsValid)
        {
            var createdAccount = await accountsService.AddAccountAsync(account);
            if (createdAccount != null) return RedirectToAction(nameof(Index));
        }

        return View(account);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var account = await accountsRepository.RetrieveSingleAsync(id);
        if (account == null) return NotFound();
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
        var account = await accountsRepository.RetrieveSingleAsync(id);
        if (account == null) return NotFound();
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
