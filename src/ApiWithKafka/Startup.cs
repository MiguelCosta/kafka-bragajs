using System;
using System.Threading;
using System.Threading.Tasks;
using ApiWithKafka.Serializers;
using Confluent.Kafka;
using Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApiWithKafka
{
    public class Startup
    {
        private IConsumer<int, UserMessage> _consumer;
        private Task _backgroundTask;
        private static IProducer<int, UserMessage> _producerUserMessage;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Add kafka producer
            ConfigKafkaProducer(services);
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IHostApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseHttpsRedirection()
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            // Add kafka consumer
            applicationLifetime.ApplicationStarted.Register(this.ConfigKafkaConsumer);

            // Dispose kafka producer and consumer
            applicationLifetime.ApplicationStopping.Register(() =>
            {
                _producerUserMessage.Flush();
                _producerUserMessage.Dispose();
                _consumer.Dispose();
                _backgroundTask.Dispose();
            });
        }

        private static void ConfigKafkaProducer(IServiceCollection services)
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            _producerUserMessage = new ProducerBuilder<int, UserMessage>(config)
                .SetValueSerializer(new ProtoSerializer<UserMessage>())
                .Build();

            services.AddSingleton(_producerUserMessage);
        }

        protected virtual void ConfigKafkaConsumer()
        {
            var conf = new ConsumerConfig
            {
                GroupId = "consumer-user",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            _consumer = new ConsumerBuilder<int, UserMessage>(conf)
                .SetValueDeserializer(new ProtoDeserializer<UserMessage>())
                .Build();
            _consumer.Subscribe("topic-users");

            _backgroundTask = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    try
                    {
                        var cr = _consumer.Consume();

                        Console.WriteLine($"Consumed message '{cr.Message.Key}' at: '{cr.TopicPartitionOffset}'.");

                        var user = new User
                        {
                            Id = cr.Message.Value.Id,
                            Name = cr.Message.Value.Name
                        };

                        DataBase.Users.Add(user);
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    }
                }
            },
            CancellationToken.None,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default);
        }
    }
}
