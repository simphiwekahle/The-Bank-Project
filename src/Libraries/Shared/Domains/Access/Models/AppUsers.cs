using System.ComponentModel.DataAnnotations;

namespace Shared.Domains.Access.Models;

public class AppUsers
{
    [Key]
    public int Id { get; set; }

	[Required(ErrorMessage = "Please Enter Your First Name")]
	[Display(Name = "First Name")]
	public string Name { get; set; } = string.Empty;

	[Required(ErrorMessage = "Please Enter Your Surname")]
	[Display(Name = "Surname")]
	public string Surname { get; set; } = string.Empty;

	[Required(ErrorMessage = "Please Enter Your ID Number")]
	[Display(Name = "ID Number")]
	public string Id_Number { get; set; } = string.Empty;

	[Required(ErrorMessage = "Please Enter Your Phone Number")]
	[Display(Name = "Phone Number")]
	public string Phone_Number { get; set; } = string.Empty;

	[Required(ErrorMessage = "Please Enter Your Email Address")]
	[Display(Name = "Email Address")]
	[EmailAddress]
	public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateOnly Date_Of_Birth { get; set; }
}
