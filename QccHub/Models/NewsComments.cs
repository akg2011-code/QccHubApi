using QccHubApi.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QccHubApi.Models
{
    public class NewsComments : Entities
    {
        [ForeignKey("News")]
        public int NewsID { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
        public DateTime Time { get; set; }
        public string Comment { get; set; }

        public virtual News News { get; set; }
        public virtual User User { get; set; }
    }
}
