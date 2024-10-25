using ForumApp.Repositories;
using BenzForum.Helpers;
using BenzForum.Models;
using BenzForum.Data.ModelsIn;
using BenzForum.Data.Factory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ForumApp.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<DBUser> _userRepository;
        private readonly IJwtUtils _jwtUtils;
        private readonly IModelFactory<DBUser> _modelFactory;
        private readonly ILogger<UserService> _logger;

        public UserService(IRepository<DBUser> userRepository, IJwtUtils jwtUtils, IModelFactory<DBUser> modelFactory, ILogger<UserService> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _jwtUtils = jwtUtils ?? throw new ArgumentNullException(nameof(jwtUtils));
            _modelFactory = modelFactory ?? throw new ArgumentNullException(nameof(modelFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Register(AuthRequest registerUser)
        {
            try
            {
                // Check if the user already exists
                var existingUser = await _userRepository.Get(u => u.Username == registerUser.Username);
                if (existingUser != null)
                {
                    _logger.LogWarning("User already exists: {Username}", registerUser.Username);
                    throw new InvalidOperationException("User already exists.");
                }

                // Hash password and save user
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerUser.Password);

                var dbUser = _modelFactory.Create();
                dbUser.Username = registerUser.Username;
                dbUser.PasswordHash = passwordHash;

                await _userRepository.Add(dbUser);
                _logger.LogInformation("User registered successfully: {Username}", registerUser.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering a user.");
                throw;
            }
        }

        public async Task<string?> Login(AuthRequest loginUser)
        {
            try
            {
                var user = await _userRepository.Get(u => u.Username == loginUser.Username);
                if (user == null || !BCrypt.Net.BCrypt.Verify(loginUser.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Invalid login attempt for user: {Username}", loginUser.Username);
                    return null;
                }

                var token = _jwtUtils.GenerateToken(user);
                _logger.LogInformation("User logged in successfully: {Username}", loginUser.Username);
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in a user.");
                throw;
            }
        }
    }
}

