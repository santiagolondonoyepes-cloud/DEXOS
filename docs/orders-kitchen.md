# DEXOS.Orders + DEXOS.Kitchen

## Descripción
Motor de comandas digital para órdenes pagadas, con distribución por estaciones y notificaciones en tiempo real.

## Objetivo
- Generar comandas automáticamente cuando una orden es pagada.
- Distribuir por cocina, barra y pastelería.
- Actualizar estados PREPARING → READY → DELIVERED.
- Notificar a POS, cocina y web del tenant usando SignalR.

## Dependencias
- DEXOS.Domain
- DEXOS.Payments
- DEXOS.Inventory

## Responsabilidades
- Crear comandas desde órdenes pagadas.
- Asignar estación y prioridad.
- Notificar cambios de estado.

## Clases / Interfaces / Servicios / Repositorios
- KitchenOrder
- KitchenStation
- IKitchenOrderRepository
- IKitchenService
- KitchenService

## Casos de uso
- Crear comanda desde orden pagada.
- Marcar comanda en preparación.
- Marcar comanda lista.
- Marcar comanda entregada.

## Pruebas
- Orden no pagada no genera comanda.
- Comanda cambia a READY cuando termina preparación.
