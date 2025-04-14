# eStoreAPI

An ASP.NET Core-based RESTful API for e-commerce applications. This API provides endpoints for product management, user authentication, order processing, and more. It leverages Entity Framework Core for data access, JWT for secure authentication, and Swagger/OpenAPI for interactive API documentation.

---

## Table of Contents

- [Features](#features)
- [Requirements](#requirements)
- [Setup and Installation](#setup-and-installation)
- [Running the Application](#running-the-application)
- [API Endpoints](#api-endpoints)
- [Swagger / OpenAPI Documentation](#swagger--openapi-documentation)
- [Code Documentation](#code-documentation)
- [Additional Documentation](#additional-documentation)
- [License](#license)

---

## Features

- **User Authentication:** Secure login and registration using JWT tokens.
- **Product Management:** CRUD operations for products.
- **Order Processing:** Create, view, and manage customer orders.
- **Database Integration:** Using Entity Framework Core for ORM.
- **Interactive API Documentation:** Automatic Swagger UI integration.
- **Extensible and Modular Architecture:** Easily customizable and scalable.

---

## Requirements

- [.NET 6 SDK](https://dotnet.microsoft.com/download) (or later)
- A SQL Server instance or another supported database for Entity Framework Core
- Node.js (if you plan to use any client-side tools)
- Git

---

## Setup and Installation

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/vborischik/eStoreAPI.git
   cd eStoreAPI
