using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetBakeryWpfApp.Model
{
    public class UserService
    {
        private UserModel _currentUser;

        public UserModel CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }
    }
}
