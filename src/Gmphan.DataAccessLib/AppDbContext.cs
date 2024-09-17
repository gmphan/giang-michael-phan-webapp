using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Gmphan.ModelLib;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gmphan.DataAccessLib
{
    public class AppDbContext : IdentityDbContext<IdentityUser> 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<QuoteCollection> QuoteCollections { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // : IdentityDbContext require the line of code below to work
            base.OnModelCreating(modelBuilder);

            /***
            * HasData method to seed initial data into a database when the migrations are applied.
            * The _ = is typically used in C# to discard a result (though in this case, it may be unnecessary). 
            It simply means that the return value of the method is ignored. 
            Since HasData doesn’t return anything significant that needs to be captured, 
            this is a way to indicate that the return value is not used.
            ***/
            _ = modelBuilder.Entity<QuoteCollection>().HasData(
                new QuoteCollection
                {
                    Id = 1,
                    Quote = "No goal, no growth.",
                    Author = "Google",
                    Type = "Motivation",
                    CreatedDate = new DateTime(),
                    ModifiedDate = new DateTime()
                });
        }
    }
}