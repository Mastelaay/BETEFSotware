using BET.Domain;
using BETEFSotware.App_Data;
using BETEFSotware.Extensions;
using BET.Domain.Models;
using System.Linq;
using BET.Domain.Interfaces;

namespace BETEFSotware.Managers
{
    public class UsersManager : IUsers
    {
        private IBETUnitOfWork _uow { get; set; }
        public UsersManager(IBETUnitOfWork uow)
        {
            _uow = uow;
        }
        public ApiItemResponseModel<UsersModel> LoginUser(string userName, string userPassword)
        {
            var repo = _uow.GetRepo<User>();

            var checkUser = repo.GetByIQuerable(s => s.Email == userName).FirstOrDefault().MapProperties<UsersModel>();
            if (checkUser.UserId != 0)
            {
                bool isPasswordMatched = SaltManager.VerifyPassword(userPassword, checkUser.Password, checkUser.UserSalt);
                if (isPasswordMatched)
                {
                    return new ApiItemResponseModel<UsersModel>
                    {
                        Message = "Success Login",
                        IsSuccess = true,
                        Item = checkUser
                    };
                }

            }
            return new ApiItemResponseModel<UsersModel>
            {
                IsSuccess = false,
                Message = "username and password does not match! "
            };
        }

        public ApiResponseModel RegisterUser(UsersModel model)
        {
            var repo = _uow.GetRepo<User>();
            var checkUser = repo.GetByIQuerable(s => s.Email == model.Email && s.Name == model.Name).FirstOrDefault();
            if (checkUser == null)
            {
                var hashSalt = SaltManager.GenerateSaltedHash(64, model.Password);
                model.Password = hashSalt.Hash;
                model.UserSalt = hashSalt.Salt;
                checkUser = model.ToSaveUserLogin();
                repo.Add(checkUser);
                repo.SaveChanges();
            }
            else if (checkUser.UserId != 0)
            {
                bool isPasswordMatched = SaltManager.VerifyPassword(model.Password, checkUser.Password, checkUser.UserSalt);
                if (isPasswordMatched)
                {
                    return new ApiResponseModel
                    {
                        IsSuccess = true,
                        Message = "User already exist"
                    };
                }
            }

            return new ApiResponseModel { IsSuccess = true, Message = "User Saved Successfully" };
        }

        }
    }
