using System;
using System.IO.Ports;

/// <summary>
/// Link.
/// </summary>
namespace Linklaget
{
	/// <summary>
	/// Link.
	/// </summary>
	public class Link
	{
		/// <summary>
		/// The DELIMITE for slip protocol.
		/// </summary>
		const byte DELIMITER = (byte)'A';
		/// <summary>
		/// The buffer for link.
		/// </summary>
		private byte[] buffer;
		/// <summary>
		/// The serial port.
		/// </summary>
		SerialPort serialPort;

		/// <summary>
		/// Initializes a new instance of the <see cref="link"/> class.
		/// </summary>
		public Link (int BUFSIZE, string APP)
		{
			// Create a new SerialPort object with default settings.
            
			#if DEBUG
				if(APP.Equals("FILE_SERVER"))
				{
					serialPort = new SerialPort("/dev/ttySn0",115200,Parity.None,8,StopBits.One);
				}
				else
				{
					serialPort = new SerialPort("/dev/ttySn1",115200,Parity.None,8,StopBits.One);
				}
			#else
				serialPort = new SerialPort("/dev/ttyS1",115200,Parity.None,8,StopBits.One);
			#endif

            
			serialPort = new SerialPort("/dev/ttyS1", 115200, Parity.None, 8, StopBits.One);
			if(!serialPort.IsOpen)
				serialPort.Open();

			buffer = new byte[(BUFSIZE*2)];

			// Uncomment the next line to use timeout
			//serialPort.ReadTimeout = 500;

			serialPort.DiscardInBuffer ();
			serialPort.DiscardOutBuffer ();
		}

		/// <summary>
		/// Send the specified buf and size.
		/// </summary>
		/// <param name='buf'>
		/// Buffer.
		/// </param>
		/// <param name='size'>
		/// Size.
		/// </param>
		public void send (byte[] buf, int size)
		{
			buffer[0] = DELIMITER;
			var j = 1;

            for (int i = 0; i < size; i++)
			{     
                switch (buf[i])
				{
					case (byte)'A':
						buffer[j++] = (byte)'B';
                        buffer[j++] = (byte)'C';
						break;

					case (byte)'B':
						buffer[j++] = (byte)'B';
                        buffer[j++] = (byte)'D';
						break;
					
					default:
						buffer[j++] = buf[i];
						break;
				}
			}

			buffer[j] = DELIMITER;

			serialPort.Write(buffer, 0, buffer.Length);
		}

		/// <summary>
		/// Receive the specified buf and size.
		/// </summary>
		/// <param name='buf'>
		/// Buffer.
		/// </param>
		/// <param name='size'>
		/// Size.
		/// </param>
		public int receive (ref byte[] buf)
		{
			var buffer = new byte[buf.Length];
			int byteReceived;
			var i = 0;

            while (true)
			{
				byteReceived = serialPort.ReadByte();
				if (byteReceived == (-1))
					break;

				buf[i++] = (byte)byteReceived;
                
			}

			i = 0;
			var j = 0; 

			if (buf[i++]==(byte)'A')
			{
				while(buf[i]!=(byte)'A')
				{
					if (buf[i] == (byte)'B')
					{
						i++;

						if (buf[i] == (byte)'C')
							buffer[j++] = (byte)'A';

						if (buf[i] == (byte)'D')
							buffer[j++] = (byte)'B';
					}
					else { buffer[j++] = buf[i]; }

					i++;
				}
			}

			buffer.CopyTo(buf, 0);

			return j;
		}
	}
}
