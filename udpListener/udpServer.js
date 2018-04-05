const dgram = require('dgram');
const udpServer = dgram.createSocket('udp4');

udpServer.on('error', (err) => {
  console.log(`udpServer error:\n${err.stack}`);
  udpServer.close();
});

var PORT = 11000;
var HOST = '192.168.1.70';

udpServer.on('listening', function () {
    var address = udpServer.address();
    console.log('UDP udpServer listening on ' + address.address + ":" + address.port);
});

udpServer.on('message', function (message, remote) {
    console.log(remote.address + ':' + remote.port +' - ' + message + message.length);
	if(message.length != 24)
		return;
	var index = 0;
    var rx = message.readFloatLE(index);
	index += 4;
	var ry = message.readFloatLE(index);
	index += 4;
	var rz = message.readFloatLE(index);
	index += 4;
	var state = message.readInt32LE(index);
	index += 4;
	var x = message.readFloatLE(index);
	index += 4;
	var y = message.readFloatLE(index);
	console.log(rx,ry,rz,state,x,y);
});

udpServer.bind(PORT, HOST);