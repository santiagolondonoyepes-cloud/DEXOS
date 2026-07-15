# DEXOS

DEXOS es un ecosistema SaaS multi-tenant para operar negocios físicos con una visión de plataforma empresarial. La pieza central del dominio es la Orden, que se convierte en el origen de los flujos operativos, financieros, analíticos y de experiencia del cliente.

## Visión

- Registrar un dato una sola vez y reutilizarlo en todo el ecosistema.
- Unificar POS, cocina, barra, inventario, caja, contabilidad, CRM, reporting e IA alrededor de la Orden.
- Asegurar soporte para miles de empresas, sucursales, usuarios y millones de transacciones.

## Principios de arquitectura

- Cloud Native
- Multi Tenant
- API First
- Event Driven
- Clean Architecture
- DDD (Domain Driven Design)
- SOLID
- CQRS cuando sea necesario
- Repository Pattern
- Dependency Injection
- Microservicios preparados para escalar

## Pila tecnológica inicial

- Backend: ASP.NET Core 9, C#, EF Core, SQL Server, SignalR, JWT, FluentValidation, Serilog, Swagger, Hangfire, Redis
- Frontend: Blazor, MudBlazor, TailwindCSS
- Mobile: .NET MAUI
- Cloud: Azure SQL, Azure Storage, Azure App Services, Azure Key Vault, Azure Monitor, Application Insights
- DevOps: GitHub Actions, Docker, Docker Compose

## Estructura del repositorio

```text
/src
  /DEXOS.API
  /DEXOS.Application
  /DEXOS.Domain
  /DEXOS.Infrastructure
  /DEXOS.Shared
  /DEXOS.Identity
  /DEXOS.Notifications
  /DEXOS.Payments
  /DEXOS.Inventory
  /DEXOS.Orders
  /DEXOS.Accounting
  /DEXOS.Reporting
  /DEXOS.CRM
  /DEXOS.AI
/apps
  /DEXOS.Admin
  /DEXOS.POS
  /DEXOS.Kitchen
  /DEXOS.Bar
  /DEXOS.PDA
  /DEXOS.Kiosk
  /DEXOS.QR
/tests
/docs
/scripts
```

## Fase 1: base arquitectónica

1. Definir el modelo de dominio y el agregado principal: Orden.
2. Establecer el contrato de identidad, tenancy y seguridad.
3. Preparar el patrón de capas y módulos para la evolución incremental.
4. Implementar un flujo de negocio mínimo de creación y confirmación de Orden.

## Nota de arquitectura

La solución está diseñada para crecer con modularidad, bajo acoplamiento y alta cohesión. El agregado Orden será el corazón del sistema y el punto de integración de los módulos operativos.
