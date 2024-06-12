using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MySql.Data.MySqlClient;
using SweetBakeryWpfApp.Repositories;
using SweetBakeryWpfApp.View.UsedRawDataChildsView;
using SweetBakeryWpfApp.ViewModel;

namespace SweetBakeryWpfApp.View
{
    public partial class UsedRawView : UserControl
    {
        public UsedRawView()
        {
            InitializeComponent();
            CurrViewChild.Content = new UsedRawInfoView_UR();

            bindStatusActivity("Відкрито сторінку 'Сировина'");
        }

        private void UsedRawRadioButton_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((RadioButton)e.Source).Uid);
            GridCursor.Margin = new Thickness(10 + (200 * index), 0, 0, 350);

            usedRawPageBtn.Foreground = Brushes.Black;

            BrushConverter bc = new BrushConverter();
            Brush brush = (Brush)bc.ConvertFrom("#8F8F8F");
            obtainingRawPageBtn.Foreground = brush;

            CurrViewChild.Content = null;
            CurrViewChild.Content = new UsedRawInfoView_UR();
        }

        private void ObtainingRawRadioButton_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((RadioButton)e.Source).Uid);
            GridCursor.Margin = new Thickness(10 + (200 * index), 0, 0, 350);

            obtainingRawPageBtn.Foreground = Brushes.Black;

            BrushConverter bc = new BrushConverter();
            Brush brush = (Brush)bc.ConvertFrom("#8F8F8F");
            usedRawPageBtn.Foreground = brush;

            CurrViewChild.Content = null;
            CurrViewChild.Content = new ObtainingRawView_UR();
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
