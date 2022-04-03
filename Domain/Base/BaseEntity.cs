using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Base
{
    public class BaseEntity
    {
        public DateTime CreatedOn { get; protected set; }
        public DateTime ModifiedOn { get; protected set; }
    }
}
