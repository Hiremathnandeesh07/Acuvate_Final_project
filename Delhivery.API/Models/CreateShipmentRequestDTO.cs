namespace Delhivery.API.Models
{
    public class CreateShipmentRequestDTO
    {
        public string AWBNumber { get; set; } = "";

        public string SenderName { get; set; } = "";

        public string ReceiverName { get; set; } = "";

        public string Origin { get; set; } = "";

        public string Destination { get; set; } = "";

        public decimal WeightKg { get; set; }

        //public string Status { get; set; } = "";
    }
}
