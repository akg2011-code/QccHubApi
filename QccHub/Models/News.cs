using QccHubApi.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QccHubApi.Models
{
    public class News :Entities
    {
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime Time { get; set; }
        [ForeignKey("Company")]
        public string CompanyID { get; set; }
        public virtual User Company { get; set; }
    }
}
