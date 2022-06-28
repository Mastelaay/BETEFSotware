using BET.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BET.Domain.Interfaces
{
    public interface IUsers
    {
        /// <summary>
        /// login user with email and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        ApiItemResponseModel<UsersModel> LoginUser(string userName, string userPassword);

        /// <summary>
        ///  Register users pass in userModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ApiResponseModel RegisterUser(UsersModel model);
    }
}

