# 🛡️ SecureAuthApi

![.NET 8](https://img.shields.io/badge/.NET-8.0-blue.svg)
![Docker](https://img.shields.io/badge/Docker-Enabled-blue.svg)
![PostgreSQL](https://img.shields.io/badge/Database-PostgreSQL-336791.svg)

A production-ready ASP.NET Core 8 Web API built with a **Clean Architecture** approach. This project features a custom-built security gauntlet including **JWT Authentication**, **Role-Based Access Control (RBAC)**, and **API Key Middleware**.

---

## 🚀 Features
- **Custom Identity System**: Replaced heavy ASP.NET Identity with a lightweight 2-table (Users & Roles) system for total control.
- **RBAC (Role-Based Access Control)**: Granular permissions using "Admin" and "User" roles.
- **Multi-Layer Security**: 
  - **Static:** `X-API-KEY` required for every request.
  - **Dynamic:** JWT Bearer tokens for user sessions.
  - **Hashing:** Industry-standard **BCrypt** for password security.
- **Dockerized**: Fully orchestrated API and Database environment.

---

## 🛠️ Tech Stack
- **Framework**: .NET 8 (Web API)
- **Database**: PostgreSQL 16
- **Security**: JWT, BCrypt, Middleware
- **DevOps**: Docker & Docker Compose
- **Tools**: DBeaver (Database Management), cURL (Testing)

---

## 📦 Getting Started

### 1. Prerequisites
- Docker & Docker Compose installed.
- (Optional) .NET 8 SDK if running locally without Docker.

### 2. Launching the Stack
Run the following command in the root directory to build and start the API and PostgreSQL database:
```bash
docker compose up --build -d

3. Database Access

The system automatically seeds an initial Admin user on startup:

    Admin Email: admin@company.com

    Admin Password: Admin123!

    Postgres Port: 5432 (Connect via DBeaver)

     Gemini said

Here is the complete, high-level Professional README for your project. This version combines the features, tech stack, setup, API reference, and testing into one clean, scannable document that looks great on GitHub.
Markdown

# 🛡️ SecureAuthApi

![.NET 8](https://img.shields.io/badge/.NET-8.0-blue.svg)
![Docker](https://img.shields.io/badge/Docker-Enabled-blue.svg)
![PostgreSQL](https://img.shields.io/badge/Database-PostgreSQL-336791.svg)

A production-ready ASP.NET Core 8 Web API built with a **Clean Architecture** approach. This project features a custom-built security gauntlet including **JWT Authentication**, **Role-Based Access Control (RBAC)**, and **API Key Middleware**.

---

## 🚀 Features
- **Custom Identity System**: Replaced heavy ASP.NET Identity with a lightweight 2-table (Users & Roles) system for total control.
- **RBAC (Role-Based Access Control)**: Granular permissions using "Admin" and "User" roles.
- **Multi-Layer Security**: 
  - **Static:** `X-API-KEY` required for every request.
  - **Dynamic:** JWT Bearer tokens for user sessions.
  - **Hashing:** Industry-standard **BCrypt** for password security.
- **Dockerized**: Fully orchestrated API and Database environment.

---

## 🛠️ Tech Stack
- **Framework**: .NET 8 (Web API)
- **Database**: PostgreSQL 16
- **Security**: JWT, BCrypt, Middleware
- **DevOps**: Docker & Docker Compose
- **Tools**: DBeaver (Database Management), cURL (Testing)

---

## 📦 Getting Started

### 1. Prerequisites
- Docker & Docker Compose installed.
- (Optional) .NET 8 SDK if running locally without Docker.

### 2. Launching the Stack
Run the following command in the root directory to build and start the API and PostgreSQL database:
```bash
docker compose up --build -d

3. Database Access

The system automatically seeds an initial Admin user on startup:

    Admin Email: admin@company.com

    Admin Password: Admin123!

    Postgres Port: 5432 (Connect via DBeaver)

📍 API Reference
Method	Endpoint	Auth Level	Description
POST	/api/auth/login	API Key	Authenticates user & returns JWT
POST	/api/auth/register	API Key + Admin JWT	Registers new users (Admin only)
GET	/api/weatherforecast	API Key + JWT	Sample data for all logged-in users
GET	/api/weatherforecast/admin-stats	API Key + Admin JWT	Restricted data for Admins only

Note: All requests must include the header: X-API-KEY: MySecureApiKey123
🧪 Automated Testing

A bash script is provided to test the full security lifecycle (Login -> Register -> Access Control).

chmod +x test_api.sh
./test_api.sh


Folder Structure (Clean Architecture)
SecureAuthApi/
├── Src/
│   ├── Controllers/      # API Endpoints & Routing
│   ├── Infrastructure/   # DbContext, Migrations, Data Seeding
│   ├── Models/           # Entities (User, Role) & DTOs
│   ├── Services/         # JWT Generation & Business Logic
│   └── Middleware/       # API Key Validation Logic
├── Dockerfile            # Multi-stage build for .NET
└── docker-compose.yml    # Infrastructure orchestration