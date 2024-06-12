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
    public partial class ShopsInfoView_ED : UserControl
    {
        public ShopsInfoView_ED()
        {
            InitializeComponent();

            
            //set default info to fields
            this.PreviewMouseDown += IsFieldActive_PreviewMouseDown;


            txtShopTitle.Text = "Назва...";
            txtShopTitle.GotFocus += RemoveTextShopTitle;
            txtShopTitle.LostFocus += AddTextShopTitle;

            txtCode.Text = "0000";
            txtCode.GotFocus += RemoveTextShopCode;
            txtCode.LostFocus += AddTextShopCode;

            //load information
            BindShopsInfo();

            bindStatusActivity("Відкрито сторінку 'Клієнти'");
        }

   
        
        //__________________ACTIONS REGION_________________________


        //Edit Data in "Shops"
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            DataRowView rowView = (DataRowView)button.DataContext;

            EditShopInfoActionFieldView_ED editWindow = new EditShopInfoActionFieldView_ED(rowView);
            editWindow.ShowDialog();

            BindShopsInfo();
        }

        //Add Data to "Shops" Table
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string newId = txtCode.Text;
            string newTitle = txtShopTitle.Text;

            // Check default values in textblocks
            if (newId == "0000" || newTitle == "Назва...")
            {
                MessageBox.Show("Поля порожні, будь ласка введіть дані");
                return;
            }

            // Check the length of the id
            if (newId.Length < 4)
            {
                MessageBox.Show("Код повинен містити 4 цифри");
                return;
            }

            using (DB_Connect db = new DB_Connect())
            {
                // Check if the id is unique
                string checkQuery = "INSERT INTO `shops` (`shop_id`, `shop_title`) VALUES (@newId, @newTitle);";
                MySqlCommand checkCommand = new MySqlCommand(checkQuery, db.getConnection());

                checkCommand.Parameters.AddWithValue("@newId", newId);
                checkCommand.Parameters.AddWithValue("@newTitle", newTitle);

                db.openConnection();

                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Такий код вже існує, створіть інший");
                    db.closeConnection();
                    return;
                }

                db.closeConnection();

                // if id is unique
                string query = "INSERT INTO `shops` (`shop_id`, `shop_title`) VALUES (@newId, @newTitle);";
                MySqlCommand command = new MySqlCommand(query, db.getConnection());

                command.Parameters.AddWithValue("@newId", newId);
                command.Parameters.AddWithValue("@newTitle", newTitle);

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

            BindShopsInfo();
            txtShopTitle.Clear();
            txtCode.Clear();
            txtShopTitle.Text = "Назва...";
            txtCode.Text = "0000";

            bindStatusActivity("Додано дані в таблицю 'Клієнти'");
        }

        //Delete Data from "Shops" Table
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            DataRowView rowView = (DataRowView)button.DataContext;

            int shop_id = Convert.ToInt32(rowView["shop_id"].ToString());

            DB_Connect db = new DB_Connect();
            MySqlCommand command = new MySqlCommand("DELETE FROM `shops` WHERE `shop_id` = @otid", db.getConnection());
            command.Parameters.Add("@otid", MySqlDbType.VarChar).Value = shop_id;
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

            BindShopsInfo();

            bindStatusActivity("Видалення даних з таблиці 'Клієнти'");
        }



        
        //__________________LOAD INFORMATION_________________________

        
        //load info from db table 'shops' to window table
        public void BindShopsInfo()
        {
            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `shops`", db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            shopsInfo_editDataForm.ItemsSource = table.DefaultView;
        }


        
        
        //__________________VALIDATION_________________________


        //Change content of title field from "text"->""
        public void RemoveTextShopTitle(object sender, EventArgs e)
        {
            if (txtShopTitle.Text == "Назва...")
            {
                txtShopTitle.Text = "";
                txtShopTitle.Focus();
            }
        }

        //Change content of title field from ""->"text"
        public void AddTextShopTitle(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtShopTitle.Text))
            {
                txtShopTitle.Text = "Назва...";
            }
        }


        //Change content of title field from "text"->""
        public void RemoveTextShopCode(object sender, EventArgs e)
        {
            if (txtCode.Text == "0000")
            {
                txtCode.Text = "";
                txtCode.Focus();
            }
        }   

        //Change content of code field from ""->"text"
        public void AddTextShopCode(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                txtCode.Text = "0000";
            }
        }

        
        //check if field is active
        private void IsFieldActive_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (txtShopTitle != null && txtShopTitle.IsFocused && string.IsNullOrWhiteSpace(txtShopTitle.Text))
            {
                txtShopTitle.Text = "Назва...";
            }
            if (txtCode != null && txtCode.IsFocused && string.IsNullOrWhiteSpace(txtCode.Text))
            {
                txtCode.Text = "0000";
            }
        }
        
        
        
        //validation for int numbers (0-9)
        private void numberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
       
        //validation for ukrainian letters
        private void letterValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-щьюяїієґА-ЩЬЮЯЇІЄҐ ]+");
            e.Handled = regex.IsMatch(e.Text);
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
