using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Config
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseMySqlIdentityColumn();

            builder.Property(e => e.studentName)
            .IsRequired()
            .HasMaxLength(250);
            builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(250);
            builder.Property(e => e.Address)
            .IsRequired(false)
            .HasMaxLength(450);

            builder.HasData(new List<Student>()
            {
                new Student
                {
                    Id = 1,
                    studentName = "YoungWxrdie",
                    Address = "Hanoi",
                    Email = "xnxx@gmail.com",
                    DOB = new DateTime(2000, 1, 1)
                },
                new Student
                {
                    Id = 2,
                    studentName = "OldMCK",
                    Address = "Hanoi",
                    Email = "xvideos@gmail.com",
                    DOB = new DateTime(2001, 1, 1)
                }
            });
        }
    }
}
