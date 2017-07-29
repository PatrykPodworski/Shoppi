using System.ComponentModel.DataAnnotations;

namespace Shoppi.Models.Account
{
    public class AccountRegisterViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}