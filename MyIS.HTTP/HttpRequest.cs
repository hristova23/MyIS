using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIS.HTTP
{
    public class HttpRequest
    {
        public HttpRequest(string httpRequestAsString)
        {
            this.Headers = new List<Header>();

            var lines = httpRequestAsString.Split(
                new string[] { HttpConstants.NewLine },
                StringSplitOptions.None);
            var httpInfoHeader = lines[0];
            var infoHeadersParts = httpInfoHeader.Split(' ');
            if (infoHeadersParts.Length!=3)
            {
                throw new HttpServerExeption("Invalid HTTP header line.");
            }

            var httpMethod = infoHeadersParts[0];

            switch (httpMethod)
            {
                case"GET":
                    this.Method = HttpMethodType.Get;
                    break;
                case "POST":
                    this.Method = HttpMethodType.Post;
                    break;
                case "PUT":
                    this.Method = HttpMethodType.Put;
                    break;
                case "DELETE":
                    this.Method = HttpMethodType.Delete;
                    break;
                default:
                    this.Method = HttpMethodType.Unknown;
                    break;
            }
            this.Path = infoHeadersParts[1];
            switch (infoHeadersParts[2])
            {
                case"HTTP/1.0":
                    this.Version = HttpVersionType.Http10;
                    break;
                case "HTTP/1.1":
                    this.Version = HttpVersionType.Http11;
                    break;
                case "HTTP/2.0":
                    this.Version = HttpVersionType.Http20;
                    break;
                default:
                    this.Version = HttpVersionType.Http11;
                    break;
            }

            bool isInHeader = true;
            StringBuilder bodyStrBuilder = new StringBuilder();

            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                {
                    isInHeader = false;
                    continue;
                }

                if (isInHeader)
                {
                    var headerParts = line.Split(
                        new string[] { ": " }, 
                        2, 
                        StringSplitOptions.None);
                    if (headerParts.Length!=2)
                    {
                        throw new HttpServerExeption($"Invalid header: {line}");
                    }

                    var header = new Header(headerParts[0], headerParts[1]);
                    this.Headers.Add(header);
                }
                else
                {
                    bodyStrBuilder.AppendLine(line);
                }
            }

            this.Body = bodyStrBuilder.ToString();
        }
        public HttpMethodType Method {get;set;}
        public string Path { get; set; }
        public HttpVersionType Version { get; set; }
        public IList<Header> Headers { get; set; }
        public string Body { get; set; }
    }
}
