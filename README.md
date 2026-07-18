# PollBuilder

Ứng dụng tạo và bình chọn (poll) trực tuyến, xây dựng theo mô hình **Clean Architecture** với kiến trúc **microservices**, gồm một backend API (ASP.NET Core Web API) và một frontend MVC (ASP.NET Core MVC) giao tiếp với nhau qua HTTP.

## Mục lục

- [Mô tả dự án](#mô-tả-dự-án)
- [Kiến trúc](#kiến-trúc)
- [Công nghệ sử dụng](#công-nghệ-sử-dụng)
- [Hướng dẫn cài đặt](#hướng-dẫn-cài-đặt)
- [Triển khai (Deployment)](#triển-khai-deployment)

## Mô tả dự án

PollBuilder cho phép người dùng:

- Đăng ký / đăng nhập tài khoản (xác thực bằng JWT).
- Tạo poll (câu hỏi bình chọn) với nhiều lựa chọn (option).
- Chia sẻ poll qua mã/URL riêng.
- Bình chọn cho một hoặc nhiều option (mỗi user chỉ được vote một lần trên mỗi câu hỏi).
- Xem kết quả bình chọn theo thời gian thực dưới dạng số lượng phiếu và phần trăm.
- Người tạo poll có thể đóng (close) poll để ngừng nhận vote.

## Kiến trúc

Dự án áp dụng **Clean Architecture**, tách biệt rõ các tầng để đảm bảo khả năng bảo trì và kiểm thử độc lập:

```
┌─────────────────────────────────────────────────────────┐
│                     PollBuilder.MVC                       │
│         (Frontend – Razor Views, thin HTTP client)        │
│   - Cookie Authentication cho trình duyệt                 │
│   - IPollApiClient / IAuthApiClient (typed HttpClient)     │
│   - JwtForwardingHandler: đính JWT vào request gọi API     │
└───────────────────────────┬────────────────────────────┘
                             │ HTTP (REST)
┌───────────────────────────▼────────────────────────────┐
│                     PollBuilder.API                       │
│              (Backend – REST API, JWT Auth)                │
│                                                             │
│  ┌───────────────────────────────────────────────────┐   │
│  │  Presentation: Controllers (PollController, ...)   │   │
│  ├───────────────────────────────────────────────────┤   │
│  │  Application: CQRS (MediatR) – Commands / Queries, │   │
│  │  Validators (FluentValidation), DTOs, Behaviors    │   │
│  ├───────────────────────────────────────────────────┤   │
│  │  Domain: Entities (User, Poll, Question, Option,   │   │
│  │  Vote), Domain Exceptions, Business Rules          │   │
│  ├───────────────────────────────────────────────────┤   │
│  │  Infrastructure: EF Core (PollBuilderDbContext,    │   │
│  │  PollBuilderIdentityDbContext), Identity, JWT       │   │
│  │  Token Service, Repository implementations         │   │
│  └───────────────────────────────────────────────────┘   │
└───────────────────────────┬────────────────────────────┘
                             │
                    ┌────────▼────────┐
                    │   SQL Server     │
                    │  (2 DbContext:   │
                    │  Domain + Identity)│
                    └─────────────────┘
```

### Nguyên tắc thiết kế chính

- **Tách 2 DbContext**: `PollBuilderDbContext` quản lý dữ liệu nghiệp vụ (Poll, Question, Option, Vote), `PollBuilderIdentityDbContext` quản lý dữ liệu Identity (user, role) — tránh trộn lẫn concern xác thực với concern nghiệp vụ.
- **CQRS với MediatR**: mọi thao tác đọc/ghi được mô hình hóa thành Command/Query riêng biệt, xử lý qua Handler tương ứng.
- **Validation pipeline**: `ValidationBehavior` (MediatR Pipeline Behavior) tự động chạy FluentValidation trước khi Command/Query tới Handler, trả lỗi 400 nếu dữ liệu không hợp lệ.
- **JWT Authentication**: API xác thực bằng Bearer token; MVC dùng cookie cho trình duyệt và forward JWT khi gọi ngầm đến API qua `JwtForwardingHandler`.
- **Rate Limiting**: giới hạn số request theo IP (chính sách chung) và theo user (chính sách riêng cho hành động vote) để chống spam/gian lận phiếu bầu.
- **Global Exception Handling**: `ExceptionHandlingMiddleware` map các domain exception (ví dụ `PollClosedException`) thành mã HTTP phù hợp.

## Công nghệ sử dụng

| Thành phần | Công nghệ |
|---|---|
| Backend API | ASP.NET Core Web API (.NET 10) |
| Frontend | ASP.NET Core MVC, Razor View, Bootstrap |
| ORM | Entity Framework Core 10 |
| Database | SQL Server |
| Authentication | ASP.NET Core Identity + JWT Bearer |
| CQRS / Mediator | MediatR |
| Validation | FluentValidation |
| API Documentation | Scalar (OpenAPI) |

## Hướng dẫn cài đặt

### Yêu cầu

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB, Express, hoặc instance đầy đủ)
- (Tùy chọn) Visual Studio 2022+ hoặc VS Code

### 1. Clone repository

```bash
git clone https://github.com/stephenhwan/EBikeShop.git
cd EBikeShop
git checkout MService
```

### 2. Cấu hình connection string và JWT

Trong `PollBuilder.API/appsettings.json` (và tạo `appsettings.Development.json` nếu cần), cấu hình:

```json
{
  "ConnectionStrings": {
    "DomainConnection": "Server=(localdb)\\mssqllocaldb;Database=PollBuilderDb;Trusted_Connection=True;",
    "IdentityConnection": "Server=(localdb)\\mssqllocaldb;Database=PollBuilderIdentityDb;Trusted_Connection=True;"
  },
  "Jwt": {
    "Issuer": "PollBuilder",
    "Audience": "PollBuilderClient",
    "Key": "<chuỗi bí mật ít nhất 32 ký tự>"
  }
}
```

Trong `PollBuilder.MVC/appsettings.json`, trỏ đến địa chỉ API:

```json
{
  "ApiBaseUrl": "https://localhost:{port_của_API}"
}
```

### 3. Khởi tạo database (EF Core Migrations)

Chạy migration cho từng DbContext riêng biệt:

```bash
cd PollBuilder.API

dotnet ef database update --context PollBuilderDbContext
dotnet ef database update --context PollBuilderIdentityDbContext
```

### 4. Chạy ứng dụng

Mở 2 terminal riêng (hoặc cấu hình multiple startup projects trong Visual Studio):

```bash
# Terminal 1 — API
cd PollBuilder.API
dotnet run

# Terminal 2 — MVC
cd PollBuilder.MVC
dotnet run
```

Mặc định:
- API: `https://localhost:7xxx` (xem `launchSettings.json` để biết port chính xác), có thể xem tài liệu API tại `/scalar`.
- MVC: `https://localhost:5xxx`, truy cập bằng trình duyệt để sử dụng ứng dụng.

### 5. Kiểm tra nhanh

- Đăng ký tài khoản mới tại trang MVC.
- Đăng nhập, tạo một poll với ít nhất 2 option.
- Mở poll bằng một tài khoản khác (hoặc chế độ ẩn danh, nếu poll hỗ trợ), thực hiện vote.
- Xem trang kết quả (`/Home/Results` hoặc tương đương) để kiểm tra số phiếu/phần trăm hiển thị đúng.

## Triển khai (Deployment)

Dự án hiện **chỉ chạy ở môi trường local** (chưa có bản triển khai công khai/live). Để chạy thử, vui lòng làm theo [Hướng dẫn cài đặt](#hướng-dẫn-cài-đặt) ở trên.
