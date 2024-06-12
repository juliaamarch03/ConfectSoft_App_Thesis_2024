using LiveCharts;
using LiveCharts.Wpf;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using SweetBakeryWpfApp.Repositories;
using SweetBakeryWpfApp.ViewModel;
using System;
using System.Data;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;

namespace SweetBakeryWpfApp.View
{
    
    public partial class DashboardDataView : UserControl
    {
        public DashboardDataView()
        {
            InitializeComponent();

            bindOrderedProductionPerCurMonth();
            generateChartMostOrderedTypeOrderPerCurMonth();
            generateChartSalaryLastSixMonth();
            generateChartShopsOrder();

            bindCountSalaryPerDay();
            bindCountSalaryPerWeek();
            bindCountSalaryPerMonth();
            bindCountSalaryPerYear();
            bindOrderRawPerNextMonth();

            greetingUserLabel.Content = "З поверненням, " + LoginViewModel.UserInfo + " !";

            bindStatusActivity("Відкрито Головну сторінку - Статистику працівника");
        }

        // Chart __Most Ordered Producition Per Current Month__
        private void bindOrderedProductionPerCurMonth()
        {
            string confectioner = LoginViewModel.UserInfo;

            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string monthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
            string monthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToString("yyyy-MM-dd");

            string query = $"SELECT cake_title, COUNT(cake_title) as total_quantity FROM `daily_report_cakes` " +
                           $"WHERE baking_date BETWEEN '{monthStart}' AND '{monthEnd}' AND confectioner = '{confectioner}' " +
                           $"GROUP BY cake_title " +
                           $"ORDER BY total_quantity";

            MySqlCommand command = new MySqlCommand(query, db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            List<Brush> colors = new List<Brush>
            {
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EAE4E9")), // magnolia
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1E6")), // linen
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FAD2E1")), // mimi pink
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E2ECE9")), // mint cream
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DFE7FD")), // lavender (web)
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CDDAFD")), // periwinkle
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F0EFEB")), // isabelline
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDE2E4")), // misty rose
            };

            LiveCharts.Wpf.PieChart pieChart = new LiveCharts.Wpf.PieChart()
            {
                LegendLocation = LegendLocation.Right
            };

            SeriesCollection pieData = new SeriesCollection();

            int i = 0;
            foreach (DataRow row in table.Rows)
            {
                string pastry = row["cake_title"].ToString();
                double quantity = Math.Round(Convert.ToDouble(row["total_quantity"]),3);

                pieData.Add(new PieSeries { Values = new ChartValues<double> { quantity }, Title = pastry, Fill = colors[i % colors.Count], PushOut = 5 }) ;

                i++;
            }

            pieChart.Series = pieData;

            MadeperCurrentMonthData.Children.Add(pieChart);
        }

        // Chart __Type orders per month__
        private void generateChartMostOrderedTypeOrderPerCurMonth()
        {
            string confectioner = LoginViewModel.UserInfo;

            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string monthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
            string monthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToString("yyyy-MM-dd");

            string query = $"SELECT order_type, COUNT(order_type) as total_quantity FROM `daily_report_cakes` " +
                           $"WHERE baking_date BETWEEN '{monthStart}' AND '{monthEnd}' AND confectioner = '{confectioner}' " +
                           $"GROUP BY order_type " +
                           $"ORDER BY total_quantity";

            MySqlCommand command = new MySqlCommand(query, db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            List<Brush> colors = new List<Brush>
            {
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CDDAFD")), // periwinkle
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F0EFEB")), // isabelline
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDE2E4")), // misty rose
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FAD2E1")), // mimi pink
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E2ECE9")), // mint cream
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DFE7FD")), // lavender (web)
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EAE4E9")), // magnolia
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1E6")), // linen
            };

            LiveCharts.Wpf.PieChart pieChart = new LiveCharts.Wpf.PieChart()
            {
                LegendLocation = LegendLocation.Right
            };

            SeriesCollection pieData = new SeriesCollection();

            int i = 0;
            foreach (DataRow row in table.Rows)
            {
                string pastry = row["order_type"].ToString();
                double quantity = Math.Round(Convert.ToDouble(row["total_quantity"]), 3);

                pieData.Add(new PieSeries { Values = new ChartValues<double> { quantity }, Title = pastry, Fill = colors[i % colors.Count], PushOut = 5 });

                i++;
            }

            pieChart.Series = pieData;

            mostOrderedProductsData.Children.Add(pieChart);
        }

        private void generateChartSalaryLastSixMonth()
        {
            string confectioner = LoginViewModel.UserInfo;

            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string query = $"SET lc_time_names = 'uk_UA'; " +
                           $"SELECT DATE_FORMAT(baking_date, '%M %Y') as month, " +
                           $"SUM(earned_per_day) as total_earned FROM daily_report_cakes " +
                           $"WHERE baking_date >= DATE_SUB(CURDATE(), INTERVAL 6 MONTH) " +
                           $"AND confectioner = '{confectioner}' " +
                           $"GROUP BY month ORDER BY month;";

            MySqlCommand command = new MySqlCommand(query, db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            List<Brush> colors = new List<Brush>
            {
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DFE7FD")), // lavender (web)
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDE2E4")), // misty rose
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EAE4E9")), // magnolia
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FAD2E1")), // mimi pink
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E2ECE9")), // mint cream
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1E6")), // linen
            };

            LiveCharts.Wpf.CartesianChart columnChart = new LiveCharts.Wpf.CartesianChart()
            {
                LegendLocation = LegendLocation.Bottom
            };

            SeriesCollection columnData = new SeriesCollection();
            List<string> labels = new List<string>();

            int i = 0;
            foreach (DataRow row in table.Rows)
            {
                string month = row["month"].ToString();
                double totalEarned = Math.Round(Convert.ToDouble(row["total_earned"]),3);

                columnData.Add(new ColumnSeries { Values = new ChartValues<double> { Math.Round(totalEarned,3)},
                    Title = month, Fill = colors[i % colors.Count] });
                labels.Add(month);

                i++;
            }

            columnChart.Series = columnData;

            salaryForLastSixMonth.Children.Add(columnChart);
        }

        private void generateChartShopsOrder()
        {
            string confectioner = LoginViewModel.UserInfo;

            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string monthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
            string monthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToString("yyyy-MM-dd");

            string query = $"SELECT `sent_to_shop`, COUNT(`sent_to_shop`) as total_quantity FROM `daily_report_cakes` " +
                           $"WHERE `date_sent` BETWEEN '{monthStart}' AND '{monthEnd}' AND `confectioner` = '{confectioner}' " +
                           $"AND `cake_status`='Відправлено' GROUP BY sent_to_shop " +
                           $"ORDER BY total_quantity";

            MySqlCommand command = new MySqlCommand(query, db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            List<Brush> colors = new List<Brush>
            {
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CDDAFD")), // periwinkle
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EAE4E9")), // magnolia
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1E6")), // linen
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F0EFEB")), // isabelline
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDE2E4")), // misty rose
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FAD2E1")), // mimi pink
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E2ECE9")), // mint cream
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DFE7FD")), // lavender (web)
                
            };

            LiveCharts.Wpf.PieChart pieChart = new LiveCharts.Wpf.PieChart()
            {
                LegendLocation = LegendLocation.Right
            };

            SeriesCollection pieData = new SeriesCollection();

            int i = 0;
            foreach (DataRow row in table.Rows)
            {
                string pastry = row["sent_to_shop"].ToString();
                double quantity = double.Parse(row["total_quantity"].ToString());

                pieData.Add(new PieSeries { Values = new ChartValues<double> { quantity }, Title = pastry, Fill = colors[i % colors.Count], PushOut = 5 });

                i++;
            }

            pieChart.Series = pieData;

            mostOrdersFromShops.Children.Add(pieChart);
        }

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

        private void bindOrderRawPerNextMonth()
        {
            string confectioner = LoginViewModel.UserInfo;

            string monthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
            string monthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, 
                DateTime.Now.Month)).ToString("yyyy-MM-dd");

            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand($"SELECT `raw_title`, SUM(used_raw_quantity) as raw_result " +
                $"FROM `used_raw_report` WHERE `used_raw_date` " +
                $"BETWEEN '{monthStart}' AND '{monthEnd}' GROUP BY raw_title", db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            usedRawReport_DailyReportForm.ItemsSource = table.DefaultView;
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
