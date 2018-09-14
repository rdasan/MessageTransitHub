namespace MessageLibrary.Contracts
{
	public interface IAddUser
	{
		string FirstName { get; set; }
		string LastName { get; set; }
		string Password { get; set; }
		string EmailAddress { get; set; }
	}
}
