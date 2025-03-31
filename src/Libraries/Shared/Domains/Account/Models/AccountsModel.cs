using System.ComponentModel.DataAnnotations;

namespace Shared.Domains.Accounts.Models;

public class AccountsModel
{
	[Key]
	public int Code { get; set; }

    [Required(ErrorMessage = "Please Enter Person Code")]
    [Display(Name = "Person Code")]
    public int Person_Code { get; set; }

    [Required(ErrorMessage = "Please Enter Account Number")]
    [Display(Name = "Account Number")]
    public string Account_Number { get; set; } = string.Empty;
    public decimal Outstanding_Balance { get; set; }
    public bool IsActive { get; set; }
}
