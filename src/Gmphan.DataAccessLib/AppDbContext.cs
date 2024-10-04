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
        public DbSet<ResumeHeader> ResumeHeaders { get; set; }
        public DbSet<ResumeSummary> ResumeSummaries { get; set; }
        public DbSet<ResumeExperience> ResumeExperiences { get; set; }
        public DbSet<ResumeDescription> ResumeDescriptions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // : IdentityDbContext require the line of code below to work
            base.OnModelCreating(modelBuilder);

            // Optionally configure the relationship explicitly, though EF usually handles this automatically
            // cascading delete: deleting a ResumeExperience will automatically delete all related ResumeDescriptions.
            modelBuilder.Entity<ResumeExperience>()
                .HasMany(re => re.Descriptions)
                .WithOne(rd => rd.ResumeExperience)
                .HasForeignKey(rd => rd.ResumeExperienceId)
                .OnDelete(DeleteBehavior.Cascade);

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
                    UpdatedDate = new DateTime()
                }
            );
            _ = modelBuilder.Entity<ResumeHeader>().HasData(
                new ResumeHeader
                {
                    Id = 1,
                    FirstName = "Giang",
                    MiddleName = "Michael",
                    LastName = "Phan",
                    Headline = "Developer",
                    PhoneNum = "123456789",
                    Email = "Gmphan7@gmail.com",
                    Country = "United States",
                    StreetAddress = "2192 Murry Trail",
                    City = "Morrow",
                    State = "GA",
                    ZipCode = "30260",
                    LinkedIn = "https://www.linkedin.com/in/giang-phan/",
                    GitHub = "https://github.com/gmphan",
                    CreatedDate = new DateTime(),
                    UpdatedDate = new DateTime()
                }
            );
            _ = modelBuilder.Entity<ResumeSummary>().HasData(
                new ResumeSummary
                {
                    Id = 1,
                    Summary = "short summary",
                    CreatedDate = new DateTime(),
                    UpdatedDate = new DateTime()
                }
            );
            _ = modelBuilder.Entity<ResumeExperience>().HasData(
                new ResumeExperience
                {
                    Id = 1,
                    Title = "Software Engineer",
                    Company = "Clayton State University",
                    Country = "United States",
                    City = "Morrow",
                    State = "GA",
                    ZipCode = "30260",
                    CurrentlyWorkHere = true,
                    FromMonth = "January",
                    ToMonth = "December",
                    FromYear = 2010,
                    ToYear = 2024,
                    CreatedDate = new DateTime(),
                    UpdatedDate = new DateTime()
                }
            );
            // Seed ResumeDescription and link to ResumeExperience with Id 1
            _ = modelBuilder.Entity<ResumeDescription>().HasData(
                new ResumeDescription
                {
                    Id = 1,
                    ResumeExperienceId = 1, // Foreign key reference
                    DescriptionText = "Developed various university applications."
                },
                new ResumeDescription
                {
                    Id = 2,
                    ResumeExperienceId = 1, // Foreign key reference
                    DescriptionText = "Led the team in adopting Agile methodologies."
                },
                new ResumeDescription
                {
                    Id = 3,
                    ResumeExperienceId = 1, // Foreign key reference
                    DescriptionText = "Implemented security enhancements for the internal network."
                }
            );
        }
    }
}