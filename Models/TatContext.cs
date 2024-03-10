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

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Product1> Products1 { get; set; }

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
                .HasConstraintName("FK__Locations__Physi__403A8C7D");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Locations__UserI__3F466844");
        });

        modelBuilder.Entity<Preference>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Preferences", "users");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Preferenc__UserI__4222D4EF");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC076BBB1524");

            entity.ToTable("Products", "inventory");

            entity.Property(e => e.Cost).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.InvFlavor).HasMaxLength(100);
            entity.Property(e => e.InvShape).HasMaxLength(100);
            entity.Property(e => e.InvSize).HasMaxLength(100);
        });

        modelBuilder.Entity<Product1>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Products", "users");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Products__UserId__440B1D61");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__States__3214EC07117E6E2D");

            entity.ToTable("States", "lookups");

            entity.Property(e => e.StateAbbreviation)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.StateName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07D73FF19A");

            entity.ToTable("Users", "users");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053443CAC171").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(32)
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
