using QccHub.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QccHub.Data
{
    public class Country : BaseEntity , ICreationAuditable, ISoftDeletable
    {
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

    }
}
