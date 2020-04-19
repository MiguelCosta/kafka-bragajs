'use strict';

var kafka = require('kafka-node');
var protobuf = require('protocol-buffers');
var fs = require('fs');

console.log('Hello Consumer Object (NodeJS)');

var options = {
  kafkaHost: 'localhost:9092',
  groupId: 'consumer-node-object',
  keyEncoding: 'buffer',
  encoding: 'buffer',
  fromOffset: 'earliest',
  outOfRangeOffset: 'earliest'
};

var consumerGroup = new kafka.ConsumerGroup(options, ['topic-users']);

var messagesProto = protobuf(fs.readFileSync('../Contracts/userMessage.proto'));

consumerGroup.on('message', function (message) {
  var user = messagesProto.UserMessage.decode(message.value);
  console.log(user);
});

consumerGroup.on('error', function (error) {
  console.log(error);
})
