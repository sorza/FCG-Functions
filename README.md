# ⚡ FCG-Functions

Azure Functions — Processamento serverless event-driven para FCG Games.

[![Azure Functions](https://img.shields.io/badge/Azure-Functions-0078D4?logo=microsoft-azure)](https://azure.microsoft.com/services/functions/)
[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Serverless](https://img.shields.io/badge/Architecture-Serverless-yellow)](https://azure.microsoft.com/)
[![Event-Driven](https://img.shields.io/badge/Pattern-Event--Driven-orange)](https://docs.microsoft.com/azure/architecture/)

## 📝 Descrição

**FCG-Functions** executa tarefas assíncronas serverless:

- ✅ **WelcomeEmailFunction**: Envia email de boas-vindas (trigger: UserCreatedEvent)
- ✅ **PaymentNotificationFunction**: Notifica sobre status de pagamento (trigger: PaymentProcessedEvent)
- ✅ **Auto-scaling**: Escala de 0 a milhares de instâncias automaticamente
- ✅ **Sem overhead**: Pague apenas pelos milissegundos executados

---

## 🚀 Pré-requisitos

- Azure Functions Core Tools v4
- .NET 8 SDK
- Azure CLI
- Conta Azure com Service Bus e SendGrid ativados

---

## 🏗️ Estrutura

```
Functions/
├── Email/
│   ├── WelcomeEmailFunction.cs      → Trigger: UserCreatedEvent
│   └── PaymentNotificationFunction.cs → Trigger: PaymentProcessedEvent
```

---

## ⚙️ Configuração Local

**local.settings.json**:
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "ServiceBusConnection": "<connection-string>",
    "SendGridApiKey": "<api-key>"
  }
}
```

---

## 🚀 Como Executar

### Local
```bash
cd FCG-Functions
func start
```

### Deploy no Azure
```bash
az login
func azure functionapp publish fcg-functions
func azure functionapp logstream fcg-functions
```

---

## 🧪 Testes

```bash
# Publicar evento no Service Bus
az servicebus topic message send \
  --resource-group fcg-microsservices \
  --namespace-name fcg-azure-servicebus \
  --topic-name users-topic \
  --body '{
    "userId":"123",
    "email":"test@example.com",
    "createdAt":"2026-01-09T00:00:00Z"
  }'

# Monitorar logs
func start --verbose
```

---

## 🐳 Docker

```dockerfile
FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0

ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

COPY publish /home/site/wwwroot
```

---

## ☸️ Kubernetes

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: fcg-functions
spec:
  replicas: 1
  selector:
    matchLabels:
      app: fcg-functions
  template:
    metadata:
      labels:
        app: fcg-functions
    spec:
      containers:
      - name: fcg-functions
        image: fcg-functions:latest
        env:
        - name: ServiceBusConnection
          valueFrom:
            secretKeyRef:
              name: servicebus-secret
              key: connection-string
        - name: SendGridApiKey
          valueFrom:
            secretKeyRef:
              name: sendgrid-secret
              key: api-key
```

---

## 📊 Monitoramento

- **Application Insights**: Métricas, logs estruturados
- **Execution Count**: Número de execuções
- **Execution Duration**: Tempo de processamento
- **Failure Rate**: Taxa de falhas/retries
- **Dead Letter Messages**: Mensagens problemáticas

---

## 📚 Referências

- [Azure Functions Docs](https://docs.microsoft.com/azure/azure-functions/)
- [Service Bus Triggers](https://docs.microsoft.com/azure/azure-functions/functions-bindings-service-bus-trigger)
- [SendGrid .NET SDK](https://github.com/sendgrid/sendgrid-csharp)

---

**FIAP Tech Challenge — Fase 4**
