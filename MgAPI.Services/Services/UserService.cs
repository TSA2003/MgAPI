using MgAPI.Business.Services.Interfaces;
using MgAPI.Data;
using MgAPI.Data.Entities;
using MgAPI.Services.Authorization;
using MgAPI.Services.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using BCryptNet = BCrypt.Net.BCrypt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MgAPI.Business.JSONModels;
using MgAPI.Data.Repositories;

namespace MgAPI.Business.Services
{
    public class UserService : IUserService
    {
        private UserRepository _repository;
        private IJwtUtils _jwtUtils;
        private readonly AppSettings _appSettings;

        public UserService(UserRepository repository, IJwtUtils jwtUtils, IOptions<AppSettings> appSettings)
        {
            _repository = repository;
            _jwtUtils = jwtUtils;
            _appSettings = appSettings.Value;
        }


        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _repository.Read(x => x.Username == model.Username);

            // validate
            if (user == null || !BCryptNet.Verify(model.Password, user.PasswordHash))
                throw new AppException("Username or password is incorrect");

            // authentication successful so generate jwt token
            var jwtToken = _jwtUtils.GenerateJwtToken(user);

            return new AuthenticateResponse(user, jwtToken);
        }

        public IEnumerable<User> GetAll()
        {
            return _repository.ReadAll();
        }

        public User GetById(string id)
        {
            var user = _repository.Read(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }

        public User Create(CreateUserRequest model)
        {
            User user = new User
            {
                ID = Guid.NewGuid().ToString(),
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Username = model.Username,
                Email = model.Email,
                CreationDate = DateTime.Now,
                Role = (Role)Enum.Parse(typeof(Role), "Moderator"),
                PasswordHash = BCryptNet.HashPassword(model.Password)
            };

            _repository.Create(user);

            return user;
        }

        public User Edit(EditUserRequest model)
        {
            User user = _repository.Read(x => x.ID == model.ID);
            user.Firstname = model.Firstname;
            user.Lastname = model.Lastname;
            user.Username = model.Username;
            user.Email = model.Email;
            //user.PasswordHash = BCryptNet.HashPassword(model.Password);

            _repository.Update(user);

            return user;
        }

        public void ChangePassword(string id, ChangePasswordRequest model)
        {
            User user = _repository.Read(x => x.ID == id);

            if (user.PasswordHash == BCryptNet.HashPassword(model.OldPassword))
            {
                user.PasswordHash = BCryptNet.HashPassword(model.NewPassword);
                _repository.Update(user);
            }
            else
            {
                throw new ArgumentException("Wrong password!");
            }
        }

        public void Delete(string id)
        {
            var user = _repository.Read(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            _repository.Delete(id);
        }

    }
}
