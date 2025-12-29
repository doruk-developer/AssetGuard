# 🛡️ AssetGuard - Enterprise Fixed Asset Management

![Project Status](https://img.shields.io/badge/Status-Production%20Ready-success?style=flat-square)
![Framework](https://img.shields.io/badge/.NET%208.0-MVC-purple?style=flat-square)
![Architecture](https://img.shields.io/badge/Architecture-N--Tier%20%28Layered%29-blue?style=flat-square)
![Database](https://img.shields.io/badge/Database-SQL%20Server%20%2B%20EF%20Core-red?style=flat-square)

**AssetGuard** is a robust and scalable **Fixed Asset Management System** designed to bridge the gap between digital inventory and physical assets. Built with **.NET 8 MVC** on a solid **N-Tier Architecture**, it serves as a "Single Source of Truth" for corporate assets.

The project goes beyond standard CRUD operations by implementing advanced software engineering patterns such as **EF Core Interceptors** for automated data integrity, **Auto-Discovery Connection Strategy** for zero-config deployment, and **Canvas-based QR generation** for offline tracking.

> **Note:** The user interface (UI) is designed in **Turkish** to simulate a local corporate environment, while the codebase and architectural documentation follow **English** standards.

---

## 🌟 Key Features

### 🏢 Physical Tracking & QR Module
*   **Offline QR Generation:** Generates unique digital identity tags for assets using `QRCoder` within the Business Layer (No external API dependency).
*   **Canvas-Based Rendering:** Integrates asset metadata (Serial No, Name) with the QR code on the client-side using HTML5 Canvas for high-quality PNG downloads and direct printing.
*   **Actionable Tags:** Scanned codes redirect authorized personnel directly to the asset's detail page for instant status verification.

### 🛡️ Enterprise Data Integrity (The "Black Box")
*   **Interceptor-Based Audit Trail:** Automatically tracks *Who* did *What* and *When*. Implemented via **EF Core Interceptors**, capturing `CreatedBy`, `ModifiedBy`, and timestamps for every transaction without polluting the Controllers.
*   **Soft Delete Architecture:** Prevents accidental data loss. Deletion commands are intercepted and converted to `IsDeleted = true` updates.
*   **Global Query Filters:** Automatically excludes soft-deleted records from the application UI while preserving them in the database for audit and recovery purposes.

### 🧠 Smart Architecture & Portability
*   **Auto-Discovery Connection Strategy:** A custom startup logic in `Program.cs` that intelligently pings multiple connection strings (`Work`, `Home`, `Local`) to find the active SQL Server. This enables true **"Plug & Play"** portability across different physical environments without changing config files.
*   **JSON-Based Theme Persistence:** User preferences (Dark Mode, Sidebar Color, Chart Type) are stored in server-side JSON files (`ThemeData`), demonstrating file-based persistence alongside relational data.

### 📊 Advanced Analytics & UX
*   **Progressive Dashboard:** SPA-like experience using AJAX to refresh KPIs, Charts, and Tables without full page reloads.
*   **Role-Based Access Control (RBAC):** Strict segregation between **Admins** and **Users**. Unauthorized UI elements (buttons, columns) are completely removed from the DOM to ensure security.
*   **Professional Reporting:** Zebra-striped, formatted Excel exports generated via `ClosedXML`.

---

## 🏗️ Technical Architecture

This project strictly follows the **N-Tier Architecture** principles to ensure separation of concerns, testability, and maintainability.

### 1. Layered Structure
*   **AssetGuard.Entity:** POCO classes, DTOs (`DashboardStatsDTO`), and the abstract `BaseEntity` that enforces Audit standards across all tables.
*   **AssetGuard.DataAccess:** EF Core Context, Migrations, and Concrete Repositories (`EfEntityRepositoryBase`). Handles the critical `SaveChanges` overriding logic for Soft Delete and Auditing.
*   **AssetGuard.Business:** Validation rules (`FluentValidation`), Business Logic, and Mapping. Acts as the brain of the operation.
*   **AssetGuard.WebUI:** The presentation layer containing Controllers, ViewModels, and AdminLTE-based Views.

### 2. Database Strategy
*   **Code-First:** Database schema is managed entirely through C# migrations.
*   **Database Seeding:** Automatically populates the database with default Roles, Users, and Sample Inventory upon the first run if the database is empty.

---

## 🛠️ Technologies Used

| Area | Technologies |
| :--- | :--- |
| **Framework** | .NET 8.0 (ASP.NET Core MVC) |
| **Data Access** | Entity Framework Core 8, SQL Server |
| **Validation** | FluentValidation |
| **Frontend** | HTML5, CSS3, JavaScript (ES6+), jQuery, AdminLTE 3 |
| **Visualization** | Chart.js (Dynamic Analytics), QRCoder (Asset Tags) |
| **Reporting** | ClosedXML (Excel Export) |
| **Tools** | Visual Studio 2022, SQL Server Management Studio (SSMS) |

---

## 🚀 Installation & Setup

To run this project locally:

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/doruk-developer/AssetGuard.git
    ```
2.  **Open in Visual Studio:** Double click the `.sln` file.
3.  **Database Configuration (Auto-Magic):**
    *   The system uses **Auto-Discovery**. It will try to connect to specific environments first, then fall back to `LocalDB`.
    *   *Optional:* Update `appsettings.json` -> `DefaultConnection` if you want to force a specific SQL instance.
4.  **Initialize Database:**
    *   Open **Package Manager Console**.
    *   Set **Default project** to `AssetGuard.DataAccess`.
    *   Run command: `Update-Database`
5.  **Run (F5):** The system will automatically seed default data (Roles, Admin User, Sample Assets) on the first launch.

---

## 🔐 Default Login Credentials

The system initializes with two default accounts to demonstrate RBAC capabilities:

| Role | Username | Password | Access Level |
| :--- | :--- | :--- | :--- |
| **Admin** | `admin` | `123` | Full Access (CRUD, System Settings, User Mgmt) |
| **User** | `user` | `321` | Read-Only, Reporting, Dashboard Access |

---

## ⚠️ Technical Notes & Disclaimers

*   **Data Safety:** Items deleted via the UI are **Soft Deleted**. To permanently remove them or restore them, a Database Administrator must toggle the `IsDeleted` flag in SQL Server.
*   **Theme Storage:** User themes are stored in `AssetGuard.WebUI/ThemeData`. Ensure the application has write permissions to this folder.
*   **Printing:** The QR Code printing feature opens a popup window. Ensure your browser allows popups for the `localhost` domain.

---

## 📄 License

This project is open-source and available under the [MIT License](LICENSE). You are free to copy, modify, and distribute the code for educational and portfolio purposes.

## 🙏 Acknowledgments

*   **AdminLTE** for the professional dashboard template.
*   **QRCoder** & **ClosedXML** for their excellent .NET libraries.
*   **Open Source Community** for continuous inspiration.

<br>

<div align="center">
  <sub>Built with 💙 using .NET 8.0 and modern software engineering practices.</sub><br>
  <sub>This project showcases production-ready patterns suitable for enterprise environments.</sub>
</div>