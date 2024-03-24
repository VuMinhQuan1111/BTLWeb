using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BTLWeb.Models;

public partial class BtlwebContext : DbContext
{
    public BtlwebContext()
    {
    }

    public BtlwebContext(DbContextOptions<BtlwebContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblCategory> TblCategories { get; set; }

    public virtual DbSet<TblComment> TblComments { get; set; }

    public virtual DbSet<TblFavorite> TblFavorites { get; set; }

    public virtual DbSet<TblPost> TblPosts { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-RPK6PAD\\SQLEXPRESS;Database=BTLWeb;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__tblCateg__D54EE9B4ECAF2B3F");

            entity.ToTable("tblCategory");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryDescription)
                .HasMaxLength(100)
                .HasColumnName("category_description");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<TblComment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__tblComme__E79576870678AD55");

            entity.ToTable("tblComment");

            entity.Property(e => e.CommentId)
                .ValueGeneratedNever()
                .HasColumnName("comment_id");
            entity.Property(e => e.CommentText)
                .HasMaxLength(50)
                .HasColumnName("comment_text");
            entity.Property(e => e.CreateAt).HasColumnName("create_at");
            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.UsersId).HasColumnName("users_id");

            entity.HasOne(d => d.Post).WithMany(p => p.TblComments)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblCommen__post___32AB8735");

            entity.HasOne(d => d.Users).WithMany(p => p.TblComments)
                .HasForeignKey(d => d.UsersId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblCommen__users__339FAB6E");
        });

        modelBuilder.Entity<TblFavorite>(entity =>
        {
            entity.HasKey(e => e.FavoriteId).HasName("PK__tblFavor__46ACF4CBE5C51E17");

            entity.ToTable("tblFavorite");

            entity.Property(e => e.FavoriteId)
                .ValueGeneratedNever()
                .HasColumnName("favorite_id");
            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.UsersId).HasColumnName("users_id");

            entity.HasOne(d => d.Post).WithMany(p => p.TblFavorites)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK__tblFavori__post___367C1819");

            entity.HasOne(d => d.Users).WithMany(p => p.TblFavorites)
                .HasForeignKey(d => d.UsersId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblFavori__users__37703C52");
        });

        modelBuilder.Entity<TblPost>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__tblPost__3ED787667901BB0C");

            entity.ToTable("tblPost");

            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.PostAuthor)
                .HasMaxLength(50)
                .HasColumnName("post_author");
            entity.Property(e => e.PostContent)
                .HasMaxLength(500)
                .HasColumnName("post_content");
            entity.Property(e => e.PostCreateAt).HasColumnName("post_create_at");
            entity.Property(e => e.PostImg)
                .HasMaxLength(50)
                .HasColumnName("post_img");
            entity.Property(e => e.PostTitle)
                .HasMaxLength(100)
                .HasColumnName("post_title");
            entity.Property(e => e.UsersId).HasColumnName("users_id");

            entity.HasOne(d => d.Category).WithMany(p => p.TblPosts)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblPost__categor__2FCF1A8A");

            entity.HasOne(d => d.Users).WithMany(p => p.TblPosts)
                .HasForeignKey(d => d.UsersId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblPost__users_i__2EDAF651");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.UsersId).HasName("PK__tblUsers__EAA7D14B57460696");

            entity.ToTable("tblUsers");

            entity.Property(e => e.UsersId).HasColumnName("users_id");
            entity.Property(e => e.UsersEmail)
                .HasMaxLength(50)
                .HasColumnName("users_email");
            entity.Property(e => e.UsersName)
                .HasMaxLength(50)
                .HasColumnName("users_name");
            entity.Property(e => e.UsersPass)
                .HasMaxLength(50)
                .HasColumnName("users_pass");
            entity.Property(e => e.UsersRole)
                .HasMaxLength(50)
                .HasColumnName("users_role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
