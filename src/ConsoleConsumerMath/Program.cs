using System;
using System.Threading;
using Confluent.Kafka;

namespace ConsoleConsumerMath
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello Kafka Calculator!");

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

                        if (int.TryParse(cr.Message.Value, out var number))
                        {
                            var result = 9 * number;
                            Console.WriteLine($"9 * {number} = {result}");
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
