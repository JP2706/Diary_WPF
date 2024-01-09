using Diary.Models.Configuration;
using Diary.Models.Domains;
using System;
using System.Data.Entity;
using System.Linq;

namespace Diary
{
    public class ApplicationDbContext : DbContext
    {
   
        public ApplicationDbContext()
            : base(ConnectionStringBuild())
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new StudentConfiguration());
            modelBuilder.Configurations.Add(new GroupConfiguration());
            modelBuilder.Configurations.Add(new RatingConfiguration());
        }

        private static string ConnectionStringBuild()
        {
            //Nazwa serwera bazy danych jest niepotrzebna poniewa¿ to tak naprawdê instancja bazydanych która mo¿na wpisaæ poprstu tak localhost/SQLEXPRESS
            return $"Server={Properties.Settings.Default.SqlServerName};Database={Properties.Settings.Default.SqlDataBaseName};User Id={Properties.Settings.Default.SqlLogin};Password={Properties.Settings.Default.SqlPassword};";
        }
    }

    
}