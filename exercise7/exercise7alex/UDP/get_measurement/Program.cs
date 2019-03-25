using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace get_measurement
{
	class get_measurement
	{
		private const int port = 9000;

		public static void Main(string[] args)
		{
			// socket to send to
			Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			IPAddress serverIp = IPAddress.Parse(args[0]);

			// Socket to receive ready
			UdpClient udpListener = new UdpClient(port);
			IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, port);

			// from string to bytes
			byte[] sendbuf = Encoding.ASCII.GetBytes(args[1]);
			IPEndPoint ep = new IPEndPoint(serverIp, port);

			s.SendTo(sendbuf, ep);

			Console.WriteLine("Message sent to server");

			//Actually read
			Console.WriteLine("Before listening");
			byte[] bytes = udpListener.Receive(ref groupEP);
			Console.WriteLine("After listening");
			Console.WriteLine($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");

			udpListener.Close();

		}
	}
}