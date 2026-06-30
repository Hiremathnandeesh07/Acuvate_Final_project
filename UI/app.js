const BASE_URL = "http://localhost:5234/api/shipments";

let shipments = [];
let activeFilter = "All";

document.addEventListener("DOMContentLoaded", async function () {
  bindEvents();
  await loadShipments();
  loadStats();
});

function bindEvents() {
  const form = document.getElementById("bookShipmentForm");
  if (form) {
    form.addEventListener("submit", handleBookShipment);
  }

  const originInput = document.getElementById("origin");
  if (originInput) {
    originInput.addEventListener("input", updateAutoAwb);
  }

  const searchButton = document.getElementById("searchButton");
  if (searchButton) {
    searchButton.addEventListener("click", handleSearch);
  }

  const filter = document.getElementById("statusFilter");
  if (filter) {
    filter.addEventListener("change", function (event) {
      activeFilter = event.target.value;
      displayShipments(shipments);
    });
  }

  document.addEventListener("click", function (event) {
    if (event.target.classList.contains("deleteButton")) {
      const shipmentId = event.target.dataset.id;
      showDeleteConfirmation(shipmentId);
    }
  });

  document.addEventListener("change", function (event) {
    if (event.target.classList.contains("updateStatus")) {
      updateShipmentStatus(event.target.dataset.awb, event.target.value);
    }
  });
}

function showLoading() {
  const loading = document.getElementById("loading");
  if (loading) {
    loading.style.display = "block";
  }
}

function hideLoading() {
  const loading = document.getElementById("loading");
  if (loading) {
    loading.style.display = "none";
  }
}

function showNotification(message, success = true) {
  const notification = document.getElementById("notification");
  if (!notification) {
    return;
  }

  notification.textContent = message;
  notification.style.background = success ? "green" : "red";
  notification.style.display = "block";

  setTimeout(function () {
    notification.style.display = "none";
  }, 3000);
}

function showDeleteConfirmation(shipmentId) {
  if (!shipmentId) {
    return;
  }

  const overlay = document.createElement("div");
  overlay.className = "delete-confirm-overlay";
  overlay.innerHTML = `
    <div class="delete-confirm-modal" role="dialog" aria-modal="true" aria-labelledby="deleteConfirmTitle">
      <h3 id="deleteConfirmTitle">Delete shipment?</h3>
      <p>This action cannot be undone. The shipment will be removed from the list.</p>
      <div class="delete-confirm-actions">
        <button type="button" class="delete-confirm-cancel">Cancel</button>
        <button type="button" class="delete-confirm-delete">Delete</button>
      </div>
    </div>
  `;

  document.body.appendChild(overlay);
  document.body.classList.add("modal-open");

  const closeModal = () => {
    overlay.remove();
    document.body.classList.remove("modal-open");
  };

  overlay.addEventListener("click", function (event) {
    if (event.target === overlay) {
      closeModal();
    }
  });

  overlay
    .querySelector(".delete-confirm-cancel")
    .addEventListener("click", closeModal);
  overlay
    .querySelector(".delete-confirm-delete")
    .addEventListener("click", function () {
      closeModal();
      deleteShipment(shipmentId);
    });
}

async function requestJson(url, options = {}) {
  const response = await fetch(url, {
    headers: {
      "Content-Type": "application/json",
    },
    ...options,
  });

  if (!response.ok) {
    const contentType = response.headers.get("content-type") || "";
    let errorMessage = "Request failed";

    if (contentType.includes("application/json")) {
      try {
        const errorData = await response.json();
        errorMessage =
          errorData.message ||
          errorData.title ||
          errorData.error ||
          JSON.stringify(errorData);
      } catch {
        errorMessage = "Request failed";
      }
    } else {
      errorMessage = await response.text();
    }

    throw new Error(errorMessage);
  }

  const contentType = response.headers.get("content-type") || "";
  if (contentType.includes("application/json")) {
    return response.json();
  }

  return response.text();
}

