using System;
using System.Collections.Generic;
using Confluent.Kafka;
using Google.Protobuf;

namespace ApiWithKafka.Serializers
{
    /// <summary>
    /// Protobuf tutorial https://www.matthowlett.com/2018-05-31-protobuf-kafka-dotnet.html
    /// </summary>
    public class ProtoDeserializer<T> : IDeserializer<T>
        where T : Google.Protobuf.IMessage<T>, new()
    {
        private MessageParser<T> _parser;

        public ProtoDeserializer()
        {
            _parser = new MessageParser<T>(() => new T());
        }

        public IEnumerable<KeyValuePair<string, object>>
            Configure(IEnumerable<KeyValuePair<string, object>> config, bool isKey)
                => config;

        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
            => _parser.ParseFrom(data.ToArray());

        public void Dispose()
        {
        }
    }
}
