using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response
{
    public class ShortEventResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime EventDate { get; set; }
        public string Venue { get; set; } = null!;
    }
}
