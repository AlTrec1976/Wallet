namespace Wallet.BLL.Logic.Contracts.Kafka
{
    public interface IKafkaProducer
    {
        Task ProduceAsync(string topic, object messenge);
    }
}
