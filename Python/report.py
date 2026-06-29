import requests
import csv
import sys

from datetime import datetime

API_BASE_URL = "http://localhost:5234/api/shipments"
 
# Calling the APIs

# get shipments
def get_shipments():

    response = requests.get(API_BASE_URL)
    response.raise_for_status()
    # meaning od .raise_for_status()
    # If the HTTP request failed (4xx or 5xx), 
    # immediately throw an exception. Otherwise, do nothing.
    return response.json()

# get stats
def get_stats():

    response = requests.get(API_BASE_URL + "/stats")
    response.raise_for_status()
    return response.json()


def export_csv(shipments):

      filename = (
          "delhivery_report_"
          + datetime.today().strftime("%Y%m%d")
          + ".csv"
      )

      with open(
          filename,
          "w",
          newline=""
      ) as file:

          writer = csv.writer(file)

          writer.writerow([
              "ShipmentId",
              "AWBNumber",
              "SenderName",
              "ReceiverName",
              "Origin",
              "Destination",
              "WeightKg",
              "Status",
              "BookedAt",
              "DeliveredAt"
          ])

          for shipment in shipments:

              writer.writerow([
                  shipment["shipmentId"],
                  shipment["awbNumber"],
                  shipment["senderName"],
                  shipment["receiverName"],
                  shipment["origin"],
                  shipment["destination"],
                  shipment["weightKg"],
                  shipment["status"],
                  shipment["bookedAt"],
                  shipment["deliveredAt"]
              ])

      print("CSV exported successfully.")


# handling API call

def main():

  try:

      shipments = get_shipments()
      stats = get_stats()

  except requests.exceptions.ConnectionError:
      print("ERROR: DSIP API is offline.")
      sys.exit(1)

  except requests.exceptions.Timeout:
      print("ERROR: Request timed out.")
      sys.exit(1)

  except requests.exceptions.HTTPError as ex:
      print(f"ERROR: HTTP {ex.response.status_code}")
      sys.exit(1)

  except requests.exceptions.RequestException as ex:
      print(f"ERROR: {ex}")
      sys.exit(1)



  total_shipments = len(shipments)
  average_weight = sum(
      shipment["weightKg"]
      for shipment in shipments
  ) / total_shipments

  # heavest shipments
  heaviest = max(
      shipments,
      key=lambda shipment: shipment["weightKg"]
  )


  # report printing
  print("=" * 48)

  print(" DELHIVERY - END OF DAY SHIPMENT REPORT")

  print(
      " Date            :",
      datetime.today().strftime("%Y-%m-%d")
  )

  print("=" * 48)

  print(f" Total Shipments : {total_shipments}")

  print(f" Booked          : {stats['booked']}")

  print(f" In Transit      : {stats['inTransit']}")

  print(f" Out for Delivery: {stats['outForDelivery']}")

  print(f" Delivered       : {stats['delivered']}")

  print(f" RTO             : {stats['rto']}")

  print("-" * 48)

  print(
      f" Avg Weight      : {average_weight:.2f} kg"
  )

  print(
      f" Heaviest        : {heaviest['awbNumber']} ({heaviest['weightKg']} kg)"
  )

  print("=" * 48)



  # exporting CSV
  if "--export" in sys.argv:

      export_csv(shipments)

  

if __name__ == "__main__":
  main()