using MessageLibrary.Contracts;

namespace TestAccountService.Models
{
    public class AddUser : IAddUser
    {
	    public string FirstName { get; set; }
	    public string LastName { get; set; }
	    public string Password { get; set; }
	    public string EmailAddress { get; set; }
    }
}
