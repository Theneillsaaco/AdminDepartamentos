[![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/Theneillsaaco/AdminDepartamentos) ![CI](https://github.com/Theneillsaaco/AdminDepartamentos/actions/workflows/dotnet-tests.yml/badge.svg)


# AdminDepartamento

API en ASP.NET Core (.NET 10) para la administración de departamentos. Proyecto personal siguiendo principios de Clean Architecture.

## Tabla de contenidos
- Descripción
- Tecnologías
- Requisitos
- Configuración
- Ejecución local
- Estructura (Clean Architecture)
- Endpoints principales
- Testing
- Despliegue
- Roadmap
- Contribución
- Licencia
- Autor

## Descripción
AdminDepartamento es una API que permite gestionar unidades habitacionales, inquilinos, pagos e interesados, con autenticación basada en Identity y JWT, documentación con Swagger, soporte para caching y dominio implementado en F#.

## Tecnologías
- .NET 10 SDK
- ASP.NET Core 10 (Runtime)
- Entity Framework Core 10
- F# 10 (Dominio)
- ASP.NET Core Identity + JWT
- Swagger / OpenAPI
- xUnit (Testing)

## Requisitos
- .NET 10 SDK
- ASP.NET Runtime 10
- F# SDK 10
- (Opcional) Docker y Docker Compose
- Base de datos compatible con EF Core (configurable vía ConnectionStrings)

## Configuración
Crea un archivo appsettings.Development.json (o appsettings.json) con el siguiente esquema:
```json
{
  "Logging":{
    "LogLevel": {
      "Default": "Debug", "System": "Information", "Microsoft": "Information"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings":{
    "DataBase": "Server=...;Database=...;User Id=...;Password=...;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "TU_CLAVE_SECRETA",
    "Issuer": "TU_EMISOR"
  },
  "Email":
  {
    "Host": "smtp.tu-host.com",
    "Port": 587,
    "UserName": "usuario@dominio.com",
    "Password": "password",
    "Para": "destino@dominio.com"
  } 
}
```
Notas:
- ConnectionStrings: usa el provider de tu BD (SQL Server, PostgreSQL, etc.).
- Jwt: Key segura (mínimo 32 chars) y Issuer consistente con la validación.
- Email: ajusta host/puerto y credenciales reales.

## Ejecución local
1) Restaurar dependencias:
- dotnet restore
2) Aplicar migraciones (si usas EF Core Migrations):
- dotnet ef database update
3) Ejecutar:
- dotnet run --project AdminDepartamentos.API
4) Navegar a Swagger:
- http://localhost:54321/swagger (según launchSettings.json)

Variables de entorno útiles:
- ASPNETCORE_ENVIRONMENT=Development
- ConnectionStrings__DataBase
- Jwt__Key
- Jwt__Issuer
- Email__Host, Email__Port, Email__UserName, Email__Password, Email__Para

## Estructura (Clean Architecture)
- Domain.FSharp: Entidades y modelos de dominio en F# (Inquilino, Pago, UnidadHabitacional, Interesado, value objects, error handling con Railway Oriented Programming).
- Application: Casos de uso, interfaces, validaciones.
- Infrastructure: Contexto EF Core (DepartContext), implementaciones de repositorios, Identity, autenticación JWT, extensiones.
- API (Presentation): Endpoints, Swagger, configuración de middlewares.
- IOC/Dependencies: Registro de servicios (DbContext, Identity, Auth, Swagger, OutputCache).
- Unit.Test: Tests unitarios con xUnit y EF Core InMemory.

## Endpoints principales (placeholder)
- Autenticación (JWT)
- Inquilinos: CRUD
- Pagos: CRUD y consulta por inquilino
- Unidades habitacionales: CRUD
- Interesados: registro y consulta

Se documentan en /swagger al ejecutar en local.

## Testing
El proyecto incluye tests unitarios en `AdminDepartamentos.Unit.Test` utilizando xUnit y EF Core InMemory.

Comandos:
- Ejecutar todos los tests: `dotnet test`
- Ejecutar test específico: `dotnet test --filter "FullyQualifiedName~<Namespace>.<TestClassName>.<TestName>"`

Ejemplo:
```bash
dotnet test --filter "FullyQualifiedName~AdminDepartamentos.Unit.Test.PagoTests.CanCreatePago"
```

## Despliegue (placeholder)
- Dockerfile y docker-compose (pendiente si aplica)
- Azure App Service / AWS / VPS (definir)
- Migraciones automatizadas en CI/CD (pendiente)

## Roadmap
- [ ] Endpoints completos y validaciones
- [x] Envío de emails (notificaciones de pago/retraso)
- [ ] Tests unitarios e integrales

## Contribución
- Estilo: seguir convenciones de .NET y Clean Architecture (ver `AGENTS.md` para detalles).
- C#: Strict nullability, PascalCase para miembros públicos, camelCase para locales.
- F#: Railway Oriented Programming para manejo de errores, inmutabilidad, XML docs.
- PRs: crear ramas feature/..., incluir descripción y pruebas.
- Issues: usar templates y reproducibilidad.

## Licencia
Pendiente de definir (MIT recomendada para proyectos personales abiertos).

## Autor
Proyecto personal de administración de departamentos.
