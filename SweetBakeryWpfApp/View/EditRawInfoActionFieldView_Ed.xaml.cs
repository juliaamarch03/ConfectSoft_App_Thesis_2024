using MySql.Data.MySqlClient;
using SweetBakeryWpfApp.Repositories;
using SweetBakeryWpfApp.ViewModel;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace ConfectSoft.View
{
    public partial class EditRawInfoActionFieldView_Ed : Window
    {
        private DataRowView rowView;
        public EditRawInfoActionFieldView_Ed(DataRowView rowView)
        {
            InitializeComponent();

            this.rowView = rowView;

            idTextBox.Text = rowView["raw_id"].ToString();
            titleTextBox.Text = rowView["raw_title"].ToString();

            bindStatusActivity("Відкрито сторінку 'Редагувати дані про сировину'");
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string newId = idTextBox.Text;
            string newTitle = titleTextBox.Text;
            string oldId = rowView["raw_id"].ToString();

            DB_Connect db = new DB_Connect();

            using (db)
            {
                // Перевірка чи вже існує такий raw_title
                string checkQuery = "SELECT COUNT(*) FROM `raw` WHERE `raw_title` = @newTitle;";
                MySqlCommand checkCommand = new MySqlCommand(checkQuery, db.getConnection());
                checkCommand.Parameters.AddWithValue("@newTitle", newTitle);

                db.openConnection();

                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Сировина з такою назвою вже існує!");
                    db.closeConnection();
                    return;
                }

                string query = "UPDATE `raw` SET `raw_id` = @newId, `raw_title`= @newTitle WHERE `raw`.`raw_id` = @oldId;";
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

            bindStatusActivity("Змінено дані на сторінці 'Редагувати дані про сировину'");

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        //Validation of in numbers (0-9)
        private void numberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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

        //Validation of Ukrainian letter
        private void letterValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-щьюяїієґА-ЩЬЮЯЇІЄҐ ]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //Validation of double numbers (like 1.223)
        private void numberFValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (e.Text == "." && !textBox.Text.Contains("."))
            {
                e.Handled = false;
            }
            else
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
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
