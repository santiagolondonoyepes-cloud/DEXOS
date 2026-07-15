# Seguridad DEXOS

## Controles recomendados

- Autenticación con JWT y refresh tokens.
- Autorización por roles y permisos a nivel de tenant.
- HTTPS obligatorio para toda comunicación.
- Contraseñas con hash BCrypt.
- Rate limiting para endpoints públicos y sensibles.
- Auditoría y trazabilidad para acciones críticas.
- Protección contra inyección SQL, XSS, CSRF y ataques de replay.

## Diseño multi-tenant seguro

- Todas las consultas deben filtrar por TenantId.
- Los usuarios nunca deben cruzar el límite de tenant.
- Los secretos y credenciales deben gestionarse en Azure Key Vault.
- Los logs deben incluir tenant, usuario, acción y resultado.
