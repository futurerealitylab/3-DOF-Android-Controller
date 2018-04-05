const dgram = require('dgram');
const server = dgram.createSocket('udp4');

server.on('error', (err) => {
  console.log(`server error:\n${err.stack}`);
  server.close();
});

var PORT = 11000;
var HOST = '192.168.1.70';

server.on('listening', function () {
    var address = server.address();
    console.log('UDP Server listening on ' + address.address + ":" + address.port);
});

server.on('message', function (message, remote) {
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

server.bind(PORT, HOST);