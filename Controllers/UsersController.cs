using BET.Domain;
using BET.Domain.Interfaces;
using BETEFSotware.Managers;
using BET.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BETEFSotware.Controllers
{
    [RoutePrefix("Users")]
    public class UsersController : ApiController
    {
        private IBETUnitOfWork _uow;
        private IUsers _userManager;

        public UsersController(IBETUnitOfWork uow)
        {
            _uow = uow;
            _userManager = new UsersManager(_uow);
        }


 
        [Route("registerUses/v1")]
        public ApiResponseModel RegisterUser(UsersModel model)
        {
            var request = _userManager.RegisterUser(model);
            return new ApiResponseModel { IsSuccess = request.IsSuccess, Message = request.Message };
        }



        [HttpGet]
        [Route("login-user/v1/{userName}/{password}")]
        public ApiItemResponseModel<UsersModel> LoginUser(string userName, string password)
        {
            var request = _userManager.LoginUser(userName, password);
            return new ApiItemResponseModel<UsersModel> { IsSuccess = request.IsSuccess, Message = request.Message, Item = request.Item };
        }

    }
}
