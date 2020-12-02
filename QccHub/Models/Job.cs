﻿using QccHubApi.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QccHubApi.Models
{
    public class Job : Entities
    {
        public string Title { get; set; }
        public string Description { get; set; }
        [ForeignKey("Company")]
        public string CompanyID { get; set; }
        public virtual User Company { get; set; }

    }
}
