# Diseño multi-tenant

## Objetivo

Permitir que múltiples empresas convivan en la misma plataforma sin compartir datos ni permisos.

## Estrategia

- Escalabilidad lógica por TenantId.
- Aislamiento de datos por empresa.
- Configuración por tenant.
- Separación de usuarios, productos, clientes, inventario y sucursales.

## Recomendaciones

- Todas las entidades transaccionales deben incluir TenantId.
- Los permisos deben resolverse a nivel de tenant.
- La base de datos debe estar preparada para particiones lógicas o físicas futuras.
- El middleware de autorización debe validar el tenant activo en cada request.
