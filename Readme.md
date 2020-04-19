# Kafka - BragaJS

Libraries:

- C/C++: https://github.com/edenhill/librdkafka
- Dotnet: https://github.com/confluentinc/confluent-kafka-dotnet
- NodeJS: https://github.com/SOHU-Co/kafka-node

Desktop Client:

- Conduktor: https://www.conduktor.io/

## Run Kafka

```bash
> docker-compose up
```

## DotNet

### Producer Numbers

```bash
> cd ./src/ConsoleProducerNumbers
> dotnet run
```

### Consumer Numbers

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

## NodeJS

### Consumer

Simple type

```bash
> cd ./src/ConsoleConsumerNode
> npm i
> node app.js
```

With Protobuf

```bash
> cd ./src/ConsoleConsumerNodeObject
> npm i
> node app.js
```

Other commands
```bash
# Stop node processes (Windows)
> taskkill /im node.exe /f
```
