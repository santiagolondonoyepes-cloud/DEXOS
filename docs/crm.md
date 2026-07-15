# DEXOS.CRM

## Descripción
Módulo de clientes y relaciones con el negocio para fidelización, segmentación y marketing.

## Objetivo
- Gestionar clientes y contacto.
- Mantener historial de compras y preferencias.
- Preparar segmentación y fidelización con puntos, descuentos y promociones.

## Dependencias
- DEXOS.Domain
- DEXOS.Application
- DEXOS.Payments

## Responsabilidades
- Crear y actualizar clientes.
- Registrar eventos de compra.
- Programar campañas por segmento.

## Clases / Interfaces / Servicios / Repositorios
- Customer
- LoyaltyProgram
- CustomerSegment
- ICustomerRepository
- ICustomerService

## Casos de uso
- Crear cliente
- Consultar historial
- Asignar segmento
- Aplicar promoción

## Pruebas
- Cliente sin email debe rechazarse.
- Segmento se actualiza al comprar.
