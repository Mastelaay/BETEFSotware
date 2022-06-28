using BETEFSotware.App_Data;
using BET.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BETEFSotware.Extensions
{
    public static class UsersExtension
    {
        public static User ToSaveUserLogin(this UsersModel model)
        {
            var userLogin = new User()
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                CreatedDate = DateTime.Now,
                UserSalt = model.UserSalt,
                IsActive = true,

            };
            return userLogin;
        }

        public static UsersModel ToReturnUserLogin(this UsersModel entity)
        {
            if (entity == null) return null;

            return new UsersModel
            {
                Name = entity.Name,
                Email = entity.Email,
                Password = entity.Password,
                CreatedDate = entity.CreatedDate,
                UserSalt = entity.UserSalt,
                IsActive = true,
                UserId = entity.UserId,
            };
        }
    }
}