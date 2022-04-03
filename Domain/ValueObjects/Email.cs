using CSharpFunctionalExtensions;
using Domain.DomainExceptions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ValueObjects
{
    public class Email : ValueObject
    {
        public string Value { get; private set; }

        public Email (string value)
        {
            if ((string.IsNullOrEmpty(value)) || (!value.Contains("@")))
                throw new EmailInvalidException("Email Invalid Exception");

            Value = value;
        }
    
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }


    }
}
