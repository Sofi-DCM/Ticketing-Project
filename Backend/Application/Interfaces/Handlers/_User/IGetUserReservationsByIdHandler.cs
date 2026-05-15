

using Application.Response;

namespace Application.Interfaces.Handlers._User
{
    public interface IGetUserReservationsByIdHandler
    {
        public Task<IList<UserReservationResponse>> HandleAsync(int userId, CancellationToken ct);
    }
}
