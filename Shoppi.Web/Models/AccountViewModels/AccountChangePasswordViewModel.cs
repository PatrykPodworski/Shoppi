using System.ComponentModel.DataAnnotations;

namespace Shoppi.Web.Models.AccountViewModels
{
    public class AccountChangePasswordViewModel
    {
        [Required]
        [Display(Name = "Old Password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string NewPasswordConfirm { get; set; }
    }
}