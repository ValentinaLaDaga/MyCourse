using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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

            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                    /* entity.Property(e => e.Id).ValueGeneratedNever();

                     entity.Property(e => e.Description).HasColumnType("TEXT (10000)");

                     entity.Property(e => e.Duration)
                         .IsRequired()
                         .HasColumnType("TEXT (8)")
                         .HasDefaultValueSql("'00:00:00'");

                     entity.Property(e => e.Title)
                         .IsRequired()
                         .HasColumnType("TEXT (100)");

                     entity.HasOne(d => d.Course)
                         .WithMany(p => p.Lessons)
                         .HasForeignKey(d => d.CourseId); */

            });
            
        }//end method OnModelCreating
    }//end class
}
