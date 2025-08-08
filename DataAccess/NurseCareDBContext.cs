using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NurseCare.Model;

namespace NurseCare.DataAccess
{
    public class NurseCareDBContext:DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Patient> Patients { get; set; } 
        public DbSet<Nurse> Nurses { get; set; }
        public DbSet<ContactAddress> ContactAddresses { get; set; }
        public DbSet<UpdateBedInfo> UpdateBedInfos { get; set; }
        public DbSet<UpdateTeamInfo> UpdateTeamInfos { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<Bed> Beds { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {    
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("Server=31.187.75.26;Database=admin_IOTService;User Id=admin_kongsak;Password=26May@2522;Charset=utf8mb4;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Model.Person>().ToTable("Person");
            modelBuilder.Entity<Model.Patient>().ToTable("Patient");
            modelBuilder.Entity<Model.Nurse>().ToTable("Nurse");
            modelBuilder.Entity<Model.ContactAddress>().ToTable("ContactAddress");
            modelBuilder.Entity<Model.UpdateBedInfo>().ToTable("UpdateBedInfo");
            modelBuilder.Entity<Model.UpdateTeamInfo>().ToTable("UpdateTeamInfo");
            modelBuilder.Entity<Model.UserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<Model.Bed>().ToTable("Bed");
        }
    }
}
