using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StarterApp.Database.Models;

namespace StarterApp.Database.Data;

public class AppDbContext : DbContext
{

    public AppDbContext()
    { }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var a = Assembly.GetExecutingAssembly();
        // var resources = a.GetManifestResourceNames();
        using var stream = a.GetManifestResourceStream("StarterApp.Database.appsettings.json");

        var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();

        optionsBuilder.UseSqlServer(
            config.GetConnectionString("DevelopmentConnection")
        );
    }

    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Conference> Conferences { get; set; }
    public DbSet<UserConference> UserConferences { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.PasswordSalt).HasMaxLength(255);
        });

        // Configure Role entity
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        // Configure UserRole entity
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasIndex(e => new { e.UserId, e.RoleId }).IsUnique();

            entity.HasOne(ur => ur.User)
                  .WithMany(u => u.UserRoles)
                  .HasForeignKey(ur => ur.UserId);

            entity.HasOne(ur => ur.Role)
                  .WithMany(r => r.UserRoles)
                  .HasForeignKey(ur => ur.RoleId);
        });

        // Configure Conference entity
        modelBuilder.Entity<Conference>(entity =>
        {
            entity.HasOne(c => c.Speaker)
                .WithMany() 
                .HasForeignKey(c => c.SpeakerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure UserConference entity
        modelBuilder.Entity<UserConference>(entity =>
        {
            entity.HasKey(uc => new { uc.UserId, uc.ConferenceId });

            entity.HasOne(uc => uc.User)
                .WithMany(u => u.UserConferences)
                .HasForeignKey(uc => uc.UserId);

            entity.HasOne(uc => uc.Conference)
                .WithMany(c => c.UserConferences)
                .HasForeignKey(uc => uc.ConferenceId);
        });

    }

}