using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using QccHub.Data.Interfaces;

namespace QccHub.Data
{
    public class Answers : BaseEntity , ICreationAuditable
    {
        public string Text { get; set; }
        [ForeignKey("Question")]
        public int QuestionID { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual Question Question { get; set; }
        public virtual User User { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get ; set; }
    }
}
