using JwtApplication.Data.models;
using Microsoft.EntityFrameworkCore;

namespace JwtApplication.Data
{
    public class DatabaseContext : DbContext
    {
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserInfo> UserInfos { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Token> Token { get; set; }
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Token>(entity =>
            {
                entity.ToTable("Token");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AccessToken);
                entity.Property(e => e.RefreshToken);
                entity.Property(e => e.CreatedTime);
                entity.Property(e => e.ExpiredTime);


            });
            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.ToTable("UserInfo");
                entity.Property(e => e.DisplayName).HasMaxLength(50);
                entity.Property(e => e.UserName).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Password).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.CreatedDate);

            }
            );

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Description);
            });

            modelBuilder.Entity<UserRole>().HasKey(e => new { e.UserId, e.RoleId });
            modelBuilder.Entity<UserRole>()
                        .HasOne<UserInfo>(ur => ur.UserInfo)
                        .WithMany(u => u.UserRoles)
                        .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                        .HasOne<Role>(ur => ur.Role)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.RoleId);

            base.OnModelCreating(modelBuilder);
        }


    }
}
