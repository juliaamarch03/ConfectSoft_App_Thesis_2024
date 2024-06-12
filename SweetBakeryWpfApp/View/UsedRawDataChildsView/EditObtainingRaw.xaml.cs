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

namespace ConfectSoft.View.UsedRawDataChildsView
{
    public partial class EditObtainingRaw : Window
    {
        private DataRowView rowView;

        public EditObtainingRaw(DataRowView rowView)
        {
            this.rowView = rowView;

            InitializeComponent();
            
            //Set values from table to fields
            txtCodeObtaining.Text = rowView["raw_obtaining_id"].ToString();
            txtRawTitle.Text = rowView["raw_title"].ToString();
            dateOfObtaining.Text = rowView["obtaining_date"].ToString();
            dateFinalCalculating.Text = rowView["final_calculation_date"].ToString();

            double rawKg = double.Parse(rowView["raw_quantity"].ToString());
            txtRawQuantity.Text = rawKg.ToString(CultureInfo.InvariantCulture);

            bindStatusActivity("Відкрито сторінку 'Редагувати поставку сировини'");
        }


        //__________________ACTIONS REGION_________________________

        
        //Edit data from table 'obtainiing_raw'
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            string oldObtainingRawID = txtCodeObtaining.Text;
            
            string newObtainingRawID = txtCodeObtaining.Text;
            string newRawTitle = txtRawTitle.Text;
            string newQuantity = txtRawQuantity.Text;
            string newObtainingDate = dateOfObtaining.Text;
            string newFinalCalculation = dateFinalCalculating.Text;

            //Change to db date style
            DateTime obtainingDate = DateTime.ParseExact(newObtainingDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            string formattedObtainingDate = obtainingDate.ToString("yyyy-MM-dd");

            DateTime finalCalculatingDate = DateTime.ParseExact(newFinalCalculation, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            string formattedCalculatingDate = finalCalculatingDate.ToString("yyyy-MM-dd");

            double rawQuantityKg = double.Parse(newQuantity, CultureInfo.InvariantCulture);

            DB_Connect db = new DB_Connect();

            using (db)
            {
                string query = "UPDATE `raw_obtainiing` SET " +
                    "`raw_quantity`=@rawQuantityKg, `obtaining_date`= @formattedObtainingDate, `final_calculation_date`=@formattedCalculatingDate" +
                    " WHERE `raw_obtainiing`.`raw_obtaining_id` = @oldObtainingRawID;";
                MySqlCommand command = new MySqlCommand(query, db.getConnection());

                command.Parameters.AddWithValue("@oldObtainingRawID", oldObtainingRawID);
                command.Parameters.AddWithValue("@newObtainingRawID", newObtainingRawID);
                command.Parameters.AddWithValue("@newRawTitle", newRawTitle);
                command.Parameters.AddWithValue("@rawQuantityKg", rawQuantityKg);
                command.Parameters.AddWithValue("@formattedObtainingDate", formattedObtainingDate);
                command.Parameters.AddWithValue("@formattedCalculatingDate", formattedCalculatingDate);

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

            bindStatusActivity($"Відредаговано поставку сировини ID = {oldObtainingRawID}");
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
