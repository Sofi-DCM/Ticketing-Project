using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase._Event.Commands.CreateEvent
{
    public class CreateEventCommand
    {
        public int UserId { get; set; }
        public string name { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Venue { get; set; } = string.Empty;
        //public string status { get; set; }??

    }
}
