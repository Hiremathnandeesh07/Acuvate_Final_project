using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delhivery.Console.Models
{
    internal class Shipment
    {
        public int ShipmentId { get; set; }
        public string AWBNumber { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double WeightKg { get; set; }
        public string Status { get; set; }
        public DateTime BookedAt { get; set; }
    }

    
}
