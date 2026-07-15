# DEXOS.Payments

## Objetivo
Este módulo encapsula la integración de pagos para DEXOS con un enfoque Clean Architecture y principios SOLID. La intención es que el dominio y los casos de uso no dependan de Stripe o Checkout.com directamente, sino de una abstracción de pagos preparada para multi-tenant.

## Componentes principales
- IPaymentService: contrato para iniciar, confirmar y procesar webhooks de pagos.
- PaymentRequest / PaymentConfirmationRequest / PaymentWebhookRequest: modelos neutrales para cada tenant y proveedor.
- SimulatedPaymentService: adapter stub que simula confirmación de pago para permitir desarrollo y pruebas sin integrar una pasarela real.

## Flujo de negocio
1. La orden se crea y se confirma.
2. El sistema entra en estado PENDING_PAYMENT.
3. El servicio de pagos genera un intento simulado y devuelve una referencia.
4. El webhook o confirmación posterior marca la orden como PAID.
5. Solo entonces la orden puede avanzar a SENT_TO_KITCHEN.

## Diseño
- El dominio conserva la regla de negocio: ninguna orden entra a cocina sin estar PAID.
- La capa de aplicación orquesta el flujo entre repositorio y servicio de pagos.
- El módulo es extensible para agregar un adapter real de Stripe o Checkout.com por tenant en el futuro sin modificar el contrato central.
