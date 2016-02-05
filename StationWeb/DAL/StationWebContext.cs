using StationWeb.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace StationWeb.DAL
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class StationWebContext:DbContext
    {
        public StationWebContext() : base("StationWebDB")
        {
        }

        public DbSet<Station> Stations { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<DataPoint> DataPoints { get; set; }
        public DbSet<Measure> Measure { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}