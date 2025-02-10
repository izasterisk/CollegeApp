using CollegeApp.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data
{
    public class CollegeDBContext : DbContext
    {
        public CollegeDBContext(DbContextOptions<CollegeDBContext> options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Student>().HasData(new List<Student>()
            //{
            //    new Student
            //    {
            //        Id=1,
            //        studentName="YoungWxrdie",
            //        Address="Hanoi",
            //        Email="xnxx@gmail.com",
            //        DOB= new DateTime(2000, 1, 1)
            //    },
            //    new Student
            //    {
            //        Id=2,
            //        studentName="OldMCK",
            //        Address="Hanoi",
            //        Email="xvideos@gmail.com",
            //        DOB= new DateTime(2001, 1, 1)
            //    }
            //});
            //modelBuilder.Entity<Student>(entity =>
            //{
            //    entity.Property(e => e.studentName)
            //    .IsRequired()
            //    .HasMaxLength(250);
            //    entity.Property(e => e.Email)
            //    .IsRequired()
            //    .HasMaxLength(250);
            //    entity.Property(e => e.Address)
            //    .IsRequired(false)
            //    .HasMaxLength(450);
            //});

            modelBuilder.ApplyConfiguration(new StudentConfig());
        }
    }
}
