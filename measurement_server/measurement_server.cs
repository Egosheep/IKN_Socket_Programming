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

                        String LoadData = File.ReadAllText("/proc/loadavg");
					    Byte[] sendLoadData = Encoding.ASCII.GetBytes(LoadData);

                        Console.WriteLine("Loadavg retrieved");
						udpServer.Send(sendLoadData, sendLoadData.Length);
					    Console.WriteLine("Data sent");

                        break;

					case "u":
					    udpServer.Connect(clientEP); //Establish connection to client 
					    Console.WriteLine("Connected to client.");

                        Byte[] sendUptimeData = File.ReadAllBytes("/proc/uptime");
					    Console.WriteLine("uptime retrieved");

                        udpServer.Send(sendUptimeData, sendUptimeData.Length);
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
