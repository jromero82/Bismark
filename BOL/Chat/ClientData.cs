using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Bismark.BOL
{
    public struct ClientData
    {
        public Socket Socket;
        public string User;
        public DateTime ConnectTime;
    }
}
