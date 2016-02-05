using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationWeb.DTOs
{
    public class SensorDetails
    {
        public int equipmentID { get; set; }
        public String Model { get; set; }
        public String measureType { get; set; }
    }
}