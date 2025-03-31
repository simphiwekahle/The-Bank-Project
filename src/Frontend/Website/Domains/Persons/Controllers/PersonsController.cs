using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Domains.Persons.Models;
using Website.Domains.Persons.Services;

namespace Website.Controllers;

public class PersonsController(
    IPersonsServices personsService) : Controller
{
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var persons = await personsService.GetPersonsAsync();
        return View(persons);
    }

    public async Task<IActionResult> Details(int id)
    {
        if (ModelState.IsValid)
        {
            var person = await personsService.GetSinglePersonAsync(id);

            if (person is null)
                return NotFound();

            return View(person);
        }
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Create()
    {
        return View(new PersonsModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PersonsModel person)
    {
        if (!ModelState.IsValid)
        {
            return View(person);
        }

        var addedPerson = await personsService.AddPersonAsync(person);
        if (addedPerson != null)
        {
            TempData["SuccessMessage"] = $"{addedPerson.Name} {addedPerson.Surname} has been created successfully!";
            return RedirectToAction("Login", "Access");
        }

        TempData["ErrorMessage"] = "A person with this ID Number already exists.";
        return View(person);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var person = await personsService.GetSinglePersonAsync(id);
        if (person == null)
            return NotFound();

        return View(person);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PersonsModel person)
    {
        if (id != person.Code) return NotFound();

        if (ModelState.IsValid)
        {
            var success = await personsService.UpdatePersonAsync(id, person);
            if (success)
                return RedirectToAction(nameof(Index));
        }
        return View(person);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (ModelState.IsValid)
        {
            var person = await personsService.GetSinglePersonAsync(id);
            var result = await personsService.RemovePersonAsync(id);

            if (result)
            {
                TempData["SuccessMessage"] = $"{person!.persons.Name} {person!.persons.Surname} has been deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = $"Failed to delete {person!.persons.Name} {person!.persons.Surname}. Ensure they have no linked accounts.";
            }

            return RedirectToAction("Index");
        }
        return View();
    }
}
