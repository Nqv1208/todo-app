
Với bối cảnh bạn đang làm todo-app bằng Next.js (App Router) + shadcn + next-themes, mình sẽ thiết kế DOMAIN cho hệ thống TodoList “level Notion” theo Clean Architecture + khả năng scale lớn.

Mình không đi vào UI hay code, mà tập trung Domain – thứ quyết định dự án sống lâu hay chết sớm.

🎯 Mục tiêu hệ thống

Một Productivity Platform chứ không chỉ Todo:

Todo

Page / Note

Workspace

Collaboration

Permission

Activity log

AI (sau này)

👉 Todo chỉ là 1 module trong hệ sinh thái

🧱 TẦNG DOMAIN (CORE CỦA DỰ ÁN)
1️⃣ Bounded Context tổng thể
┌──────────────┐
│ Identity     │  Auth, User, Session
└──────────────┘
        ↓
┌──────────────┐
│ Workspace    │  Team / Personal Space
└──────────────┘
        ↓
┌──────────────┐
│ Content      │  Page, Block, Todo, Database
└──────────────┘
        ↓
┌──────────────┐
│ Collaboration│  Share, Permission, Comment
└──────────────┘
        ↓
┌──────────────┐
│ Activity     │  Audit, History, Analytics
└──────────────┘

🧠 CORE DOMAIN: CONTENT

Đây là trái tim giống Notion

🧩 Aggregate Root: ContentItem
ContentItem
├── id
├── type (PAGE | TODO | DATABASE)
├── title
├── icon
├── cover
├── parentId
├── workspaceId
├── position
├── createdAt
├── updatedAt


👉 Mọi thứ đều là Content

🧩 Block (Notion-style)
Block
├── id
├── contentItemId
├── type (TEXT, CHECKBOX, TODO, IMAGE, ...)
├── properties (JSON)
├── position
├── createdAt

✅ TODO DOMAIN (PHẦN RIÊNG)
🧩 Todo (Entity)
Todo
├── id
├── contentItemId
├── status (TODO | DOING | DONE)
├── priority (LOW | MEDIUM | HIGH)
├── dueDate
├── assigneeId
├── completedAt


👉 Todo không đứng một mình
👉 Nó là 1 dạng Block / ContentItem

🧩 Subtask (Value Object)
SubTask
├── id
├── title
├── isDone

👥 WORKSPACE DOMAIN
Workspace
├── id
├── name
├── type (PERSONAL | TEAM)
├── ownerId
├── createdAt

WorkspaceMember
├── workspaceId
├── userId
├── role (OWNER | ADMIN | MEMBER | GUEST)

🔐 IDENTITY DOMAIN
User
├── id
├── email
├── name
├── avatar
├── status

Session
├── id
├── userId
├── expiresAt

🤝 COLLABORATION DOMAIN
Permission
├── id
├── resourceType (PAGE | TODO)
├── resourceId
├── subjectType (USER | ROLE)
├── level (READ | WRITE | ADMIN)

Comment
├── id
├── resourceId
├── userId
├── content
├── createdAt

📜 ACTIVITY DOMAIN
ActivityLog
├── id
├── actorId
├── action (CREATE | UPDATE | DELETE)
├── targetType
├── targetId
├── metadata (JSON)
├── createdAt

🧠 DOMAIN RULES (RẤT QUAN TRỌNG)
❌ Không cho phép

Todo tồn tại ngoài Workspace

Block không có ContentItem

User sửa nội dung nếu không có Permission

✅ Bắt buộc

Mọi hành động ghi ActivityLog

Mọi Content đều versionable (sau này)

🧩 CLEAN ARCHITECTURE MAPPING
/domain
 ├── entities
 │    ├── Todo.ts
 │    ├── ContentItem.ts
 │    ├── Workspace.ts
 │
 ├── value-objects
 │    ├── TodoStatus.ts
 │    ├── Role.ts
 │
 ├── repositories
 │    ├── TodoRepository.ts
 │
 ├── services
 │    ├── PermissionService.ts
 │
 └── events
      ├── TodoCompletedEvent.ts

🚀 KHẢ NĂNG MỞ RỘNG SAU NÀY

✔️ Kanban
✔️ Calendar
✔️ Database View
✔️ AI assistant
✔️ Offline-first
✔️ Realtime collaboration (WebSocket / Yjs)
✔️ Plugin system

🧠 TƯ DUY QUAN TRỌNG (NOTION DÙNG)

❝ Không build Todo app
❝ Build Content Platform có Todo

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

PostgreSQL: dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

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
dotnet add package AWSSDK.S3 hoặc dotnet add package Azure.Storage.Blobs

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