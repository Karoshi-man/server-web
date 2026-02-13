<div align="center">

  <h1>Server-Side Web Development</h1>
  <p>
    <strong>Master's Degree Program | Data Engineering</strong><br>
    Lviv Polytechnic National University
  </p>

  <p>
    <a href="https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-8.0">
      <img src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET" />
    </a>
    <a href="https://getbootstrap.com/">
      <img src="https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white" alt="Bootstrap" />
    </a>
    <a href="https://www.microsoft.com/en-us/sql-server">
      <img src="https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white" alt="SQL Server" />
    </a>
     <a href="#">
      <img src="https://img.shields.io/badge/Glassmorphism-Ui-00dfd8?style=for-the-badge" alt="UI Design" />
    </a>
  </p>

  <p>
    This repository contains the source code and documentation for laboratory works focused on building scalable, secure, and modern web applications using the <strong>ASP.NET Core</strong> ecosystem.
  </p>

  <br />

  <p>
    <a href="#-course-syllabus"><strong>Explore the Labs »</strong></a>
    <br />
    <br />
    <a href="#-visual-showcase">View Demo</a>
    ·
    <a href="https://github.com/Karoshi-man/server-web/issues">Report Bug</a>
    ·
    <a href="https://github.com/Karoshi-man/server-web/pulls">Request Feature</a>
  </p>
</div>

<hr />

## 📚 Course Syllabus & Progress

| ID | Topic | Key Concepts | Status |
| :--- | :--- | :--- | :--- |
| **Lab 1** | **MVC Architecture & Data Modeling** | Entity Framework Core, CRUD Operations, SQL Server, Many-to-Many Relationships, LINQ. | ✅ Completed |
| **Lab 2** | **Authentication & Authorization** | ASP.NET Identity, Roles & Claims, JWT/Cookies, Secure Password Hashing. | ⏳ Planned |
| **Lab 3** | **RESTful API Development** | API Controllers, Swagger/OpenAPI, DTOs, Automapper, Postman Testing. | ⏳ Planned |
| **Lab 4** | **Advanced Architecture & Deployment** | Clean Architecture, Unit Testing (xUnit), Dockerization, CI/CD Pipelines. | ⏳ Planned |
| **Lab 5** | **Real-time Communication** | SignalR, WebSockets, Interactive Dashboards. | ⏳ Planned |

> *Note: The syllabus matches the Master's degree curriculum requirements.*

---

## 🎨 Visual Showcase

The project features a custom **Cyberpunk / Dark Glassmorphism** design language, moving away from standard Bootstrap templates to ensure a unique user experience.

### 🏠 Home Page
> *A minimalist landing page with holographic text effects and glass cards.*
![Home Page](https://via.placeholder.com/800x400?text=Insert+Home+Page+Screenshot+Here)

### 📄 Articles Grid
> *Responsive grid layout with neon hover effects and dynamic badges.*
![Articles Page](https://via.placeholder.com/800x400?text=Insert+Articles+Screenshot+Here)

### ✍️ Content Creation
> *Focus-mode editor with multi-select author support.*
![Create Page](https://via.placeholder.com/800x400?text=Insert+Create+Page+Screenshot+Here)

---

## 🛠️ Tech Stack

This project is built using the latest industry standards:

* **Framework:** ASP.NET Core 8.0 / 9.0 (MVC)
* **Database:** Microsoft SQL Server (LocalDB) via Entity Framework Core
* **ORM:** Entity Framework Core (Code-First approach)
* **Frontend:** Razor Views, Bootstrap 5, Custom CSS3 (Glassmorphism), jQuery
* **Tools:** Visual Studio 2022, Git

---

## 🚀 Getting Started

Follow these steps to set up the project locally.

### Prerequisites

* [.NET SDK](https://dotnet.microsoft.com/download) installed.
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or LocalDB which comes with VS).

### Installation

1.  **Clone the repository**
    ```bash
    git clone [https://github.com/Karoshi-man/server-web.git](https://github.com/Karoshi-man/server-web.git)
    cd server-web
    ```

2.  **Restore dependencies**
    ```bash
    dotnet restore
    ```

3.  **Update Database** (Apply Migrations)
    ```bash
    cd lab1
    dotnet ef database update
    ```

4.  **Run the application**
    ```bash
    dotnet run
    ```

---

## 👤 Author

**Martin (Karoshi-man)**
* Github: [@Karoshi-man](https://github.com/Karoshi-man)
* Role: Master's Student, Data Engineering

---

<div align="center">
  <small>&copy; 2026 Online Journal Project. Designed for educational purposes.</small>
</div>
