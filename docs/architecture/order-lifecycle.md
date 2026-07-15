# Ciclo de vida de la Orden

## 1. Creación

El usuario inicia una orden desde un canal habilitado. El sistema asigna TenantId, SucursalId y una referencia temporal.

## 2. Adición de ítems

Los ítems se agregan a la Orden. La solución valida disponibilidad, precio, impuestos y reglas de negocio.

## 3. Confirmación

Al confirmar la Orden, el sistema:

- reserva inventario
- activa la preparación en cocina o barra
- genera un evento de negocio
- calcula montos finales

## 4. Pago

La Orden puede ser pagada en el momento o diferida. El módulo de pagos valida el método, genera la autorización y actualiza el estado.

## 5. Finalización

Una vez cerrada la Orden, se integran los módulos de caja, contabilidad, CRM y reporting.
