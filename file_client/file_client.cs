using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace tcp
{
	class file_client
	{
		/// <summary>
		/// The PORT.
		/// </summary>
		const int PORT = 9000;
		/// <summary>
		/// The BUFSIZE.
		/// </summary>
		const int BUFSIZE = 1000;

		/// <summary>
		/// Initializes a new instance of the <see cref="file_client"/> class.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments. First ip-adress of the server. Second the filename
		/// </param>
		private file_client (string[] args)
		{
			IPAddress localAddress = IPAddress.Parse("10.0.0.2");
			TcpListener clientSocket = new TcpListener(localAddress, PORT);
			clientSocket.Start();
			TcpClient client = new TcpClient(args[0], PORT);
			Console.WriteLine("Connected to server!");

			NetworkStream stream = client.GetStream();
			receiveFile(args[1], stream);
			client.GetStream().Close();
			client.Close();
			clientSocket.Stop();
		}

		/// <summary>
		/// Receives the file.
		/// </summary>
		/// <param name='fileName'>
		/// File name.
		/// </param>
		/// <param name='io'>
		/// Network stream for reading from the server
		/// </param>
		private void receiveFile (String fileName, NetworkStream io)
		{
			FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);

			try
			{
				io.CopyTo(fs);
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				throw;
			}
			fs.Close();
		}

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments.
		/// </param>
		public static void Main (string[] args)
		{
			Console.WriteLine ("Client starts...");
			new file_client(args);
		}
	}
}
