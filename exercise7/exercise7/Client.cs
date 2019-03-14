using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP
{
    public class Client
    {
		static void Main(string[] args)
        {
            UDPSocket s = new UDPSocket();
            s.Server("10.0.0.2", 9000);

            UDPSocket c = new UDPSocket();
            c.Client("10.0.0.1", 9000);
            c.Send("TEST!");

            Console.ReadKey();
        }
    }
}
