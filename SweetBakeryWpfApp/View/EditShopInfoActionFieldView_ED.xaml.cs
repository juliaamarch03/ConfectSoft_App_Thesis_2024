using MySql.Data.MySqlClient;
using SweetBakeryWpfApp.Repositories;
using SweetBakeryWpfApp.ViewModel;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace ConfectSoft.View
{
    public partial class EditShopInfoActionFieldView_ED : Window
    {
        private DataRowView rowView;

        public EditShopInfoActionFieldView_ED(DataRowView rowView)
        {
            InitializeComponent();

            this.rowView = rowView;

            //Set Value to fields
            idTextBox.Text = rowView["shop_id"].ToString();
            titleTextBox.Text = rowView["shop_title"].ToString();

            bindStatusActivity("Відкрито сторінку 'Редагувати дані про клієнтів'");
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string newId = idTextBox.Text;
            string newTitle = titleTextBox.Text;
            string oldId = rowView["shop_id"].ToString();

            DB_Connect db = new DB_Connect();

            using (db)
            {
                // Перевірка чи вже існує такий shop_title
                string checkQuery = "SELECT COUNT(*) FROM `shops` WHERE `shop_title` = @newTitle;";
                MySqlCommand checkCommand = new MySqlCommand(checkQuery, db.getConnection());
                checkCommand.Parameters.AddWithValue("@newTitle", newTitle);

                db.openConnection();

                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Магазин з такою назвою вже існує!");
                    db.closeConnection();
                    return;
                }

                string query = "UPDATE `shops` SET `shop_id` = @newId, `shop_title`= @newTitle WHERE `shops`.`shop_id` = @oldId;";
                MySqlCommand command = new MySqlCommand(query, db.getConnection());

                command.Parameters.AddWithValue("@newId", newId);
                command.Parameters.AddWithValue("@newTitle", newTitle);
                command.Parameters.AddWithValue("@oldId", oldId);

                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Успішно змінено!");
                }
                else
                {
                    MessageBox.Show("Помилка!");
                }

                db.closeConnection();
            }

            this.Close();

            bindStatusActivity("Змінено дані на сторінці 'Редагувати дані про клієнтів'");

        }




        //__________________CONTROL BAR BUTTONS_________________________


        //Close window
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }




        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void pnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);

        }

        private void pnlControlBar_MouseEnter(object sender, MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }


        
        
        //__________________VALIDATION_________________________



        //Validation of in numbers (0-9)
        private void numberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //Validation of Ukrainian letter
        private void letterValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-щьюяїієґА-ЩЬЮЯЇІЄҐ ]+");
            e.Handled = regex.IsMatch(e.Text);
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
