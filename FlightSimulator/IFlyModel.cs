using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator;

namespace FlightSimulator
{
    interface IFlyModel
    {
        void connect(string ip, int port);
    }

    class MyFlyModel : IFlyModel
    {
        IClient telnetClient;
        volatile Boolean stop;

        public void connect(string ip, int port)
        {
            // todo
        }
    }
}
