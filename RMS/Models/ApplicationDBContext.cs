using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace RMS.Models
{
    public class ApplicationDbContext : IdentityUser
    {
        public ApplicationDbContext() : base("name=RMSEntities")
        {


        }
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<Adminler> Adminler { get; set; }
        public virtual DbSet<Masalar> Masalar { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Personeller> Personeller { get; set; }
        public virtual DbSet<Siparisler> Siparisler { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}