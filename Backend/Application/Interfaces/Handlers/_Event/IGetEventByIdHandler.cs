using Application.Response;

namespace Application.Interfaces.Handlers._Event
{
    public interface IGetEventByIdHandler
    {
        public Task<ShortEventResponse> HandleAsync(int id);
    }
}
