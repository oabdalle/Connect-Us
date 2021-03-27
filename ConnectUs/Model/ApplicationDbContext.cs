using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ConnectUs.Model.Entities;
namespace ConnectUs.Model
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(1000);

        }
        public DbSet<Event> Event { get; set; }
        public DbSet<EventImage> EventImage { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Profile> Profile { get; set; }
    }
}
