﻿using System.ComponentModel.DataAnnotations;

namespace DotNetFlicks.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        //[EmailAddress]
        public string Email { get; set; }
    }
}
