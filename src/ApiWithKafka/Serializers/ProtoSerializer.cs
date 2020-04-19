using System.Collections.Generic;
using Confluent.Kafka;
using Google.Protobuf;

namespace ApiWithKafka.Serializers
{
    /// <summary>
    /// Protobuf tutorial https://www.matthowlett.com/2018-05-31-protobuf-kafka-dotnet.html
    /// </summary>
    public class ProtoSerializer<T> : ISerializer<T> where T : IMessage<T>
    {
        public IEnumerable<KeyValuePair<string, object>>
            Configure(IEnumerable<KeyValuePair<string, object>> config, bool isKey)
                => config;

        public void Dispose()
        {
        }

        public byte[] Serialize(T data, SerializationContext context)
            => data.ToByteArray();
    }
}
