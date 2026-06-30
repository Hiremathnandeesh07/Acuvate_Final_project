using Delhivery.API.Models;
using Delhivery.Data.Exceptions;
using Delhivery.Data.Models;
using Delhivery.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Delhivery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentsController : ControllerBase
    {
        private readonly ShipmentRepository _repository;
        public ShipmentsController(ShipmentRepository repository)
        {
            _repository = repository;
        }


        // get : api/shipments
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_repository.GetAll());
        }

        //get : api/shipments/{awb}
        [HttpGet("{awb}")]
        public IActionResult GetByAWB(string awb)
        {
            try
            {
                Shipment shipment = _repository.GetByAWB(awb);
                return Ok(shipment);
            }
            catch (ShipmentNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
                
            }
        }

        // POST : api/shipments

        [HttpPost]
        public IActionResult Book([FromBody] CreateShipmentRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.AWBNumber) || request.AWBNumber.Trim().ToLower()=="string")
                return BadRequest("AWB Number is required.");

            if (string.IsNullOrWhiteSpace(request.SenderName) ||
    request.SenderName.Trim().ToLower() == "string")
            {
                return BadRequest("Sender Name is required.");
            }

            if (!Regex.IsMatch(request.SenderName, @"^[A-Za-z ]+$"))
            {
                return BadRequest("Sender Name should contain only letters.");
            }

            if (string.IsNullOrWhiteSpace(request.ReceiverName) ||
    request.ReceiverName.Trim().ToLower() == "string")
            {
                return BadRequest("Receiver Name is required.");
            }

            if (!Regex.IsMatch(request.ReceiverName, @"^[A-Za-z ]+$"))
            {
                return BadRequest("Receiver Name should contain only letters.");
            }

            if (string.IsNullOrWhiteSpace(request.Origin) ||
    request.Origin.Trim().ToLower() == "string")
            {
                return BadRequest("Origin is required.");
            }

            if (!Regex.IsMatch(request.Origin, @"^[A-Za-z ]+$"))
            {
                return BadRequest("Origin should contain only letters.");
            }

            if (string.IsNullOrWhiteSpace(request.Destination) ||
      request.Destination.Trim().ToLower() == "string")
            {
                return BadRequest("Destination is required.");
            }

            if (!Regex.IsMatch(request.Destination, @"^[A-Za-z ]+$"))
            {
                return BadRequest("Destination should contain only letters.");
            }

            if (request.WeightKg <= 0)
                return BadRequest("Weight must be greater than zero.");

            Shipment shipment = new Shipment
            {
                AWBNumber = request.AWBNumber,
                SenderName = request.SenderName,
                ReceiverName = request.ReceiverName,
                Origin = request.Origin,
                Destination = request.Destination,
                WeightKg = request.WeightKg,
            };

            Shipment bookedShipment = _repository.Book(shipment);

            return CreatedAtAction(
                nameof(GetByAWB),
                new { awb = bookedShipment.AWBNumber },
                bookedShipment);
        }


        // PUT : api/shipments/{awb}/status

        [HttpPut("{awb}/status")]
        public IActionResult UpdateStatus(string awb, [FromBody] UpdateStatusRequest request)
        {
            string[] validStatuses =
                        {
                            ShipmentStatus.Booked,
                            ShipmentStatus.InTransit,
                            ShipmentStatus.OutForDelivery,
                            ShipmentStatus.Delivered,
                            ShipmentStatus.RTO
                        };

            if (!validStatuses.Contains(request.Status))
            {
                return BadRequest(new
                {
                    Message = "Invalid Status.",
                    ValidStatuses = validStatuses
                });
            }

            try
            {
                Shipment updatedShipment = _repository.UpdateStatus(awb, request.Status);

                return Ok(updatedShipment);
            }
            catch (ShipmentNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        // get : api/shipments.stats

        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            List<Shipment> shipments = _repository.GetAll();

            var stats = new
            {
                Booked = shipments.Count(s => s.Status == "Booked"),
                InTransit = shipments.Count(s => s.Status == "In Transit"),
                OutForDelivery = shipments.Count(s => s.Status == "Out for Delivery"),
                Delivered = shipments.Count(s => s.Status == "Delivered"),
                RTO = shipments.Count(s => s.Status == "RTO")
            };

            return Ok(stats);
        }

        // delete : api/shipments/{id}
        [HttpDelete("{id}")]
        public IActionResult Cancel(int id)
        {
            try
            {
                _repository.Cancel(id);

                return NoContent();
            }
            catch (ShipmentNotFoundException ex)
            {
                return NotFound(new
                {
                    Message = ex.Message
                });
            }
        }


    }
}
