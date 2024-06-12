using MySql.Data.MySqlClient;
using SweetBakeryWpfApp.Repositories;
using SweetBakeryWpfApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;

namespace SweetBakeryWpfApp.View.UsedRawDataChildsView
{
    // Class for storage our raw remainder data for queries
    public class RawMaterial
    {
        public int RawId { get; set; }
        public string RawTitle { get; set; }
        public double TotalUsed { get; set; }
        public double QuantityPerMonth { get; set; }
        public DateTime ObtainingDate { get; set; }
        public DateTime FinalCalculationDate { get; set; }
        public string Status { get; set; }
        public double Remainder { get; set; }
    }

    public partial class UsedRawInfoView_UR : UserControl
    {
        // Constructor
        public UsedRawInfoView_UR()
        {
            InitializeComponent();

            //LoadData
            bindUsedRawCurrentMonth();

            bindStatusActivity("Відкрито сторінку 'Використана сировина: Розрахунок залишку сировини'");
        }

        
        //__________________LOAD INFORMATION_________________________

        private void  bindUsedRawCurrentMonth()
        {
           string confectioner = LoginViewModel.UserInfo;
           DB_Connect db = new DB_Connect();
            MySqlCommand command = new MySqlCommand("SELECT raw_id, MAX(raw_title) as raw_title, " +
                "SUM(used_raw_quantity) as total_used, " +
                 "MAX(raw_quantity_per_month) as raw_quantity_per_month, " +
                 "MAX(obtaining_date) as obtaining_date, MAX(final_calculation_date) " +
                 "as final_calculation_date FROM used_raw_report " +
                 "WHERE MONTH(obtaining_date) = @currentMonth AND " +
                 "used_raw_date BETWEEN obtaining_date AND final_calculation_date GROUP BY raw_id", db.getConnection());

            var currentMonth = DateTime.Now.Month;

           command.Parameters.Add("@confectioner", MySqlDbType.VarChar).Value = confectioner;
           command.Parameters.Add("@currentMonth", MySqlDbType.Int32).Value = currentMonth;

           db.openConnection();

           MySqlDataReader reader = command.ExecuteReader();

           List<RawMaterial> materials = new List<RawMaterial>();

           while (reader.Read())
           {
              double totalUsed = Convert.ToDouble(reader["total_used"]);
              double quantityPerMonth = Convert.ToDouble(reader["raw_quantity_per_month"]);

              double remainder = Math.Round(quantityPerMonth - totalUsed,3);

              string status;
              if (remainder > 0)
              {
                 status = "Добре, є залишок";
              }
              else if (remainder == 0)
              {
                 status = "Залишку немає";
              }
              else
              {
                 status = "Недостача";
              }

                materials.Add(new RawMaterial
                {
                    RawId = Convert.ToInt32(reader["raw_id"]),
                    RawTitle = reader["raw_title"].ToString(),
                    TotalUsed = Math.Round(totalUsed,3),
                    QuantityPerMonth = quantityPerMonth,
                    ObtainingDate = Convert.ToDateTime(reader["obtaining_date"]),
                    FinalCalculationDate = Convert.ToDateTime(reader["final_calculation_date"]),
                    Status = status,
                    Remainder = remainder
                });
           }

            db.closeConnection();

            remainingRawCurMonth.ItemsSource = materials;
        }



        //__________________ACTIONS REGION_________________________


        //Search Raw remainder by custom month
        private void btnSearchData_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string confectioner = LoginViewModel.UserInfo;
            DB_Connect db = new DB_Connect();

            MySqlCommand command = new MySqlCommand("SELECT raw_id, MAX(raw_title) as raw_title, SUM(used_raw_quantity) as total_used, " +
                 "MAX(raw_quantity_per_month) as raw_quantity_per_month, MAX(obtaining_date) as obtaining_date, MAX(final_calculation_date) " +
                 "as final_calculation_date FROM used_raw_report " +
                 "WHERE MONTH(obtaining_date) = MONTH(@customMonth) AND YEAR(obtaining_date) = YEAR(@customMonth) AND confectioner = @confectioner AND " +
                 "used_raw_date BETWEEN obtaining_date AND final_calculation_date GROUP BY raw_id", db.getConnection());

