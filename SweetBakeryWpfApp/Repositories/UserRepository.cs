using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SweetBakeryWpfApp.Model;
using MySql.Data.MySqlClient;

namespace SweetBakeryWpfApp.Repositories
{
    class UserRepository : RepositoryBase, IUserRepository
    {
        public string UserName { get; private set; }
        public string UserSurname { get; private set; }
        public string UserPosition { get; private set; }
        public string UserEmailAddress { get; private set; }
        public string UserPhoneNumber { get; private set; }
        public DateTime UserBDay { get; private set; }
        public string UserCountry { get; private set; }
        public string UserPostCode { get; private set; }
        public string UserCity { get; private set; }
        public string UserRegion { get; private set; }
        public string UserPersonalCode { get; private set; }

        public void Add(UserModel userModel)
        {
            throw new NotImplementedException();
        }
        
        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from `users` where `username`=@username and `user_password`=@password";
                command.Parameters.Add("@username", MySqlDbType.Int32).Value = credential.UserName;
                command.Parameters.Add("@password", MySqlDbType.VarChar).Value = credential.Password;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        UserName = reader["user_name"].ToString();
                        UserSurname = reader["user_surname"].ToString();
                        UserPosition = reader["user_role"].ToString();
                        UserEmailAddress = reader["user_email"].ToString();
                        UserPhoneNumber = reader["user_phone"].ToString();
                        UserBDay = DateTime.Parse(reader["user_birthday"].ToString());
                        UserCountry = reader["user_country"].ToString();
                        UserPostCode = reader["user_postal_code"].ToString();
                        UserCity = reader["user_city"].ToString();
                        UserRegion = reader["user_region"].ToString();
                        UserPersonalCode = reader["username"].ToString();
                        validUser = true;
                    }
                    else
                    {
                        validUser = false;
                    }
                }
            }
            return validUser;
        }
        public void Edit(UserModel userModel)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<UserModel> GetByAll()
        {
            throw new NotImplementedException();
        }
        public UserModel GetById(int id)
        {
            throw new NotImplementedException();
        }
        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
