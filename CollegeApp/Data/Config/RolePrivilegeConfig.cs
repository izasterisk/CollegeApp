using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Config
{
    public class RolePrivilegeConfig : IEntityTypeConfiguration<RolePrivilege>
    {
        public void Configure(EntityTypeBuilder<RolePrivilege> builder)
        {
            builder.ToTable("RolePrivileges");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseMySqlIdentityColumn();

            builder.Property(n => n.RolePrivilegeName).HasMaxLength(250).IsRequired();
            builder.Property(n => n.Description);
            builder.Property(n => n.IsActive).IsRequired();
            builder.Property(n => n.IsDeleted).IsRequired();
            builder.Property(n => n.CreatedDate).IsRequired();

            builder.HasOne(x => x.Role).WithMany(x => x.RolePrivileges).HasForeignKey(x => x.RoleId)
                .HasConstraintName("FK_RolePrivileges_Roles");
        }
    }
}
