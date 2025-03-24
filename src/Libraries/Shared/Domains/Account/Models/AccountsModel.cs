using System.ComponentModel.DataAnnotations;

namespace Shared.Domains.Accounts.Models;

public class AccountsModel
{
	public int Code { get; set; }
	public int Person_Code { get; set; }
	public string Account_Number { get; set; } = string.Empty;
	public decimal Outstanding_Balance { get; set; }
}
