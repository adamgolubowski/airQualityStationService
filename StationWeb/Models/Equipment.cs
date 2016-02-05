
using System.Collections.Generic;

namespace StationWeb.Models
{
    public class Equipment
    {
        public int ID { get; set; }
        //FK
        public int StationID { get; set; }
        //Navigation property
        //public Station station { get; set; }
        //FK
        public int SensorID { get; set; }
        //Navigation property
        public virtual Sensor sensor { get; set; }
        //Navigation property
        public virtual ICollection<DataPoint> datapoint { get; set; }

    }
}