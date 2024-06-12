using MySql.Data.MySqlClient;
using SweetBakeryWpfApp.Repositories;
using SweetBakeryWpfApp.ViewModel;
using System;
using System.Data;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace ConfectSoft.View.DailyReportChildsView
{
    public partial class EditUsedRawView : Window
    {
        public string userName { get; set; }
        public static string userInfo { get; set; }

        public EditUsedRawView()
        {
            InitializeComponent();

            txtCodeRaw.Text = "0000";
            txtCodeRaw.LostFocus += AddTextCode;
            txtCodeRaw.GotFocus += RemoveTextCode;

            txtUsedRawQuantity.Text = "0.000";
            txtUsedRawQuantity.LostFocus += AddTextQuantity;
            txtUsedRawQuantity.GotFocus += RemoveTextQuantity;

            bindUsedRawReport();
            LoadRawList();

            bindStatusActivity("Відкрито сторінку 'Додати використану сировину'");
        }

        //__________________LOAD INFORMATION_________________________


        //Show db table "used_raw_report" to window table
        private void bindUsedRawReport()
        {
            string confectioner = LoginViewModel.UserInfo;

            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `used_raw_report` WHERE `confectioner`=@confectioner AND `used_raw_date` = CURDATE();", db.getConnection());
            command.Parameters.AddWithValue("@confectioner", confectioner);
            adapter.SelectCommand = command;
            adapter.Fill(table);
            usedRawReport_DailyReportForm.ItemsSource = table.DefaultView;
        }

        //Set data after choosing raw in Code
        private void LoadRawList()
        {
            try
            {
                DB_Connect db = new DB_Connect();
                db.openConnection();
                MySqlCommand sc = new MySqlCommand("SELECT * FROM `raw`", db.getConnection());
                MySqlDataReader reader;
                reader = sc.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("raw_title", typeof(string));
                dt.Load(reader);
                comboBoxRaw.ItemsSource = dt.DefaultView;
                comboBoxRaw.DisplayMemberPath = "raw_title";
                comboBoxRaw.SelectedValuePath = "raw_title";
                db.closeConnection();
            }
            catch (Exception)
            {

            }

            comboBoxRaw.SelectedIndex = 0;
        }

        private void comboBoxRawSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxRaw.SelectedItem != null)
            {
                DataRowView row = (DataRowView)comboBoxRaw.SelectedItem;

                double raw_code = double.Parse(row["raw_id"].ToString());
                txtCodeRaw.Text = raw_code.ToString(CultureInfo.InvariantCulture);

                string query = $"SELECT * FROM `raw_obtainiing` WHERE `raw_title` = '{row["raw_title"].ToString()}' " +
                    $"AND MONTH(obtaining_date) = MONTH(CURRENT_DATE()) AND YEAR(obtaining_date) = YEAR(CURRENT_DATE())";

                DB_Connect db = new DB_Connect();
                MySqlCommand command = new MySqlCommand(query, db.getConnection());

                db.openConnection();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtObtainingQuantity.Text = reader["raw_quantity"].ToString();
                        dateOfObtaining.Text = reader["obtaining_date"].ToString();
                        dateOfFinCalculation.Text = reader["final_calculation_date"].ToString();
                        double rawKg = double.Parse(reader["raw_quantity"].ToString());
                        txtObtainingQuantity.Text = rawKg.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        txtObtainingQuantity.Text = "";
                        dateOfObtaining.Text = "";
                        dateOfFinCalculation.Text = "";
                    }
                }

                db.closeConnection();
            }
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

        //Change content of code field from ""->"text"
        public void AddTextCode(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodeRaw.Text))
            {
                txtCodeRaw.Text = "0000";
            }
        }

        //Change content of code field from "text"->""
        public void RemoveTextCode(object sender, EventArgs e)
        {
            if (txtCodeRaw.Text == "0000")
            {
                txtCodeRaw.Text = "";
                txtCodeRaw.Focus();
            }
        }

        //Change content of quantity field from ""->"text"
        public void AddTextQuantity(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsedRawQuantity.Text))
            {
                txtUsedRawQuantity.Text = "0.000";
            }
        }

        //Change content of quantity field from "text"->""
        public void RemoveTextQuantity(object sender, EventArgs e)
        {
            if (txtUsedRawQuantity.Text == "0.000")
            {
                txtUsedRawQuantity.Text = "";
                txtUsedRawQuantity.Focus();
            }
        }




        //__________________ACTIONS REGION_________________________



        //Open window for adding report
        private void btnAddUsedRawReport_Click(object sender, RoutedEventArgs e)
        {          
            string confectioner = LoginViewModel.UserInfo;

            string raw_id = txtCodeRaw.Text;

            string raw_title = comboBoxRaw.Text;
            
            string raw_used_quantity = txtUsedRawQuantity.Text;
            double used_raw = double.Parse(raw_used_quantity, CultureInfo.InvariantCulture);

            string date_used_raw = dateOfUsing.Text;
            string date_fincalc_raw = dateOfFinCalculation.Text;
            string date_obtaining_raw = dateOfObtaining.Text;
            string obtainingQuantity = txtObtainingQuantity.Text;

            if (string.IsNullOrWhiteSpace(raw_used_quantity) || double.Parse(raw_used_quantity, CultureInfo.InvariantCulture) == 0.000)
            {
                MessageBox.Show("Введіть к-сть використаної сировини!");
                return;
            }

            if (string.IsNullOrWhiteSpace(date_obtaining_raw) || string.IsNullOrWhiteSpace(date_fincalc_raw) || string.IsNullOrWhiteSpace(obtainingQuantity))
            {
                MessageBox.Show("Поставка сировини не була додана. Будь ласка, спочатку додайте поставку сировини.");
                return;
            }

            DateTime usingDate = DateTime.ParseExact(date_used_raw, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            string formattedUsingDate = usingDate.ToString("yyyy-MM-dd");

            DateTime obtainingDate = DateTime.ParseExact(date_obtaining_raw, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            string formattedObtainingDate = obtainingDate.ToString("yyyy-MM-dd");


            DateTime calculationDate = DateTime.ParseExact(date_fincalc_raw, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            string formattedCalcDate = calculationDate.ToString("yyyy-MM-dd");


            double obtainingRawQuantity = double.Parse(obtainingQuantity, CultureInfo.InvariantCulture);


            using (DB_Connect db = new DB_Connect())
            {
                string query = "INSERT INTO `used_raw_report`(`raw_id`, `raw_title`, `used_raw_quantity`, `used_raw_date`, " +
                    "`confectioner`, `raw_quantity_per_month`, `obtaining_date`, `final_calculation_date`) " +
                    "VALUES (@rawID, @rawTITLE, @rawUsedQuantity, @rawUsedDate, @confectioner, @obtainedRawQuantity, " +
                    "@obtainingDate, @calculationDate);";
                MySqlCommand command = new MySqlCommand(query, db.getConnection());

                command.Parameters.AddWithValue("@rawID", raw_id);
                command.Parameters.AddWithValue("@rawTITLE", raw_title);
                command.Parameters.AddWithValue("@rawUsedQuantity", used_raw);
                command.Parameters.AddWithValue("@rawUsedDate", formattedUsingDate);
                command.Parameters.AddWithValue("@confectioner", confectioner);
                command.Parameters.AddWithValue("@obtainedRawQuantity", obtainingRawQuantity);
                command.Parameters.AddWithValue("@obtainingDate", formattedObtainingDate);
                command.Parameters.AddWithValue("@calculationDate", formattedCalcDate);

                db.openConnection();

                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Дані успішно додано!");
                }
                else
                {
                    MessageBox.Show("Помилка при додаванні даних!");
                }

                db.closeConnection();
            }

            bindUsedRawReport();
            txtCodeRaw.Text = "0000";
            txtUsedRawQuantity.Text = "0.000";


            bindStatusActivity($"Додано дані в таблицю 'Звіт використаної сировини' _rawID={raw_id}");
        }

             
        
        //__________________CONTROL BUTTONS_________________________



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

        //CLose window
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            bindStatusActivity($"Закрито сторінку 'Додати використану сировину'");
        }

       //Minimize window
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
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
