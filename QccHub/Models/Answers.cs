using QccHubApi.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QccHubApi.Models
{
    public class Answers : Entities
    {
        public string Text { get; set; }
        [ForeignKey("Question")]
        public int QuestionID { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual Question Question { get; set; }
        public virtual User User { get; set; }
    }
}
