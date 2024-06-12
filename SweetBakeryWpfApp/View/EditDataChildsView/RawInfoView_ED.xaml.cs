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
    public partial class RawInfoView_ED : UserControl
    {
        public RawInfoView_ED()
        {
            InitializeComponent();

           
            //set default info to fields
            this.PreviewMouseDown += IsFieldActive_PreviewMouseDown;

            txtRawTitle.Text = "Назва...";
            txtRawTitle.GotFocus += RemoveTextRawTitle;
            txtRawTitle.LostFocus += AddTextRawTitle;

            txtCode.Text = "0000";
            txtCode.GotFocus += RemoveTextRawCode;
            txtCode.LostFocus += AddTextRawCode;

            
            // load information
            BindRawInfo();

            bindStatusActivity("Відкрито сторінку 'Сировина'");
        }


        //__________________LOAD INFORMATION_________________________

        //Load db table 'raw' to window table
        public void BindRawInfo()
        {
           DB_Connect db = new DB_Connect();
           DataTable table = new DataTable();
           MySqlDataAdapter adapter = new MySqlDataAdapter();
           MySqlCommand command = new MySqlCommand("SELECT * FROM `raw`", db.getConnection());
           adapter.SelectCommand = command;
           adapter.Fill(table);
           rawInfo_editDataForm.ItemsSource = table.DefaultView;
        }


        
        
        
        //__________________ACTIONS REGION_________________________


        //Edit Data in "Raw"
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            DataRowView rowView = (DataRowView)button.DataContext;

            EditRawInfoActionFieldView_Ed editWindow = new EditRawInfoActionFieldView_Ed(rowView);
            editWindow.ShowDialog();

            BindRawInfo();
        }

        //Add Data to "Raw" Table
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string newId = txtCode.Text;
            string newTitle = txtRawTitle.Text;

            // Check default values in textblocks
            if (newId == "0000" || newTitle == "Назва...")
            {
                MessageBox.Show("Поля порожні, будь ласка введіть дані");
                return;
            }

            // Check the length of code
            if (newId.Length < 4)
            {
                MessageBox.Show("Код повинен містити 4 цифри");
                return;
            }

            using (DB_Connect db = new DB_Connect())
            {
                // Check if the id is unique
                string checkQuery = "SELECT COUNT(*) FROM `raw` WHERE `raw_id` = @newId;";
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
                string query = "INSERT INTO `raw` (`raw_id`, `raw_title`) VALUES (@newId, @newTitle);";
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

            BindRawInfo();
            txtRawTitle.Clear();
            txtCode.Clear();
            txtRawTitle.Text = "Назва...";
            txtCode.Text = "0000";

            bindStatusActivity("Додано дані в таблицю 'Сировина'");
        }


        //Delete Data from "Raw" Table
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            DataRowView rowView = (DataRowView)button.DataContext;

            int raw_id = Convert.ToInt32(rowView["raw_id"].ToString());

            DB_Connect db = new DB_Connect();
            MySqlCommand command = new MySqlCommand("DELETE FROM `raw` WHERE `raw_id` = @otid", db.getConnection());
            command.Parameters.Add("@otid", MySqlDbType.VarChar).Value = raw_id;
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

            BindRawInfo();

            bindStatusActivity("Видалено дані з таблиці 'Сировина'");
        }





        //__________________VALIDATION_________________________


        //check if field is active
        private void IsFieldActive_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
           if (txtRawTitle != null && txtRawTitle.IsFocused && string.IsNullOrWhiteSpace(txtRawTitle.Text))
           {
              txtRawTitle.Text = "Назва...";
           }
           if (txtCode != null && txtCode.IsFocused && string.IsNullOrWhiteSpace(txtCode.Text))
           {
              txtCode.Text = "0000";
           }
        }

        
        //Change content of title field from "text"->""
        public void RemoveTextRawTitle(object sender, EventArgs e)
        {
            if (txtRawTitle.Text == "Назва...")
            {
                txtRawTitle.Text = "";
                txtRawTitle.Focus();
            }
        }

        //Change content of title field from ""->"text"
        public void AddTextRawTitle(object sender, EventArgs e)
                {
                    if (string.IsNullOrWhiteSpace(txtRawTitle.Text))
                    {
                        txtRawTitle.Text = "Назва...";
                    }
                }

        
        
        //Change content of code field from "text"->""
        public void RemoveTextRawCode(object sender, EventArgs e)
        {
            if (txtCode.Text == "0000")
            {
                txtCode.Text = "";
                txtCode.Focus();
            }
        }

        //Change content of code field from ""->"text"
        public void AddTextRawCode(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
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

        //validation for Ukrainian letter
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
