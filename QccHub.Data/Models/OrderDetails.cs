using QccHub.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QccHub.Data
{
    public class OrderDetails : BaseEntity, ICreationAuditable, ISoftDeletable
    {
        [ForeignKey("Order")]
        public int OrderID { get; set; }
        public double Price { get; set; }
        [ForeignKey("Item")]
        public int ItemID { get; set; }
        public Order Order { get; set; }
        public Item Item { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
