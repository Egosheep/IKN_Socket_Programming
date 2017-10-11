using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace get_measurement
{
	class get_measurement
	{
		const int PORT = 9000;

		private get_measurement(string[] args)
		{
			IPAddress serverAddress = IPAddress.Parse(args[0]);
			var udpClient = new UdpClient(PORT);
			Console.WriteLine("Client started.");

			IPEndPoint serverEP = new IPEndPoint(serverAddress, PORT);
			Console.WriteLine("Endpoint established.");

			udpClient.Connect(serverEP);
			Console.WriteLine("Connected to server.");

			Byte[] recData;
			switch (args[1].ToLower())
			{
				case "l":
					Byte[] sendL = Encoding.ASCII.GetBytes(args[1]);
					udpClient.Send(sendL, sendL.Length);
					recData = udpClient.Receive(ref serverEP);
					Console.WriteLine(Encoding.ASCII.GetString(recData));
					Console.ReadKey();
					break;

				case "u":
					Byte[] sendU = Encoding.ASCII.GetBytes(args[1]);
					udpClient.Send(sendU, sendU.Length);
					recData = udpClient.Receive(ref serverEP);
					Console.WriteLine(Encoding.ASCII.GetString(recData));
					Console.ReadKey();
					break;

				default:
					Console.WriteLine("U or L, you stupido idioti.");
					break;
			}
		}

		static void Main(string[] args)
		{
			new get_measurement(args);
		}
	}
}
