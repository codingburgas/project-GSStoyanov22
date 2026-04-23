# 🏷️ Auction House

Auction House is a web-based auction platform developed as a school project for 11th grade (2025/2026).

The platform allows users to create auctions, place bids in real time, and automatically closes auctions when the timer expires. The project is built using ASP.NET Core MVC, Entity Framework Core, and SQL Server.

---

## ✨ Features

### 👤 User Features
- Register and log in
- Create auctions
- Browse active auctions
- Place bids in real time
- View bidding history
- View won auctions

### 🛠️ Admin Features
- Delete auctions
- Block users
- Manage auction records

### ⏳ Auction Features
- Real-time bidding
- Automatic auction timer
- Highest bid tracking
- Winner determination

---

## 💻 Technologies Used

### 🔙 Backend
- ASP.NET Core MVC
- C#
- Entity Framework Core
- LINQ

### 🎨 Frontend
- Razor Views
- Bootstrap
- JavaScript

### 🗄️ Database
- SQL Server

---

## 🗂️ Database Structure

The system uses three main entities:

### 👤 User
Stores:
- Username
- Email
- Password
- Role

### 📦 Auction
Stores:
- Title
- Description
- Starting Price
- Current Price
- End Time

### 💰 Bid
Stores:
- Bid Amount
- Bid Time
- User ID
- Auction ID

### 🔗 Relationships
- One User → Many Auctions
- One Auction → Many Bids
- One User → Many Bids

---

## 🏗️ Project Architecture

The project follows the MVC pattern:

- **Models** – represent database entities
- **Views** – handle the user interface
- **Controllers** – process requests and business logic
- **Services** – contain reusable business logic

Dependency Injection is used to connect services with controllers.

---

## 🔐 Validation and Security

The project includes:

- Data validation using Data Annotations
- Role-based authorization
- Bid validation logic
- Protected auction management

### 👥 Roles
- **User** – can create auctions and place bids
- **Admin** – can manage auctions and users

---

## ▶️ Run the Project

To run the project, open **Command Prompt** and execute:

```bash
cd C:\Users\User\source\repos\project-GSStoyanov22\AuctionHouse
dotnet run --project src/AuctionHouse.Web/AuctionHouse.Web.csproj
```

After the application starts, open the URL shown in the terminal in your browser.

---

## 🚀 Future Improvements

Possible future improvements:
- Real-time updates with SignalR
- Email notifications
- Product image uploads
- Auction categories
- Payment integration

---

## 👨‍💻 Authors

School Project – 11th Grade  
2025/2026
