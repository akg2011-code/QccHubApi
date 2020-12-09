﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace QccHub.Data.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public bool IsTrusted { get; set; } // for companies
        public string LigitDocument { get; set; } // for companies
        public DateTime DateOfBirth { get; set; }
        [ForeignKey("Gender")]
        public int? GenderID { get; set; }
        [ForeignKey("Country")]
        public int? NationalityID { get; set; }
        
        public string Address { get; set; }
        public string CVFilePath { get; set; }
        public string ProfileImagePath { get; set; }

        public virtual Gender Gender { get; set; }
        public virtual Country Country { get; set; }

        public virtual ICollection<UserJobPositions> JobPositions { get; } = new List<UserJobPositions>();
        public virtual ICollection<ApplicationUserRole> UserRoles { get; } = new List<ApplicationUserRole>();
    }
}
