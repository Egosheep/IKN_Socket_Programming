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
                string dataString = Encoding.ASCII.GetString(data);
			        
                Console.WriteLine("Recieved: " + dataString+ " from client.");
                
				//Get choice from client
				switch (dataString.ToLower())
				{

					case "l":
						Byte[] sendLoadData = File.ReadAllBytes("/proc/loadavg");
						udpServer.Send(sendLoadData, sendLoadData.Length);
						break;

					case "u":
						Byte[] sendUptimeData = File.ReadAllBytes("/proc/uptime");
						udpServer.Send(sendUptimeData, sendUptimeData.Length);
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
