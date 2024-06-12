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

namespace ConfectSoft.View
{
    public partial class AddReportView_BakedCakes : Window
    {
        public AddReportView_BakedCakes()
        {
            InitializeComponent();

            BindBakedCakesReport();
            LoadCakesList();
            LoadOrderTypesList();

            txtBakedQuantity.Text = "0.000";
            txtBakedQuantity.LostFocus += AddTextQuantity;
            txtBakedQuantity.GotFocus += RemoveTextQuantity;

            bindStatusActivity("Відкрито сторінку 'Додати випечену продукцію'");
        }


        //__________________LOAD INFORMATION_________________________

        
        //Load list of cakes for combobox
        private void LoadCakesList()
        {
            try
            {
                DB_Connect db = new DB_Connect();
                db.openConnection();
                MySqlCommand sc = new MySqlCommand("SELECT * FROM `cakes`", db.getConnection());
                MySqlDataReader reader;
                reader = sc.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("cake_title", typeof(string));
                dt.Load(reader);
                comboBoxCakes.ItemsSource = dt.DefaultView;
                comboBoxCakes.DisplayMemberPath = "cake_title";
                comboBoxCakes.SelectedValuePath = "cake_title";
                db.closeConnection();
            }
            catch (Exception)
            {

            }

            comboBoxCakes.SelectedIndex = 0;
        }

         //Load list of order type for combobox
        private void LoadOrderTypesList()
        {
            try
            {
                DB_Connect db = new DB_Connect();
                db.openConnection();
                MySqlCommand sc = new MySqlCommand("SELECT * FROM `order_type`", db.getConnection());
                MySqlDataReader reader;
                reader = sc.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("order_type_title", typeof(string));
                dt.Load(reader);
                comboBoxOrderType.ItemsSource = dt.DefaultView;
                comboBoxOrderType.DisplayMemberPath = "order_type_title";
                comboBoxOrderType.SelectedValuePath = "order_type_title";
                db.closeConnection();
            }
            catch (Exception)
            {

            }

            comboBoxOrderType.SelectedIndex = 0;
        }
        
        //Show table of Daily report with current date to show user what they added to table
        private void BindBakedCakesReport()
        {
            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `daily_report_cakes` WHERE DATE(baking_date) = CURDATE()", db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            bakedCakesReport_DailyReportForm.ItemsSource = table.DefaultView;
        }    
        
        //Set data after choosing cake in Code and Price
        private void comboBoxCakes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView row = (DataRowView)comboBoxCakes.SelectedItem;
            double cakePriceKg = double.Parse(row["cake_price_kg"].ToString());
            txtCakePriceKg.Text = cakePriceKg.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

   
       
        
        
        
        //__________________CONTROL BAR BUTTONS_________________________


        //Close window
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            bindStatusActivity("Закрито сторінку 'Додати випечену продукцію'");
        }

        //Minimize window
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
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

        //Change content of quantity field from ""->"text"
        public void AddTextQuantity(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBakedQuantity.Text))
            {
                txtBakedQuantity.Text = "0.000";
            }
        }

        //Change content of quantity field from "text"->""
        public void RemoveTextQuantity(object sender, EventArgs e)
        {
            if (txtBakedQuantity.Text == "0.000")
            {
                txtBakedQuantity.Text = "";
                txtBakedQuantity.Focus();
            }
        }





        //__________________ACTIONS REGION_________________________


        //Add new record to db table and table in app
        private void btnAddBakedCakesReport_Click(object sender, RoutedEventArgs e)
        {
            string confectioner = LoginViewModel.UserInfo;
            string newCakeTitle = comboBoxCakes.Text;
            string newCakePriceKg = txtCakePriceKg.Text;
            string newBakedQuantity = txtBakedQuantity.Text;
            string newBakingDate = dateOfBaking.Text;
            string newOrderType = comboBoxOrderType.Text;

            if (string.IsNullOrEmpty(newBakedQuantity) || double.Parse(newBakedQuantity, CultureInfo.InvariantCulture) == 0.000)
            {
                MessageBox.Show("Будь ласка, введіть випечену к-сть випічки у кг!");
                return;
            }

            DateTime bakingDate = DateTime.ParseExact(newBakingDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            string formattedBakingDate = bakingDate.ToString("yyyy-MM-dd");

            double cakePriceKg = double.Parse(newCakePriceKg, CultureInfo.InvariantCulture);
            double bakedQuantity = double.Parse(newBakedQuantity, CultureInfo.InvariantCulture);

            double result = Math.Round(cakePriceKg * bakedQuantity, 2);

            using (DB_Connect db = new DB_Connect())
            {
                string query = "INSERT INTO `daily_report_cakes`(`cake_title`, `cake_price_kg`, `quantity_kg`, `baking_date`, " +
                    "`order_type`, `confectioner`, `earned_per_day`) " +
                    "VALUES (@newCakeTitle, @newCakePriceKg, @newBakedQuantity, @newBakingDate, @newOrderType," +
                    "@confectioner, @earner_per_day);";
                MySqlCommand command = new MySqlCommand(query, db.getConnection());

                command.Parameters.AddWithValue("@newCakeTitle", newCakeTitle);
                command.Parameters.AddWithValue("@newCakePriceKg", newCakePriceKg);
                command.Parameters.AddWithValue("@newBakedQuantity", newBakedQuantity);
                command.Parameters.AddWithValue("@newBakingDate", formattedBakingDate);
                command.Parameters.AddWithValue("@newOrderType", newOrderType);
                command.Parameters.AddWithValue("@confectioner", confectioner);
                command.Parameters.AddWithValue("@earner_per_day", result);

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

            BindBakedCakesReport();

            bindStatusActivity($"Додано нові дані в таблицю на сторінці 'Додати випечену продукцію'");

        }

        //Remove data from table
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            DataRowView rowView = (DataRowView)button.DataContext;

            int daily_report_id = Convert.ToInt32(rowView["daily_report_id"].ToString());

            DB_Connect db = new DB_Connect();
            MySqlCommand command = new MySqlCommand("DELETE FROM `daily_report_cakes` WHERE `daily_report_id` = @otid", db.getConnection());
            command.Parameters.Add("@otid", MySqlDbType.VarChar).Value = daily_report_id;
            db.openConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Запис було успішно видалено");
            }
            else
            {
                MessageBox.Show("Виникла помилка при видаленні запису");
            }
            db.closeConnection();

            BindBakedCakesReport();
            bindStatusActivity("Видалено дані на сторінці 'Додати випечену продукцію'");
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
