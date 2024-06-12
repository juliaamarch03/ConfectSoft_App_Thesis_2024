using MySql.Data.MySqlClient;
using SweetBakeryWpfApp.Repositories;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using ConfectSoft.View;
using SweetBakeryWpfApp.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using ConfectSoft.View.DailyReportChildsView;

namespace SweetBakeryWpfApp.View
{
    //Class for storage data for baked cakes report
    public class DailyReport
    {
        public int DailyReportId { get; set; }
        public string CakeTitle { get; set; }
        public double CakePriceKg { get; set; }
        public double QuantityKg { get; set; }
        public DateTime BakingDate { get; set; }
        public string OrderType { get; set; }
    }

    public partial class BakedCakesView_DR : UserControl
    {
        ObservableCollection<DailyReport> Reports = new ObservableCollection<DailyReport>();


        //__________________CONSTRUCTOR________________________

        public BakedCakesView_DR()
        {
            InitializeComponent();

            //Load info
            BindBakedCakesReport();
            bindStatusActivity("Вхід на сторінку 'Звіт: Відправлена продукція'");
        }



        //__________________LOAD INFORMATION_________________________


        //Load db table 'daily_report_cakes' to window table
        private void BindBakedCakesReport()
        {
            string confectioner = LoginViewModel.UserInfo;

            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand($"SELECT * FROM `daily_report_cakes` where `confectioner`='{confectioner}'", db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            foreach (DataRow row in table.Rows)
            {
                Reports.Add(new DailyReport
                {
                    DailyReportId = Convert.ToInt32(row["daily_report_id"]),
                    CakeTitle = row["cake_title"].ToString(),
                    CakePriceKg = Convert.ToDouble(row["cake_price_kg"]),
                    QuantityKg = Math.Round(Convert.ToDouble(row["quantity_kg"]), 3),
                    BakingDate = Convert.ToDateTime(row["baking_date"]),
                    OrderType = row["order_type"].ToString()
                });
            }

            bakedCakesReport_DailyReportForm.ItemsSource = Reports;
        }

        //Load db table 'daily_report_cakes' for printing to window table
        private void bindDailyReportTableForPrinting()
        {
            string confectioner = LoginViewModel.UserInfo;
            string dateOfBakin = dateOfBaking.SelectedDate.Value.ToString("yyyy-MM-dd");

            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string query = $"SELECT MAX(daily_report_id), MAX(cake_title), MAX(cake_price_kg), SUM(quantity_kg), MAX(baking_date), MAX(order_type) " +
                           $"FROM `daily_report_cakes` " +
                           $"WHERE baking_date = '{dateOfBakin}' AND confectioner = '{confectioner}' " +
                           $"GROUP BY cake_title";

            MySqlCommand command = new MySqlCommand(query, db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            Reports.Clear();

            foreach (DataRow row in table.Rows)
            {
                Reports.Add(new DailyReport
                {
                    DailyReportId = Convert.ToInt32(row["MAX(daily_report_id)"]),
                    CakeTitle = row["MAX(cake_title)"].ToString(),
                    CakePriceKg = Convert.ToDouble(row["MAX(cake_price_kg)"]),
                    QuantityKg = Math.Round(Convert.ToDouble(row["SUM(quantity_kg)"]), 3),
                    BakingDate = Convert.ToDateTime(row["MAX(baking_date)"]),
                    OrderType = row["MAX(order_type)"].ToString()
                });
            }

            bakedCakesReport_DailyReportForm.ItemsSource = Reports;
        }   



        //__________________ACTIONS REGION_________________________


        //Search Data by Date
        private void btnSearchData_Click(object sender, RoutedEventArgs e)
        {

            string confectioner = LoginViewModel.UserInfo;
            string dateOfBakin = dateOfBaking.SelectedDate.Value.ToString("yyyy-MM-dd");

            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string query = $"SELECT * FROM `daily_report_cakes` " +
                           $"WHERE baking_date = '{dateOfBakin}' AND confectioner = '{confectioner}' ";

            MySqlCommand command = new MySqlCommand(query, db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            Reports.Clear();

            foreach (DataRow row in table.Rows)
            {
                Reports.Add(new DailyReport
                {
                    DailyReportId = Convert.ToInt32(row["daily_report_id"]),
                    CakeTitle = row["cake_title"].ToString(),
                    CakePriceKg = Convert.ToDouble(row["cake_price_kg"]),
                    QuantityKg = Convert.ToDouble(row["quantity_kg"]),
                    BakingDate = Convert.ToDateTime(row["baking_date"]),
                    OrderType = row["order_type"].ToString()
                });
            }

            bakedCakesReport_DailyReportForm.ItemsSource = Reports;
            btnSearchData.Visibility = Visibility.Collapsed;
            btnCancelSearchData.Visibility = Visibility.Visible;

            bindStatusActivity($"Здійснено пошук випеценої продукції за датою {dateOfBakin}");
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
            Paragraph title = new Paragraph(new Run("Звіт випеченої продукції за " 
                + dateOfBaking.SelectedDate.Value.ToString("dd MMMM yyyy") + " р."));
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

            headerRow.Cells.Add(CreateCell("Код"));
            headerRow.Cells.Add(CreateCell("Випічка"));
            headerRow.Cells.Add(CreateCell("Всього випечено"));
            headerRow.Cells.Add(CreateCell("Ціна за кг"));
            headerRow.Cells.Add(CreateCell("Дата випікання"));

            headerGroup.Rows.Add(headerRow);
            table.RowGroups.Add(headerGroup);

            // Add data from Datagrid to table
            TableRowGroup itemGroup = new TableRowGroup();
            foreach (DailyReport item in bakedCakesReport_DailyReportForm.Items)
            {
                TableRow row = new TableRow();

                row.Cells.Add(CreateCell(item.DailyReportId.ToString()));
                row.Cells.Add(CreateCell(item.CakeTitle.ToString()));
                row.Cells.Add(CreateCell(item.QuantityKg.ToString()));
                row.Cells.Add(CreateCell(item.CakePriceKg.ToString()));
                row.Cells.Add(CreateCell(item.BakingDate.ToString("dd.MM.yyyy")));

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

            bindStatusActivity($"Згенеровано звіт випечепної продукції за {dateOfBaking}");
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

        //Open window for adding report
        private void btnAddBakedCakesReport_Click(object sender, RoutedEventArgs e)
        {
            AddReportView_BakedCakes editWindow = new AddReportView_BakedCakes();
            editWindow.ShowDialog();
        }

        //Edit data in table 'daily_report_cakes'
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            DailyReport reportView = (DailyReport)button.DataContext;

            EditBakedCakesReportView_DR editWindow = new EditBakedCakesReportView_DR(reportView);
            editWindow.ShowDialog();
        }

        //Remove data from table 'daily_report_cakes'
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            DailyReport rowView = (DailyReport)button.DataContext;

            int daily_report_id = Convert.ToInt32(rowView.DailyReportId.ToString());

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

            bindStatusActivity("Здійснено видалення даних з таблиці випеченої продукції");

            Reports.Remove(rowView);
        }

        //Close search for table 'dayly_report_cakes'
        private void btnCancelSearchData_Click(object sender, RoutedEventArgs e)
        {
            BindBakedCakesReport();
            btnCancelSearchData.Visibility = Visibility.Collapsed;
            btnSearchData.Visibility = Visibility.Visible;

            bindStatusActivity("Закрито пошук випеченої продукції");
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
