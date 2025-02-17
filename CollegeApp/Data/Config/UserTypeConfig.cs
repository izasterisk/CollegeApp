using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Config
{
    public class UserTypeConfig : IEntityTypeConfiguration<UserType>
    {
        public void Configure(EntityTypeBuilder<UserType> builder)
        {
            builder.ToTable("UserTypes");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseMySqlIdentityColumn();

            builder.Property(n => n.Name).IsRequired().HasMaxLength(250);
            builder.Property(n => n.Description).HasMaxLength(1500);

            builder.HasData(new List<UserType>()
            {
                new UserType
                {
                    Id = 1,
                    Name = "Student",
                    Description = "Student User"
                },
                new UserType
                {
                    Id = 2,
                    Name = "Faculty",
                    Description = "Student User"
                },
                new UserType
                {
                    Id = 3,
                    Name = "Supporting Staff",
                    Description = "Supporting Staff User"
                },
                new UserType
                {
                    Id = 4,
                    Name = "Parents",
                    Description = "Parent User"
                }
            });
        }
    }
}
