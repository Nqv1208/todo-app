🧱 1. CORE FRAMEWORK
Microsoft.AspNetCore.App
.NET 8 SDK

🧠 2. DOMAIN LAYER (PURE – KHÔNG DEPEND)

❌ KHÔNG cài package nào

✔️ Domain chỉ gồm:

Entity

ValueObject

Domain Event

Interface (Repository, Service)

👉 Đây là nguyên tắc sống còn của Clean Architecture

📦 3. APPLICATION LAYER
CQRS + Validation
dotnet add package MediatR
dotnet add package MediatR.Extensions.Microsoft.DependencyInjection
dotnet add package FluentValidation
dotnet add package FluentValidation.DependencyInjectionExtensions

Mapping
dotnet add package AutoMapper

🗄️ 4. INFRASTRUCTURE – DATABASE
ORM
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools

Provider (chọn 1)

PostgreSQL:

dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL


SQL Server:

dotnet add package Microsoft.EntityFrameworkCore.SqlServer

🔐 5. AUTH / SECURITY
JWT
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer

Identity (tuỳ chọn)
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore

Password hashing
dotnet add package BCrypt.Net-Next

🌐 6. API LAYER
Versioning
dotnet add package Asp.Versioning.Mvc

Swagger
dotnet add package Swashbuckle.AspNetCore

🧪 7. VALIDATION / ERROR HANDLING
dotnet add package Hellang.Middleware.ProblemDetails


📌 Chuẩn RFC 7807
📌 Giống API lớn (Google, Stripe)

📜 8. LOGGING / OBSERVABILITY
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.File

🔄 9. REALTIME (COLLAB)
dotnet add package Microsoft.AspNetCore.SignalR


📌 Realtime cursor, block editing

📦 10. CACHE / PERFORMANCE
dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis

📡 11. EVENT / MESSAGE (SAU NÀY)
dotnet add package MassTransit


📌 Outbox pattern
📌 Async processing

📂 12. FILE / STORAGE
dotnet add package AWSSDK.S3


hoặc

dotnet add package Azure.Storage.Blobs

🧪 13. TESTING
dotnet add package xunit
dotnet add package FluentAssertions
dotnet add package Moq

🧱 14. CLEAN ARCHITECTURE MAPPING
Domain        → (no deps)
Application   → MediatR, FluentValidation
Infrastructure→ EF Core, Identity, Serilog
API           → Swagger, Auth, Versioning

🧠 GỢI Ý CHUẨN KIẾN TRÚC

✔️ CQRS + MediatR
✔️ Domain Event → ActivityLog
✔️ PermissionService trong Application
✔️ EF Core cho CRUD, Dapper cho report

🚀 STACK ĐỀ XUẤT CHO DỰ ÁN NÀY
Layer	Tech
API	ASP.NET Core 8
App	MediatR + FluentValidation
DB	PostgreSQL
Auth	JWT + OAuth
Realtime	SignalR
Cache	Redis
Log	Serilog