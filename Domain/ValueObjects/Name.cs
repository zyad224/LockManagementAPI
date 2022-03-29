using CSharpFunctionalExtensions;
using Domain.DomainExceptions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ValueObjects
{
    public class Name : ValueObject
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
      
        public Name(string firstName, string lastName)
        {
            if ((string.IsNullOrEmpty(firstName)) || (string.IsNullOrEmpty(lastName)))
                throw new NameInvalidException("Name Invalid Exception");

            FirstName = firstName;
            LastName = lastName;
        }
          
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
        }


    }
}
