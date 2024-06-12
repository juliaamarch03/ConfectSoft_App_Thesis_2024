using MySql.Data.MySqlClient;
using SweetBakeryWpfApp.Repositories;
using SweetBakeryWpfApp.ViewModel;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConfectSoft.View
{
    public partial class UserProfileView : UserControl
    {
        public UserProfileView()
        {
            InitializeComponent();

            bindUsersInfo();
            bindUserProfilePic(profilePicEllipse, LoginViewModel.UserInfo);

            bindStatusActivity("Відкрито сторінку 'Профіль покристувача'");
        }

        public void bindUserProfilePic(Ellipse userEllipse, string userName)
        {
            // Перевірка імені користувача
            switch (userName)
            {
                case "Таня Шевченко":
                    userEllipse.Fill = new ImageBrush(new BitmapImage(new Uri("D:/OTHER/University/4 курс/Дипломна робота/Практична реалізація/WPF APP/SweetBakeryWpfApp/Images/UsersPics/122032.jpg")));
                    break;
                case "Аня Трачук":
                    userEllipse.Fill = new ImageBrush(new BitmapImage(new Uri("D:/OTHER/University/4 курс/Дипломна робота/Практична реалізація/WPF APP/SweetBakeryWpfApp/Images/UsersPics/120120.jpg")));
                    break;
                case "Давид Лукінчук":
                    userEllipse.Fill = new ImageBrush(new BitmapImage(new Uri("D:/OTHER/University/4 курс/Дипломна робота/Практична реалізація/WPF APP/SweetBakeryWpfApp/Images/UsersPics/100101.jpg")));
                    break;
                // додайте більше випадків за потреби
                default:
                    userEllipse.Fill = new ImageBrush(new BitmapImage(new Uri("D:/OTHER/University/4 курс/Дипломна робота/Практична реалізація/WPF APP/SweetBakeryWpfApp/Images/UsersPics/000.png")));
                    break;
            }
        }


        private void bindUsersInfo() 
        {
            //First row load data
            string confectioner = LoginViewModel.UserInfo;
            string user_role = LoginViewModel.UserPosition;
            string location = LoginViewModel.UserCity + ", " + LoginViewModel.UserRegion + ", " + LoginViewModel.UserCountry;

            //Second row load data
            string firstName = LoginViewModel.UserFirstName;
            string lastName = LoginViewModel.UserLastName;
            string userEmail = LoginViewModel.UserEmailAddress;
            string userBDay = LoginViewModel.UserBDay.ToString("dd.MM.yyyy");
            string userPhoneNumber = LoginViewModel.UserPhoneNumber;

            //Third row load data
            string country = LoginViewModel.UserCountry;
            string postCode = LoginViewModel.UserPostCode;
            string fullLocation = LoginViewModel.UserCity + ", " + LoginViewModel.UserRegion;


            //First row set data
            userFullNameLabel.Content = null;
            userPositionLabel.Content = null;
            userLocationLabel.Content = null;

            userFullNameLabel.Content = confectioner;
            userPositionLabel.Content = user_role;
            userLocationLabel.Content = location;

            //Second row set data
            userFirstNameLabel.Content = null;
            userEmailAdressLabel.Content = null;
            userBDayLabel.Content = null;
            userLastNameLabel.Content = null;
            userPhoneNumberLabel.Content = null;

            userFirstNameLabel.Content = firstName;
            userEmailAdressLabel.Content = userEmail;
            userBDayLabel.Content = userBDay;
            userLastNameLabel.Content = lastName;
            userPhoneNumberLabel.Content = userPhoneNumber;

            //Third row set data
            userCountryLabel.Content = null;
            userPostCodeLabel.Content = null;
            userLocationSecLabel.Content = null;

            userCountryLabel.Content = country;
            userPostCodeLabel.Content = postCode;
            userLocationSecLabel.Content = fullLocation;
        }


        // ___________________ USER ACTIVITY ___________________

        // method to bind status activity
        private void bindStatusActivity(string _actionTitle)
        {
            string _usersUsername = LoginViewModel.UserPersonalCode;
            string _userFullname = LoginViewModel.UserInfo;
            string _actionTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            DB_Connect db = new DB_Connect();

            using (db)
            {
                string query = $"INSERT INTO `users_actions` (`action_title`, `users_username`, " +
                    $"`user_fullname`, `action_time`) VALUES (@actionTitle, @usersUsername, @userFullname, @actionTime);";
                MySqlCommand command = new MySqlCommand(query, db.getConnection());

                command.Parameters.AddWithValue("@actionTitle", _actionTitle);
                command.Parameters.AddWithValue("@usersUsername", _usersUsername);
                command.Parameters.AddWithValue("@userFullname", _userFullname);
                command.Parameters.AddWithValue("@actionTime", _actionTime);

                db.openConnection();

                command.ExecuteNonQuery();

                db.closeConnection();
            }
        }
    }
}
