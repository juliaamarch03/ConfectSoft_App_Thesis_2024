using MySql.Data.MySqlClient;
using SweetBakeryWpfApp.Repositories;
using SweetBakeryWpfApp.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace ConfectSoft.View
{
    public class UserActivity
    {
        public int action_id { get; set; }
        public string action_title { get; set; }
        public int users_username { get; set; }
        public string user_fullname { get; set; }
        public DateTime action_time { get; set; }
    }

    public partial class UsersHistoryActivity_View : UserControl
    {
        ObservableCollection<UserActivity> UserActivityCollection = new ObservableCollection<UserActivity>();

        public UsersHistoryActivity_View()
        {
            InitializeComponent();
            loadUsersPersonalCodeList();
            loadDefaultUserActivity();
        }

        private void btnSearchUsersActivity_Click(object sender, RoutedEventArgs e)
        {
            string _personalCode = comboBoxUsersPersonalCode.SelectedValue.ToString();
            string _actionDateStart = actionDate.SelectedDate.Value.ToString("yyyy-MM-dd 00:00:00");
            string _actionDateEnd = actionDate.SelectedDate.Value.ToString("yyyy-MM-dd 23:59:59");

            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string query = $"SELECT * FROM `users_actions` WHERE action_time BETWEEN @actionDateStart AND @actionDateEnd";

            if (_personalCode != "Всі користувачі")
            {
                query += " AND users_username = @personalCode";
            }

            MySqlCommand command = new MySqlCommand(query, db.getConnection());
            command.Parameters.Add("@actionDateStart", MySqlDbType.DateTime).Value = _actionDateStart;
            command.Parameters.Add("@actionDateEnd", MySqlDbType.DateTime).Value = _actionDateEnd;

            if (_personalCode != "Всі користувачі")
            {
                command.Parameters.Add("@personalCode", MySqlDbType.VarChar).Value = _personalCode;
            }

            adapter.SelectCommand = command;
            adapter.Fill(table);

            UserActivityCollection.Clear();

            foreach (DataRow row in table.Rows)
            {
                UserActivityCollection.Add(new UserActivity
                {
                    action_id = row["action_id"] != DBNull.Value ? Convert.ToInt32(row["action_id"]) : 0,
                    action_title = row["action_title"] != DBNull.Value ? row["action_title"].ToString() : string.Empty,
                    users_username = row["users_username"] != DBNull.Value ? Convert.ToInt32(row["users_username"]) : 0,
                    user_fullname = row["user_fullname"] != DBNull.Value ? row["user_fullname"].ToString() : string.Empty,
                    action_time = row["action_time"] != DBNull.Value ? Convert.ToDateTime(row["action_time"]) : DateTime.MinValue
                });
            }

            usersActiviteTbl.ItemsSource = UserActivityCollection;
            btnSearchUsersActivity.Visibility = Visibility.Collapsed;
            btnCancelSearchUsersActivity.Visibility = Visibility.Visible;

            bindStatusActivity($"Здійснено пошук історії користувача {_personalCode} за датою {_actionDateStart}");
        }


        //load list with users personal code
        private void loadUsersPersonalCodeList()
        {
            try
            {
                DB_Connect db = new DB_Connect();
                db.openConnection();
                MySqlCommand sc = new MySqlCommand("SELECT * FROM `users`", db.getConnection());
                MySqlDataReader reader;
                reader = sc.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("username", typeof(string));
                dt.Load(reader);

                // Add a record "All users" до DataTable
                DataRow row = dt.NewRow();
                row["username"] = "Всі користувачі";
                dt.Rows.InsertAt(row, 0);

                comboBoxUsersPersonalCode.ItemsSource = dt.DefaultView;
                comboBoxUsersPersonalCode.DisplayMemberPath = "username";
                comboBoxUsersPersonalCode.SelectedValuePath = "username";
                db.closeConnection();
            }
            catch (Exception)
            {

            }

            comboBoxUsersPersonalCode.SelectedIndex = 0;
        }

        //Show all users activity as defaut view
        private void loadDefaultUserActivity()
        {
            DB_Connect db = new DB_Connect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand($"SELECT * FROM `users_actions` ORDER BY `action_time` DESC", db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            foreach (DataRow row in table.Rows)
            {
                UserActivityCollection.Add(new UserActivity
                {
                    action_id = row["action_id"] != DBNull.Value ? Convert.ToInt32(row["action_id"]) : 0,
                    action_title = row["action_title"] != DBNull.Value ? row["action_title"].ToString() : string.Empty,
                    users_username = row["users_username"] != DBNull.Value ? Convert.ToInt32(row["users_username"]) : 0,
                    user_fullname = row["user_fullname"] != DBNull.Value ? row["user_fullname"].ToString() : string.Empty,
                    action_time = row["action_time"] != DBNull.Value ? Convert.ToDateTime(row["action_time"]) : DateTime.MinValue
                });
            }

            usersActiviteTbl.ItemsSource = UserActivityCollection;
        }        

        // show user's full name when select from combobox
        private void comboBoxUsersPersonalCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView row = (DataRowView)comboBoxUsersPersonalCode.SelectedItem;
            string _userName = row["user_name"].ToString();
            string _userSurName = row["user_surname"].ToString();
            txtUserFullName.Text = _userName + " "+ _userSurName;
        }

        //Method to delete record from database and window table
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            UserActivity rowView = (UserActivity)button.DataContext;

            int action_id = Convert.ToInt32(rowView.action_id.ToString());

            DB_Connect db = new DB_Connect();
            MySqlCommand command = new MySqlCommand("DELETE FROM `users_actions` WHERE `action_id` = @otid", db.getConnection());
            command.Parameters.Add("@otid", MySqlDbType.VarChar).Value = action_id;
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

            bindStatusActivity("Здійснено видалення даних з таблиці історія користувачів");

            UserActivityCollection.Remove(rowView);            
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

        private void btnCancelSearchUsersActivity_Click(object sender, RoutedEventArgs e)
        { 
            btnCancelSearchUsersActivity.Visibility = Visibility.Collapsed;
            btnSearchUsersActivity.Visibility = Visibility.Visible;
            UserActivityCollection.Clear();
            loadDefaultUserActivity();
        }
    }
}
