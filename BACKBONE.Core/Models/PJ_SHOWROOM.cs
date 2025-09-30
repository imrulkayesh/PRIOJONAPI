using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BACKBONE.Core.Models
{
    internal class PJ_SHOWROOM
    {
        public string SHOWROOM_ID { get; set; }
        public string SHOWROOM_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string LOCATION_LAT { get; set; }
        public string LOCATION_LONG { get; set; }
        public string THANA_ID { get; set; }
        public string BU_ID { get; set; }
        public string CDU { get; set; }
        public DateTime CDT { get; set; }
        public int ACT { get; set; }
    }
}
