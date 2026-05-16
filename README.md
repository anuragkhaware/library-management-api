# 📚 Library Management API

A production-ready REST API built with **ASP.NET Core (.NET 10)** for managing a library system — books, members, borrowing and returns.

## 🛠 Tech Stack

![.NET](https://img.shields.io/badge/.NET_10-512BD4?style=flat&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=flat&logo=csharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=flat&logo=microsoftsqlserver&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat&logo=docker&logoColor=white)
![Dapper](https://img.shields.io/badge/Dapper-grey?style=flat)
![JWT](https://img.shields.io/badge/JWT-black?style=flat&logo=jsonwebtokens)

## ✨ Features

- Full CRUD for Books
- JWT Authentication — Register & Login
- Borrow & Return flow with due dates
- Overdue book tracking
- Role-based authorization
- Auto DB initialization on startup
- Scalar API documentation
- Fully containerized with Docker Compose

## 🚀 Run with Docker

```bash
docker-compose up --build
```

API → http://localhost:5200/library

> DB and all tables are created automatically on first run.

## 🔑 Auth Flow

```
POST /api/Auth/register  → Get JWT token
POST /api/Auth/login     → Get JWT token
Add header: Authorization: Bearer <token>
```

## 📋 API Endpoints

### Auth
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | /api/Auth/register | ❌ | Register new member |
| POST | /api/Auth/login | ❌ | Login and get token |

### Books
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | /api/Books | ❌ | Get all books |
| GET | /api/Books/{id} | ❌ | Get book by ID |
| POST | /api/Books | ✅ | Add new book |
| PUT | /api/Books/{id} | ✅ | Update book |
| DELETE | /api/Books/{id} | ✅ | Delete book |

### Borrow
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | /api/Borrow | ✅ | Borrow a book |
| PUT | /api/Borrow/{id}/return | ✅ | Return a book |
| GET | /api/Borrow/overdue | ✅ | Get overdue books |
| GET | /api/Borrow/member/{id} | ✅ | Get member history |

## 🗄 Database Schema

```
Books          — Id, Title, Author, Genre, IsAvailable
Members        — Id, FullName, Email, PasswordHash, Role, CreatedAt
BorrowRecords  — Id, BookId, MemberId, BorrowedAt, DueDate, ReturnedAt, IsReturned
```

## 📁 Project Structure

```
LibraryManagementAPI/
├── Controllers/    — API endpoints
├── Services/       — Business logic
├── Repositories/   — Data access (Dapper)
├── Models/         — Database entities
├── DTOs/           — Request/Response objects
└── init.sql        — Auto DB initialization
```