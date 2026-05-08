
using Domain.Constants;

namespace Application.UseCase._Event.Queries.GetActiveEvents
{
    public class GetActiveEventsQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public SortEventsBy SortBy { get; set; } = SortEventsBy.Newest;
    }
}
