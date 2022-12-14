using Microsoft.AspNetCore.Identity;

namespace UserService
{
    public class User : IdentityUser
    {
        public User() { }
        public User(string userName, string email)
        {
            this.UserName = userName;
            this.Email = email;
        }
    }
}