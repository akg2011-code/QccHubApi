using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QccHub.DTOS
{
    public class UpdateInfoVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? GenderID { get; set; }
        public string Bio  { get; set; }
        public string ProfileImage { get; set; }
        public IFormFile CV { get; set; }
    }
}
