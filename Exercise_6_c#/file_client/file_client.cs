using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace tcp
{
    class file_client
    {
        private long _fileSize;

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
        private file_client(string[] args)
        {
            string host = args[0];
            string fileToRequest = args[1];

            TcpClient clientSocket = new TcpClient();   // Socket is made for the client
            clientSocket.Connect(host, PORT);   // Client socket is connected to the host
            NetworkStream serverStream = clientSocket.GetStream(); // NetworkStream object is made from the connection
            LIB.writeTextTCP(serverStream, fileToRequest);  // Message requesting file is sent to the host


            _fileSize = LIB.getFileSizeTCP(serverStream);

            if (_fileSize > 0) // The first thing the server is to send is the filesize
            {
                receiveFile(fileToRequest, serverStream); // Mangler at blive implementeret
                Console.WriteLine($"File: {fileToRequest} received from server - Filesize: {_fileSize}");
            }
            else
            {
                Console.WriteLine("Filesize was less than 1"); //,from server: " + lineFromServer);
            }

            serverStream.Close();
            clientSocket.Close();
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
        private void receiveFile(String fileName, NetworkStream io)
        {
            FileStream fs = File.Create(fileName);
            int bytesReceived = 0;
            int bytesReceivingNow;
            byte[] buf = new byte[BUFSIZE];

            // Denne skal skiftes hvis der kun læses en byte af gangen
            //string fileRead = LIB.readTextTCP(io);

            // Der skal læses 1000 bytes af gangen. Hvis vi bare brugte LIB.readTextTCP
            while (bytesReceived < _fileSize)
            {
                bytesReceivingNow = io.Read(buf, 0, buf.Length);
                fs.Write(buf, 0, bytesReceivingNow); // buf is the bytes being written, bytesReceivingNow is the
                                                     // size of the bytes being  written

                bytesReceived += bytesReceivingNow;
            }


            //byte[] data = new UTF8Encoding(true).GetBytes(fileRead);
            //fs.Write(data, 0, data.Length);

            // Skal nok benytte noget filestream io til at gemme filen. Tjek apollos :^)
            fs.Close();

        }

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name='args'>
        /// The command-line arguments.
        /// </param>
        public static void Main(string[] args)
        {
            Console.WriteLine("Client starts...");
            new file_client(args);
        }
    }
}
