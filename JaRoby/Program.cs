using System.Net.Sockets;
using System.Net;
using System.Text;


var ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.2.17"), 2131);

using Socket client = new(
    ipEndPoint.AddressFamily,
    SocketType.Stream,
    ProtocolType.Tcp);

await client.ConnectAsync(ipEndPoint);

var buffer = new byte[1_024];
var received = await client.ReceiveAsync(buffer, SocketFlags.None);

var initMessage = Encoding.UTF8.GetString(buffer, 0, received);
Console.WriteLine($"Message received: \"{initMessage}\"");


// Login Send message.
string message = "LOGIN,01,OPERATEUR,1";
var messageBytes = Encoding.UTF8.GetBytes(message);
await client.SendAsync(messageBytes, SocketFlags.None);
Console.WriteLine($"Sent request {@message}");

received = await client.ReceiveAsync(buffer, SocketFlags.None);
var response = Encoding.UTF8.GetString(buffer, 0, received);
Console.WriteLine($"Message received: \"{response}\"");


while (true)
{
    if(!client.Connected)
    { break; }

    message = @"[001, R02,, BNQ, BFRN, 0000000ASS,/ BTYP == 4,/ BCHQ =="" "", BNUM, BDAT, +BGLB, BNOM, BMNT, +BFRN]";
    // Send message.
    messageBytes = Encoding.UTF8.GetBytes(message);
    await client.SendAsync(messageBytes, SocketFlags.None);
    Console.WriteLine($"Sent request {@message}");

    received = await client.ReceiveAsync(buffer, SocketFlags.None);
    response = Encoding.UTF8.GetString(buffer, 0, received);
    Console.WriteLine($"Message received: \"{response}\"");

    
}






