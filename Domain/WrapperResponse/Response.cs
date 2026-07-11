using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.WrapperResponse
{
    public class Response<T>
    {
        public bool Success { get; init; }

        public string Message { get; init; } = string.Empty;

        public T? Data { get; init; }

        public IEnumerable<string>? Errors { get; init; }
    }
}
