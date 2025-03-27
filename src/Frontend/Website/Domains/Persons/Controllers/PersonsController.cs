using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        return View();
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PersonsModel person)
    {
        if (ModelState.IsValid)
        {
            var addedPerson = await personsService.AddPersonAsync(person);
            if (addedPerson != null)
                return RedirectToAction("Login", "Access");
        }
        return View(person);
    }

    public async Task<IActionResult> Edit(int id)
    {
        //var person = await personsService.GetSinglePersonAsync(id);
        //if (person == null) return NotFound();
        //return View(person);
        return null;
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

    public async Task<IActionResult> Delete(int id)
    {
        var person = await personsService.GetSinglePersonAsync(id);

        if (person == null) 
            return NotFound();

        return View(person);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (ModelState.IsValid)
        {
            var success = await personsService.RemovePersonAsync(id);
            
            if (success)
                return RedirectToAction("Index", "Persons");
        }
        
        return View();
    }
}
