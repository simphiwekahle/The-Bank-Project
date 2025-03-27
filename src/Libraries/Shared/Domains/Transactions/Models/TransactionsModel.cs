using System.ComponentModel.DataAnnotations;

namespace Shared.Domains.Transactions.Models;

public class TransactionsModel
{
	public int Code { get; set; }
	public int Account_Code { get; set; }
	public DateTime Transaction_Date { get; set; }
	public DateTime Capture_Date { get; set; }

	[Required(ErrorMessage = "Please Enter the Transacton's Amount")]
	[Display(Name = "Amount")]
	public decimal Amount { get; set; }
	public string Description { get; set; } = string.Empty;
}
