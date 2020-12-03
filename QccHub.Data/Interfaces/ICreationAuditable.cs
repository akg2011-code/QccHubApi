using System;
using System.Collections.Generic;
using System.Text;

namespace QccHub.Data.Interfaces
{
    public interface ICreationAuditable
    {
        string CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
    }
}
