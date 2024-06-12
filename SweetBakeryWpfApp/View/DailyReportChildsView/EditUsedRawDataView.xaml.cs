using MySql.Data.MySqlClient;
using SweetBakeryWpfApp.Repositories;
using SweetBakeryWpfApp.ViewModel;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using static SweetBakeryWpfApp.View.UsedRawView_DR;

namespace ConfectSoft.View.DailyReportChildsView
{
    public partial class EditUsedRawDataView : Window
    {
        private UsedRawReport report;

        // Constructor
        public EditUsedRawDataView(UsedRawReport report)
        {
            InitializeComponent();

            this.report = report;

            txtCode.Text = report.UsedRawId.ToString();
            txtRawTitle.Text = report.RawTitle;
            txtRawQuantity.Text = report.QuantityKg.ToString(CultureInfo.InvariantCulture);
            dateOfUsing.Text = report.UsingDate.ToString();

            bindStatusActivity("Відкрито сторінку 'Редагувати використану сировину'");
        }

        // ___________________ ACTIONS ___________________


        //Close window
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            bindStatusActivity("Закрито сторінку 'Редагувати використану сировину'");
        }

        // Action for edditing data in table
        private void btnEditReport_Click(object sender, RoutedEventArgs e)
        {
            string newRawQuantity = txtRawQuantity.Text;
            string newUsingDate = dateOfUsing.Text;
            string rawId = txtCode.Text;

            DateTime usingDate = DateTime.ParseExact(newUsingDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            string formattedUsingDate = usingDate.ToString("yyyy-MM-dd");

            DB_Connect db = new DB_Connect();

            using (db)
            {
                string query = "UPDATE `used_raw_report` SET `used_raw_quantity`= @newRawQuantity, " +
                    "`used_raw_date`=@usingDate WHERE `used_raw_report`.`used_raw_id` = @rawId;";
                MySqlCommand command = new MySqlCommand(query, db.getConnection());

                command.Parameters.AddWithValue("@newRawQuantity", newRawQuantity);
                command.Parameters.AddWithValue("@rawId", rawId);
                command.Parameters.AddWithValue("@usingDate", formattedUsingDate);

                db.openConnection();

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
            bindStatusActivity($"Змінено дані в таблиці звіту використаної сировини _rawID={rawId}");
        }

        
        
        // ___________________ VALIDATION ___________________

        //Validation of in numbers (0.9)
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

        //Validation of in numbers (0-9)
        private void numberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }



        // ___________________ CONTROL BAR SETTINGS ___________________

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
