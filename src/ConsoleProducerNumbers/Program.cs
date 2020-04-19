using System;
using System.Diagnostics;
using System.Text;
using Confluent.Kafka;

namespace ConsoleProducerNumbers
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello Kafka Numbers Producer!");

            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };

            using var producer = new ProducerBuilder<string, string>(config).Build();

            while (true)
            {
                var input = Console.ReadKey().KeyChar.ToString();

                try
                {
                    producer.Produce(
                        "topic-numbers",
                        new Message<string, string>
                        {
                            Value = input,
                            Key = input,
                            Headers = new Headers
                            {
                                new Header("bragajs", Encoding.UTF8.GetBytes("bragajs kafka"))
                            }
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
