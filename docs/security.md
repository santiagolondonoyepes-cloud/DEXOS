# Seguridad y Roles

## Descripción
Marco de seguridad para un SaaS multi-tenant con RBAC, auditoría y trazabilidad.

## Objetivo
- Aplicar RBAC por roles y permisos.
- Garantizar mínimo privilegio y separación de funciones.
- Aislar datos por tenant y registrar eventos sensibles.

## Dependencias
- DEXOS.Identity
- DEXOS.API
- DEXOS.Infrastructure

## Responsabilidades
- Gestionar usuarios y roles.
- Auditar acciones críticas.
- Rastrear eventos en tiempo real.

## Clases / Interfaces / Servicios / Repositorios
- Role
- Permission
- AuditLog
- IIdentityService
- IAuditService

## Casos de uso
- Administrador crea usuario.
- Usuario con rol limitado no accede a otros tenants.
- Auditoría registra cambios de stock o pagos.

## Pruebas
- Falta de permiso debe bloquear acceso.
- Auditoría registra operaciones críticas.
