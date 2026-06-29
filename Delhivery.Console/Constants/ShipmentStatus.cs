using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delhivery.Console.Constants
{
    internal class ShipmentStatus
    {
        public const string Booked = "Booked";
        public const string InTransit = "In Transit";
        public const string OutForDelivery = "Out for Delivery";
        public const string Delivered = "Delivered";
        public const string RTO = "RTO";

        public static readonly List<string> AllStatuses = new()
        {
            Booked,
            InTransit,
            OutForDelivery,
            Delivered,
            RTO
        };
    }
}
