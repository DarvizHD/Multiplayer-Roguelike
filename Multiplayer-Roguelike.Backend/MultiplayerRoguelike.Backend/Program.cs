using System;
using Backend;

var server = new Server(7777);
server.Start();

Console.WriteLine("Press ENTER to shutdown...");
Console.ReadLine();

server.Stop();