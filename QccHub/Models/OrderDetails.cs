using QccHubApi.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QccHubApi.Models
{
    public class OrderDetails : Entities
    {
        [ForeignKey("Order")]
        public int OrderID { get; set; }
        public double Price { get; set; }
        [ForeignKey("Item")]
        public int ItemID { get; set; }
        public Order Order { get; set; }
        public Item Item { get; set; }
    }
}
