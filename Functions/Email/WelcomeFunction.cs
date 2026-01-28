
using FCG.Shared.Contracts.Events.Domain.Users;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text.Json;

namespace FCG_Functions.Functions.Email
{
    public class WelcomeFunction(ISendGridClient sendGridClient, ILogger<WelcomeFunction> logger)
    {       

        [Function("SendWelcomeEmailFunction")]
        public async Task Run([ServiceBusTrigger(topicName:"users-topic", subscriptionName:"send-welcome-email", Connection = "ServiceBusConnection")] string message)
        {            
            logger.LogInformation($"Mensagem recebida: {message}");

            var user = JsonSerializer.Deserialize<UserCreatedEvent>(message);            
            
            var subject = $"Bem-vindo ao FCG-Games, {user.Name}!";
            var plainTextContent = $"Olá {user.Name}, obrigado por se cadastrar!";
            var htmlContent = $"Olá <strong>{user.Name}</strong>, obrigado por se cadastrar!";
           
            var from = new EmailAddress("alexandre.zordan@outlook.com", "FCG Games");
            var to = new EmailAddress(user!.Email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await sendGridClient.SendEmailAsync(msg); 

            logger.LogInformation($"Email enviado para {user.Email}, status: {response.StatusCode}");
        }

    }
}
