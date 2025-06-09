using System;
using System.Collections.Generic;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public interface IUserDataAccess
    {
        List<UserModel> GetAllUsers();
        List<UserModel> GetAdmins();
        UserModel GetUserById(int id);
        UserModel GetUserByEmail(string email);
        UserModel InsertUser(UserModel user);
        bool UpdateUser(int id, UserModel user);
        bool UpdateCredentials(int id, string email, string passwordHash);
        bool DeleteUser(int id);
    }
}
