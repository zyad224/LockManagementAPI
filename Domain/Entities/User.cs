using Domain.Base;
using Domain.DomainExceptions;
using Domain.Shared;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class User:BaseEntity
    {
        [Key]
        [Required(ErrorMessage = "UserId is Required"), MaxLength(60)]
        public string UserId { get; private set;}

        public Name Name { get; private set; }

        public Email Email { get; private set; }
        public string Role { get; private set; }
        public string Password { get; private set; }

        [NotMapped]
        public string Jwt { get; private set; }

        public virtual IEnumerable<Lock> Locks { get; }


        private User()
        {

        }
        public User(Name name, Email email, string role, string password)
        {
            if ((name == null) ||
                (email == null) || 
                (string.IsNullOrEmpty(role)) ||
                (string.IsNullOrEmpty(password)))
                throw new UserInvalidException("User Invalid Exception");

            UserId = UUIDGenerator.NewUUID();
            Name = name;
            Email = email;
            Role = role;
            Password = password;
            CreatedOn = DateTime.UtcNow;
            ModifiedOn = DateTime.UtcNow;
        }

        public void SetUserName(Name name)
        {
            if ((string.IsNullOrEmpty(name.FirstName)) || (string.IsNullOrEmpty(name.LastName)))
                throw new NameInvalidException("Name Invalid Exception");
            Name = name;
            ModifiedOn = DateTime.UtcNow;

        }
        public void SetUserPassword(string password)
        {
            if ((string.IsNullOrEmpty(password)))
                throw new PasswordInvalidException("Password Invalid Exception");
            Password = password;
            ModifiedOn = DateTime.UtcNow;

        }
        public void SetUserEmail(Email email)
        {
            if ((string.IsNullOrEmpty(email.Value)) || (!email.Value.Contains("@")))
                throw new EmailInvalidException("Email Invalid Exception");
            Email = email;
            ModifiedOn = DateTime.UtcNow;

        }
        public void SetUserRole(string role)
        {
            if ((string.IsNullOrEmpty(role)))
                throw new RoleInvalidException("Role Invalid Exception");
            Role = role;
            ModifiedOn = DateTime.UtcNow;

        }

        public void SetUserJwt(string jwt)
        {
            if ((string.IsNullOrEmpty(jwt)))
                throw new UserInvalidJwt("JWT Invalid Exception");
            Jwt = jwt;

        }

    }
}
