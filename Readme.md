# Kafka - Sample

### Kafka cluster

```bash
> docker-compose up
```

### Producer

```bash
> cd ./src/ConsoleProducerNumbers
> dotnet run
```

### Consumer

```bash
> cd ./src/ConsoleConsumerMusic
> dotnet run
```

```bash
> cd ./src/ConsoleConsumerMath
> dotnet run
```

### Api with kafka

```bash
> cd ./src/ApiWithKafka
> dotnet run
```

### Generate Contracts

```bash
> cd ./src/Contracts
> protoc userMessage.proto --csharp_out .
```
