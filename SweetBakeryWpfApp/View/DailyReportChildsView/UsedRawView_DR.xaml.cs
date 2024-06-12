using ConfectSoft.View.DailyReportChildsView;
using MySql.Data.MySqlClient;
using SweetBakeryWpfApp.Repositories;
using SweetBakeryWpfApp.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SweetBakeryWpfApp.View
{
    public partial class UsedRawView_DR : UserControl
    {
        //Class for storage data for used raw report
        public class UsedRawReport
        {
            public int UsedRawId { get; set; }
            public int RawId { get; set; }
            public string RawTitle { get; set; }
            public double QuantityKg { get; set; }
            public DateTime UsingDate { get; set; }
        }

        ObservableCollection<UsedRawReport> Reports = new ObservableCollection<UsedRawReport>();

        public UsedRawView_DR()
        {
            InitializeComponent();

            bindUsedRawReport();
            bindStatusActivity("Відкрито сторінку 'Звіт використаної сировини'");
        }

        //__________________LOAD INFORMATION_________________________


        //load db table 'used_raw_report' to window table
        private void bindUsedRawReport()
        {
            string confectioner = LoginViewModel.UserInfo;

            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand($"SELECT * FROM `used_raw_report` WHERE `confectioner`='{confectioner}'", db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            foreach (DataRow row in table.Rows)
            {
                Reports.Add(new UsedRawReport
                {
                    UsedRawId = Convert.ToInt32(row["used_raw_id"]),
                    RawId = Convert.ToInt32(row["raw_id"]),
                    RawTitle = row["raw_title"].ToString(),
                    QuantityKg = Math.Round(Convert.ToDouble(row["used_raw_quantity"]), 3),
                    UsingDate = Convert.ToDateTime(row["used_raw_date"]),
                });
            }

            usedRawReport_DailyReportForm.ItemsSource = Reports;
        }

        //Load db table 'used_Raw_report' for printing to window table
        private void bindDailyReportTableForPrinting()
        {
            string confectioner = LoginViewModel.UserInfo;
            string dateOfUsing = dateOfBaking.SelectedDate.Value.ToString("yyyy-MM-dd");

            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string query = $"SELECT MAX(used_raw_id), MAX(raw_id), MAX(raw_title), SUM(used_raw_quantity), MAX(used_raw_date)" +
                           $"FROM `used_raw_report` " +
                           $"WHERE used_raw_date = '{dateOfUsing}' AND confectioner = '{confectioner}' " +
                           $"GROUP BY raw_id";

            MySqlCommand command = new MySqlCommand(query, db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            Reports.Clear();

            foreach (DataRow row in table.Rows)
            {
                Reports.Add(new UsedRawReport
                {
                    UsedRawId = Convert.ToInt32(row["MAX(used_raw_id)"]),
                    RawId = Convert.ToInt32(row["MAX(raw_id)"]),
                    RawTitle = row["MAX(raw_title)"].ToString(),
                    QuantityKg = Math.Round(Convert.ToDouble(row["SUM(used_raw_quantity)"]), 3),
                    UsingDate = Convert.ToDateTime(row["MAX(used_raw_date)"]),
                });
            }

            usedRawReport_DailyReportForm.ItemsSource = Reports;
        }




        //__________________ACTIONS REGION_________________________


        //Search Data by Date
        private void btnSearchData_Click(object sender, RoutedEventArgs e)
        {
            string confectioner = LoginViewModel.UserInfo;
            string dateOfUsing = dateOfBaking.SelectedDate.Value.ToString("yyyy-MM-dd");

            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string query = $"SELECT * FROM `used_raw_report` " +
                           $"WHERE `used_raw_date` = '{dateOfUsing}' AND `confectioner` = '{confectioner}' ";

            MySqlCommand command = new MySqlCommand(query, db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            Reports.Clear();

            foreach (DataRow row in table.Rows)
            {
                Reports.Add(new UsedRawReport
                {
                    UsedRawId = Convert.ToInt32(row["used_raw_id"]),
                    RawId = Convert.ToInt32(row["raw_id"]),
                    RawTitle = row["raw_title"].ToString(),
                    QuantityKg = Math.Round(Convert.ToDouble(row["used_raw_quantity"]), 3),
                    UsingDate = Convert.ToDateTime(row["used_raw_date"]),
                });
            }

            usedRawReport_DailyReportForm.ItemsSource = Reports;
            btnSearchData.Visibility = Visibility.Collapsed;
            btnCancelSearchData.Visibility = Visibility.Visible;

            bindStatusActivity($"Здійснено пошук використаної сировини за {dateOfUsing}");
        }

        //Generate report for printing by query date
        private void btnPrintData_Click(object sender, RoutedEventArgs e)
        {
            bindDailyReportTableForPrinting();

            if (usedRawReport_DailyReportForm.Items.Count == 0)
            {
                MessageBox.Show("Немає даних для друку");
                return;
            }

            string confectioner = LoginViewModel.UserInfo;
            string _userRole = LoginViewModel.UserPosition;

            //Creating of new FlowDocument
            FlowDocument doc = new FlowDocument();
            doc.PageWidth = 842;
            doc.PageHeight = 595;
            doc.PagePadding = new Thickness(30);
            doc.ColumnWidth = Double.PositiveInfinity;
            doc.FontFamily = new System.Windows.Media.FontFamily("Arial");

            // Adding of current date and time
            Paragraph currentTime = new Paragraph(new Run("Дата та час генерації звіту: " + DateTime.Now.ToString()));
            currentTime.FontSize = 9;
            currentTime.TextAlignment = TextAlignment.Right;
            doc.Blocks.Add(currentTime);

            //Adding header
            Paragraph title = new Paragraph(new Run("Звіт використаної сировини за " + dateOfBaking.SelectedDate.Value.ToString("dd MMMM yyyy") + " р."));
            title.FontSize = 14;
            title.TextAlignment = TextAlignment.Left;
            doc.Blocks.Add(title);

            //Add confectioner's name
            Paragraph confectionerName = new Paragraph(new Run("Кондитер: " + confectioner));
            confectionerName.FontSize = 12;
            confectionerName.TextAlignment = TextAlignment.Right;
            doc.Blocks.Add(confectionerName);

            //Add confectioner's role
            Paragraph confectionerRole = new Paragraph(new Run("Посада: " + _userRole));
            confectionerRole.FontSize = 12;
            confectionerRole.TextAlignment = TextAlignment.Right;
            doc.Blocks.Add(confectionerRole);

            //Creating of new table
            Table table = new Table();
            table.CellSpacing = 0;
            table.FontSize = 12;
            table.BorderBrush = System.Windows.Media.Brushes.Black;
            table.BorderThickness = new Thickness(0.5);

            // Set columns' width
            int numOfColumns = 8;
            for (int i = 0; i < numOfColumns; i++)
            {
                TableColumn column = new TableColumn();
                column.Width = new GridLength(1, GridUnitType.Auto); // Змінено на Auto
                table.Columns.Add(column);

            }

            // Creating and adding header of table
            TableRowGroup headerGroup = new TableRowGroup();
            TableRow headerRow = new TableRow();

            headerRow.Cells.Add(CreateCell("Код сировини"));
            headerRow.Cells.Add(CreateCell("Сировина"));
            headerRow.Cells.Add(CreateCell("Всього використано"));
            headerRow.Cells.Add(CreateCell("Дата використання"));

            headerGroup.Rows.Add(headerRow);
            table.RowGroups.Add(headerGroup);

            // Add data from Datagrid to table
            TableRowGroup itemGroup = new TableRowGroup();
            foreach (UsedRawReport item in usedRawReport_DailyReportForm.Items)
            {
                TableRow row = new TableRow();

                row.Cells.Add(CreateCell(item.RawId.ToString()));
                row.Cells.Add(CreateCell(item.RawTitle.ToString()));
                row.Cells.Add(CreateCell(item.QuantityKg.ToString()));
                row.Cells.Add(CreateCell(item.UsingDate.ToString("dd.MM.yyyy")));

                itemGroup.Rows.Add(row);
            }

            table.RowGroups.Add(itemGroup);

            //Add table to FlowDocument
            doc.Blocks.Add(table);

            // Creating of new PrintDialog
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {
                //Show print window
                printDialog.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator, "Printing Document");
            }

            bindStatusActivity("Згенеровано 'Звіт використаної сировини'");
        }

        public TableCell CreateCell(string text)
        {
            TableCell cell = new TableCell(new Paragraph(new Run(text)));
            cell.Padding = new Thickness(5);
            cell.TextAlignment = TextAlignment.Left;
            cell.BorderBrush = System.Windows.Media.Brushes.Black;
            cell.BorderThickness = new Thickness(0.5);
            return cell;
        }

        //Open window for adding report
        private void btnAddBakedCakesReport_Click(object sender, RoutedEventArgs e)
        {
            EditUsedRawView editWindow = new EditUsedRawView();
            editWindow.ShowDialog();

        }

        //Edit data in table 'daily_report_cakes'
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            UsedRawReport reportView = (UsedRawReport)button.DataContext;

            EditUsedRawDataView editWindow = new EditUsedRawDataView(reportView);
            editWindow.ShowDialog();
        }

        //Remove data from table 'daily_report_cakes'
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            UsedRawReport rowView = (UsedRawReport)button.DataContext;

            int daily_report_id = Convert.ToInt32(rowView.UsedRawId.ToString());

            DB_Connect db = new DB_Connect();
            MySqlCommand command = new MySqlCommand("DELETE FROM `used_raw_report` WHERE `used_raw_id` = @otid", db.getConnection());
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

            bindUsedRawReport();
            bindStatusActivity($"Видалено дані з таблиці 'Звіт використаної сировини' ID={daily_report_id}");
        }

        private void btnCancelSearchData_Click(object sender, RoutedEventArgs e)
        {
            btnSearchData.Visibility = Visibility.Visible;
            btnCancelSearchData.Visibility = Visibility.Collapsed;
            bindUsedRawReport();
            bindStatusActivity("Закрито пошук даних зі звіту використаної сировини");
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
