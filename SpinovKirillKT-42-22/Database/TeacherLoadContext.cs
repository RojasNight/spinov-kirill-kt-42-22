using Microsoft.EntityFrameworkCore;
using SpinovKirillKT_42_22.Database.Configurations;
using SpinovKirillKT_42_22.Models;

namespace SpinovKirillKT_42_22.Database
{
    public class TeacherLoadContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<AcademicDegree> AcademicDegrees { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<Load> Loads { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new TeacherConfiguration());
            modelBuilder.ApplyConfiguration(new AcademicDegreeConfiguration());
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new DisciplineConfiguration());
            modelBuilder.ApplyConfiguration(new LoadConfiguration());
        }

        public TeacherLoadContext(DbContextOptions<TeacherLoadContext> options) : base(options)
        {
        }
    }
}
