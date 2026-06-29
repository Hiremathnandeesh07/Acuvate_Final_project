using Delhivery.Data.Exceptions;
using Delhivery.Data.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delhivery.Data.Repository
{
    public class ShipmentRepository
    {
        private readonly string _connectionString;
        public ShipmentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }



        public List<Shipment> GetAll()
        {
            List<Shipment> shipments = new List<Shipment>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetAllShipments", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Shipment shipment = new Shipment();

                    shipment.ShipmentId = Convert.ToInt32(reader["ShipmentId"]);
                    shipment.AWBNumber = reader["AWBNumber"].ToString()!;
                    shipment.SenderName = reader["SenderName"].ToString()!;
                    shipment.ReceiverName = reader["ReceiverName"].ToString()!;
                    shipment.Origin = reader["Origin"].ToString()!;
                    shipment.Destination = reader["Destination"].ToString()!;
                    shipment.WeightKg = Convert.ToDecimal(reader["WeightKg"]);
                    shipment.Status = reader["Status"].ToString()!;
                    shipment.BookedAt = Convert.ToDateTime(reader["BookedAt"]);

                    if (reader["DeliveredAt"] != DBNull.Value)
                    {
                        shipment.DeliveredAt = Convert.ToDateTime(reader["DeliveredAt"]);
                    }

                    shipments.Add(shipment);
                }
            }

            return shipments;
        }


        public Shipment GetByAWB(string awb)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetShipmentByAWB", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@AWBNumber", awb);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Shipment shipment = new Shipment();

                    shipment.ShipmentId = Convert.ToInt32(reader["ShipmentId"]);
                    shipment.AWBNumber = reader["AWBNumber"].ToString()!;
                    shipment.SenderName = reader["SenderName"].ToString()!;
                    shipment.ReceiverName = reader["ReceiverName"].ToString()!;
                    shipment.Origin = reader["Origin"].ToString()!;
                    shipment.Destination = reader["Destination"].ToString()!;
                    shipment.WeightKg = Convert.ToDecimal(reader["WeightKg"]);
                    shipment.Status = reader["Status"].ToString()!;
                    shipment.BookedAt = Convert.ToDateTime(reader["BookedAt"]);

                    if (reader["DeliveredAt"] != DBNull.Value)
                    {
                        shipment.DeliveredAt = Convert.ToDateTime(reader["DeliveredAt"]);
                    }

                    return shipment;
                }

                throw new ShipmentNotFoundException($"Shipment with AWB '{awb}' not found.");
            }
        }


        public Shipment Book(Shipment shipment)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Shipments
                                    (
                                        AWBNumber,
                                        SenderName,
                                        ReceiverName,
                                        Origin,
                                        Destination,
                                        WeightKg
                                    )
                                    VALUES
                                    (
                                        @AWBNumber,
                                        @SenderName,
                                        @ReceiverName,
                                        @Origin,
                                        @Destination,
                                        @WeightKg
                                    )";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@AWBNumber", shipment.AWBNumber);
                command.Parameters.AddWithValue("@SenderName", shipment.SenderName);
                command.Parameters.AddWithValue("@ReceiverName", shipment.ReceiverName);
                command.Parameters.AddWithValue("@Origin", shipment.Origin);
                command.Parameters.AddWithValue("@Destination", shipment.Destination);
                command.Parameters.AddWithValue("@WeightKg", shipment.WeightKg);

                connection.Open();

                command.ExecuteNonQuery();
            }
            return GetByAWB(shipment.AWBNumber);
        }


        public Shipment UpdateStatus(string awb, string status)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("usp_UpdateShipmentStatus", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@AWBNumber", awb);
                command.Parameters.AddWithValue("@NewStatus", status);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    throw new ShipmentNotFoundException($"shipment with {awb} not found");
                }
            }
            return GetByAWB(awb);
        }


        public void Cancel(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("usp_CancelShipment", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ShipmentId", id);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new ShipmentNotFoundException($"Shipment with Id '{id}' not found.");
                }
            }
        }

    }



}
