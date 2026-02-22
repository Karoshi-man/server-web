<div align="center">

  <h1>iJournal &mdash; Collaborative Publishing Platform</h1>
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

The syllabus has been adapted to reflect the advanced implementation of the platform, fulfilling Master's degree curriculum requirements.

| ID | Topic | Key Concepts & Implementation | Status |
| :--- | :--- | :--- | :--- |
| **Lab 1** | **MVC Architecture & Data Modeling** | EF Core Code-First, CRUD Operations, Complex Many-to-Many Relationships (`ArticleAuthors`), Navigation Properties, Custom HTML5/CSS3 Grid UI. | ✅ Completed |
| **Lab 2** | **Identity, Security & Authorization** | ASP.NET Core Identity, Role-based Access Control (`Admin` vs `User`), Custom Auth Views (Login/Register), Server-side Age Validation, Anti-autofill security hacks. | ✅ Completed |
| **Lab 3** | **Business Logic & Collaboration** | Co-author Invitation System (Pending/Accept/Decline), State Management (Draft vs Published), 10-point Decimal Rating System, Dynamic Filtering & Sorting. | ✅ Completed |
| **Lab 4** | **RESTful API Development** | API Controllers, Swagger/OpenAPI, DTOs, Automapper, Data Serialization. | ⏳ Planned |
| **Lab 5** | **Advanced Architecture & Deployment** | Clean Architecture refactoring, Dockerization, SignalR for real-time notifications. | ⏳ Planned |

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
<img src="https://via.placeholder.com/800x400?text=Insert+Screenshot_1+(Library)+Here" alt="Library" width="100%">

### ✍️ The Editor Canvas
> *A distraction-free, focus-mode editor with custom styled select menus and glowing action buttons.*
<img src="https://via.placeholder.com/800x400?text=Insert+Screenshot_5+(Editor)+Here" alt="Editor" width="100%">

### 👤 Author Initialization
> *A secure onboarding screen demonstrating responsive grid layouts and server-side model validation.*
<img src="https://via.placeholder.com/800x400?text=Insert+Screenshot_3+(Init+Profile)+Here" alt="Profile Init" width="100%">

### 🔒 System Access (Admin Auth)
> *A minimalist, dark-themed authentication portal protected against browser autofill interventions.*
<img src="https://via.placeholder.com/800x400?text=Insert+Screenshot_4+(Login)+Here" alt="Auth" width="100%">

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
    git clone [https://github.com/Karoshi-man/server-web.git](https://github.com/Karoshi-man/server-web.git)
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
* Role: Master's Student, Data Engineering (Future ML Engineer)

---

<div align="center">
  <small>&copy; 2026 iJournal Project. Designed in Lviv.</small>
</div>
