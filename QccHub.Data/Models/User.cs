﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace QccHub.Data
{
    public class User : IdentityUser
    {
        public bool IsTrusted { get; set; }
        public string LigitDocument { get; set; }
        public DateTime DateOfBirth { get; set; }
        [ForeignKey("Gender")]
        public int? GenderID { get; set; }
        [ForeignKey("Country")]
        public int? NationalityID { get; set; }
        public string JobPossition { get; set; }
        public int? YearsOfExperience { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Address { get; set; }
        public string CVFilePath { get; set; }
        public string ProfileImagePath { get; set; }

        public virtual Gender Gender { get; set; }
        public virtual Country Country { get; set; }

    }
}