async function loadShipments() {
  showLoading();

  try {
    const response = await requestJson(BASE_URL);
    shipments = Array.isArray(response) ? response : response.data || [];
    displayShipments(shipments);
    updateAutoAwb();
  } catch (error) {
    console.error(error);
    showNotification(
      "Unable to load shipments. Check whether the API is running and CORS is enabled.",
      false,
    );
  } finally {
    hideLoading();
  }
}

function displayShipments(data) {
  const filtered =
    activeFilter === "All"
      ? data
      : data.filter((shipment) => shipment.status === activeFilter);

  const rows = filtered
    .map(
      (shipment) => `
    <tr>
      <td>${shipment.awbNumber || ""}</td>
      <td>${shipment.senderName || ""}</td>
      <td>${shipment.receiverName || ""}</td>
      <td>${shipment.origin || ""} → ${shipment.destination || ""}</td>
      <td>${shipment.weightKg || ""}</td>
      <td>${getStatusBadge(shipment.status)}</td>
      <td>

      
        <select class="updateStatus" data-awb="${shipment.awbNumber || ""}">
          <option value="Booked" ${shipment.status === "Booked" ? "selected" : ""}>Booked</option>
          <option value="In Transit" ${shipment.status === "In Transit" ? "selected" : ""}>In Transit</option>
          <option value="Out for Delivery" ${shipment.status === "Out for Delivery" ? "selected" : ""}>Out for Delivery</option>
          <option value="Delivered" ${shipment.status === "Delivered" ? "selected" : ""}>Delivered</option>
          <option value="RTO" ${shipment.status === "RTO" ? "selected" : ""}>RTO</option>
        </select>
      </td>
      <td>
        <button class="deleteButton" data-id="${shipment.shipmentId || ""}">Delete</button>
      </td>
    </tr>
  `,
    )
    .join("");

  const tbody = document.querySelector("#shipmentTable tbody");
  if (tbody) {
    tbody.innerHTML = rows;
  }

  updateTotalCount();
}

function updateTotalCount() {
  const totalCount = document.getElementById("totalCount");
  if (totalCount) {
    totalCount.textContent = shipments.length;
  }
}

function updateAutoAwb() {
  const origin = document.getElementById("origin")?.value || "";
  const awbInput = document.getElementById("awbNumber");

  if (!awbInput) {
    return;
  }

  awbInput.value = generateAwbFromOrigin(origin);
}

function getNextShipmentId() {
  const numericIds = shipments
    .map((shipment) => Number(shipment.shipmentId))
    .filter((id) => Number.isInteger(id) && id > 0);

  return numericIds.length > 0 ? Math.max(...numericIds) + 1 : 1;
}

function generateAwbFromOrigin(origin) {
  const cleanedOrigin = String(origin)
    .trim()
    .replace(/[^A-Za-z]/g, "")
    .toUpperCase();
  const prefix = cleanedOrigin.slice(0, 3) || "DEL";
  const year = new Date().getFullYear();

  const nextId = getNextShipmentId();
  return `${prefix}${year}00${nextId}`;
}

function getStatusBadge(status) {
  switch (status) {
    case "Booked":
      return `<span class="badge booked">${status}</span>`;
    case "In Transit":
      return `<span class="badge inTransit">${status}</span>`;
    case "Out for Delivery":
      return `<span class="badge outForDelivery">${status}</span>`;
    case "Delivered":
      return `<span class="badge delivered">${status}</span>`;
    case "RTO":
      return `<span class="badge rto">${status}</span>`;
    default:
      return status || "";
  }
}

