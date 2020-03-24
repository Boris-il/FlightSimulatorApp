using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator
{
    interface IClient
    {
        void connect(string ip, int port);
        void disconnect();
        void write(string commandl);
        string read();
    }
}
