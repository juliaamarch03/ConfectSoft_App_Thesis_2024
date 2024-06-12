using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MySql.Data.MySqlClient;
using SweetBakeryWpfApp.Repositories;
using ConfectSoft.View;
using SweetBakeryWpfApp.ViewModel;

namespace SweetBakeryWpfApp.View.EditDataChildsView
{
    public partial class CakesInfoView_ED : UserControl
    {
        public CakesInfoView_ED()
        {
            InitializeComponent();

            //Set Default Info
            this.PreviewMouseDown += IsFieldActive_PreviewMouseDown;

            txtCakeTitle.Text = "Назва...";
            txtCakeTitle.GotFocus += RemoveTextCakeTitle;
            txtCakeTitle.LostFocus += AddTextCakeTitle;

            txtCode.Text = "0000";
            txtCode.GotFocus += RemoveTextCakeCode;
            txtCode.LostFocus += AddTextCakeCode;

            txtCakePrice.Text = "00.00";
            txtCakePrice.GotFocus += RemoveTextCakePrice;
            txtCakePrice.LostFocus += AddTextCakePrice;


            //Load info in Window
            BindCakesInfo();
            bindStatusActivity("Відкрито сторінку 'Дані про випічку'");
        }


        //__________________LOAD INFORMATION_________________________

        //Load information from db table "cakes" to windows table
        public void BindCakesInfo()
        {
            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `cakes`", db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            cakesInfo_editDataForm.ItemsSource = table.DefaultView;
        }




        //__________________VALIDATION_________________________

        //Validation of in numbers (0-9)
        private void numberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //Validation of Ukrainian letter
        private void letterValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-щьюяїієґА-ЩЬЮЯЇІЄҐ ]+");
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



        //Change content of price field from "text"->""
        public void RemoveTextCakePrice(object sender, EventArgs e)
        {
            if (txtCakePrice.Text == "00.00")
            {
                txtCakePrice.Text = "";
                txtCakePrice.Focus();
            }
        }

        //Change content of price field from ""->"text"
        public void AddTextCakePrice(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCakePrice.Text))
            {
                txtCakePrice.Text = "00.00";
            }
        }

        //Change content of cake title field from ""->"text"
        public void AddTextCakeTitle(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCakeTitle.Text))
            {
                txtCakeTitle.Text = "Назва...";
            }
        }

        //Change content of cake title field from "text"->""
        public void RemoveTextCakeTitle(object sender, EventArgs e)
        {
            if (txtCakeTitle.Text == "Назва...")
            {
                txtCakeTitle.Text = "";
                txtCakeTitle.Focus();
            }
        }

        //Change content of cake code field from ""->"text"
        public void AddTextCakeCode(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                txtCode.Text = "0000";
            }
        }

        //Change content of cake code field from "text"->""
        public void RemoveTextCakeCode(object sender, EventArgs e)
        {
            if (txtCode.Text == "0000")
            {
                txtCode.Text = "";
                txtCode.Focus();
            }
        }

        //Validation Is Field active / No -> change to title of field 
        private void IsFieldActive_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (txtCakeTitle != null && txtCakeTitle.IsFocused && string.IsNullOrWhiteSpace(txtCakeTitle.Text))
            {
                txtCakeTitle.Text = "Назва...";
            }
            if (txtCode != null && txtCode.IsFocused && string.IsNullOrWhiteSpace(txtCode.Text))
            {
                txtCode.Text = "0000";
            }
            if (txtCakePrice != null && txtCakePrice.IsFocused && string.IsNullOrWhiteSpace(txtCakePrice.Text))
            {
                txtCakePrice.Text = "00.00";
            }
        }






        //__________________ACTIONS REGION_________________________


        //Edit Data in "Cakes"
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            DataRowView rowView = (DataRowView)button.DataContext;

            EditCakesInfoActionField_ED editWindow = new EditCakesInfoActionField_ED(rowView);
            editWindow.ShowDialog();

            BindCakesInfo();
        }

        //Add Data to "Cakes" Table
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string newId = txtCode.Text;
            string newTitle = txtCakeTitle.Text;
            string newPrice = txtCakePrice.Text;

            // Перевірка на дефолтні значення
            if (newId == "0000" || newTitle == "Назва..." || newPrice == "00.00")
            {
                MessageBox.Show("Поля порожні, будь ласка введіть дані");
                return;
            }

            // Перевірка на довжину коду
            if (newId.Length < 4)
            {
                MessageBox.Show("Код повинен містити 4 цифри");
                return;
            }

            using (DB_Connect db = new DB_Connect())
            {
                // Перевірка на унікальність коду
                string checkQuery = "SELECT COUNT(*) FROM `cakes` WHERE `cake_id` = @newId;";
                MySqlCommand checkCommand = new MySqlCommand(checkQuery, db.getConnection());
                checkCommand.Parameters.AddWithValue("@newId", newId);

                db.openConnection();

                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Такий код вже існує, створіть інший");
                    db.closeConnection();
                    return;
                }

                db.closeConnection();

                // Якщо код унікальний, додаємо новий запис
                string query = "INSERT INTO `cakes` (`cake_id`, `cake_title`, `cake_price_kg`) VALUES (@newId, @newTitle, @newPrice);";
                MySqlCommand command = new MySqlCommand(query, db.getConnection());

                command.Parameters.AddWithValue("@newId", newId);
                command.Parameters.AddWithValue("@newTitle", newTitle);
                command.Parameters.AddWithValue("@newPrice", newPrice);

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

            BindCakesInfo();
            txtCakeTitle.Clear();
            txtCode.Clear();
            txtCakePrice.Clear();
            txtCakeTitle.Text = "Назва...";
            txtCode.Text = "0000";
            txtCakePrice.Text = "00.00";

            bindStatusActivity("Додано нову випічку до системи");
        }

        //Delete Data from "Cakes" Table
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            DataRowView rowView = (DataRowView)button.DataContext;

            int cake_id = Convert.ToInt32(rowView["cake_id"].ToString());

            DB_Connect db = new DB_Connect();
            MySqlCommand command = new MySqlCommand("DELETE FROM `cakes` WHERE `cake_id` = @otid", db.getConnection());
            command.Parameters.Add("@otid", MySqlDbType.VarChar).Value = cake_id;
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

            BindCakesInfo();

            bindStatusActivity("Видалено дані з таблиці 'Торти' ");
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

        private void btnEditData_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
