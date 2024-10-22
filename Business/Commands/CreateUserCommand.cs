using Domain.Infrastructure.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Business.Commands
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public CreateUserCommand(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }

    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public CreateUserHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Username = request.Username,
                Role = "User" // Definir um papel padrão
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            await _userRepository.CreateAsync(user);
            return user.Id;
        }
    }
}
