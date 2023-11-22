using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore;

namespace Awesome.AI.Web.copenhagenai;

public partial class CopenhagenaiContext : DbContext
{
    public CopenhagenaiContext()
    {
    }

    public CopenhagenaiContext(DbContextOptions<CopenhagenaiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Doc> Docs { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=xxxx;port=xxxx;database=xxxx;user=xxxx;password=xxxx", 
            ServerVersion.AutoDetect("server=xxxx;port=xxxx;database=xxxx;user=xxxx;password=xxxx"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("docs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Header)
                .HasMaxLength(100)
                .HasColumnName("header");
            entity.Property(e => e.Sort).HasColumnName("sort");
            entity.Property(e => e.Text)
                .HasMaxLength(5000)
                .HasColumnName("text");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("section");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Header)
                .HasMaxLength(100)
                .HasColumnName("header");
            entity.Property(e => e.Sort).HasColumnName("sort");
            entity.Property(e => e.Subheader)
                .HasMaxLength(200)
                .HasColumnName("subheader");
            entity.Property(e => e.Teaser)
                .HasMaxLength(1000)
                .HasColumnName("teaser");
            entity.Property(e => e.Text)
                .HasMaxLength(5000)
                .HasColumnName("text");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
