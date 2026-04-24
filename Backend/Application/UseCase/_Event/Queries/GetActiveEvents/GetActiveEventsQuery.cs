using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.UseCase._Event.Queries.GetActiveEvents
{
    public class GetActiveEventsQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public SortEventsBy SortBy { get; set; } = SortEventsBy.Newest;
    }
}
