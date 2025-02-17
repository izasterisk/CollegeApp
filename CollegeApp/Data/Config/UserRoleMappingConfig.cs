using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Config
{
    public class UserRoleMappingConfig : IEntityTypeConfiguration<UserRoleMapping>
    {
        public void Configure(EntityTypeBuilder<UserRoleMapping> builder)
        {
            builder.ToTable("UserRoleMappings");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseMySqlIdentityColumn();

            builder.HasIndex(x => new { x.UserId, x.RoleId }, "UK_UserRoleMapping").IsUnique();

            builder.Property(n => n.UserId).IsRequired();
            builder.Property(n => n.RoleId).IsRequired();

            builder.HasOne(x => x.Role).WithMany(x => x.UserRoleMappings).HasForeignKey(x => x.RoleId)
                .HasConstraintName("FK_UserRoleMapping_Role");
            builder.HasOne(x => x.User).WithMany(x => x.UserRoleMappings).HasForeignKey(x => x.UserId)
                .HasConstraintName("FK_UserRoleMapping_User");
        }
    }
}
