using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIS.HTTP
{
    public class HttpServerExeption : Exception
    {
        public HttpServerExeption(string message)
            : base(message)
        {
        }
    }
}
