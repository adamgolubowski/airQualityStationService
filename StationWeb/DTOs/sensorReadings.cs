using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StationWeb.Models;

namespace StationWeb.DTOs
{
    public class SensorReadings : SensorDetails
    {
        public List<DataPoint> readings { get; set; }
    }
}