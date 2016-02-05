namespace StationWeb.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using StationWeb.Models;
    using System.Collections.Generic;
    internal sealed class Configuration : DbMigrationsConfiguration<StationWeb.DAL.StationWebContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(StationWeb.DAL.StationWebContext context)
        {
            var measureTypes = new List<Measure>()
            {
                new Measure {Type ="Temp C" },
                new Measure {Type="CO" },
                new Measure {Type="O3" },
                new Measure {Type="Dust" },
                new Measure {Type="Humidity" }
            };
            measureTypes.ForEach(m => context.Measure.Add(m));
            context.SaveChanges();

            var sensors = new List<Sensor>(){
                new Sensor{Model="MQ-131",measureID=measureTypes.Single(m=>m.Type=="O3").ID },
                new Sensor{Model="MQ-9",measureID=measureTypes.Single(m=>m.Type=="CO").ID },
                new Sensor{Model="GP2Y1010AU0F",measureID=measureTypes.Single(m=>m.Type=="Dust").ID },
                new Sensor{Model="DHT-21 T",measureID=measureTypes.Single(m=>m.Type=="Temp C").ID },
                new Sensor{Model="DHT-21 H",measureID=measureTypes.Single(m=>m.Type=="Humidity").ID }
            };
            sensors.ForEach(s => context.Sensors.Add(s));
            context.SaveChanges();


            var stations = new List<Station>()
            {
                new Station{
                    Name ="Bialystok-Centrum",
                    LocLattitude =53.134699,
                    LocLongitude =23.157905,
                    Key ="KLHG6QKLGCEHTWKGNQJ2PDYL68GQWHBN"
                }
            };
            stations.ForEach(s => context.Stations.Add(s));
            context.SaveChanges();
            
            var equipments = new List<Equipment>()
            {
                new Equipment{
                    StationID =stations.Single(s=>s.Name=="Bialystok-Centrum").ID,
                    SensorID=sensors.Single(s=>s.Model=="MQ-9").ID
                },
                new Equipment{
                    StationID =stations.Single(s=>s.Name=="Bialystok-Centrum").ID,
                    SensorID=sensors.Single(s=>s.Model=="MQ-131").ID
                },
                new Equipment{
                    StationID =stations.Single(s=>s.Name=="Bialystok-Centrum").ID,
                    SensorID=sensors.Single(s=>s.Model=="GP2Y1010AU0F").ID
                },
                new Equipment{
                    StationID =stations.Single(s=>s.Name=="Bialystok-Centrum").ID,
                    SensorID=sensors.Single(s=>s.Model=="DHT-21 T").ID
                },
                new Equipment{
                    StationID =stations.Single(s=>s.Name=="Bialystok-Centrum").ID,
                    SensorID=sensors.Single(s=>s.Model=="DHT-21 H").ID
                }
            };
            equipments.ForEach(e => context.Equipments.Add(e));
            context.SaveChanges();
        }
    }
}
