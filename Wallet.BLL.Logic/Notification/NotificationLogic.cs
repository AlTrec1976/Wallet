using Confluent.Kafka;
using EmailService.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Wallet.BLL.Logic.Contracts.Kafka;
using Wallet.BLL.Logic.Contracts.Notififcation;
using Wallet.Common.Entities.KafkaModels;

namespace Wallet.BLL.Logic.Notification
{
    public class NotificationLogic : INotificationLogic
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly ILogger<NotificationLogic> _logger;
        private readonly IOptions<Kafka> _options;

        public NotificationLogic(IHttpClientFactory httpClientFactory,
            IKafkaProducer kafkaProducer,
            ILogger<NotificationLogic> logger,
            IOptions<Kafka> options)
        {
            _httpClientFactory = httpClientFactory;
            _kafkaProducer = kafkaProducer;
            _logger = logger;
            _options = options;
        }

        public async Task SendAsync(EmailServiceMessage message)
        {
            try
            {
                await _kafkaProducer.ProduceAsync(_options.Value.Topic, message);
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
