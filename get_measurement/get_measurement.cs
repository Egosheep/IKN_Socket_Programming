using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

		    try
		    {
		        IPAddress serverAddress = IPAddress.Parse(args[0]);

		        var udpClient = new UdpClient(PORT);
		        Console.WriteLine("Client started.");

		        IPEndPoint serverEP = new IPEndPoint(serverAddress, PORT);
		        Console.WriteLine("Endpoint established.");

		        udpClient.Connect(serverEP);
		        Console.WriteLine("Connected to server.");

		        Byte[] sendLetter;
		        Byte[] recData;
                switch (args[1].ToLower())
                {
                    case "l":
                        sendLetter = Encoding.ASCII.GetBytes(args[1]);
                        udpClient.Send(sendLetter, sendLetter.Length);
                        Console.WriteLine(args[1] + " sendt til server");

                        recData = udpClient.Receive(ref serverEP);
                        Console.WriteLine("Information fra server modtaget:");
                        Console.WriteLine(Encoding.ASCII.GetString(recData));
                        break;

                    case "u":
                        sendLetter = Encoding.ASCII.GetBytes(args[1]);
                        udpClient.Send(sendLetter, sendLetter.Length);
                        Console.WriteLine(args[1] + " sendt til server");

                        recData = udpClient.Receive(ref serverEP);
                        Console.WriteLine("Information fra server modtaget:");
                        Console.WriteLine(Encoding.ASCII.GetString(recData));
                        break;

                    default:
                        Console.WriteLine("U or L, you stupido idioti.");
                        break;
                }
            }
		    catch (System.FormatException)
		    {
		        Console.WriteLine($"Felj: Den indtastede IP adresse: {args[0]} har forkert format.");
		    }
		    catch (System.Net.Sockets.SocketException)
		    {
		        Console.WriteLine($"Fejl: Kunne ikke oprette forbindelse til {args[0]}, indtast en valid IP adresse.");
		    }
        }

		static void Main(string[] args)
		{
			new get_measurement(args);
		}
	}
}
