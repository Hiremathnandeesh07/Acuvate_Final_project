using Delhivery.Console.Constants;
using Delhivery.Console.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delhivery.Console.Services
{
    internal class ShipmentService
    {
        private readonly List<Shipment> shipments = new List<Shipment>();
        // i used readonly because It means this variable cannot point to another list later.
        private int nextShipmentId = 1;
        public bool BookShipment(Shipment shipment,out string message)
        {
            if (string.IsNullOrWhiteSpace(shipment.AWBNumber))
            {
                message = "AWB Number cannot be empty.";
                return false;
            }

            if (shipments.Any(s =>
                          s.AWBNumber.Equals(shipment.AWBNumber, StringComparison.OrdinalIgnoreCase)))
            {
                message = "AWB Number already exists.";
                return false;
            }

            if (shipment.WeightKg <= 0)
            {
                message = "Weight must be greater than zero.";
                return false;
            }

            if (!ShipmentStatus.AllStatuses.Contains(shipment.Status))
            {
                message = "Invalid shipment status.";
                return false;
            }

            shipment.ShipmentId = nextShipmentId++;
            shipment.BookedAt = DateTime.Now;

            shipments.Add(shipment);
            message = "Shipment booked successfully.";
            return true;

        }
        public List<Shipment> ListShipments()
        {
            return new List<Shipment>(shipments);
        }

        public bool UpdateStatus(string awb,string newStatus,out string message)
        {

            // CAN BE WRITTEN THIS WAY ALSO
            //Shipment shipment = null;

            //foreach (Shipment s in shipments)
            //{
            //    if (s.AWBNumber.Equals(awb, StringComparison.OrdinalIgnoreCase))
            //    {
            //        shipment = s;
            //        break;
            //    }
            //}


            Shipment shipment = shipments.FirstOrDefault(s =>s.AWBNumber.Equals(awb, StringComparison.OrdinalIgnoreCase));

            if (shipment == null)
            {
                message = "Shipment not found.";
                return false;
            }

            if (!ShipmentStatus.AllStatuses.Contains(newStatus))
            {
                message = "Invalid status.";
                return false;
            }

            shipment.Status = newStatus;

            message = "Status updated successfully.";
            return true;

        }
        public bool CancelShipment(string awb,out string message)
        {
            Shipment shipment = shipments.FirstOrDefault(s =>
               s.AWBNumber.Equals(awb, StringComparison.OrdinalIgnoreCase));

            if (shipment == null)
            {
                message = "Shipment not found.";
                return false;
            }

            shipments.Remove(shipment);

            message = "Shipment cancelled successfully.";
            return true;
        }


    }
}
