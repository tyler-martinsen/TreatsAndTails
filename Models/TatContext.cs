using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TreatsAndTails.Models;

public partial class TatContext : DbContext
{
    public TatContext()
    {
    }

    public TatContext(DbContextOptions<TatContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Preference> Preferences { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=TAT;Trusted_Connection=True;Encrypt=false;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Locations", "users");

            entity.Property(e => e.PhysicalAddressCity).HasMaxLength(255);
            entity.Property(e => e.PhysicalAddressLine1).HasMaxLength(255);
            entity.Property(e => e.PhysicalAddressLine2).HasMaxLength(255);
            entity.Property(e => e.PhysicalAddressZip).HasMaxLength(5);

            entity.HasOne(d => d.PhysicalAddressStateNavigation).WithMany()
                .HasForeignKey(d => d.PhysicalAddressState)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Locations__Physi__3E52440B");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Locations__UserI__3D5E1FD2");
        });

        modelBuilder.Entity<Preference>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Preferences", "users");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Preferenc__UserI__403A8C7D");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__States__3214EC074B8553FD");

            entity.ToTable("States", "lookups");

            entity.Property(e => e.StateAbbreviation)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.StateName)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC074DFB86FF");

            entity.ToTable("Users", "users");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534A78B7DED").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
