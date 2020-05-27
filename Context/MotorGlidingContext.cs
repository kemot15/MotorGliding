using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MotorGliding.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Context
{
    public class MotorGlidingContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public MotorGlidingContext(DbContextOptions<MotorGlidingContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int> { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole<int> { Id = 2, Name = "User", NormalizedName = "USER" },
                new IdentityRole<int> { Id =3, Name = "Anonymous", NormalizedName = "ANONYMOUS"});
            base.OnModelCreating(builder);
        }

        public DbSet<Image> Images { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Features> Features { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Calendar> Calendar { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<OrderUser> OrderUsers { get; set; }

    }
}
