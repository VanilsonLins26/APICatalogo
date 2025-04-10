﻿using Microsoft.OpenApi.MicrosoftExtensions;
using System.ComponentModel.DataAnnotations;


namespace APICatalogo.DTOs
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User name is required")]
        public string? UsernName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

    }
}
