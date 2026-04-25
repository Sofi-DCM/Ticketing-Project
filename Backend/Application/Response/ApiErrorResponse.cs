using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = null!;
    }
}
