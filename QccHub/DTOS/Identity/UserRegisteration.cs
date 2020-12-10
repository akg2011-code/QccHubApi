using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QccHub.DTOS
{
    public class UserRegisteration
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password),Compare("Password",ErrorMessage = "Passwords are not matched")]
        public string ConfirmPassword { get; set; }
        public int RoleId { get; set; }

    }
}
