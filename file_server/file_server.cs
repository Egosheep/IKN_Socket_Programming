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
		    IPAddress localAddress = IPAddress.Any; 
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
					Console.WriteLine("Stream created.");

					clientData = LIB.readTextTCP(stream);
					Console.WriteLine("Read data from client.");

					String requestedFile = LIB.extractFileName(clientData);
					Console.WriteLine("Extracted " + requestedFile + "from client.");

                    //til filer på vilkårlige placeringer
				    long FileLength = LIB.check_File_Exists(clientData);

                    if (FileLength > 0) //tjekker om filen findes på den givne sti
				    {
				        Console.WriteLine($"Fuld sti:{clientData}" +
				                          $"\nstørrelse:{FileLength}");

                        LIB.writeTextTCP(stream, FileLength.ToString()); //sender størrelsen på filen til client
				        SendFile(clientData,stream);
				        Console.WriteLine("File sent.");
                    }
                    else
                    {
                        LIB.writeTextTCP(stream, 0.ToString()); //hvis filen ikke findes skrives der blot 0 til clienten
                    }
          
                    client.GetStream().Close();
					Console.WriteLine("Close stream");
					client.Close();
					Console.WriteLine("Close client");
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
		private void SendFile (String fileName,/* long fileSize, */NetworkStream io)
		{
			//Send filesize
            /*
			try
			{
				Console.WriteLine("Sending filesize.");
				LIB.writeTextTCP(io, fileSize.ToString());
				Console.WriteLine("Sent filesize"+ fileSize);
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				throw;
			}
            */

			//Send file
			FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
			try
			{
				Console.WriteLine("Sending filestream over stream.");
				fs.CopyTo(io, BUFSIZE);
				Console.WriteLine("Sent filestream.");
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				throw;
			}
			fs.Close();
			Console.WriteLine("Filestream closed.");
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
