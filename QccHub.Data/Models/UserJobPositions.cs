using QccHub.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QccHub.Data.Models
{
    public class UserJobPositions : BaseEntity, ICreationAuditable, ISoftDeletable
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public string JobPossition { get; set; }
        public int? YearsOfExperience { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public bool IsCurrentPosition { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
