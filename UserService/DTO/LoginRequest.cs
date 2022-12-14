using System.ComponentModel.DataAnnotations;

namespace UserService.DTO
{

    public class LoginRequest
    {
        [Required(ErrorMessage = "Email can not be empty.")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Password can not be empty.")]
        public string Password { get; set; } = null!;
    }

}