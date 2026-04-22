using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response
{
    public class PagedEventsResponse
    {
        public ICollection<ShortEventResponse>? Events { get; set; }
        public int TotalEventsInBd { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = null!;
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}