function isValidTextValue(value) {
  return typeof value === "string" && /^[A-Za-z\s'.-]+$/.test(value.trim());
}

async function loadStats() {
  try {
    const stats = await requestJson(`${BASE_URL}/stats`);
    document.getElementById("bookedCount").textContent = stats.booked ?? 0;
    document.getElementById("inTransitCount").textContent =
      stats.inTransit ?? 0;
    document.getElementById("outForDeliveryCount").textContent =
      stats.outForDelivery ?? 0;
    document.getElementById("deliveredCount").textContent =
      stats.delivered ?? 0;
    document.getElementById("rtoCount").textContent = stats.rto ?? 0;
    document.getElementById("totalCount").textContent = shipments.length ?? 0;
  } catch (error) {
    console.error(error);
    showNotification("Unable to load statistics.", false);
  }
}

async function handleBookShipment(event) {
  event.preventDefault();

  updateAutoAwb();
  const awbNumber = document.getElementById("awbNumber").value.trim();
  const senderName = document.getElementById("senderName").value.trim();
  const receiverName = document.getElementById("receiverName").value.trim();
  const origin = document.getElementById("origin").value.trim();
  const destination = document.getElementById("destination").value.trim();
  const weightKg = Number(document.getElementById("weightKg").value);

  if (!awbNumber || !senderName || !receiverName || !origin || !destination) {
    showNotification("Please fill in all shipment fields.", false);
    return;
  }

  if (!isValidTextValue(senderName)) {
    showNotification(
      "Sender name must contain only letters and spaces.",
      false,
    );
    return;
  }

  if (!isValidTextValue(receiverName)) {
    showNotification(
      "Receiver name must contain only letters and spaces.",
      false,
    );
    return;
  }

  if (!isValidTextValue(origin)) {
    showNotification("Origin must be a text value.", false);
    return;
  }

  if (!isValidTextValue(destination)) {
    showNotification("Destination must be a text value.", false);
    return;
  }

  if (!weightKg || weightKg <= 0) {
    showNotification("Weight must be greater than zero.", false);
    return;
  }

  const payload = {
    awbNumber,
    senderName,
    receiverName,
    origin,
    destination,
    weightKg,
  };

  try {
    await requestJson(BASE_URL, {
      method: "POST",
      body: JSON.stringify(payload),
    });
    showNotification("Shipment booked successfully.");
    event.target.reset();
    await loadShipments();
    loadStats();
  } catch (error) {
    console.error(error);
    showNotification(error.message || "Unable to book shipment.", false);
  }
}

function handleSearch() {
  const awb = document.getElementById("searchAwb").value.trim().toUpperCase();
  const shipment = shipments.find((item) => String(item.awbNumber) === awb);
  const card = document.getElementById("shipmentCard");

  if (shipment) {
    card.style.display = "block";
    card.innerHTML = `
      <h3>Shipment Details</h3>
      <p><strong>AWB:</strong> ${shipment.awbNumber}</p>
      <p><strong>Sender:</strong> ${shipment.senderName}</p>
      <p><strong>Receiver:</strong> ${shipment.receiverName}</p>
      <p><strong>Route:</strong> ${shipment.origin} → ${shipment.destination}</p>
      <p><strong>Status:</strong> ${shipment.status}</p>
    `;
  } else {
    card.style.display = "block";
    card.innerHTML = "No shipment found for that AWB.";
  }
}

async function deleteShipment(id) {
  if (!id) {
    return;
  }

  try {
    await requestJson(`${BASE_URL}/${id}`, {
      method: "DELETE",
    });

    shipments = shipments.filter(
      (shipment) => String(shipment.shipmentId) !== String(id),
    );
    displayShipments(shipments);
    await loadStats();
    refreshBookingForm();

    showNotification("Shipment deleted.");
  } catch (error) {
    console.error(error);
    showNotification(
      "Delete request failed. The API endpoint may not support it yet.",
      false,
    );
  }
}

function refreshBookingForm() {
  const form = document.getElementById("bookShipmentForm");
  if (form) {
    form.reset();
  }

  updateAutoAwb();
}

async function updateShipmentStatus(awb, status) {
  try {
    await requestJson(`${BASE_URL}/${awb}/status`, {
      method: "PUT",
      body: JSON.stringify({ status }),
    });
    shipments = shipments.map((shipment) =>
      shipment.awbNumber === awb ? { ...shipment, status } : shipment,
    );
    displayShipments(shipments);
    loadStats();
    showNotification("Shipment status updated.");
  } catch (error) {
    console.error(error);
    showNotification(
      "Status update failed. The API endpoint may not support it yet.",
      false,
    );
  }
}
