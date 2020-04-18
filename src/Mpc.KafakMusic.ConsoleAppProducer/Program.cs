using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Mpc.KafakMusic.ConsoleAppProducer
{
    public static class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Hello Kafka Music Producer!");

            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            using var producer = new ProducerBuilder<string, string>(config).Build();

            while (true)
            {
                var input = Console.ReadKey().KeyChar.ToString();

                try
                {
                    producer.ProduceAsync(
                        "kafkasound-topic",
                        new Message<string, string>
                        {
                            Value = input,
                            Key = input
                        });
                }
                catch (ProduceException<Null, string> e)
                {
                    Debug.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
        }
    }
}
