using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MyCourse.Models.Entities;

namespace MyCourse.Models.Entities
{
    public partial class MyCourseDbContext : DbContext
    {
        public MyCourseDbContext()
        {
        }

        public MyCourseDbContext(DbContextOptions<MyCourseDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlite("Data Source=Data/MyCourse.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Courses"); //Superfluo se la tabella si chiama come la proprietà che espone il DbSet
                entity.HasKey(course => course.Id); //Superfluo se la proprietà si chiama Id oppure CoursesId
                //entity.HasKey(course => new { course.Id, course.Author }); //Per chiavi primarie composite (è importante rispettare l'ordine dei campi)
                entity.OwnsOne(course => course.CurrentPrice, builder =>
                {
                    builder.Property(money => money.Currency)
                    .HasConversion<string>()
                    .HasColumnName("CurrentPrice_Currency"); //Questo è superfluo perché le nostre colonne seguono già la convenzione di nomi
                    builder.Property(money => money.Amount).HasColumnName("CurrentPrice_Amount"); //Questo è superfluo perché le nostre colonne seguono già la convenzione di nomi
                });
                    
                    entity.OwnsOne(course => course.FullPrice, builder =>
                    {
                        builder.Property(money => money.Currency).HasConversion<string>();
                    });

                    //Andiamo a settare la relazione 1 a n tra l'entità corso e l'entità lezione
                    entity.HasMany(course => course.Lessons).WithOne(lesson => lesson.Course).HasForeignKey(lesson => lesson.CourseId);

            });


            //andiamo a settare la relazione n a 1 tra l'entity Lesson e l'entity Course
            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.HasOne(lesson => lesson.Course).WithMany(course => course.Lessons);

            });
            
        }//end method OnModelCreating
    }//end class
}
