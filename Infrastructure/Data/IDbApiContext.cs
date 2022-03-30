using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public interface IDbApiContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Lock> Locks { get; set; }
        public DbSet<LockAudit> LockAudits { get; set; }

    }
}
