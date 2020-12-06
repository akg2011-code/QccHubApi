using System;
using System.Collections.Generic;
using System.Text;

namespace QccHub.DTOS
{
    public class UserRegisteration
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }

    }
}
