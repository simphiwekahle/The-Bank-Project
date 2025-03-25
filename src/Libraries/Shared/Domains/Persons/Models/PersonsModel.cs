using System.ComponentModel.DataAnnotations;

namespace Shared.Domains.Persons.Models;

public class PersonsModel
{
	[Key]
	public int Code { get; set; }

	[Required(ErrorMessage = "Please Enter Your First Name")]
	[Display(Name = "First Name")]
	public string? Name { get; set; }

	[Required(ErrorMessage = "Please Enter Your Surame")]
	[Display(Name = "Surame")]
	public string? Surname { get; set; }

	[Required(ErrorMessage = "Please Enter Your ID Number")]
	[Length (13, 13)]
    [Display(Name = "ID Number")]
    public string Id_Number { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please Enter an Email Address")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please Enter a Password")]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;
}
