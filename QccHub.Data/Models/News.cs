using QccHub.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QccHub.Data
{
    public class News : BaseEntity, ICreationAuditable, ISoftDeletable
    {
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime Time { get; set; }
        [ForeignKey("Company")]
        public string CompanyID { get; set; }
        public virtual User Company { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
