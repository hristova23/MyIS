using System;
using MyIS.HTTP;

namespace DemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var httpServer = new HttpServer(80);
            httpServer.StartAsync().GetAwaiter().GetResult();
        }
    }
}
