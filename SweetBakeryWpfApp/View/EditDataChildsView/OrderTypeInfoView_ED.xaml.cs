using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ConfectSoft.View;
using MySql.Data.MySqlClient;
using SweetBakeryWpfApp.Repositories;
using SweetBakeryWpfApp.ViewModel;

namespace SweetBakeryWpfApp.View.EditDataChildsView
{
    public partial class OrderTypeInfo_ED : UserControl
    {
        public OrderTypeInfo_ED()
        {
            InitializeComponent();


            this.PreviewMouseDown += IsFieldActive_PreviewMouseDown;


            //set default info to fields
            txtOrderTypeTitle.Text = "Назва...";
            txtOrderTypeTitle.GotFocus += RemoveTextOrderTypeTitle;
            txtOrderTypeTitle.LostFocus += AddTextOrderTypeTitle;

            txtCode.Text = "0000";
            txtCode.GotFocus += RemoveTextOrderTypeCode;
            txtCode.LostFocus += AddTextOrderTypeCode;

            BindOrderTypeInfo();

            bindStatusActivity("Відкрито сторінку 'Типи замовлень'");
        }


        //__________________LOAD INFORMATION_________________________

        //Show Order Type Data in Table
        public void BindOrderTypeInfo()
        {
            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `order_type`", db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            orderTypeInfo_editDataForm.ItemsSource = table.DefaultView;
        }




        //__________________ACTIONS REGION_________________________


        //Add Data to "Order type" Table
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string newId = txtCode.Text;
            string newTitle = txtOrderTypeTitle.Text;

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
                string checkQuery = "SELECT COUNT(*) FROM `order_type` WHERE `order_type_id` = @newId;";
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

                // if id is unique
                string query = "INSERT INTO `order_type` (`order_type_id`, `order_type_title`) VALUES (@newId, @newTitle);";
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

            BindOrderTypeInfo();
            txtOrderTypeTitle.Clear();
            txtCode.Clear();
            txtOrderTypeTitle.Text = "Назва...";
            txtCode.Text = "0000";

            bindStatusActivity("Додано новий тип замовлень");
        }


        //Delete Data from "Order type" Table
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            DataRowView rowView = (DataRowView)button.DataContext;

            int order_type_id = Convert.ToInt32(rowView["order_type_id"].ToString());

            DB_Connect db = new DB_Connect();
            MySqlCommand command = new MySqlCommand("DELETE FROM `order_type` WHERE `order_type_id` = @otid", db.getConnection());
            command.Parameters.Add("@otid", MySqlDbType.VarChar).Value = order_type_id;
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

            BindOrderTypeInfo();

            bindStatusActivity("Видалено дані з таблиці 'Типи замовлень'");
        }
        
        //Edit Data in "Order type"
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            DataRowView rowView = (DataRowView)button.DataContext;

            EditData2cpWindow_ED editWindow = new EditData2cpWindow_ED(rowView);
            editWindow.ShowDialog();

            // Оновити таблицю після закриття EditWindow
            BindOrderTypeInfo();
        }



       
        //__________________VALIDATION_________________________


        //Input only numbers 0-9
        private void numberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //Input only letters in Ukrainian
        private void letterValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-щьюяїієґА-ЩЬЮЯЇІЄҐ ]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //check if field active
        private void IsFieldActive_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (txtOrderTypeTitle != null && txtOrderTypeTitle.IsFocused && string.IsNullOrWhiteSpace(txtOrderTypeTitle.Text))
            {
                txtOrderTypeTitle.Text = "Назва...";
            }
            if (txtCode != null && txtCode.IsFocused && string.IsNullOrWhiteSpace(txtCode.Text))
            {
                txtCode.Text = "0000";
            }
        }


        //Change content of title field from ""->"text"
        public void AddTextOrderTypeTitle(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOrderTypeTitle.Text))
            {
                txtOrderTypeTitle.Text = "Назва...";
            }
        }

        //Change content of title field from "text"->""
        public void RemoveTextOrderTypeTitle(object sender, EventArgs e)
        {
           if (txtOrderTypeTitle.Text == "Назва...")
           {
              txtOrderTypeTitle.Text = "";
              txtOrderTypeTitle.Focus();
           }
        }

        //Change content of code field from ""->"text"
        public void AddTextOrderTypeCode(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                txtCode.Text = "0000";
            }
        }

        //Change content of code field from "text"->""
        public void RemoveTextOrderTypeCode(object sender, EventArgs e)
        {
            if (txtCode.Text == "0000")
            {
                txtCode.Text = "";
                txtCode.Focus();
            }
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
