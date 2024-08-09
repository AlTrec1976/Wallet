using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Wallet.BLL.Logic.Contracts.Kafka;
using Wallet.Common.Entities.KafkaModels;

namespace Wallet.BLL.Logic.KafkaService
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly ILogger<KafkaProducer> _logger;
        private readonly IOptions<Kafka> _options;


        public KafkaProducer(ILogger<KafkaProducer> logger, IOptions<Kafka> options)
        {
            _logger = logger;
            _options = options;
        }

        public async Task ProduceAsync(string topic, object message)
        {
            try
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = _options.Value.Connection,
                };

                using (var producer = new ProducerBuilder<Null, string>(config).Build())
                {
                    var jsonMsg = JsonSerializer.Serialize(message);
                    var result = await producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonMsg });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при отправки сообщения в брокер");
                throw;
            }
        }
    }
}
