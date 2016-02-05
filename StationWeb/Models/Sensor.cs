using System;


namespace StationWeb.Models
{
    public class Sensor
    {
        public int ID { get; set; }
        public String Model { get; set; }
        //FK
        public int measureID { get; set; }
        //Navigation property
        public virtual Measure measure { get; set; }
    }
}
