using Listener;
using System;

namespace SOAHttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string hostIp = "127.0.0.1";
            string port;

            port = Console.ReadLine();

            var httpListener = new HttpListenerImpl(hostIp, port);
            httpListener.Start();
        }
    }
}
