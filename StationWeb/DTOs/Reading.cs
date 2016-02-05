using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationWeb.DTOs
{
    //this class represents readings sent from station.
    public class Reading
    {
        public DateTime TimeStamp { get; set; }
        public Double Value { get; set; }
        public int equipmentID { get; set; }
        public String Key { get; set; }
    }
}