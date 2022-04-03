using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class DbApiContext : DbContext, IDbApiContext
    {
        public DbApiContext(DbContextOptions<DbApiContext> options)
         : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Lock> Locks { get; set; }
        public DbSet<Audit> Audits { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            modelBuilder.Entity<User>()
            .OwnsOne(u => u.Name);
            modelBuilder.Entity<User>()
           .OwnsOne(u => u.Email);
        }
    }
}
