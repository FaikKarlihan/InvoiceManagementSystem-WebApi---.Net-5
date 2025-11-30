# Invoice Management System (IMS) - Web API

![.NET Core](https://img.shields.io/badge/.NET%205-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![MongoDB](https://img.shields.io/badge/MongoDB-47A248?style=for-the-badge&logo=mongodb&logoColor=white)

This project is a comprehensive **Invoice Management System** backend developed for residential complex/apartment management, providing invoice tracking, dues payments, and user management. The project is developed with a layered architecture inspired by **Domain Driven Design (DDD)** principles.

Additionally, it includes a separate **Payment API** service that simulates banking transactions and runs on **MongoDB**.

## 🚀 Features

### 1. Core Functions (Core API)
* **User & Apartment Management:** Full CRUD operations for users and apartments.
* **Invoice Management:** Add, delete, update, and list invoices (CRUD).
* **Admin Privileges:**
    * Assign users to apartments.
    * Assign invoices/dues to users.
* **User Privileges:**
    * Send messages to the management.
    * Make payments via credit card.
    * View invoice and payment history.
    * Change password (The system generates an automatic password upon initial registration and stores it as **hashed** in the DB).

### 2. Architecture and Tech Stack
* **Framework:** .NET 5 Web API
* **Database (Relational):** PostgreSQL (Entity Framework Core)
* **Architecture:** Layered structure based on DDD (Domain Driven Design).
* **Security:** JWT (JSON Web Token) and Claim-based authentication/authorization.
* **Mapping:** Object mapping with AutoMapper.
* **Validation:** Data consistency and security with FluentValidation.
* **Logging & Tracking:** Detailed tracking of operation results.

### 3. Payment System (Payment API - Microservice)
An external service simulating a bank to allow users to make payments.
* **Database:** MongoDB
* **Process:** Two-phase secure payment (2-Phase Commit Simulation):
    1.  **Initiate Payment:** Payment is initiated; if valid, a `payment token` is returned.
    2.  **Confirm Payment:** The payment is finalized using the token.
* **Logging:** All successful and failed transaction requests are logged.

---

## 🛠 Installation and Execution

The project can be run both containerized via **Docker** and in a **Local** environment.

### Option 1: Running with Docker (Recommended)

The project is fully dockerized. To start the entire system, including PostgreSQL and MongoDB containers:

1.  Clone the repo:
    ```bash
    git clone [https://github.com/FaikKarlihan/InvoiceManagementSystem-WebApi---.Net-5.git](https://github.com/FaikKarlihan/InvoiceManagementSystem-WebApi---.Net-5.git)
    cd InvoiceManagementSystem-WebApi---.Net-5
    ```

2.  Create the `.env` file:
    Create a file named `.env` in the project root directory by copying the `.env.example` file and defining the necessary variables.

    **Example `.env` content:**
    ```env
    # Database Settings
    POSTGRES_USER=admin
    POSTGRES_PASSWORD=password123
    POSTGRES_DB=InvoiceDb

    # Mongo Settings
    MONGO_INITDB_ROOT_USERNAME=admin
    MONGO_INITDB_ROOT_PASSWORD=password123
     
    # App Settings
    JWT_KEY=This_Is_A_Very_Secret_Key_Please_Change_It_123
    ```

3.  Start the containers:
    ```bash
    docker-compose up -d --build
    ```

### Option 2: Running Locally

If you prefer to run it via an IDE (Visual Studio / VS Code) without Docker:

**Web API (Main Service):**
* If the PostgreSQL connection string is not defined in `appsettings.json` or is left empty, the application automatically runs in **In-Memory Database** mode to allow for testing.
* *Note: A PostgreSQL connection string must be entered for production-like testing.*

**Payment API:**
* A working **MongoDB** connection (Local or Cloud) is mandatory for this service to run. The connection string must be entered in `appsettings.json`.

---

## 🔌 Ports and Access Information

Due to the application configuration, the ports exposed externally match in both the **Local** environment and inside **Docker** containers. Once the application is up, you can access it via the following addresses:

| Service | Protocol | Access Address (Swagger) | Docker Internal Port |
| :--- | :--- | :--- | :--- |
| **Web API** | HTTPS | `https://localhost:5001/swagger` | 443 |
| **Web API** | HTTP | `http://localhost:5000/swagger` | 80 |
| **Payment API** | HTTPS | `https://localhost:5003/swagger` | 443 |
| **Payment API** | HTTP | `http://localhost:5002/swagger` | 80 |

**Note:** In communication between containers, the Web API and Payment API reach each other over the internal network via port 80 (`http://web_api:80` and `http://payment_api:80`).

*(Note: Ports may vary based on your `docker-compose.yml` or `launchSettings.json` configuration; please check the relevant files.)*

---

## 🧪 Test Users and Scenarios

When the system starts for the first time, if the database is empty (or in In-Memory mode), Seed Data may run (depending on the code). For manual testing:

1.  **Admin Login:** `/api/Auth/Login` (Admin user is added via seed data - mail:a password:123456).
2.  **Add Apartment/User:** Perform operations using `Authorization: Bearer <token>` in the Header with the Admin token.
3.  **Payment:** Define a credit card balance via the Payment API, then send a payment request via the Web API.

---

## 📝 License

This project is licensed under the MIT License. See the `LICENSE` file for details.
