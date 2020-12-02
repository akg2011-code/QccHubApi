using QccHubApi.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QccHubApi.Models
{
    public class Order : Entities
    {
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public string ShippingAddress { get; set; } // عنوان الشحن
        [ForeignKey("PaymentStatus")]
        public int PaymentStatusID { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
