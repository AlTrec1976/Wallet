using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Wallet.BLL.Logic.Contracts.Kafka;

namespace Wallet.BLL.Logic.Kafka
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly ILogger<KafkaProducer> _logger;

        public KafkaProducer(ILogger<KafkaProducer> logger)
        {
            _logger = logger;
        }

        public async Task ProduceAsync(string topic, object message)
        {
            try
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = "localhost:9092",
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
