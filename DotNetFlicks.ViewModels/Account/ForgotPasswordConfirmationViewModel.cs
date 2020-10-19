using System.ComponentModel.DataAnnotations;

namespace DotNetFlicks.ViewModels.Account
{
    public class ForgotPasswordConfirmationViewModel
    {
        [Required]
        //[EmailAddress]
        public string Email { get; set; }
    }
}
