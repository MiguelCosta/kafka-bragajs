using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace ConsoleConsumerMath
{
    public static class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Hello Kafka Math Player!");

            var conf = new ConsumerConfig
            {
                GroupId = "consumer-math",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(conf).Build();
            consumer.Subscribe("topic-numbers");

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };

            try
            {
                while (true)
                {
                    try
                    {
                        var cr = consumer.Consume(cts.Token);

                        Console.WriteLine($"Consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");

                        if (int.TryParse(cr.Message.Value, out var number))
                        {
                            var result = number * number + number + 50;
                            Console.WriteLine($"Result: {result}");
                        }
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Ensure the consumer leaves the group cleanly and final offsets are committed.
                consumer.Close();
            }
        }
    }
}
