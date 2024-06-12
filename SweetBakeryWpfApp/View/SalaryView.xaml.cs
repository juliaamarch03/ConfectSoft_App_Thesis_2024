using MySql.Data.MySqlClient;
using SweetBakeryWpfApp.Repositories;
using SweetBakeryWpfApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace SweetBakeryWpfApp.View
{
    public partial class SalaryView : UserControl
    {
        public SalaryView()
        {
            InitializeComponent();

            // LOAD DATA TO DYNAMIC FIELDS FOR SALARY
            bindCountSalaryPerDay();
            bindCountSalaryPerWeek();
            bindCountSalaryPerMonth();
            bindCountSalaryPerYear();

            // 1 SECTION: CURRENT SALARY DATA: EARNED PER DAY
            bindTableEarnedPerDay();

            // 2 SECTION: CUSTOM SEARCH & COUNT EMPLOYEE'S SALARY BY TIME TYPE
            bindTimeTypeList();

            // USER ACTIVITY
            bindStatusActivity("Відкрито сторінку 'Зарплата'");
        }

        // create list of time types for second section in window - custom searching
        private void bindTimeTypeList()
        {
            List<string> timeTypeList = new List<string> { "День","Тиждень","Місяць","Рік"};
            
            // set values to combobox
            cmbBoxTimeType.ItemsSource = timeTypeList;
            //set default view
            cmbBoxTimeType.SelectedIndex = 0;
        }


        // LOAD DATA TO DYNAMIC FIELDS FOR SALARY

        // count employee's salary per day for DYNAMIC FIELD
        private void bindCountSalaryPerDay()
        {
            string confectioner = LoginViewModel.UserInfo;
            DB_Connect db = new DB_Connect();
            MySqlCommand command = new MySqlCommand("SELECT SUM(earned_per_day) FROM `daily_report_cakes` " +
                "WHERE confectioner = @confectioner AND DATE(baking_date) = CURDATE()", db.getConnection());
            command.Parameters.Add("@confectioner", MySqlDbType.VarChar).Value = confectioner;
            db.openConnection();
            var result = command.ExecuteScalar();
            double totalEarned = 0;
            if (result != DBNull.Value)
            {
                totalEarned = Math.Round(Convert.ToDouble(result), 2);
            }
            string totalEarnedStr = totalEarned.ToString("0.00", CultureInfo.InvariantCulture) + " UAH";
            db.closeConnection();
            earnedPerDayText.Text = totalEarnedStr;
        }

        // count employee's salary per week for DYNAMIC FIELD
        private void bindCountSalaryPerWeek()
        {
            string confectioner = LoginViewModel.UserInfo;
            DB_Connect db = new DB_Connect();
            MySqlCommand commandWeek = new MySqlCommand("SELECT SUM(earned_per_day) FROM `daily_report_cakes` " +
                "WHERE confectioner = @confectioner AND YEARWEEK(baking_date, 1) = YEARWEEK(CURDATE(), 1)", db.getConnection());
            commandWeek.Parameters.Add("@confectioner", MySqlDbType.VarChar).Value = confectioner;
            db.openConnection();
            var resultWeek = commandWeek.ExecuteScalar();
            double totalEarnedWeek = 0;
            if (resultWeek != DBNull.Value)
            {
                totalEarnedWeek = Math.Round(Convert.ToDouble(resultWeek), 2);
            }
            db.closeConnection();
            earnedPerWeekText.Text = totalEarnedWeek.ToString("0.00", CultureInfo.InvariantCulture) + " UAH";
        }

        // count employee's salary per month for DYNAMIC FIELD
        private void bindCountSalaryPerMonth()
        {
            string confectioner = LoginViewModel.UserInfo;
            DB_Connect db = new DB_Connect();
            MySqlCommand commandMonth = new MySqlCommand("SELECT SUM(earned_per_day) FROM `daily_report_cakes` " +
                "WHERE confectioner = @confectioner AND MONTH(baking_date) = MONTH(CURDATE())", db.getConnection());
            commandMonth.Parameters.Add("@confectioner", MySqlDbType.VarChar).Value = confectioner;
            db.openConnection();
            var resultMonth = commandMonth.ExecuteScalar();
            double totalEarnedMonth = 0;
            if (resultMonth != DBNull.Value)
            {
                totalEarnedMonth = Math.Round(Convert.ToDouble(resultMonth), 2);
            }
            db.closeConnection();
            earnedPerMonthText.Text = totalEarnedMonth.ToString("0.00", CultureInfo.InvariantCulture) + " UAH";
        }

        // count employee's salary per year for DYNAMIC FIELD
        private void bindCountSalaryPerYear()
        {
            string confectioner = LoginViewModel.UserInfo;
            DB_Connect db = new DB_Connect();
            MySqlCommand commandYear = new MySqlCommand("SELECT SUM(earned_per_day) FROM `daily_report_cakes` " +
                "WHERE confectioner = @confectioner AND YEAR(baking_date) = YEAR(CURDATE())", db.getConnection());
            commandYear.Parameters.Add("@confectioner", MySqlDbType.VarChar).Value = confectioner;
            db.openConnection();
            var resultYear = commandYear.ExecuteScalar();
            double totalEarnedYear = 0;
            if (resultYear != DBNull.Value)
            {
                totalEarnedYear = Math.Round(Convert.ToDouble(resultYear), 2);
            }
            db.closeConnection();
            earnedPerYearText.Text = totalEarnedYear.ToString("0.00", CultureInfo.InvariantCulture) + " UAH";
        }



        // 1 SECTION: CURRENT SALARY DATA: EARNED PER DAY

        // count salary per current day and show it like default view
        private void bindTableEarnedPerDay()
        {
            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `daily_report_cakes` WHERE DATE(baking_date) = CURDATE()", db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            earnedPerDayReportTable.ItemsSource = table.DefaultView;
        }



        // 2 SECTION: CUSTOM SEARCH & COUNT EMPLOYEE'S SALARY BY TIME TYPE

        //search & count by time type
        private void btnSearchCountSalaryDataByTimeType_Click(object sender, RoutedEventArgs e)
        {
            string selectedValue = cmbBoxTimeType.SelectedItem as string;

            if (selectedValue != null)
            {
                if (selectedValue == "День")//
                {
                    customCountSalaryPerDay();
                }
                if (selectedValue == "Тиждень")
                {
                    customCountSalaryPerWeek();
                }
                if (selectedValue == "Місяць")
                {
                    customCountSalaryPerMonth();
                }
                if (selectedValue == "Рік")
                {
                    customCountSalaryPerYear();
                }
            }
        }

            //CUSTOM COUNTING

        // custom counting salary by selected date
        private void customCountSalaryPerDay()
        {
            string confectioner = LoginViewModel.UserInfo;
            DB_Connect db = new DB_Connect();
            DateTime selectedDate = salaryTimeType.SelectedDate.Value;
            string dateString = selectedDate.ToString("yyyy-MM-dd");

            MySqlCommand command = new MySqlCommand("SELECT SUM(earned_per_day) FROM `daily_report_cakes` " +
                "WHERE confectioner = @confectioner AND DATE(baking_date) = @date", db.getConnection());
            command.Parameters.Add("@confectioner", MySqlDbType.VarChar).Value = confectioner;
            command.Parameters.Add("@date", MySqlDbType.VarChar).Value = dateString;

            db.openConnection();
            var result = command.ExecuteScalar();
            double totalEarned = 0;
            if (result != DBNull.Value)
            {
                totalEarned = Math.Round(Convert.ToDouble(result), 2);
            }
            string totalEarnedStr = totalEarned.ToString("0.00", CultureInfo.InvariantCulture) + " UAH";
            db.closeConnection();
            txtSalaryTimeType.Text = totalEarnedStr;

            customSearchSalaryPerDay();
        }

        // custom counting salary by selected week
        private void customCountSalaryPerWeek()
        {
            string confectioner = LoginViewModel.UserInfo;
            DB_Connect db = new DB_Connect();
            DateTime selectedDate = salaryTimeType.SelectedDate.Value;
            string yearWeekString = selectedDate.ToString("yyyy") + selectedDate.ToString("ww");

            MySqlCommand commandWeek = new MySqlCommand("SELECT SUM(earned_per_day) FROM `daily_report_cakes` " +
                "WHERE confectioner = @confectioner AND YEARWEEK(baking_date, 1) = YEARWEEK(@yearWeek,1)", db.getConnection());
            commandWeek.Parameters.Add("@confectioner", MySqlDbType.VarChar).Value = confectioner;
            commandWeek.Parameters.Add("@yearWeek", MySqlDbType.VarChar).Value = yearWeekString;

            db.openConnection();
            var resultWeek = commandWeek.ExecuteScalar();
            double totalEarnedWeek = 0;
            if (resultWeek != DBNull.Value)
            {
                totalEarnedWeek = Math.Round(Convert.ToDouble(resultWeek), 2);
            }
            db.closeConnection();
            txtSalaryTimeType.Text = totalEarnedWeek.ToString("0.00", CultureInfo.InvariantCulture) + " UAH";
            customSearchSalaryPerWeek();
        }

        // custom counting salary by selected month
        private void customCountSalaryPerMonth()
        {
            DateTime selectedDate = salaryTimeType.SelectedDate.Value;
            string monthString = selectedDate.ToString("yyyy-MM-dd");

            string confectioner = LoginViewModel.UserInfo;
            DB_Connect db = new DB_Connect();
            MySqlCommand commandMonth = new MySqlCommand("SELECT SUM(earned_per_day) FROM `daily_report_cakes` " +
                "WHERE confectioner = @confectioner AND MONTH(baking_date) = MONTH(@bakingMonth) " +
                "AND YEAR(baking_date) = YEAR(@bakingMonth)", db.getConnection());
            commandMonth.Parameters.Add("@confectioner", MySqlDbType.VarChar).Value = confectioner;
            commandMonth.Parameters.Add("@bakingMonth", MySqlDbType.VarChar).Value = monthString;
            db.openConnection();
            var resultMonth = commandMonth.ExecuteScalar();
            double totalEarnedMonth = 0;
            if (resultMonth != DBNull.Value)
            {
                totalEarnedMonth = Math.Round(Convert.ToDouble(resultMonth), 2);
            }
            db.closeConnection();
            txtSalaryTimeType.Text = totalEarnedMonth.ToString("0.00", CultureInfo.InvariantCulture) + " UAH";
            customSearchSalaryPerMonth();
        }

        // custom counting salary by selected year
        private void customCountSalaryPerYear()
        {
            string confectioner = LoginViewModel.UserInfo;
            DB_Connect db = new DB_Connect();
            MySqlCommand commandYear = new MySqlCommand("SELECT SUM(earned_per_day) FROM `daily_report_cakes` " +
                "WHERE confectioner = @confectioner AND YEAR(baking_date) = YEAR(CURDATE())", db.getConnection());
            commandYear.Parameters.Add("@confectioner", MySqlDbType.VarChar).Value = confectioner;
            db.openConnection();
            var resultYear = commandYear.ExecuteScalar();
            double totalEarnedYear = 0;
            if (resultYear != DBNull.Value)
            {
                totalEarnedYear = Math.Round(Convert.ToDouble(resultYear), 2);
            }
            db.closeConnection();
            txtSalaryTimeType.Text = totalEarnedYear.ToString("0.00", CultureInfo.InvariantCulture) + " UAH";
            customSearchSalaryPerYear();
        }


            //CUSTOM SEARCHING

        // custom searching salary by selected day
        private void customSearchSalaryPerDay()
        {
            string confectioner = LoginViewModel.UserInfo;
            string newBakingDate = salaryTimeType.Text;

            DateTime bakingDate = DateTime.ParseExact(newBakingDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            string formattedBakingDate = bakingDate.ToString("yyyy-MM-dd");

            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `daily_report_cakes` " +
                "WHERE confectioner = @confectioner AND DAY(baking_date) = DAY(@salaryDay) " +
                "AND YEAR(baking_date) = YEAR(@salaryDay)", db.getConnection());

            command.Parameters.Add("@confectioner", MySqlDbType.VarChar).Value = confectioner;
            command.Parameters.Add("@salaryDay", MySqlDbType.VarChar).Value = formattedBakingDate;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count == 0)
            {
                MessageBox.Show($"Записів за {bakingDate.ToString("MMMM yyyy", new CultureInfo("uk-UA"))} не знайдено. Зарплата - 0 UAH");
            }

            earnedPerTimeTypeReportTable.ItemsSource = table.DefaultView;
            btnSearchCountSalaryDataByTimeType.Visibility = Visibility.Collapsed;
            btnCancelSearchSalaryDataByTimeType.Visibility = Visibility.Visible;
        }

        // custom searching salary by selected week
        private void customSearchSalaryPerWeek()
        {
            string confectioner = LoginViewModel.UserInfo;
            string newBakingDate = salaryTimeType.Text;

            DateTime bakingDate = DateTime.ParseExact(newBakingDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            string formattedBakingDate = bakingDate.ToString("yyyy-MM-dd");

            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `daily_report_cakes` " +
                "WHERE confectioner = @confectioner AND WEEK(baking_date) = WEEK(@salaryDay) " +
                "AND YEAR(baking_date) = YEAR(@salaryDay)", db.getConnection());

            command.Parameters.Add("@confectioner", MySqlDbType.VarChar).Value = confectioner;
            command.Parameters.Add("@salaryDay", MySqlDbType.VarChar).Value = formattedBakingDate;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count == 0)
            {
                MessageBox.Show($"Записів за {bakingDate.ToString("MMMM yyyy", new CultureInfo("uk-UA"))} не знайдено. Зарплата - 0 UAH");
            }

            earnedPerTimeTypeReportTable.ItemsSource = table.DefaultView;
            btnSearchCountSalaryDataByTimeType.Visibility = Visibility.Collapsed;
            btnCancelSearchSalaryDataByTimeType.Visibility = Visibility.Visible;
        }

        // custom searching salary by selected month
        private void customSearchSalaryPerMonth()
        {
            string confectioner = LoginViewModel.UserInfo;
            string newBakingDate = salaryTimeType.Text;

            DateTime bakingDate = DateTime.ParseExact(newBakingDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            string formattedBakingDate = bakingDate.ToString("yyyy-MM-dd");

            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `daily_report_cakes` " +
                "WHERE confectioner = @confectioner AND MONTH(baking_date) = MONTH(@salaryDay) " +
                "AND YEAR(baking_date) = YEAR(@salaryDay)", db.getConnection());

            command.Parameters.Add("@confectioner", MySqlDbType.VarChar).Value = confectioner;
            command.Parameters.Add("@salaryDay", MySqlDbType.VarChar).Value = formattedBakingDate;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count == 0)
            {
                MessageBox.Show($"Записів за {bakingDate.ToString("MMMM yyyy", new CultureInfo("uk-UA"))} не знайдено. Зарплата - 0 UAH");
            }

            earnedPerTimeTypeReportTable.ItemsSource = table.DefaultView;
            btnSearchCountSalaryDataByTimeType.Visibility = Visibility.Collapsed;
            btnCancelSearchSalaryDataByTimeType.Visibility = Visibility.Visible;
        }

        // custom searching salary by selected year
        private void customSearchSalaryPerYear()
        {
            string confectioner = LoginViewModel.UserInfo;
            string newBakingDate = salaryTimeType.Text;

            DateTime bakingDate = DateTime.ParseExact(newBakingDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            string formattedBakingDate = bakingDate.ToString("yyyy-MM-dd");

            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `daily_report_cakes` " +
                "WHERE confectioner = @confectioner AND YEAR(baking_date) = YEAR(@salaryDay)", db.getConnection());

            command.Parameters.Add("@confectioner", MySqlDbType.VarChar).Value = confectioner;
            command.Parameters.Add("@salaryDay", MySqlDbType.VarChar).Value = formattedBakingDate;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count == 0)
            {
                MessageBox.Show($"Записів за {bakingDate.ToString("MMMM yyyy", new CultureInfo("uk-UA"))} не знайдено. Зарплата - 0 UAH");
            }

            earnedPerTimeTypeReportTable.ItemsSource = table.DefaultView;
            btnSearchCountSalaryDataByTimeType.Visibility = Visibility.Collapsed;
            btnCancelSearchSalaryDataByTimeType.Visibility = Visibility.Visible;
        }


        // cancel searching & counting if the "search btn" was clicked
        private void btnCancelSearchMonthData_Click(object sender, RoutedEventArgs e)
        {
            salaryTimeType.SelectedDate = DateTime.Now;
            salaryTimeType.DisplayDate = DateTime.Now;
            salaryTimeType.Text = DateTime.Now.ToString("dd.MM.yyyy");

            txtSalaryTimeType.Text = "";
            earnedPerTimeTypeReportTable.ItemsSource = null;

            btnCancelSearchSalaryDataByTimeType.Visibility = Visibility.Collapsed;
            btnSearchCountSalaryDataByTimeType.Visibility = Visibility.Visible;     
        }


        
        // USER ACTIVITY 

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
