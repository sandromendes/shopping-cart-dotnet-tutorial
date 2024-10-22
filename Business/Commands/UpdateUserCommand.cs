using Domain.Infrastructure.Interfaces;
using Domain.Models;
using Domain.Transfer;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Business.Commands
{
    public class UpdateUserCommand : IRequest<UserDTO>
    {
        public Guid UserId { get; }
        public UserDTO UserDto { get; }

        public UpdateUserCommand(Guid userId, UserDTO userDto)
        {
            UserId = userId;
            UserDto = userDto;
        }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UpdateUserCommandHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.UserId);
            if (existingUser == null)
                return null;

            existingUser.Username = request.UserDto.Username;
            existingUser.Role = request.UserDto.Role;
            existingUser.PasswordHash = _passwordHasher.HashPassword(existingUser, request.UserDto.Password);

            await _userRepository.UpdateAsync(existingUser);

            return new UserDTO
            {
                Id = existingUser.Id,
                Username = existingUser.Username,
                Role = existingUser.Role,
                Password = existingUser.PasswordHash
            };
        }
    }
}
