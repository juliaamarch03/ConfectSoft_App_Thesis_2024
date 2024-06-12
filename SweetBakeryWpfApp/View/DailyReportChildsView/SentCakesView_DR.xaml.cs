using ConfectSoft.View.DailyReportChildsView;
using MySql.Data.MySqlClient;
using SweetBakeryWpfApp.Repositories;
using SweetBakeryWpfApp.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SweetBakeryWpfApp.View
{
    public partial class SentCakesView_DR : UserControl
    {
        // class for storage info for sent cakes report
        public class SentCakesReport
        {
            public int DailyReportId { get; set; }
            public string CakeTitle { get; set; }
            public string OrderType { get; set; }
            public string Shop { get; set; }
            public string Status { get; set; }
            public double QuantityKg { get; set; }
            public DateTime SendDate { get; set; }
        }

        ObservableCollection<SentCakesReport> Reports = new ObservableCollection<SentCakesReport>();


        //__________________CONSTRUCTOR_________________________

        public SentCakesView_DR()
        {
            InitializeComponent();

            //Load information
            BindBakedCakesReport();
            bindStatusActivity("Відкрито сторінку 'Звіт відправленої продукції'");
        }


        //__________________LOAD INFORMATION_________________________


        //Load data from db table 'daily_report_cakes' to window table
        private void BindBakedCakesReport()
        {
            string confectioner = LoginViewModel.UserInfo;

            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand($"SELECT * FROM `daily_report_cakes` WHERE `confectioner`='{confectioner}' " +
                $"AND `cake_status`='Відправлено'", db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            foreach (DataRow row in table.Rows)
            {
                Reports.Add(new SentCakesReport
                {
                    DailyReportId = row["daily_report_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["daily_report_id"]),
                    CakeTitle = row["cake_title"] == DBNull.Value ? string.Empty : row["cake_title"].ToString(),
                    QuantityKg = Math.Round(double.Parse(row["quantity_kg"].ToString()),3),
                    OrderType = row["order_type"] == DBNull.Value ? string.Empty : row["order_type"].ToString(),
                    Shop = row["sent_to_shop"] == DBNull.Value ? string.Empty : row["sent_to_shop"].ToString(),
                    SendDate = row["date_sent"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["date_sent"]),
                    Status = row["cake_status"] == DBNull.Value ? string.Empty : row["cake_status"].ToString()
                });
            }

            bakedCakesReport_DailyReportForm.ItemsSource = Reports;
        }


        //__________________ACTIONS REGION_________________________


        //Open window for adding report
        private void btnAddSentCakesReport_Click(object sender, RoutedEventArgs e)
        {
            AddSentCakesView_DR addWindow = new AddSentCakesView_DR();
            addWindow.Show();
        }

        //Remove data from table 'daily_report_cakes'
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            SentCakesReport rowView = (SentCakesReport)button.DataContext;

            int daily_report_id = Convert.ToInt32(rowView.DailyReportId.ToString());
            string newStatus = "Залишок";
            string formattedSentDate = "01-01-0001";
            string newClient = "";

            DB_Connect db = new DB_Connect();

            using (db)
            {
                string query = "UPDATE `daily_report_cakes` SET `sent_to_shop`=@newClient," +
                    " `cake_status`=@newStatus, `date_sent`=@newSentDate WHERE" +
                    " `daily_report_id`=@orderID;";
                MySqlCommand command = new MySqlCommand(query, db.getConnection());

                command.Parameters.AddWithValue("@orderID", daily_report_id);
                command.Parameters.AddWithValue("@newSentDate", formattedSentDate);
                command.Parameters.AddWithValue("@newStatus", newStatus);
                command.Parameters.AddWithValue("@newClient", newClient);

                db.openConnection();

                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Успішно видалено!");
                }
                else
                {
                    MessageBox.Show("Помилка!");
                }

                db.closeConnection();
            }
            BindBakedCakesReport();

            bindStatusActivity("Видалено дані з таблиці 'Звіт відправленої продукції'");
        }

        //Search Data by Date
        private void btnSearchData_Click(object sender, RoutedEventArgs e)
        {
            Reports.Clear();

            string confectioner = LoginViewModel.UserInfo;
            string dateBaking = dateOfBaking.Text;

            DateTime monthStart = DateTime.ParseExact(dateBaking, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            string formattedBakingDate = monthStart.ToString("yyyy-MM-dd");

            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand($"SELECT * FROM `daily_report_cakes` WHERE `confectioner`='{confectioner}' " +
                $"AND `cake_status`='Відправлено' AND `date_sent`='{formattedBakingDate}'", db.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            foreach (DataRow row in table.Rows)
            {
                Reports.Add(new SentCakesReport
                {
                    DailyReportId = row["daily_report_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["daily_report_id"]),
                    CakeTitle = row["cake_title"] == DBNull.Value ? string.Empty : row["cake_title"].ToString(),
                    QuantityKg = Math.Round(double.Parse(row["quantity_kg"].ToString()),3),
                    OrderType = row["order_type"] == DBNull.Value ? string.Empty : row["order_type"].ToString(),
                    Shop = row["sent_to_shop"] == DBNull.Value ? string.Empty : row["sent_to_shop"].ToString(),
                    SendDate = row["date_sent"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["date_sent"]),
                    Status = row["cake_status"] == DBNull.Value ? string.Empty : row["cake_status"].ToString()
                });
            }

            bakedCakesReport_DailyReportForm.ItemsSource = Reports;

            btnSearchData.Visibility = Visibility.Collapsed;
            btnCancelSearchData.Visibility = Visibility.Visible;

            bindStatusActivity($"Здійснено пошук відправленої продукції за {dateBaking}");
        }

        //Generate report for printing by query date
        private void btnPrintData_Click(object sender, RoutedEventArgs e)
        {
            bindDailyReportTableForPrinting();

            if (bakedCakesReport_DailyReportForm.Items.Count == 0)
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
            Paragraph title = new Paragraph(new Run("Звіт відправленої продукції за " + dateOfBaking.SelectedDate.Value.ToString("dd MMMM yyyy") + " р."));
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

            headerRow.Cells.Add(CreateCell("Випічка"));
            headerRow.Cells.Add(CreateCell("К-сть (кг)"));
            headerRow.Cells.Add(CreateCell("Тип замовлення"));
            headerRow.Cells.Add(CreateCell("Клієнт"));
            headerRow.Cells.Add(CreateCell("Дата відправлення"));

            headerGroup.Rows.Add(headerRow);
            table.RowGroups.Add(headerGroup);

            // Add data from Datagrid to table
            TableRowGroup itemGroup = new TableRowGroup();
            foreach (SentCakesReport item in bakedCakesReport_DailyReportForm.Items)
            {
                TableRow row = new TableRow();

                row.Cells.Add(CreateCell(item.CakeTitle.ToString()));
                row.Cells.Add(CreateCell(item.QuantityKg.ToString()));
                row.Cells.Add(CreateCell(item.OrderType.ToString()));
                row.Cells.Add(CreateCell(item.Shop.ToString()));
                row.Cells.Add(CreateCell(item.SendDate.ToString("dd.MM.yyyy")));

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
            bindStatusActivity($"Згенеровано звіт відправленої продукції");
        }

        //Set style of Table border
        public TableCell CreateCell(string text)
        {
            TableCell cell = new TableCell(new Paragraph(new Run(text)));
            cell.Padding = new Thickness(5);
            cell.TextAlignment = TextAlignment.Left;
            cell.BorderBrush = System.Windows.Media.Brushes.Black;
            cell.BorderThickness = new Thickness(0.5);
            return cell;
        }

        private void bindDailyReportTableForPrinting()
        {
            Reports.Clear();

            string confectioner = LoginViewModel.UserInfo;
            string dateBaking = dateOfBaking.Text;

            DateTime monthStart = DateTime.ParseExact(dateBaking, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            string formattedBakingDate = monthStart.ToString("yyyy-MM-dd");

            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand($"SELECT `cake_title` as cakes, `sent_to_shop` as shops, `cake_status`, " +
                    $"SUM(`quantity_kg`) as total_kg, `order_type` order_types, `date_sent` " +
                    $"FROM `daily_report_cakes` WHERE `confectioner`='{confectioner}' " +
                    $"AND `cake_status`='Відправлено' AND `date_sent`='{formattedBakingDate}' " +
                    $"GROUP BY `cake_title`, `order_type`, `sent_to_shop`", db.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            foreach (DataRow row in table.Rows)
            {
                Reports.Add(new SentCakesReport
                {
                    CakeTitle = row["cakes"] == DBNull.Value ? string.Empty : row["cakes"].ToString(),
                    QuantityKg = Math.Round(double.Parse(row["total_kg"].ToString()),3),
                    OrderType = row["order_types"] == DBNull.Value ? string.Empty : row["order_types"].ToString(),
                    Shop = row["shops"] == DBNull.Value ? string.Empty : row["shops"].ToString(),
                    SendDate = row["date_sent"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["date_sent"]),
                    Status = row["cake_status"] == DBNull.Value ? string.Empty : row["cake_status"].ToString()
                });
            }
            bakedCakesReport_DailyReportForm.ItemsSource = Reports;
        }

        private void btnCancelSearchData_Click(object sender, RoutedEventArgs e)
        {
            Reports.Clear();
            btnSearchData.Visibility = Visibility.Visible;
            btnCancelSearchData.Visibility = Visibility.Collapsed;
            BindBakedCakesReport();

            bindStatusActivity("Закрито пошук відправленої продукції");
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