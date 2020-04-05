using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

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
            //try
            //{
                this.tcp_client = new TcpClient(ip, port);
               // set recieve timeout 10 seconds
                tcp_client.ReceiveTimeout = 5000;
                Console.WriteLine("Establishing Connection");
                Console.WriteLine("Server Connected");
                this.stream = tcp_client.GetStream();
            //}
            /*catch
            {
                Console.WriteLine("Connection Error");
            }*/
        }

        public void disconnect()
        {
            this.stream.Close();
            this.tcp_client.Close();
            Console.WriteLine("Server Disconnected");
        }

        public string read()
        {
            // Buffer to store the response bytes.
            byte[] inData = new byte[256];
            // String to store the response ASCII representation.
            String responseData;
            // Read the first batch of the TcpServer response bytes.
            int bytes = stream.Read(inData, 0, inData.Length);
            responseData = Encoding.ASCII.GetString(inData, 0, bytes);
            //Console.WriteLine("Received: {0}", responseData);
            return responseData;
        }

        public void write(string command)
        {
            //Console.WriteLine(command);
            // Translate the passed message into ASCII and store it as a Byte array
            byte[] outData = new byte[1024];
            outData = Encoding.ASCII.GetBytes(command);
            // Send the message to the connected TcpServer. 
            this.stream.Write(outData, 0, outData.Length);

            //Console.WriteLine("Sent: {0}", outData);
        }
    }
}
