using Microsoft.EntityFrameworkCore;
using SkipAuth.Models;

namespace SkipAuth;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FileEntity> Files { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupStudent> GroupStudents { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=51.250.40.237;Database=database;Username=developer;Password=jDyu38dcxDO32;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("files_pkey");

            entity.ToTable("files");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.FileData).HasColumnName("file_data");
            entity.Property(e => e.RequestId).HasColumnName("request_id");

            entity.HasOne(d => d.Request).WithMany(p => p.Files)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("files_request_id_fkey");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("groups_pkey");

            entity.ToTable("groups");

            entity.HasIndex(e => e.Number, "groups_number_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Number).HasColumnName("number");
        });

        modelBuilder.Entity<GroupStudent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("group_students_pkey");

            entity.ToTable("group_students");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.StudentId).HasColumnName("student_id");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupStudents)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("group_students_group_id_fkey");

            entity.HasOne(d => d.Student).WithMany(p => p.GroupStudents)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("group_students_student_id_fkey");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("requests_pkey");

            entity.ToTable("requests");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Comment)
                .HasMaxLength(255)
                .HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatorId).HasColumnName("creator_id");
            entity.Property(e => e.DateEnd).HasColumnName("date_end");
            entity.Property(e => e.DateStart).HasColumnName("date_start");
            entity.Property(e => e.FileInDean)
                .HasDefaultValue(false)
                .HasColumnName("file_in_dean");
            entity.Property(e => e.ModeratorId).HasColumnName("moderator_id");
            entity.Property(e => e.Reason)
                .HasMaxLength(20)
                .HasColumnName("reason");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'PENDING'::character varying")
                .HasColumnName("status");

            entity.HasOne(d => d.Creator).WithMany(p => p.Requests)
                .HasForeignKey(d => d.CreatorId)
                .HasConstraintName("requests_creator_id_fkey");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity
                .HasKey(e => e.Id).HasName("tokens_pkey");
            
            entity.ToTable("tokens");

            entity.Property(e => e.ExpireAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expire_at");
            entity.Property(e => e.Token1)
                .HasMaxLength(255)
                .HasColumnName("token");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tokens_user_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Login)
                .HasMaxLength(30)
                .HasColumnName("login");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValueSql("'STUDENT'::character varying")
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
