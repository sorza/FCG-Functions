# FCG-Functions

Este repositório contém as **Azure Functions** utilizadas pelo ecossistema **FCG Games**, responsáveis por processar eventos publicados nos microsserviços (ex.: `Users`, `Payments`) e executar ações assíncronas como envio de e-mails de boas-vindas e notificações de status de pagamento.

---

## Visão Geral

- **Serverless**: implementado em Azure Functions, escalando automaticamente conforme a demanda.
- **Event-driven**: consome eventos publicados em tópicos do **Azure Service Bus**.
- **Desacoplado**: cada função executa uma responsabilidade específica (ex.: envio de e-mail), sem acoplamento direto aos microsserviços.
- **Integração com SendGrid**: envio de e-mails transacionais para usuários.

---

## Estrutura

- `WelcomeFunction`  
  Escuta o tópico `users-topic` e envia e-mail de boas-vindas para novos usuários.

- *(futuro)* `PaymentNotificationFunction`  
  Escutará o tópico `payments-topic` e enviará e-mails de atualização de status de pagamento.

---

## Configuração

### Variáveis de Ambiente (App Settings no Azure)

- `AzureWebJobsStorage` → Storage Account usada pelo runtime do Azure Functions.
- `ServiceBusConnection` → Connection string do Azure Service Bus.
- `SendGridApiKey` → Chave da API do SendGrid para envio de e-mails.

### Local Development

No arquivo `local.settings.json`:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "ServiceBusConnection": "<sua-connection-string>",
    "SendGridApiKey": "<sua-api-key>"
  }
}
