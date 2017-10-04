using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace tcp
{
	class file_server
	{
		/// <summary>
		/// The PORT
		/// </summary>
		const int PORT = 9000;
		/// <summary>
		/// The BUFSIZE
		/// </summary>
		const int BUFSIZE = 1000;

		/// <summary>
		/// Initializes a new instance of the <see cref="file_server"/> class.
		/// Opretter en socket.
		/// Venter på en connect fra en klient.
		/// Modtager filnavn
		/// Finder filstørrelsen
		/// Kalder metoden sendFile
		/// Lukker socketen og programmet
 		/// </summary>
		private file_server ()
		{
			Byte[] readBuffer = new Byte[BUFSIZE];
			String clientData = null;
			IPAddress localAddress = IPAddress.Parse("10.0.0.1");
			TcpListener serverSocket = new TcpListener(localAddress, PORT);
			serverSocket.Start();

			while(true)
			{
				try
				{
					Console.WriteLine("Waiting for connection...");

					TcpClient client = serverSocket.AcceptTcpClient();
					Console.WriteLine("Client connected!");
					NetworkStream stream = client.GetStream();

					int streamCheck;
					while ((streamCheck = stream.Read(readBuffer, 0, readBuffer.Length)) != 0)
					{
						clientData = LIB.readTextTCP(stream);
					}

					String requestedFile = LIB.extractFileName(clientData);
					long fileLength = LIB.check_File_Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + requestedFile);
					//new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\" + requestedFile).Length;

					SendFile(requestedFile, fileLength, stream);

					client.GetStream().Close();
					client.Close();
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;
				}
			}
		}

		/// <summary>
		/// Sends the file.
		/// </summary>
		/// <param name='fileName'>
		/// The filename.
		/// </param>
		/// <param name='fileSize'>
		/// The filesize.
		/// </param>
		/// <param name='io'>
		/// Network stream for writing to the client.
		/// </param>
		private void SendFile (String fileName, long fileSize, NetworkStream io)
		{
			//Send filesize
			Byte[] size = BitConverter.GetBytes(fileSize);
			try
			{
				io.Write(size, 0, 0);
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				throw;
			}
			io.Flush();

			//Send file
			FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
			try
			{
				fs.CopyTo(io, BUFSIZE);
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
			Console.WriteLine ("Server starts...");
			var server =  new file_server();
		}
	}
}
