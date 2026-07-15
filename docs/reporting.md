# DEXOS.Reporting

## Descripción
Módulo de reportes y dashboards para ventas, inventario, clientes y trazabilidad operativa.

## Objetivo
- Exponer métricas en tiempo real.
- Generar reportes descargables en PDF o Excel.
- Integrar trazabilidad con Azure Monitor y Application Insights.

## Dependencias
- DEXOS.Domain
- DEXOS.Inventory
- DEXOS.CRM
- DEXOS.Payments

## Responsabilidades
- Consolidar métricas.
- Publicar dashboards.
- Emitir reportes exportables.

## Clases / Interfaces / Servicios / Repositorios
- DashboardMetric
- ReportRequest
- IReportingService
- ReportingService

## Casos de uso
- Ver ventas del día
- Exportar inventario
- Exportar clientes

## Pruebas
- Reporte devuelve datos agregados.
- Exportación genera archivo.
