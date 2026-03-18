<div align="center">

  <h1 align="center">
  iJournal <br>
  Collaborative Publishing Platform
</h1>
  
  <p>
    <strong>Master's Degree Program | Data Engineering</strong><br>
    Lviv Polytechnic National University
  </p>

  <p>
    <a href="https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-8.0">
      <img src="https://img.shields.io/badge/.NET_8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET" />
    </a>
    <a href="https://learn.microsoft.com/en-us/ef/core/">
      <img src="https://img.shields.io/badge/Entity_Framework-388E3C?style=for-the-badge&logo=databricks&logoColor=white" alt="EF Core" />
    </a>
    <a href="https://www.microsoft.com/en-us/sql-server">
      <img src="https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white" alt="SQL Server" />
    </a>
     <a href="#">
      <img src="https://img.shields.io/badge/Deep_Glassmorphism_UI-000000?style=for-the-badge&logo=css3&logoColor=00ffc8" alt="UI Design" />
    </a>
  </p>

  <p>
    A scalable, secure, and visually immersive web application built with <strong>ASP.NET Core MVC</strong>. This repository contains the source code for laboratory works focused on advanced data modeling, secure authentication, and complex business logic without reliance on standard UI frameworks.
  </p>

  <br />

  <p>
    <a href="#-course-syllabus--progress"><strong>Explore the Labs »</strong></a>
    <br />
    <br />
    <a href="#-key-features-implemented">Features</a>
    ·
    <a href="#-visual-showcase">Visual Showcase</a>
    ·
    <a href="https://github.com/Karoshi-man/server-web/issues">Report Bug</a>
  </p>
</div>

---

## 📚 Course Syllabus & Progress

The syllabus directly reflects the Master's degree curriculum requirements for the "Server-Side Web Development" course.

| ID | Topic | Key Concepts & Implementation | Status |
| :--- | :--- | :--- | :--- |
| **Lab 1** | **Розробка web-додатку ASP.Net Core MVC** | EF Core Code-First, CRUD Operations, Controllers & Views, Relational Data Schema mapping. | ✅ Completed |
| **Lab 2** | **Використання сесій для тимчасового зберігання даних** | Temporary session storage for edited data, Model-level validation attributes, Clearing sessions. | ✅ Completed |
| **Lab 3** | **Авторизація та автентифікація на основі Identity** | ASP.NET Core Identity, Individual accounts, Role-based Access Control (Admin), Admin Panel for user management. | ✅ Completed |
| **Lab 4** | **Використання SignalR Core для створення чатів** | Real-time messaging, SignalR Hubs, Private and Public messages, File/Image transfer. | ✅ Completed |
| **Lab 5** | **Побудова REST-сервісу з використанням Web API** | ASP.NET Core Web API, Repository Pattern, API Controllers, Swagger UI testing. | ⏳ Planned |
| **Lab 6** | **Розробка мікросервісів** | Microservices Architecture, Service separation for models. | ⏳ Planned |

---

## ✨ Key Features Implemented

* **🔐 Enterprise-Grade Security:** Custom implementation of ASP.NET Identity. Strict route protection (`[Authorize]`), logical authorization (users can only edit/delete their own articles), and a dedicated "System Access" portal for Administrators.
* **🤝 Collaborative Publishing:** A robust "Cabinet" system where authors can manage drafts, publish articles, and send secure email invitations to other authors to co-write publications.
* **📊 Dynamic Library Filtering:** Users can sort the global library by Category, Date, Article Rating, or "Top Authors" using LINQ-powered queries.
* **⭐ Precision Rating System:** Both Articles and Authors have a 1-to-10 rating system calculating average scores to the first decimal point (`double` precision).
* **🚫 Zero-Framework UI:** The entire platform abandons Bootstrap in favor of a 100% custom **Deep Blue / Cyberpunk Glassmorphism** design language. Features include `backdrop-filter` blurring, neon cyan (`#00ffc8`) accents, and complex CSS Grids.

---

## 🎨 Visual Showcase

The UI was designed to feel like a high-end desktop application or a developer-focused tool.

### 🏠 The Library (Main Feed)
> *Features holographic text, glass-panel cards, dynamic rating badges, and instant dropdown filters.*
<img src="https://placehold.co/800x400/0a0f1e/00ffc8?text=Library+Feed+%7C+Glassmorphism+UI&font=Montserrat" alt="Library" width="100%">

### ✍️ The Editor Canvas
> *A distraction-free, focus-mode editor with custom styled select menus and glowing action buttons.*
<img src="https://placehold.co/800x400/0a0f1e/00ffc8?text=Focus-Mode+Editor+%7C+Neon+Accents&font=Montserrat" alt="Editor" width="100%">

### 👤 Author Initialization & Profile
> *A secure onboarding screen demonstrating responsive grid layouts and three-part full name combinations.*
<img src="https://placehold.co/800x400/0a0f1e/00ffc8?text=Secure+Author+Initialization&font=Montserrat" alt="Profile Init" width="100%">

### 🔒 System Access (Admin Auth)
> *A minimalist, dark-themed authentication portal protected against browser autofill interventions.*
<img src="https://placehold.co/800x400/0a0f1e/00ffc8?text=System+Access+%7C+Admin+Portal&font=Montserrat" alt="Auth" width="100%">

---

## 🛠️ Tech Stack

* **Backend Framework:** ASP.NET Core 8.0 (MVC)
* **Database & ORM:** Microsoft SQL Server via Entity Framework Core (Code-First)
* **Security:** ASP.NET Core Identity (Cookie-based auth, Role Management)
* **Frontend Architecture:** Razor Views (`.cshtml`), Vanilla CSS3, HTML5
* **Design System:** Custom CSS (Flexbox, CSS Grid, Backdrop Filters, Custom SVG Icons) - *No external CSS frameworks used.*

---

## 🚀 Getting Started

Follow these steps to set up the project locally.

### Prerequisites
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or LocalDB included with Visual Studio)

### Installation

1.  **Clone the repository**
    ```bash
    git clone https://github.com/Karoshi-man/server-web.git
    cd server-web
    ```

2.  **Restore dependencies**
    ```bash
    dotnet restore
    ```

3.  **Apply Database Migrations**
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

**Martin Fesenko (Karoshi-man)**
* Github: [@Karoshi-man](https://github.com/Karoshi-man)
* Role: Master's Student, Data Engineering

---

<div align="center">
  <small>&copy; 2026 iJournal Project. Designed in Lviv</small>
</div>
