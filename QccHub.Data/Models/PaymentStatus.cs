using QccHub.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QccHub.Data
{
    public class PaymentStatus : BaseEntity, ICreationAuditable , ISoftDeletable
    {
        public string StatusName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
