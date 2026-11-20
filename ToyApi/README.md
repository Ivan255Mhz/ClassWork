# ToyApi — заготовка Minimal API

Отдельный Minimal API-проект, который смотрит в **ту же PostgreSQL-базу**, что и `WebApplication4`.

## Что уже настроено

- ASP.NET Core 8, Minimal API;
- EF Core 9 + Npgsql;
- `ToyDbContext` с `Toys`, `Categories`, `Manufactures`;
- сущности под существующую схему БД;
- строка подключения в `appsettings.json`;
- Swagger;
- порт `5090` (HTTP) / `7090` (HTTPS);
- `/health` для проверки запуска.

## Что писать студентам

Endpoint'ы Minimal API. Лучше выносить их в отдельный файл:

```text
ToyApi/
└── Endpoints/
    └── ToyEndpoints.cs
```

И подключать в `Program.cs`:

```csharp
app.MapToyEndpoints();
```

## Важно

- Схемой БД владеет `WebApplication4`.
- В `ToyApi` **не нужно** создавать миграции и вызывать `EnsureCreated()`.
- Если в основной БД появится новая колонка, её нужно добавить в сущности `ToyApi` вручную.
- Строку подключения в реальном проекте хранят в переменных окружения или user-secrets.

## Запуск

```powershell
dotnet run --project ToyApi
```

Swagger:

```text
https://localhost:7090/swagger
```
