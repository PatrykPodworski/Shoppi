using System.ComponentModel.DataAnnotations;

namespace Shoppi.Models.Account
{
    public class AccountRegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}