using EF_Assignment02.Configurations;
using EF_Assignment02.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Assignment02.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Organizer> Organizers { get; set; }

        public DbSet<OrganizerProfile> OrganizerProfiles { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Attendee> Attendees { get; set; }

        public DbSet<Badge> Badges { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=DESKTOP-3QRHU6G\\SQLEXPRESS;Database=EFAssignment02;Trusted_Connection=True;TrustServerCertificate=True;"
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration(new OrganizerProfileConfiguration());
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Event>()
                .HasOne(e => e.Organizer)
                .WithMany(o => o.Events)
                .HasForeignKey(e => e.OrganizerId)
                .IsRequired();

            modelBuilder.Entity<Badge>()
    .HasOne(b => b.Attendee)
    .WithOne(a => a.Badge)
    .HasForeignKey<Badge>(b => b.AttendeeId)
    .IsRequired();

            modelBuilder.Entity<Badge>()
                .HasIndex(b => b.BadgeNumber)
                .IsUnique();

            modelBuilder.Entity<Badge>()
    .ToTable(b => b.HasCheckConstraint(
        "CK_Badge_Tier",
        "[Tier] IN ('Standard', 'VIP')"
    ));

        }
    }
}
