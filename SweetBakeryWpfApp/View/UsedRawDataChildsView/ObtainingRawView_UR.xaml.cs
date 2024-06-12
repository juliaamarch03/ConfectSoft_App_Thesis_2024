using ConfectSoft.View.UsedRawDataChildsView;
using MySql.Data.MySqlClient;
using SweetBakeryWpfApp.Repositories;
using SweetBakeryWpfApp.ViewModel;
using System;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SweetBakeryWpfApp.View.UsedRawDataChildsView
{
    public partial class ObtainingRawView_UR : UserControl
    {
        public ObtainingRawView_UR()
        {
            InitializeComponent();

            //Load information
            bindObtainingRaw();
            LoadRawList();

            
            //Set default values to fields
            this.PreviewMouseDown += IsFieldActive_PreviewMouseDown;
            
            txtCodeObtaining.Text = "0000";
            txtCodeObtaining.LostFocus += AddTextCode;
            txtCodeObtaining.GotFocus += RemoveTextCode;


            txtRawQuantity.Text = "0.000";
            txtRawQuantity.LostFocus += AddTextQuantity;
            txtRawQuantity.GotFocus += RemoveTextQuantity;

            bindStatusActivity("Відкрито сторінку 'Поставка сировини'");
        }



        //__________________LOAD INFORMATION_________________________


        //Load data from db table "obtainiing_raw"
        private void bindObtainingRaw()
        {
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            string firstDayMonth = firstDayOfMonth.ToString("yyyy-MM-dd");

            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `raw_obtainiing` ORDER BY CASE WHEN `obtaining_date` " +
                ">= @first_date THEN 0 ELSE 1 END, `obtaining_date` DESC; ", db.getConnection());
            
            command.Parameters.AddWithValue("@first_date", firstDayMonth);
            adapter.SelectCommand = command;
            adapter.Fill(table);
            rawObtainingInfo_editDataForm.ItemsSource = table.DefaultView;

            
        }

        //bind comboBox with raw list from db
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

        //Set data after choosing raw in Code
        private void comboBoxRawSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView row = (DataRowView)comboBoxRaw.SelectedItem;
            double cakePriceKg = double.Parse(row["raw_id"].ToString());
            raw_id_code.Content = cakePriceKg.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }





        //__________________VALIDAION_________________________


        // Validation of double numbers (like "0.12")
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

        // Validation of int numbers (0-9)
        private void numberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        
        //Validation Is Field active / No -> change to title of field 
        private void IsFieldActive_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (txtRawQuantity != null && txtRawQuantity.IsFocused && string.IsNullOrWhiteSpace(txtRawQuantity.Text))
            {
                txtRawQuantity.Text = "0.000";
            }
            if (txtCodeObtaining != null && txtCodeObtaining.IsFocused && string.IsNullOrWhiteSpace(txtCodeObtaining.Text))
            {
                txtCodeObtaining.Text = "0000";
            }
        }

        
        
        //Change content of code field from ""->"text"
        public void AddTextCode(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodeObtaining.Text))
            {
                txtCodeObtaining.Text = "0000";
            }
        }

        //Change content of code field from "text"->""
        public void RemoveTextCode(object sender, EventArgs e)
        {
            if (txtCodeObtaining.Text == "0000")
            {
                txtCodeObtaining.Text = "";
                txtCodeObtaining.Focus();
            }
        }



        //Change content of quantity field from ""->"text"
        public void AddTextQuantity(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRawQuantity.Text))
            {
                txtRawQuantity.Text = "0.000";
            }
        }

        //Change content of quantity field from "text"->""
        public void RemoveTextQuantity(object sender, EventArgs e)
        {
            if (txtRawQuantity.Text == "0.000")
            {
                txtRawQuantity.Text = "";
                txtRawQuantity.Focus();
            }
        }





        //__________________ACTIONS REGION_________________________


        //Table button "Edit info"
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            DataRowView rowView = (DataRowView)button.DataContext;

            EditObtainingRaw editWindow = new EditObtainingRaw(rowView);
            editWindow.ShowDialog();

            bindObtainingRaw();
        }

        //Table button "Remove info"
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            DataRowView rowView = (DataRowView)button.DataContext;

            int raw_obtaining_id = Convert.ToInt32(rowView["raw_obtaining_id"].ToString());

            DB_Connect db = new DB_Connect();
            MySqlCommand command = new MySqlCommand("DELETE FROM `raw_obtainiing` WHERE `raw_obtaining_id` = @otid", db.getConnection());
            command.Parameters.Add("@otid", MySqlDbType.VarChar).Value = raw_obtaining_id;
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

            bindObtainingRaw();
            bindStatusActivity($"Видалено дані з таблиці 'Поставка сировини' ID = {raw_obtaining_id}");
        }

        //Add info button to table
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (repeatedObtainingRawCheckBox.IsChecked == true)
            {
                addObtainingIfWASChecked();
            }
            else
            {
                addObtainingIfNOTChecked();
            }

        }


        // add new obtaining raw is the checkbox wasn't checked
        private void addObtainingIfNOTChecked()
        {
            string newObtainingRawID = txtCodeObtaining.Text;
            string newRawID = raw_id_code.Content.ToString();
            string newRawTitle = comboBoxRaw.Text;
            string newQuantity = txtRawQuantity.Text;
            string newObtainingDate = dateOfObtaining.Text;
            string newFinalCalculation = dateFinalCalculating.Text;

            //Change to db date style
            DateTime obtainingDate = DateTime.ParseExact(newObtainingDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            string formattedObtainingDate = obtainingDate.ToString("yyyy-MM-dd");

            DateTime finalCalculatingDate = DateTime.ParseExact(newFinalCalculation, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            string formattedCalculatingDate = finalCalculatingDate.ToString("yyyy-MM-dd");

            double rawQuantityKg = double.Parse(newQuantity, CultureInfo.InvariantCulture);

            // Перевірка на дефолтні значення
            if (newObtainingRawID == "0000" || newQuantity == "0.000")
            {
                MessageBox.Show("Поля порожні, будь ласка введіть дані");
                return;
            }

            // Перевірка на довжину коду
            if (newObtainingRawID.Length < 4)
            {
                MessageBox.Show("Код повинен містити 4 цифри");
                txtCodeObtaining.Text = "0000";
                txtRawQuantity.Text = "0.000";
                return;
            }

            using (DB_Connect db = new DB_Connect())
            {
                string checkQuery = "SELECT COUNT(*) FROM `raw_obtainiing` WHERE `raw_obtaining_id` = @newId;";
                MySqlCommand checkCommand = new MySqlCommand(checkQuery, db.getConnection());
                checkCommand.Parameters.AddWithValue("@newId", newObtainingRawID);

                db.openConnection();

                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Такий код вже існує, створіть інший");
                    db.closeConnection();
                    return;
                }
                db.closeConnection();

                string query = "INSERT INTO `raw_obtainiing` (`raw_obtaining_id`, `raw_id`, `raw_title`, `raw_quantity`, `obtaining_date`, `final_calculation_date`) " +
                               "VALUES (@newObtainingRawID, @newRawID, @newRawTitle, @rawQuantityKg, @formattedObtainingDate, @formattedCalculatingDate);";
                MySqlCommand command = new MySqlCommand(query, db.getConnection());

                db.openConnection();

                command.Parameters.AddWithValue("@newObtainingRawID", newObtainingRawID);
                command.Parameters.AddWithValue("@newRawID", newRawID);
                command.Parameters.AddWithValue("@newRawTitle", newRawTitle);
                command.Parameters.AddWithValue("@rawQuantityKg", rawQuantityKg);
                command.Parameters.AddWithValue("@formattedObtainingDate", formattedObtainingDate);
                command.Parameters.AddWithValue("@formattedCalculatingDate", formattedCalculatingDate);

                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Дані успішно додано!");
                }
                else
                {
                    MessageBox.Show("Помилка при додаванні даних!");
                }

                db.closeConnection();

                bindObtainingRaw();

                txtCodeObtaining.Clear();
                txtCodeObtaining.Text = "0000";

                txtRawQuantity.Clear();
                txtRawQuantity.Text = "0.000";

                comboBoxRaw.SelectedIndex = 0;

                dateOfObtaining.SelectedDate = System.DateTime.Now;
                dateFinalCalculating.SelectedDate = System.DateTime.Now;

                bindStatusActivity($"Додано дані в таблицю 'Поставка сировини' ID = {newObtainingRawID}");
            }
        }


        // add repeated obtaining raw is the checkbox was checked
        private void addObtainingIfWASChecked()
        {
                DB_Connect db = new DB_Connect();
                string selectedRaw = comboBoxRaw.Text;
                string newObtainingDate = dateOfObtaining.Text;

                DateTime obtainingDate = DateTime.ParseExact(newObtainingDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                string monthYear = obtainingDate.ToString("yyyy-MM-dd");

                string checkQuery = "SELECT `raw_quantity` FROM `raw_obtainiing` WHERE " +
                    "`raw_title` = @selectedRaw AND YEAR(`obtaining_date`) = YEAR(@monthYearAdd) " +
                    "AND MONTH(`obtaining_date`) = MONTH(@monthYearAdd);";

                MySqlCommand checkCommand = new MySqlCommand(checkQuery, db.getConnection());
                checkCommand.Parameters.AddWithValue("@selectedRaw", selectedRaw);
                checkCommand.Parameters.AddWithValue("@monthYearAdd", monthYear);
                db.openConnection();

                double result = Convert.ToDouble(checkCommand.ExecuteScalar());

                if (result != 0)
                {
                    MessageBox.Show("Записи знайдені. Починаємо оновлення.");
                    double existingQuantity = Convert.ToDouble(result);
                    double additionalQuantity = Convert.ToDouble(txtRawQuantity.Text, CultureInfo.InvariantCulture);
                    double newQuantity = Math.Round(existingQuantity + additionalQuantity, 3);

                    string updateQuery = "UPDATE `raw_obtainiing` SET `raw_quantity` = @newQuantity, `obtaining_date` = @monthYear WHERE " +
                        "`raw_title` = @selectedRaw AND YEAR(`obtaining_date`) = YEAR(@monthYear) " +
                        "AND MONTH(`obtaining_date`) = MONTH(@monthYear);";

                    MySqlCommand updateCommand = new MySqlCommand(updateQuery, db.getConnection());
                    updateCommand.Parameters.AddWithValue("@newQuantity", newQuantity);
                    updateCommand.Parameters.AddWithValue("@selectedRaw", selectedRaw);
                    updateCommand.Parameters.AddWithValue("@monthYear", monthYear);

                    if (updateCommand.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Повторна поставка сировини успішно проведена");
                    }
                    else
                    {
                        MessageBox.Show("Не вдалося оновити дані.");
                    }
                }
                else
                {
                    MessageBox.Show("Записи не знайдені. Додаємо нові дані.");
                    addObtainingIfNOTChecked();
                }

                db.closeConnection();
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
