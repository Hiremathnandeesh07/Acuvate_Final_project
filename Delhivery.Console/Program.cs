// See https://aka.ms/new-console-template for more information
using Delhivery.Console.Constants;
using Delhivery.Console.Models;
using Delhivery.Console.Services;

Console.WriteLine("Hello, World!");

ShipmentService shipmentService = new ShipmentService();

bool exit = false;

while (!exit)
{
    Console.Clear();

    Console.WriteLine("========================================");
    Console.WriteLine(" DELHIVERY SHIPMENT MANAGEMENT SYSTEM");
    Console.WriteLine("========================================");
    Console.WriteLine("1. Book Shipment");
    Console.WriteLine("2. List Shipments");
    Console.WriteLine("3. Update Shipment Status");
    Console.WriteLine("4. Cancel Shipment");
    Console.WriteLine("0. Exit");
    Console.WriteLine("----------------------------------------");

    Console.Write("Enter your choice: ");

    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            BookShipment();
            break;

        case "2":
            ListShipments();
            break;

        case "3":
            UpdateStatus();
            break;

        case "4":
            CancelShipment();
            break;

        case "0":
            exit = true;
            break;

        default:
            Console.WriteLine("Invalid Choice.");
            Pause();
            break;
    }
}

void BookShipment()
{
    Shipment shipment = new Shipment();

    Console.Clear();

    Console.Write("AWB Number : ");
    shipment.AWBNumber = Console.ReadLine() ?? "";

    Console.Write("Sender Name : ");
    shipment.SenderName = Console.ReadLine() ?? "";

    Console.Write("Receiver Name : ");
    shipment.ReceiverName = Console.ReadLine() ?? "";

    Console.Write("Origin : ");
    shipment.Origin = Console.ReadLine() ?? "";

    Console.Write("Destination : ");
    shipment.Destination = Console.ReadLine() ?? "";

    Console.Write("Weight (Kg) : ");

    if (!double.TryParse(Console.ReadLine(), out double weight))
    {
        Console.WriteLine("Invalid Weight.");
        Pause();
        return;
    }

    shipment.WeightKg = weight;

    Console.WriteLine();
    Console.WriteLine("Choose Status");

    Console.WriteLine("1. " + ShipmentStatus.Booked);
    Console.WriteLine("2. " + ShipmentStatus.InTransit);
    Console.WriteLine("3. " + ShipmentStatus.OutForDelivery);
    Console.WriteLine("4. " + ShipmentStatus.Delivered);
    Console.WriteLine("5. " + ShipmentStatus.RTO);

    Console.Write("Choice : ");

    string? statusChoice = Console.ReadLine();

    switch (statusChoice)
    {
        case "1":
            shipment.Status = ShipmentStatus.Booked;
            break;

        case "2":
            shipment.Status = ShipmentStatus.InTransit;
            break;

        case "3":
            shipment.Status = ShipmentStatus.OutForDelivery;
            break;

        case "4":
            shipment.Status = ShipmentStatus.Delivered;
            break;

        case "5":
            shipment.Status = ShipmentStatus.RTO;
            break;

        default:
            Console.WriteLine("Invalid Status Choice.");
            Pause();
            return;
    }

    if (shipmentService.BookShipment(shipment, out string message))
        Console.WriteLine(message);
    else
        Console.WriteLine(message);

    Pause();
}

void ListShipments()
{
    Console.Clear();

    List<Shipment> shipments = shipmentService.ListShipments();

    if (shipments.Count == 0)
    {
        Console.WriteLine("No Shipments Found.");
        Pause();
        return;
    }

    Console.WriteLine("---------------------------------------------------------------------------------------------------------------");

    Console.WriteLine("{0,-5} {1,-12} {2,-15} {3,-15} {4,-12} {5,-12} {6,-8} {7,-18}",
        "ID",
        "AWB",
        "Sender",
        "Receiver",
        "Origin",
        "Destination",
        "Weight",
        "Status");

    Console.WriteLine("---------------------------------------------------------------------------------------------------------------");

    foreach (Shipment shipment in shipments)
    {
        Console.WriteLine("{0,-5} {1,-12} {2,-15} {3,-15} {4,-12} {5,-12} {6,-8} {7,-18}",
            shipment.ShipmentId,
            shipment.AWBNumber,
            shipment.SenderName,
            shipment.ReceiverName,
            shipment.Origin,
            shipment.Destination,
            shipment.WeightKg,
            shipment.Status);
    }

    Pause();
}

void UpdateStatus()
{
    Console.Clear();
    ListShipments();
    Console.Write("Enter AWB Number : ");
    string awb = Console.ReadLine() ?? "";

    Console.WriteLine();

    Console.WriteLine("Choose New Status");

    Console.WriteLine("1. " + ShipmentStatus.Booked);
    Console.WriteLine("2. " + ShipmentStatus.InTransit);
    Console.WriteLine("3. " + ShipmentStatus.OutForDelivery);
    Console.WriteLine("4. " + ShipmentStatus.Delivered);
    Console.WriteLine("5. " + ShipmentStatus.RTO);

    Console.Write("Choice : ");



    int choice = Convert.ToInt32(Console.ReadLine());

    string newStatus = "";

    switch (choice)
    {
        case 1:
            newStatus = ShipmentStatus.Booked;
            break;

        case 2:
            newStatus = ShipmentStatus.InTransit;
            break;

        case 3:
            newStatus = ShipmentStatus.OutForDelivery;
            break;

        case 4:
            newStatus = ShipmentStatus.Delivered;
            break;

        case 5:
            newStatus = ShipmentStatus.RTO;
            break;

        default:
            Console.WriteLine("Invalid Status Choice.");
            Pause();
            return;
    }


    shipmentService.UpdateStatus(awb, newStatus, out string message);

    Console.WriteLine(message);

    Pause();
}

void CancelShipment()
{
    Console.Clear();

    Console.Write("Enter AWB Number : ");

    string awb = Console.ReadLine() ?? "";
    // if readline() returns NULL then awb becomes="";

    shipmentService.CancelShipment(awb, out string message);

    Console.WriteLine(message);

    Pause();
}

void Pause()
{
    Console.WriteLine();
    Console.WriteLine("Press Enter to continue...");
    Console.ReadLine();
}