using System.ComponentModel.DataAnnotations;

namespace Energy.Net.Features.Session
{
    public class LoginViewModel
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
