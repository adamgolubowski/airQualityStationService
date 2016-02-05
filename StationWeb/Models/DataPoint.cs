using System;

namespace StationWeb.Models
{
    public class DataPoint
    {
        public int ID { get; set; }
        public DateTime TimeStamp { get; set; }
        public Double Value { get; set; }
        //FK
        public int equipmentID { get; set; }
    }
}