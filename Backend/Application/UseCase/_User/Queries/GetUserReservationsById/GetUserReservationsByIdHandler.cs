using Application.Interfaces.Handlers._User;
using Application.Interfaces.Repositories;
using Application.Response;
using Domain.Exceptions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.UseCase._User.Queries.GetUserReservationsById
{
    public class GetUserReservationsByIdHandler : IGetUserReservationsByIdHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IReservationRepository _reservationRepository;

        public GetUserReservationsByIdHandler(IReservationRepository reservationRepository, IUserRepository userRepository)
        {
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
        }

        public async Task<IList<UserReservationResponse>> HandleAsync(int userId, CancellationToken ct)
        {
            if (userId <= 0)
                throw new ArgumentException("Los id deben ser positivos");

            if (!await _userRepository.ExistsByIdAsync(userId, ct))
                throw new NotFoundException($"No existe un usuario con id: {userId}");

            return await _reservationRepository.GetReservationsByUserIdAsync(userId, ct);
        }
    }
}
