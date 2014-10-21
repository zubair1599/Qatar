using QatarRacing.EF.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QatarRacing.EF
{
    public class RaceContext  : DbContext
    {   public DbSet<Runner> Runners { get; set; }
        
        public DbSet<WinningPrice> WinningPrizes { get; set; }

        public DbSet<Race> Races { get; set; }

        public DbSet<Link> Links { get; set; }

        public DbSet<Jockey> Jockeys { get; set; }


        public RaceContext()
            : base("defaultConnStr")
        {
    
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<Race>().HasKey(m => m.RaceID);
            modelBuilder.Entity<WinningPrice>().HasKey(m => m.WinningPriceID);
            modelBuilder.Entity<Runner>().HasKey(m => m.RunnerID);
            modelBuilder.Entity<Link>().HasKey(m => m.LinkID);
            modelBuilder.Entity<Jockey>().HasKey(m => m.JockeyID);

            modelBuilder.Entity<Link>().Property(s => s.LinkID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Race>().Property(s => s.RaceID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<WinningPrice>().Property(s => s.WinningPriceID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Runner>().Property(s => s.RunnerID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Jockey>().Property(s => s.JockeyID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //one to zeer one
            modelBuilder.Entity<Link>().HasMany(m => m.Races).WithRequired(m => m.Link);



            
            modelBuilder.Entity<Race>().HasMany(m => m.Runners).WithRequired(m => m.Race);



            modelBuilder.Entity<Jockey>().HasMany(m => m.Runners).WithRequired(m => m.Jockey);

            modelBuilder.Entity<Race>().HasMany(m => m.WinningPrices).WithRequired(m => m.Race);


            base.OnModelCreating(modelBuilder);

            
        }
    }
}
