using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using SweetBakeryWpfApp.Repositories;
using SweetBakeryWpfApp.View.EditDataChildsView;
using SweetBakeryWpfApp.ViewModel;

namespace SweetBakeryWpfApp.View
{
    public partial class EditDataView : UserControl
    {
        public EditDataView()
        {
            InitializeComponent();
            CurrViewChild.Content = new CakesInfoView_ED();

            bindStatusActivity("Відкрито сторінку 'Редагувати дані'");
        }


        private void CakesInfoRadioButton_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((RadioButton)e.Source).Uid);
            GridCursor.Margin = new Thickness(10 + (200 * index), 0, 0, 350);

            txtBlckCakes.Foreground = Brushes.Black;

            BrushConverter bc = new BrushConverter();
            Brush brush = (Brush)bc.ConvertFrom("#8F8F8F");
            txtBlckRaw.Foreground = brush;
            txtBlckShops.Foreground = brush;
            txtBlckOrderTypes.Foreground = brush;

            CurrViewChild.Content = null;
            CurrViewChild.Content = new CakesInfoView_ED();
        }
        private void RawInfoRadioButton_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((RadioButton)e.Source).Uid);
            GridCursor.Margin = new Thickness(10 + (200 * index), 0, 0, 350);

            txtBlckRaw.Foreground = Brushes.Black;

            BrushConverter bc = new BrushConverter();
            Brush brush = (Brush)bc.ConvertFrom("#8F8F8F");
            txtBlckCakes.Foreground = brush;
            txtBlckShops.Foreground = brush;
            txtBlckOrderTypes.Foreground = brush;

            CurrViewChild.Content = null;
            CurrViewChild.Content = new RawInfoView_ED();
        }
        private void ShopsInfoRadioButton_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((RadioButton)e.Source).Uid);
            GridCursor.Margin = new Thickness(10 + (200 * index), 0, 0, 350);

            txtBlckShops.Foreground = Brushes.Black;

            BrushConverter bc = new BrushConverter();
            Brush brush = (Brush)bc.ConvertFrom("#8F8F8F");
            txtBlckCakes.Foreground = brush;
            txtBlckRaw.Foreground = brush;
            txtBlckOrderTypes.Foreground = brush;

            CurrViewChild.Content = null;
            CurrViewChild.Content = new ShopsInfoView_ED();
        }
        private void OrderTypeInfoRadioButton_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((RadioButton)e.Source).Uid);
            GridCursor.Margin = new Thickness(10 + (200 * index), 0, 0, 350);

            txtBlckOrderTypes.Foreground = Brushes.Black;

            BrushConverter bc = new BrushConverter();
            Brush brush = (Brush)bc.ConvertFrom("#8F8F8F");
            txtBlckCakes.Foreground = brush;
            txtBlckRaw.Foreground = brush;
            txtBlckShops.Foreground = brush;

            CurrViewChild.Content = null;
            CurrViewChild.Content = new OrderTypeInfo_ED();
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
