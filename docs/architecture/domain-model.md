# Modelo de dominio DEXOS

## Agregado principal: Orden

La Orden es el agregado raíz del sistema. Todo el contexto operativo nace desde ella.

### Entidades principales

- Order
- OrderItem
- Customer
- Product
- Category
- Table
- Branch
- Tenant
- Payment
- Invoice

### Value Objects

- Money
- Address
- ContactInfo
- OrderStatus
- PaymentStatus

## Reglas de negocio iniciales

- Una Orden pertenece a un Tenant y a una Sucursal.
- Una Orden puede tener múltiples ítems.
- Un ítem debe referenciar un producto válido.
- Una Orden no puede pasar de Confirmed a Draft.
- El total de la orden se calcula a partir de los ítems y los impuestos.

## Integración de módulos

- POS: crea y gestiona la Orden.
- Cocina: recibe eventos de preparación.
- Inventario: reserva stock al confirmar.
- Caja: aborda pagos y cierres.
- Contabilidad: registra movimientos contables.
- CRM: enlaza al cliente y su historial.
- Reporting/IA: consumen eventos y métricas.
