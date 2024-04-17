using ScheduleBTEC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ScheduleBTEC.Context
{
    public class ConnectDB : DbContext
    {
        public ConnectDB() { }
        public ConnectDB(DbContextOptions<ConnectDB> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyStoreDB"));
        }
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<ClassEntity> ClassEntities { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Learn> Learns { get; set; }
        public virtual DbSet<Study> Studys { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Teach> Teaches { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Study>()
                .HasOne(tb => tb.Users)
                .WithMany(h => h.Study)
                .HasForeignKey(tb => tb.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Attendance>()
                .HasOne(tb => tb.Users)
                .WithMany(h => h.Attendances)
                .HasForeignKey(tb => tb.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
