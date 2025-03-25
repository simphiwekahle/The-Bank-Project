namespace Website.Configurations;

public class StoredProcedureOptions
{
	public string GetAllPersons { get; set; } = string.Empty;
	public string GetPersonByCode { get; set; } = string.Empty;
	public string InsertNewPerson { get; set; } = string.Empty;
	public string UpdatePersonByCode { get; set; } = string.Empty;
	public string DeletePersonByCode { get; set; } = string.Empty;

	public string GetAllAccounts { get; set; } = string.Empty;
	public string GetAccountByCode { get; set; } = string.Empty;
	public string InsertNewAccount { get; set; } = string.Empty;
	public string UpdateAccountById { get; set; } = string.Empty;
	public string DeleteAccountById { get; set; } = string.Empty;
	public string UpdateAccountBalance { get; set; } = string.Empty;

	public string GetAllTransactions { get; set; } = string.Empty;
	public string GetTransactionByCode { get; set; } = string.Empty;
	public string InsertNewTransaction { get; set; } = string.Empty;
	public string UpdateTransactionById { get; set; } = string.Empty;
	public string DeleteTransactionById { get; set; } = string.Empty;
}