            string customMonth = monthOfRemaining.Text;
            DateTime remainingMonth = DateTime.ParseExact(customMonth, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            string formattedRemainingMonth = remainingMonth.ToString("yyyy-MM-dd");


            command.Parameters.Add("@confectioner", MySqlDbType.VarChar).Value = confectioner;
            command.Parameters.Add("@customMonth", MySqlDbType.DateTime).Value = formattedRemainingMonth;

            db.openConnection();

            MySqlDataReader reader = command.ExecuteReader();

            List<RawMaterial> materials = new List<RawMaterial>();

            while (reader.Read())
            {
                double totalUsed = Convert.ToDouble(reader["total_used"]);
                double quantityPerMonth = Convert.ToDouble(reader["raw_quantity_per_month"]);

                double remainder = Math.Round(quantityPerMonth - totalUsed, 3);

                string status;
                if (remainder > 0)
                {
                    status = "Добре, є залишок";
                }
                else if (remainder == 0)
                {
                    status = "Залишку немає";
                }
                else
                {
                    status = "Недостача";
                }

                materials.Add(new RawMaterial
                {
                    RawId = Convert.ToInt32(reader["raw_id"]),
                    RawTitle = reader["raw_title"].ToString(),
                    TotalUsed = totalUsed,
                    QuantityPerMonth = quantityPerMonth,
                    ObtainingDate = Convert.ToDateTime(reader["obtaining_date"]),
                    FinalCalculationDate = Convert.ToDateTime(reader["final_calculation_date"]),
                    Status = status,
                    Remainder = remainder
                });
            }

            db.closeConnection();

            btnSearchData.Visibility = Visibility.Collapsed;
            btnCancelSearchData.Visibility = Visibility.Visible;

            remainingRawCustomMonth.ItemsSource = materials;

            bindStatusActivity("Здійснено пошук залишку сировини на сторінці 'Використана сировина: Розрахунок залишку сировини'");
        }

        //Show button "cancel search" when we searched needed data
        private void btnCancelSearchData_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            List<RawMaterial> materials = new List<RawMaterial>();

            materials.Clear();

            remainingRawCustomMonth.ItemsSource = null;

            btnCancelSearchData.Visibility = Visibility.Collapsed;
            btnSearchData.Visibility = Visibility.Visible;

            bindStatusActivity("Закрито пошук залишку сировини за заданий місяць 'Використана сировина: Розрахунок залишку сировини'");
        }

        //Print data from custom search raw remaining by user
        private void btnPrintData_Click(object sender, RoutedEventArgs e)
        {
            if (remainingRawCustomMonth.Items.Count == 0)
            {
                MessageBox.Show("Записів для друку немає");
                return;
            }

            string confectioner = LoginViewModel.UserInfo;
            string userRole = LoginViewModel.UserPosition;

            //Creating of new FlowDocument
            FlowDocument doc = new FlowDocument();
            doc.PageWidth = 842;
            doc.PageHeight = 595;
            doc.PagePadding = new Thickness(30);
            doc.ColumnWidth = Double.PositiveInfinity;
            doc.FontFamily = new System.Windows.Media.FontFamily("Arial");

            // Adding of current date and time
            Paragraph currentTime = new Paragraph(new Run("Дата та час генерації звіту: " + DateTime.Now.ToString()));
            currentTime.FontSize = 10;
            currentTime.TextAlignment = TextAlignment.Right;
            doc.Blocks.Add(currentTime);

            //Adding header
            Paragraph title = new Paragraph(new Run("Залишок сировини за " + monthOfRemaining.SelectedDate.Value.ToString("MMMM yyyy")));
            title.FontSize = 16;
            title.TextAlignment = TextAlignment.Left;
            doc.Blocks.Add(title);

            //Add confectioner's name
            Paragraph confectionerName = new Paragraph(new Run("Кондитер: " + confectioner));
            confectionerName.FontSize = 12;
            confectionerName.TextAlignment = TextAlignment.Right;
            doc.Blocks.Add(confectionerName);

            //Add confectioner's role
            Paragraph confectionerRole = new Paragraph(new Run("Посада: " + userRole));
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
                column.Width = new GridLength(1,GridUnitType.Auto); // Змінено на Auto
                table.Columns.Add(column);

            }

            // Creating and adding header of table
            TableRowGroup headerGroup = new TableRowGroup();
            TableRow headerRow = new TableRow();

            headerRow.Cells.Add(CreateCell("Код"));
            headerRow.Cells.Add(CreateCell("Сировина"));
            headerRow.Cells.Add(CreateCell("Всього використано"));
            headerRow.Cells.Add(CreateCell("Кількість на місяць"));
            headerRow.Cells.Add(CreateCell("Дата отримання"));
            headerRow.Cells.Add(CreateCell("Дата фінального розрахунку"));
            headerRow.Cells.Add(CreateCell("Статус"));
            headerRow.Cells.Add(CreateCell("Залишок"));

