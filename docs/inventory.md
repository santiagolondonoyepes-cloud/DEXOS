# DEXOS.Inventory

## Descripción
Módulo de gestión de inventario orientado a retail y foodservice, preparado para multi-tenant y consumo por órdenes y compras.

## Objetivo
- Gestionar productos con SKU único, precio, unidad de medida y stock.
- Registrar movimientos de stock en Kardex.
- Configurar alertas por stock mínimo/máximo.
- Integrar decrecimiento de stock con ventas y crecimiento con compras/devoluciones.

## Dependencias
- DEXOS.Domain
- DEXOS.Application
- DEXOS.Payments

## Responsabilidades
- Mantener catálogo de productos y stock.
- Persistir movimientos de inventario.
- Emitir alertas cuando el stock supera límites configurados.

## Clases / Interfaces / Servicios / Repositorios
- Producto
- InventoryMovement
- IInventoryRepository
- IInventoryService
- InventoryService

## Casos de uso
- Crear producto
- Actualizar stock
- Registrar compra
- Registrar venta
- Registrar devolución
- Consultar alertas

## Pruebas
- Crear producto con SKU duplicado debe fallar.
- Venta sin stock debe fallar.
- Compra incrementa stock.
