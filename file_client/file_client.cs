using System;
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
			//Creating a socket for the TCPClient
			TcpClient SocketForClient = new TcpClient();
			//Used the the underlying stream of data for network access
			NetworkStream networkStream;
			//String that contains the file requested
			string fileRequest ="";
			string hostName = "";
			string serverResponse = "";

			hostName = args [0];
			fileRequest = args [1];

			SocketForClient.Connect (hostName, PORT);
			networkStream = SocketForClient.GetStream();

			LIB.writeTextTCP (networkStream, fileRequest);
			serverResponse = LIB.readTextTCP (networkStream);

			if (serverResponse == "File Transfer begins now")
				receiveFile (fileRequest, networkStream);
			else if (serverResponse == "Error sending file")
				Console.WriteLine (serverResponse);
			else
				Console.WriteLine ("Something went wrong");
				
			networkStream.Close ();
			SocketForClient.Close ();
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
			string fileSize;
			byte[] receiveBuffer = new byte[BUFSIZE];
			int bytesToReceive = 0;
			int bytesReceived = 0;
			string nameOfFile = "";
			FileStream fileStream;

			fileSize = LIB.readTextTCP (io);
			Console.WriteLine ($"File size {fileSize}");
			if (fileName.Contains ("/"))
				nameOfFile = fileName.Substring (fileName.LastIndexOf ('/') + 1, fileName.Length - fileName.LastIndexOf ('/') - 1);
			else
				nameOfFile = fileName;
			fileStream = new FileStream (nameOfFile, FileMode.Create, FileAccess.Write);

			while (Convert.ToInt32(fileSize) != bytesReceived)
			{
				bytesToReceive = io.Read (receiveBuffer, 0, receiveBuffer.Length);
				fileStream.Write(receiveBuffer, 0, bytesToReceive);
				bytesReceived += bytesToReceive;
			}

			fileStream.Close();
			io.Close();
		}

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments.
		/// </param>
		public static void Main (string[] args)
		{
			if (args.Length == 2) 
			{
				Console.WriteLine ("Client starts...");
				new file_client (args);
			}
			else 
			{
				Console.WriteLine ("Not enough arguments: ./file_client.exe [IP] [FILE]");
			}
		}
	}
}
