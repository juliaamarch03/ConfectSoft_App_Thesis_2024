using System;
using System.Security;
using System.Windows.Input;
using SweetBakeryWpfApp.Repositories;
using SweetBakeryWpfApp.Model;
using System.Threading;
using System.Security.Principal;
using SweetBakeryWpfApp.View;
using ConfectSoft.View.DailyReportChildsView;
using ConfectSoft.View;

namespace SweetBakeryWpfApp.ViewModel
{
    public class LoginViewModel: ViewModelBase
    {
        //Fields
        private string _username;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible=true;
        private IUserRepository userRepository;

        //Properties
        public string Username 
        {
            get
            {
                return _username; 
            }
            set 
            { 
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public string UserName { get;  set; }
        public string UserSurname { get;  set; }

        public static string UserPosition { get; set; }
        public static string UserInfo { get; set; }
        public static string UserFirstName { get; set; }
        public static string UserLastName { get; set; }
        public static string UserEmailAddress { get; set; }
        public static string UserPhoneNumber { get; set; }
        public static DateTime UserBDay { get; set; }
        public static string UserCountry { get; set; }
        public static string UserPostCode { get; set; }
        public static string UserCity { get; set; }
        public static string UserRegion { get; set; }
        public static string UserSurn { get; set; }
        public static string UserPersonalCode { get; set; }

        public SecureString Password 
        {
            get 
            {
               return _password; 
            }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string ErrorMessage 
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        public bool IsViewVisible 
        {
            get
            {
                return _isViewVisible;
            }
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

        //->Commands
        public ICommand LoginComand { get; }
        public ICommand RecoverPasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }

        //Constructor
        public LoginViewModel()
        {
            userRepository = new UserRepository();
            LoginComand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            RecoverPasswordCommand = new ViewModelCommand(p=>ExecuteRecoverPassCommand("",""));
        }

        private void ExecuteRecoverPassCommand(string username, string email)
        {
            throw new NotImplementedException();
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 6)
                validData = false;
            else
                validData = true;
            return validData;
        }

        private void ExecuteLoginCommand(object obj)
        {
            
            var isValiduser = userRepository.AuthenticateUser(new System.Net.NetworkCredential(Username,Password));
            if (isValiduser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(
                    new GenericIdentity(Username), null);
                    IsViewVisible = false;
                UserName = userRepository.UserName;
                UserSurname = userRepository.UserSurname;

                UserFirstName = userRepository.UserName;
                UserPosition = userRepository.UserPosition;
                UserLastName = userRepository.UserSurname;
                UserEmailAddress = userRepository.UserEmailAddress;
                UserPhoneNumber = userRepository.UserPhoneNumber;
                UserBDay = userRepository.UserBDay;
                UserCountry = userRepository.UserCountry;
                UserPostCode = userRepository.UserPostCode;
                UserCity = userRepository.UserCity;
                UserRegion = userRepository.UserRegion;
                UserPersonalCode = userRepository.UserPersonalCode;


                UserInfo = userRepository.UserName + " " + userRepository.UserSurname;

                if(UserPosition == "Головний кондитер")
                {
                    Dashboard_mainConfectioner mc1 = new Dashboard_mainConfectioner();

                    mc1.userNameData.Text = UserInfo.ToString();
                    mc1.Show();
                }
                if(UserPosition == "Кондитер")
                {
                    Dashboard_mainConfectioner mc2 = new Dashboard_mainConfectioner();

                    mc2.userNameData.Text = UserInfo.ToString();
                    mc2.rdBtnEditData.Visibility = System.Windows.Visibility.Collapsed;
                    mc2.rdBtnRaw.Visibility = System.Windows.Visibility.Collapsed;
                    mc2.Show();
                }
                if(UserPosition == "Admin")
                {
                    Dashboard_Admin mc3 = new Dashboard_Admin();

                    mc3.userNameData.Text = UserInfo.ToString();
                    mc3.Show();
                }
                else
                {
                    ErrorMessage = "Користувач без ролі";
                }

                
            }
            else
            {
                ErrorMessage = "Invalid username or password";
            }
        }
    }
}
