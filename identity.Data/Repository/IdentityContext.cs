using System;
using System.Collections.Generic;
using System.Text;
using identity.Data.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace identity.Data.Repository
{
  public  class IdentityContext:IdentityDbContext<User>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>(entity =>
            {
                entity.ToTable(name: "Users");
                entity.Property(e => e.Id).HasColumnName("UserId");

            });
        }
    }
}
