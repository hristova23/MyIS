using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyIS.HTTP
{
    public class HttpServer : IHttpServer
    {
        private readonly TcpListener tcpListener;
        //TODO: Actions
        public HttpServer(int port)
        {
            this.tcpListener = new TcpListener(IPAddress.Loopback, port);
        }

        public async Task StartAsync()
        {
            this.tcpListener.Start();
            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(() => ProcessClientAsync(tcpClient));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }

        private async Task ProcessClientAsync(TcpClient tcpClient)
        {
            using (NetworkStream networkStream = tcpClient.GetStream())
            {
                //Process request
                byte[] requestBytes = new byte[1000000];//TODO: use buffer
                int bytesRead = await networkStream.ReadAsync(requestBytes, 0, requestBytes.Length);
                string requestAsString = Encoding.UTF8.GetString(requestBytes, 0, bytesRead);

                var request = new HttpRequest(requestAsString);

                //Create response text
                string content = "<h1>Home Page</h1>";

                if (request.Path == "/Login")
                {
                    content = @"<form action='Account/Login' method='post'> 
<input type=date name='date' />
<input type=text name='username' />
<input type=password name='password' /> 
<input type=submit value='Login' /> 
</form>";
                }
                else if(request.Path == "/About")
                {
                    content = "<h1>About Page</h1>";
                }

                byte[] contentAsBytes = Encoding.UTF8.GetBytes(content);
                var response = new HttpResponse(HttpResponseCode.Ok, contentAsBytes);
                response.Headers.Add(new Header("Server", "MyServer/1.0"));
                response.Headers.Add(new Header("Content-Type", "text/html"));

                byte[] responseBytes = Encoding.UTF8.GetBytes(response.ToString());
                await networkStream.WriteAsync(responseBytes, 0, responseBytes.Length);
                await networkStream.WriteAsync(response.Body, 0, response.Body.Length);

                //Print
                Console.WriteLine("Request: " + new string('=', 71));
                Console.WriteLine(request);
                Console.WriteLine("Response: " + new string('=', 70));
                Console.WriteLine(response.ToString());
                Console.WriteLine(new string('*', 80));
            }
        }

        public async Task ResetAsync()
        {
            this.Stop();
            await this.StartAsync();
        }

        public void Stop()
        {
            this.tcpListener.Stop();
        }
    }
}