            headerGroup.Rows.Add(headerRow);
            table.RowGroups.Add(headerGroup);

            // Add data from Datagrid to table
            TableRowGroup itemGroup = new TableRowGroup();
            foreach (RawMaterial item in remainingRawCustomMonth.Items)
            {
                TableRow row = new TableRow();

                row.Cells.Add(CreateCell(item.RawId.ToString()));
                row.Cells.Add(CreateCell(item.RawTitle.ToString()));
                row.Cells.Add(CreateCell(item.TotalUsed.ToString()));
                row.Cells.Add(CreateCell(item.QuantityPerMonth.ToString()));
                row.Cells.Add(CreateCell(item.ObtainingDate.ToString("dd.MM.yyyy")));
                row.Cells.Add(CreateCell(item.FinalCalculationDate.ToString("dd.MM.yyyy")));
                row.Cells.Add(CreateCell(item.Status.ToString()));
                row.Cells.Add(CreateCell(item.Remainder.ToString()));

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

            bindStatusActivity("Згенеровано звіт за заданий місяць на сторінці 'Використана сировина: Розрахунок залишку сировини'");
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

        private void btnPrintCurMonthData_Click(object sender, RoutedEventArgs e)
        {
            bindUsedRawCurrentMonth();

            if (remainingRawCurMonth.Items.Count == 0)
            {
                MessageBox.Show("Записів для друку немає");
                return;
            }

            string confectioner = LoginViewModel.UserInfo;
            string userRole = LoginViewModel.UserPosition;

            //Creating of new FlowDocument
            FlowDocument doc = new FlowDocument();
            doc.PageWidth = 842;
            doc.PageHeight = 595;
            doc.PagePadding = new Thickness(30);
            doc.ColumnWidth = Double.PositiveInfinity;
            doc.FontFamily = new System.Windows.Media.FontFamily("Arial");

            // Adding of current date and time
            Paragraph currentTime = new Paragraph(new Run("Дата та час генерації звіту: " + DateTime.Now.ToString()));
            currentTime.FontSize = 10;
            currentTime.TextAlignment = TextAlignment.Right;
            doc.Blocks.Add(currentTime);

            //Adding header
            Paragraph title = new Paragraph(new Run("Залишок сировини за " + monthOfRemaining.SelectedDate.Value.ToString("MMMM yyyy")));
            title.FontSize = 16;
            title.TextAlignment = TextAlignment.Left;
            doc.Blocks.Add(title);

            //Add confectioner's name
            Paragraph confectionerName = new Paragraph(new Run("Кондитер: " + confectioner));
            confectionerName.FontSize = 12;
            confectionerName.TextAlignment = TextAlignment.Right;
            doc.Blocks.Add(confectionerName);

            //Add confectioner's role
            Paragraph confectionerRole = new Paragraph(new Run("Посада: " + userRole));
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
            headerRow.Cells.Add(CreateCell("Сировина"));
            headerRow.Cells.Add(CreateCell("Всього використано"));
            headerRow.Cells.Add(CreateCell("Кількість на місяць"));
            headerRow.Cells.Add(CreateCell("Дата отримання"));
            headerRow.Cells.Add(CreateCell("Дата фінального розрахунку"));
            headerRow.Cells.Add(CreateCell("Статус"));
            headerRow.Cells.Add(CreateCell("Залишок"));

            headerGroup.Rows.Add(headerRow);
            table.RowGroups.Add(headerGroup);

            // Add data from Datagrid to table
            TableRowGroup itemGroup = new TableRowGroup();
            foreach (RawMaterial item in remainingRawCurMonth.Items)
            {
                TableRow row = new TableRow();

                row.Cells.Add(CreateCell(item.RawId.ToString()));
                row.Cells.Add(CreateCell(item.RawTitle.ToString()));
                row.Cells.Add(CreateCell(item.TotalUsed.ToString()));
                row.Cells.Add(CreateCell(item.QuantityPerMonth.ToString()));
                row.Cells.Add(CreateCell(item.ObtainingDate.ToString("dd.MM.yyyy")));
                row.Cells.Add(CreateCell(item.FinalCalculationDate.ToString("dd.MM.yyyy")));
                row.Cells.Add(CreateCell(item.Status.ToString()));
                row.Cells.Add(CreateCell(item.Remainder.ToString()));

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

            bindStatusActivity("Згенеровано звіт залишку сировини для друку за поточний місяць");
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