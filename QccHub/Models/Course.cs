using QccHubApi.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QccHubApi.Models
{
    public class Course : Entities
    {
        public string Name { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
        public string Inistitute { get; set; }
        public string CertifiedFilePath { get; set; }
        public virtual User User { get; set; }


    }
}
