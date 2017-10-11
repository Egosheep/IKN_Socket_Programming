using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace measurement_server
{
	class measurement_server
	{
		const int PORT = 9000;

		private measurement_server()
		{
			//IPAddress localAddress = IPAddress.Parse("10.0.0.1");
			var udpServer = new UdpClient(PORT);
			Console.WriteLine("Server started.");

			while (true)
			{
				IPEndPoint clientEP = new IPEndPoint(IPAddress.Any, 0);
                Console.WriteLine("Client created.");
				Console.WriteLine("Awaiting input:");

                Byte[] data = udpServer.Receive(ref clientEP);
                String dataString = Encoding.ASCII.GetString(data);
                Console.WriteLine("Recieved: " + dataString+ " from client.");
                
                //Get choice from client
				switch (dataString.ToLower())
				{

					case "l":
					    udpServer.Connect(clientEP); //Establish connection to client
					    Console.WriteLine("Connected to client.");

                        String loadavgData = File.ReadAllText("/proc/loadavg");
					    Byte[] loadavgDataBytes = Encoding.ASCII.GetBytes(loadavgData);
                        Console.WriteLine("Loadavg retrieved");

						udpServer.Send(loadavgDataBytes, loadavgDataBytes.Length);
					    Console.WriteLine("Data sent");

                        break;

					case "u":
					    udpServer.Connect(clientEP); //Establish connection to client 
					    Console.WriteLine("Connected to client.");
                        
                        String uptimeData = File.ReadAllText("/proc/uptime");
					    Byte[] uptimeDataBytes = Encoding.ASCII.GetBytes(uptimeData);
                        Console.WriteLine("uptime retrieved");

                        udpServer.Send(uptimeDataBytes, uptimeDataBytes.Length);
					    Console.WriteLine("Data sent");
						break;

					default:
						Console.WriteLine("Received invalid data.");
						break;
				}
			}
		}

		static void Main(string[] args)
		{
			new measurement_server();
		}
	}
}
