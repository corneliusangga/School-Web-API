using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace School_Web_API.Models;

public partial class SchoolDatabaseContext : DbContext
{
    public SchoolDatabaseContext()
    {
    }

    public SchoolDatabaseContext(DbContextOptions<SchoolDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Departement> Departements { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultString");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Departement>(entity =>
        {
            entity.ToTable("Departement");

            entity.Property(e => e.DepartementId).HasColumnName("DepartementID");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Student");

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.DepartementId).HasColumnName("DepartementID");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Departement).WithMany(p => p.Students)
                .HasForeignKey(d => d.DepartementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_Departement");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
