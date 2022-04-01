using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIS.HTTP
{
    public class HttpResponse
    {
        public HttpResponse(HttpResponseCode statusCode, byte[] body)
        {
            this.Version = HttpVersionType.Http11;
            this.StatusCode = statusCode;
            this.Headers = new List<Header>();
            this.Body = body;

            if (body != null && body.Length > 0)
            {
                this.Headers.Add(new Header("Content-Lenght", body.Length.ToString()));
            }
        }
        public HttpVersionType Version { get; set; }
        public HttpResponseCode StatusCode { get; set; }
        public IList<Header> Headers { get; set; }
        public byte[] Body { get; set; }
        public override string ToString()
        {
            var responseAsString = new StringBuilder();
            string httpVersionAsString = "HTTP/1.1";
            switch (this.Version)
            {
                case HttpVersionType.Http10:
                    httpVersionAsString = "HTTP/1.0";
                    break;
                case HttpVersionType.Http11:
                    httpVersionAsString = "HTTP/1.1";
                    break;
                case HttpVersionType.Http20:
                    httpVersionAsString = "HTTP/2.0";
                    break;
            }

            responseAsString.Append($"{httpVersionAsString} {(int)this.StatusCode} {this.StatusCode}" + HttpConstants.NewLine);
            foreach (var header in this.Headers)
            {
                responseAsString.Append(header.ToString() + HttpConstants.NewLine);
            }
            responseAsString.Append(HttpConstants.NewLine);

            return responseAsString.ToString();
        }
    }
}
