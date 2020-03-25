using System;
using System.Net.Sockets;

namespace FlightSimulator
{
    interface IClient
    {
        void connect(string ip, int port);
        void write(string command);
        string read();
        void disconnect();
    }

    class MyTelnetClient : IClient
    {
        // tcp client
        private TcpClient tcp_client;
        // data stream
        NetworkStream stream;

        //public MyTelnetClient() { }

        public void connect(string ip, int port)
        {
            try
            {
                this.tcp_client = new TcpClient(ip, port);
                this.stream = tcp_client.GetStream();
                Console.WriteLine("Establishing Connection");
                this.tcp_client.Connect(ip, port);
                Console.WriteLine("Connected");
            }
            catch
            {
                Console.WriteLine("Connection Error");
            }
        }

        public void disconnect()
        {
            this.stream.Close();
            this.tcp_client.Close();
        }

        public string read()
        {
            // Buffer to store the response bytes.
            Byte[] inData = new byte[256];
            // String to store the response ASCII representation.
            String responseData = String.Empty;
            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(inData, 0, inData.Length);
            responseData = System.Text.Encoding.ASCII.GetString(inData, 0, bytes);
            Console.WriteLine("Received: {0}", responseData);
            return responseData;
        }

        public void write(string command)
        {
            // Translate the passed message into ASCII and store it as a Byte array
            Byte[] outData = System.Text.Encoding.ASCII.GetBytes(command);
            // Send the message to the connected TcpServer. 
            this.stream.Write(outData, 0, outData.Length);

            Console.WriteLine("Sent: {0}", outData);
        }
    }
}
