﻿using QccHub.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QccHub.Data
{
    public class Order : BaseEntity, ICreationAuditable
    {
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public string ShippingAddress { get; set; } // عنوان الشحن
        [ForeignKey("PaymentStatus")]
        public int PaymentStatusID { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}