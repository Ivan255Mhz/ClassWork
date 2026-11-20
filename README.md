# WebApplication4 — учебный проект «Магазин игрушек»

Сквозной проект для занятий по ASP.NET Core. Постепенно обрастает новыми темами:
контроллеры, DTO, репозитории, EF Core, фильтры, middleware, фоновые задачи,
JWT-аутентификация и Razor Pages.

## Стек

- .NET 8 / ASP.NET Core
- Entity Framework Core 9 + PostgreSQL (Npgsql)
- Swagger (Swashbuckle)
- Razor Pages
- Docker Compose для локальной БД и pgAdmin

## Структура

```
WebApplication4/
├── BackgroundJobs/   # фоновые сервисы: очистка логов, очередь email, события lifetime
├── Controllers/      # API-контроллеры (Toy, Category, Manufacturer, Auth, Test)
├── Data/
│   ├── Dtos/         # объекты запросов/ответов
│   ├── Repositories/ # IRepository<T> и реализация поверх EF Core
│   ├── AppDbContext.cs
│   └── DependencyInjection.cs
├── Entities/         # сущности БД
├── Filters/          # action/result/exception/authorization-фильтры
├── Helpers/          # учебный AuthenticationHandler
├── Middleware/       # примеры middleware (логирование, заголовки, обёртка ответа)
├── Migrations/       # миграции EF Core
├── Pages/            # Razor Pages (страница игрушек)
├── Services/         # бизнес-логика и логирование в БД
└── Program.cs        # регистрация сервисов и конвейер middleware
```

## Запуск

1. Поднять PostgreSQL и pgAdmin:

   ```bash
   docker compose up -d
   ```

   - PostgreSQL: `localhost:5433`, БД/пользователь `toyshop`
   - pgAdmin: http://localhost:5050 (admin@example.com / admin)

2. Применить миграции (нужен инструмент `dotnet-ef`):

   ```bash
   dotnet tool install --global dotnet-ef
   dotnet ef database update --project WebApplication4
   ```

3. Запустить приложение:

   ```bash
   dotnet run --project WebApplication4
   ```

## Куда зайти

| Адрес | Что это |
| --- | --- |
| `https://localhost:7059/swagger` | Swagger UI со всеми API |
| `https://localhost:7059/Toys` | Razor Page со списком игрушек |
| `GET /api/home` | Проверка подключения к БД |
| `GET /api/toy` | Список игрушек |
| `GET /api/test/state` | Состояние фоновых задач |

Готовые запросы лежат в `WebApplication4/WebApplication4.http`.

## Второй проект: ToyApi

В решении есть отдельный проект `ToyApi` — заготовка Minimal API, которая смотрит в **ту же базу** `toyshop`.

Что уже готово:

- EF Core + Npgsql;
- `ToyDbContext` и сущности `Toy`, `Category`, `Manufacture`;
- строка подключения;
- Swagger;
- `/health` и `/health/db` для проверки запуска и подключения к БД.

Запуск:

```bash
dotnet run --project ToyApi
```

- Swagger: `https://localhost:7090/swagger`
- HTTP: `http://localhost:5090`

В `ToyApi` студенты пишут endpoint'ы Minimal API. Миграции остаются в `WebApplication4`: схемой владеет основной проект. Подробности — в `ToyApi/README.md`.

## Аутентификация

Тестовый пользователь: `admin` / `1234`.

```http
POST /api/auth/login?userName=admin&password=1234
```

Токен подписывается ключом `Jwt:Key` из `appsettings.json` и проверяется
JWT Bearer-аутентификацией. Политика `OnlyAdmin` требует роль `Admin`.

## Особенности

- Имена `Prise` (цена) и `Detalis` (детали лога) оставлены как есть,
  потому что они уже закреплены за колонками в БД и миграциях.
- Часть middleware и фильтров — учебные примеры. В `Program.cs` подключены
  не все: некоторые классы показывают возможности и включаются по мере занятий.
- Логи старше 7 дней удаляются фоновым сервисом `LogCleanService`.
