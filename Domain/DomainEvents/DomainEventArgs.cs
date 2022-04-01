using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DomainEvents
{
   public sealed class DomainEventArgs:EventArgs
    {
        public string ChangedDomainPropertyName { get; set; }
        public string ChangedDomainPropertyNewValue { get; set; }

    }
}
