namespace Mpc.KafakMusic.ConsoleAppConsumer
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using NAudio.Wave;

    public static class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Hello Kafka Music Player!");

            var conf = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(conf).Build();
            consumer.Subscribe("kafkasound-topic");

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

                        Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");

                        PlayAsync(cr.Value);
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

        private static async Task PlayAsync(string input)
        {
            var file = GetSoundFile(input);

            using var audioFile = new AudioFileReader(@"soundfiles\" + file);
            using var outputDevice = new WaveOutEvent();
            outputDevice.Init(audioFile);
            outputDevice.Play();
            while (outputDevice.PlaybackState == PlaybackState.Playing)
            {
                await Task.Delay(10);
            }
        }

        private static string GetSoundFile(string input)
        {
            return input switch
            {
                "1" => "do-stretched.wav",
                "2" => "re-stretched.wav",
                "3" => "mi-stretched.wav",
                "4" => "fa-stretched.wav",
                "5" => "sol-stretched.wav",
                "6" => "la-stretched.wav",
                "7" => "si-stretched.wav",
                _ => "do-stretched-octave.wav",
            };
        }
    }
}
