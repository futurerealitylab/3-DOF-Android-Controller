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

//
// Summary:
//   Began = 1,  A finger touched the screen.
//   Moved = 2,   A finger moved on the screen.
//   Stationary = 3,  A finger is touching the screen but hasn't moved.
//  Ended = 4,   A finger was lifted from the screen. This is the final phase of a touch.
//  Canceled = 5   The system cancelled tracking for the touch.

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