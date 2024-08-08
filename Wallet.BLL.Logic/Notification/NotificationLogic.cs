using Confluent.Kafka;
using EmailService.Contracts;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Wallet.BLL.Logic.Contracts.Kafka;
using Wallet.BLL.Logic.Contracts.Notififcation;
using Wallet.Common.Entities.HttpClientts;

namespace Wallet.BLL.Logic.Notification
{
    public class NotificationLogic : INotificationLogic
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly ILogger<NotificationLogic> _logger;

        public NotificationLogic(IHttpClientFactory httpClientFactory,
            IKafkaProducer kafkaProducer,
            ILogger<NotificationLogic> logger)
        {
            _httpClientFactory = httpClientFactory;
            _kafkaProducer = kafkaProducer;
            _logger = logger;
        }

        public async Task SendAsync(EmailServiceMessage message)
        {
            try
            {
                await _kafkaProducer.ProduceAsync("email.service.topic", message);
                _logger.LogInformation("Сообщение отправлено успешно.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
