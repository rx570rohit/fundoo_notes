using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DatabaseLayer.User
{
    public class UserPasswordModel
    {
        [Required]

        [RegularExpression("^(?=.*[A-Z]).{8,}$", ErrorMessage = "Password Have minimum 8 Characters, Should have at least 1 Upper Case and Should have at least 1 numeric number and Has exactly 1 Special Character")]
        public string Password { get; set; }
        [Required]

        [RegularExpression("^(?=.*[A-Z]).{8,}$", ErrorMessage = "Password Have minimum 8 Characters, Should have at least 1 Upper Case and Should have at least 1 numeric number and Has exactly 1 Special Character")]
        public string ConfirmPassword { get; set; }
    }
}
