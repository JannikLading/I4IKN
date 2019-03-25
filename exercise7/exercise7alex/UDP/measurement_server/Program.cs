using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace UDP
{
    public class measurement_server
    {
        private const int port = 9000;

        private static void StartListener()
        {

           
            while (true)
            {
				UdpClient udpListener = new UdpClient(port);

                // Needed to read datagram
                IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, port);

                Console.WriteLine("Waiting for clients");
                byte[] bytes = udpListener.Receive(ref groupEP);

                Console.WriteLine($"Received request from {groupEP} :");
                Console.WriteLine($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");

				string clientMsg = Encoding.ASCII.GetString(bytes);

				if (clientMsg.ToString() == "U" || clientMsg.ToString() == "u")
				{
					var buffer = File.ReadAllText("/proc/uptime");
                    
					Console.WriteLine($" Message to send: {buffer}");
					Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    
                    IPAddress clientIp = IPAddress.Parse("10.0.0.2");

                    IPEndPoint ep = new IPEndPoint(clientIp, port);

					byte[] buf = Encoding.ASCII.GetBytes($"{buffer}");

					s.SendTo(buf, ep);
					               
                    Console.WriteLine("Message sent to client");

					s.Close();
     
                }
                else if (clientMsg.ToString() == "l" || clientMsg.ToString() == "L")
                {
					var buffer = File.ReadAllText("/proc/loadavg");

                    Console.WriteLine($" Message to send: {buffer}");
                    // socket to send to
                    Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                    IPAddress clientIp = IPAddress.Parse("10.0.0.2");

                    IPEndPoint ep = new IPEndPoint(clientIp, port);

                    // from string to bytes
                    byte[] buf = Encoding.ASCII.GetBytes($"{buffer}");

                    s.SendTo(buf, ep);

                    Console.WriteLine("Message sent to client");

					s.Close();
                }
				else
                {
                    Console.WriteLine("Illegal request from client");
                }

				udpListener.Close();
            }
           }

        public static void Main()
        {
            StartListener();
        }
    }
}
