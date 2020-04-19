# Kafka - Sample

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

```bash
> cd ./src/ConsoleConsumerNode
> npm i
> node app.js
```

```bash
# Stop node processes (Windows)
> taskkill /im node.exe /f
```
