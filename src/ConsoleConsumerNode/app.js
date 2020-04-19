'use strict';

var kafka = require('kafka-node');

console.log('Hello Consumer (NodeJS)');

var options = {
  kafkaHost: 'localhost:9092',
  groupId: 'consumer-node',
  encoding: 'utf8',
  fromOffset: 'earliest',
  outOfRangeOffset: 'earliest'
};

var consumerGroup = new kafka.ConsumerGroup(options, ['topic-numbers']);

consumerGroup.on('message', function (message) {
  console.log(message);
});

consumerGroup.on('error', function (error) {
  console.log(error);
})
