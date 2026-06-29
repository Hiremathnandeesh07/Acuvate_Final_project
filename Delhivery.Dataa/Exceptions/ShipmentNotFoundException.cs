using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delhivery.Data.Exceptions
{
    public class ShipmentNotFoundException : Exception
    {
        public ShipmentNotFoundException()
            : base("Shipment not found.")
        {
        }

        public ShipmentNotFoundException(string message)
            : base(message)
        {
        }


        // here i can use the second constructure to give my own
        // exceptions instead of giving 'shipment not found' always
    }
}
