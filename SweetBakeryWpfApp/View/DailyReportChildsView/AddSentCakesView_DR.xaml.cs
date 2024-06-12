using MySql.Data.MySqlClient;
using SweetBakeryWpfApp.Repositories;
using SweetBakeryWpfApp.ViewModel;
using System;
using System.Collections.ObjectModel;
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
    public partial class AddSentCakesView_DR : Window
    {
        //Class for storage data for table "Sent cakes report to clients"
        public class SentCakesReport
        {
            public int DailyReportId { get; set; }
            public string CakeTitle { get; set; }
            public string OrderType { get; set; }
            public string Shop { get; set; }
            public string Status { get; set; }
            public DateTime SendDate { get; set; }
        }

        ObservableCollection<SentCakesReport> Reports = new ObservableCollection<SentCakesReport>();

        //Constructor
        public AddSentCakesView_DR()
        {
            InitializeComponent();

            BindBakedCakesReport();
            BindComboBoxClients();

            bindStatusActivity("Вхід на сторінку 'Додати відправлену продукцію'");
        }


        // ____________________ LOAD DATA TO WINDOW ___________________________

        // load table with baked cakes
        private void BindBakedCakesReport()
        {
            bakedCakesReport_DailyReportForm.ItemsSource = null; 

            string confectioner = LoginViewModel.UserInfo;

            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand($"SELECT * FROM `daily_report_cakes` WHERE `confectioner`='{confectioner}' " +
                $"AND `cake_status`='Залишок'", db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            foreach (DataRow row in table.Rows)
            {
                Reports.Add(new SentCakesReport
                {
                    DailyReportId = row["daily_report_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["daily_report_id"]),
                    CakeTitle = row["cake_title"] == DBNull.Value ? string.Empty : row["cake_title"].ToString(),
                    OrderType = row["order_type"] == DBNull.Value ? string.Empty : row["order_type"].ToString(),
                    Shop = row["sent_to_shop"] == DBNull.Value ? string.Empty : row["sent_to_shop"].ToString(),
                    SendDate = row["date_sent"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["date_sent"]),
                    Status = row["cake_status"] == DBNull.Value ? string.Empty : row["cake_status"].ToString()
                });
            }

            bakedCakesReport_DailyReportForm.ItemsSource = Reports;
        }

        // load clients list
        private void BindComboBoxClients()
        {
            try
            {
                DB_Connect db = new DB_Connect();
                db.openConnection();
                MySqlCommand sc = new MySqlCommand("SELECT * FROM `shops`", db.getConnection());
                MySqlDataReader reader;
                reader = sc.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("shop_title", typeof(string));
                dt.Load(reader);
                comboBoxClients.ItemsSource = dt.DefaultView;
                comboBoxClients.DisplayMemberPath = "shop_title";
                comboBoxClients.SelectedValuePath = "shop_title";
                db.closeConnection();
            }
            catch (Exception)
            {

            }

            comboBoxClients.SelectedIndex = 0;
        }


        // ____________________ ACTIONS ___________________________
        private void btnAddSentCakesReport_Click(object sender, RoutedEventArgs e)
        {
            string newSentDate = dateOfUsing.Text;
            string newStatus = "Відправлено";
            string newClient = comboBoxClients.Text;
            string orderId = txtCodeCake.Text;

            Reports.Clear();

            if(txtCodeCake.Text == "" || txtNameCake.Text == "" || txtUsedRawQuantity.Text == "")
            {
                MessageBox.Show("Перш ніж додати, виберіть продукцію яку необхідно відправити!");
            }

            //Change to db date style
            DateTime sentDate = DateTime.ParseExact(newSentDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            string formattedSentDate = sentDate.ToString("yyyy-MM-dd");

            DB_Connect db = new DB_Connect();

            using (db)
            {
                string query = "UPDATE `daily_report_cakes` SET `sent_to_shop`=@newClient," +
                    " `cake_status`=@newStatus, `date_sent`=@newSentDate WHERE" +
                    " `daily_report_id`=@orderID;";
                MySqlCommand command = new MySqlCommand(query, db.getConnection());

                command.Parameters.AddWithValue("@orderID", orderId);
                command.Parameters.AddWithValue("@newSentDate", formattedSentDate);
                command.Parameters.AddWithValue("@newStatus", newStatus);
                command.Parameters.AddWithValue("@newClient", newClient);

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
            BindBakedCakesReport();
            txtCodeCake.Text = "";
            txtNameCake.Text = "";
            txtUsedRawQuantity.Text = "";
            bindStatusActivity("Додано дані до таблиці 'Відправлена продукція'");
        }

        // close window
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // method to choose data from table for adding menu adding
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
           Button button = sender as Button;

           SentCakesReport currentRow = button.DataContext as SentCakesReport;

           txtCodeCake.Text = currentRow.DailyReportId.ToString();
           txtNameCake.Text = currentRow.CakeTitle;
           txtUsedRawQuantity.Text = currentRow.OrderType;
           comboBoxClients.SelectedItem = currentRow.Shop;
        }



        // ____________________ VALIDATION ___________________________


        //Validation of in numbers (0-9)
        private void numberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        // ____________________ CONTORL BAR SETTINGS ___________________________

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
