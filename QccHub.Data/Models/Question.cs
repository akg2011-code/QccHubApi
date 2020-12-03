using QccHub.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QccHub.Data
{
    public class Question : BaseEntity, ICreationAuditable
    {
        public string Title { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual User User { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
