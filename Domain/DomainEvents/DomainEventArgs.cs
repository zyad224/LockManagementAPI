using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DomainEvents
{
   public sealed class DomainEventArgs:EventArgs
    {
        public string DomainObject { get; set; }
        public string DomainObjectId { get; set; }
        public string ChangedDomainPropertyName { get; set; }
        public string ChangedDomainPropertyNewValue { get; set; }

    }
}
