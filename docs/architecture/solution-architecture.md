# Arquitectura de solución DEXOS

## Objetivo

Diseñar una plataforma SaaS empresarial, multi-tenant, cloud native y preparada para escalar. La idea central es que toda la operación del negocio se origine a partir de la Orden y se reutilice en los módulos de POS, cocina, inventario, caja, contabilidad, CRM, reportes e IA.

## Principios de diseño

- Una única fuente de verdad: la Orden.
- Separación clara entre Dominio, Aplicación, Infraestructura y Presentación.
- Aislamiento de tenants mediante TenantId en todas las entidades relevantes.
- Eventos de dominio para integrar módulos y reducir acoplamiento.
- Seguridad por diseño: autenticación, autorización, auditoría, secretos y trazabilidad.

## Capas propuestas

### 1. Dominio

Responsable de las reglas del negocio, entidades, agregados, value objects y eventos de dominio. No depende de infraestructura ni de frameworks.

### 2. Aplicación

Coordina casos de uso, orquesta servicios, valida entradas y expone contratos de uso para la capa de presentación y otros módulos.

### 3. Infraestructura

Implementa persistencia, integraciones externas, colas, cache, notificaciones y acceso a servicios cloud.

### 4. API

Expone contratos REST y SignalR para clientes web, móviles y servicios internos.

### 5. Aplicaciones de negocio

- Admin
- POS
- Kitchen
- Bar
- PDA
- Kiosk
- QR

## Flujo arquitectónico principal

1. El cliente crea una Orden desde un canal (POS, tablet, kiosk, app o web).
2. La orden pasa por validación de negocio y se registra en el sistema.
3. Se publican eventos de dominio como OrderCreated, OrderConfirmed, OrderCancelled y PaymentAuthorized.
4. Los módulos interesados reaccionan de forma asíncrona o síncrona.
5. La información se reutiliza en inventario, cocina, caja, reporting y CRM.

## Multi-tenancy

Cada tenant debe contar con:

- Usuarios propios
- Productos propios
- Inventario propio
- Clientes propios
- Configuración propia
- Sucursales propias

El modelo lógico se soporte con TenantId en los agregados y entidades transaccionales.

## Seguridad

- JWT y refresh tokens
- HTTPS obligatorio
- BCrypt para contraseñas
- Rate limiting
- Roles y permisos
- Auditoría de operaciones sensibles
- Protección contra SQL injection, XSS, CSRF y replay attacks

## Integración de eventos

Los eventos clave deben incluir:

- OrderCreated
- OrderItemAdded
- OrderConfirmed
- PaymentCaptured
- InventoryReserved
- KitchenPrinted
- OrderCompleted

## Decisión arquitectónica clave

Se recomienda mantener un núcleo de dominio estable y un conjunto de módulos periféricos desacoplados. Esto permite que DEXOS evolucione desde un sistema de restaurante hacia un verdadero sistema operativo empresarial sin romper los contratos de negocio.
