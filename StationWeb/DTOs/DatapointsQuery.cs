using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationWeb.DTOs
{
    public class DatapointsQuery
    {
        public int ID { get; set; }
        public DateTime TimeStamp { get; set; }
        public Double Value { get; set; }
        public int equipmentID { get; set; }
        public String Model { get; set; }
        public String measureType { get; set; }
    }
}