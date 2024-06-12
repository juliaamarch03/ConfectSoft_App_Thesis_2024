using MySql.Data.MySqlClient;
using SweetBakeryWpfApp.Repositories;
using SweetBakeryWpfApp.View;
using SweetBakeryWpfApp.ViewModel;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace ConfectSoft.View.DailyReportChildsView
{
    public partial class EditBakedCakesReportView_DR : Window
    {
        private DailyReport report;

        //Construstor
        public EditBakedCakesReportView_DR(DailyReport report)
        {
            InitializeComponent();

            this.report = report;

            txtCode.Text = report.DailyReportId.ToString();
            txtCakeTitle.Text = report.CakeTitle;
            txCakeQuantity.Text = report.QuantityKg.ToString(CultureInfo.InvariantCulture);
            dateOfBaking.Text = report.BakingDate.ToString();
            txtOrderType.Text = report.OrderType;

            bindStatusActivity("Вхід на сторінку 'Редагувати випечену продукцію'");
        }

        //Action to edit data in Baked cakes report
        private void btnEditReport_Click(object sender, RoutedEventArgs e)
        {
            string newCakeTitle = txtCakeTitle.Text;
            string newCakeQuantity = txCakeQuantity.Text;
            string newBakingDate = dateOfBaking.Text;
            string cakeId = txtCode.Text;

            DateTime bakingDate = DateTime.ParseExact(newBakingDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            string formattedBakingDate = bakingDate.ToString("yyyy-MM-dd");

            DB_Connect db = new DB_Connect();

            using (db)
            {
                string query = $"UPDATE `daily_report_cakes` SET `quantity_kg`= @newCakeQuantity, " +
                    $"`baking_date`=@bakingDate, `earned_per_day`=ROUND(`cake_price_kg`*@newCakeQuantity, 2) " +
                    "WHERE `daily_report_cakes`.`daily_report_id` = @cakeId;";
                MySqlCommand command = new MySqlCommand(query, db.getConnection());

                command.Parameters.AddWithValue("@newTitle", newCakeTitle);
                command.Parameters.AddWithValue("@newCakeQuantity", newCakeQuantity);
                command.Parameters.AddWithValue("@cakeId", cakeId);
                command.Parameters.AddWithValue("@bakingDate", formattedBakingDate);


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
            bindStatusActivity($"Оновлення даних в таблиці 'Випечена продукція' _cakeID={cakeId}");
        }


        //__________________VALIDATION_________________________



        //Validation of in numbers (0-9)
        private void numberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
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
