using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Config
{
    public class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseMySqlIdentityColumn();

            builder.Property(n => n.DepartmentName)
            .IsRequired()
            .HasMaxLength(250);

            builder.Property(n => n.Description)
            .IsRequired(false)
            .HasMaxLength(250);

            builder.HasData(new List<Department>()
            {
                new Department
                {
                    Id = 1,
                    DepartmentName = "ECE",
                    Description = "ECE"
                },
                new Department
                {
                    Id = 2,
                    DepartmentName = "CSE",
                    Description = "CSE"
                }
            });
        }
    }
}
