using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commom
{
    public class APIExceptions : Exception
    {
        public int StatusCode { get; }
        public string? ErrorCode { get; }

        public APIExceptions(string message, int statusCode = 500, string? errorCode = null)
            : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }
}
