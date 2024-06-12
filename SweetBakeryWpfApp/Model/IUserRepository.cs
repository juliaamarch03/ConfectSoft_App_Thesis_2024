using System;
using System.Collections.Generic;
using System.Net;

namespace SweetBakeryWpfApp.Model
{
    public interface IUserRepository
    {
        string UserSurname { get; }
        string UserName { get; }
        string UserEmailAddress { get; }
        string UserPosition { get; }
        string UserPhoneNumber { get; }
        DateTime UserBDay { get; }
        string UserCountry { get; }
        string UserPostCode { get; }
        string UserCity { get; }
        string UserRegion { get; }
        string UserPersonalCode { get; }

        bool AuthenticateUser(NetworkCredential credential);
        void Add(UserModel userModel);
        void Edit(UserModel userModel);
        void Remove(int id);
        UserModel GetById(int id);
        
        IEnumerable<UserModel> GetByAll();
    }
}
