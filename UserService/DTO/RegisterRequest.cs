namespace UserService.DTO
{
    public class RegisterRequest
    {
        public string userName { get; set; }

        public string email { get; set; }

        public string password { get; set; }

        public RegisterRequest() { }

        public RegisterRequest(string userName, string email, string password)
        {
            this.userName = userName;
            this.email = email;
            this.password = password;
        }
    }
}