using System;
using System.Collections.Generic;
using System.Linq;

namespace StationWeb.Models
{

    public class Station
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Double LocLongitude { get; set; }
        public Double LocLattitude { get; set; }
        public string Key { get; set; }
        //Navigation property
        public virtual ICollection<Equipment> equipment { get; set; }
    }
}


