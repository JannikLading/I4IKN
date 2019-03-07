using System;
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

        private Byte[] bytebuffer = new Byte[BUFSIZE];



        /// <summary>
        /// Initializes a new instance of the <see cref="file_server"/> class.
        /// Opretter en socket.
        /// Venter på en connect fra en klient.
        /// Modtager filnavn
        /// Finder filstørrelsen
        /// Kalder metoden sendFile
        /// Lukker socketen og programmet
        /// </summary>
        private file_server()
        {
            IPAddress IpServ = IPAddress.Parse("10.0.0.1");

            TcpListener serverSocket = new TcpListener(IpServ, PORT);
            TcpClient clientSocket = default(TcpClient);

            


           
                
               
					serverSocket.Start();
					Console.WriteLine(" >> Server Started");
                    clientSocket = serverSocket.AcceptTcpClient();
                    Console.WriteLine(" >> Accept connection from client");
                    NetworkStream networkStream = clientSocket.GetStream();
                    var dataFromClient = LIB.readTextTCP(networkStream);
                    Console.WriteLine(" >> Data from client - " + dataFromClient);

                    //Gets filename
                    String filename = LIB.extractFileName(dataFromClient);




                    //checks if file exist and returns size of file
                    var fileSize = LIB.check_File_Exists(filename);

                    // Sends file or error message
                    if (fileSize != 0)
                    {
                        sendFile(filename, fileSize, networkStream);
                    }
                    else
                    {
                        LIB.writeTextTCP(networkStream, "404 Error file not found");
                        Console.WriteLine("Error file not found");
                    }

                    serverSocket.Stop();
                    clientSocket.Close();
                    networkStream.Close();

                    String serverResponse = "Last Message from client" + dataFromClient;

                    Console.WriteLine(" >> " + serverResponse);
                
                


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
        private void sendFile(String fileName, long fileSize, NetworkStream io)
        {

			LIB.writeTextTCP(io, "File Transfer begins now"); 
            // Sends fileSize to client
            LIB.writeTextTCP(io, fileSize.ToString());

            LIB.SendBytePackages(fileName, bytebuffer,io);
            
           

        }

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name='args'>
        /// The command-line arguments.
        /// </param>
        public static void Main(string[] args)
        {
            Console.WriteLine("Server starts...");
            file_server file = new file_server();
        }
    }
}
