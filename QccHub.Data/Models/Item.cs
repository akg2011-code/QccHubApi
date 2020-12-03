﻿using QccHub.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QccHub.Data
{
    public class Item : BaseEntity, ICreationAuditable, ISoftDeletable
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        [ForeignKey("Supplier")]
        public string SupplierID { get; set; }
        public int Stock { get; set; } // المخزون
        public string Code { get; set; }
        public string PromotionCode { get; set; }
        public double Discount { get; set; }
        public int FixedStock { get; set; } // مقدار مخزون مينفعش يقل عنه
        public DateTime PromotoionExpireDate { get; set; } // معاد انتهاء العرض

        public User Supplier { get; set; }
        public Category Category { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
