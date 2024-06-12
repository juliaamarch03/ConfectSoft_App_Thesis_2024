// ADMIN PAGE
// PAGE WITH USERS' DATA

using MySql.Data.MySqlClient;
using SweetBakeryWpfApp.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConfectSoft.View
{
    public partial class UsersDataView : UserControl
    {
        List<string> roles = new List<string> { "Головний кондитер", "Кондитер", "Адмін" };

        private Dictionary<string, object> _initialData = new Dictionary<string, object>();

        private string _userRole;
        private string _name;
        private string _surname;
        private string _email;
        private string _phoneNumber;
        private DateTime _birthday;
        private string _country;
        private int _postalCode;
        private string _city;
        private string _region;
        private int _personalCode;
        private string _password;

        // Constructor
        public UsersDataView()
        {
            InitializeComponent();
            loadUsersPersonalCodeList();
            loadDataToComboBox();

            userPasswordBox.Password = "12345678";

            CultureInfo culture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

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

                comboBoxUsersPersonalCode.ItemsSource = null;
                comboBoxUsersPersonalCode.ItemsSource = dt.DefaultView;
                comboBoxUsersPersonalCode.DisplayMemberPath = "username";
                comboBoxUsersPersonalCode.SelectedValuePath = "username";
                db.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при завантаженні даних: " + ex.Message);
            }

            comboBoxUsersPersonalCode.SelectedIndex = 0;
        }

        //select data based on user's personal code
        private void comboBoxUsersPersonalCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxUsersPersonalCode.SelectedItem == null)
                return;

            if (comboBoxUsersPersonalCode.SelectedItem != null)
            {
                LoadSelectedUserData();
            }

            DataRowView row = (DataRowView)comboBoxUsersPersonalCode.SelectedItem;
            string selectedUsername = row["username"].ToString();

            DB_Connect db = new DB_Connect();

            using (db)
            {
                db.openConnection();
                MySqlCommand sc = new MySqlCommand("SELECT * FROM `users` WHERE `username`=@username", db.getConnection());
                sc.Parameters.AddWithValue("@username", selectedUsername);
                MySqlDataReader reader = sc.ExecuteReader();

                if (reader.Read())
                {
                    string _userName = reader["user_name"].ToString();
                    string _userSurName = reader["user_surname"].ToString();
                    int _personalCode = Convert.ToInt32(reader["username"].ToString());
                    string _userPassword = reader["user_password"].ToString();
                    string _userRole = reader["user_role"].ToString();
                    DateTime _userBirthday = Convert.ToDateTime(reader["user_birthday"].ToString());
                    string _userEmail = reader["user_email"].ToString();
                    string _userPhone = reader["user_phone"].ToString();
                    string _userCountry = reader["user_country"].ToString();
                    string _userCity = reader["user_city"].ToString();
                    string _userRegion = reader["user_region"].ToString();
                    int _userPostalCode = Convert.ToInt32(reader["user_postal_code"].ToString());

                    // 1 section
                    userFullNameLabel.Content = _userName + " " + _userSurName;
                    userPositionLabel.Text = _userRole;
                    userLocationLabel.Text = _userCity + ", " + _userRegion + ", " + _userCountry;

                    // 2 section
                    userFirstNameLabel.Text = _userName;
                    userLastNameLabel.Text = _userSurName;
                    userEmailAdressLabel.Text = _userEmail;
                    userPhoneNumberLabel.Text = _userPhone;
                    userBDayLabel.Text = _userBirthday.ToString("dd.MM.yyyy");
                    maskUserPhoneNumber.Text = _userPhone;
                    maskUserBDay.Text = _userBirthday.ToString();

                    // 3 section
                    userCountryLabel.Text = _userCountry;
                    userLocationSecLabel.Text = _userCity + ", " + _userRegion;
                    userPostCodeLabel.Text = _userPostalCode.ToString();

                    // 4 section
                    personalCode.Text = _personalCode.ToString();
                    userPassword.Text = _userPassword.ToString();
                }

                db.closeConnection();
            }
        }
        
        //load list with user's role
        private void loadDataToComboBox()
        {
            foreach (string role in roles)
            {
                userRoleComboBox.Items.Add(role);
            }
            userRoleComboBox.SelectedIndex = 0;
        }
        
        
        //save initial data for canceling edit mode
        private void SaveInitialData()
        {
            _initialData["userRole"] = userRoleComboBox.Text;
            _initialData["name"] = userFirstNameLabel.Text;
            _initialData["surname"] = userLastNameLabel.Text;
            _initialData["email"] = userEmailAdressLabel.Text;
            _initialData["phoneNumber"] = userPhoneNumberLabel.Text;
            _initialData["birthday"] = userBDayLabel.Text;
            _initialData["country"] = userCountryLabel.Text;
            _initialData["postalCode"] = userPostCodeLabel.Text;
            _initialData["city"] = userLocationSecLabel.Text.Split(',')[0].Trim();
            _initialData["region"] = userLocationSecLabel.Text.Split(',')[1].Trim();
            _initialData["personalCode"] = personalCode.Text;
            _initialData["password"] = userPassword.Text;
        }

        //restore initital data after closing edit mode
        private void RestoreInitialData()
        {
            userRoleComboBox.Text = _initialData["userRole"].ToString();
            userFirstNameLabel.Text = _initialData["name"].ToString();
            userLastNameLabel.Text = _initialData["surname"].ToString();
            userEmailAdressLabel.Text = _initialData["email"].ToString();
            userPhoneNumberLabel.Text = _initialData["phoneNumber"].ToString();
            userBDayLabel.Text = _initialData["birthday"].ToString();
            userCountryLabel.Text = _initialData["country"].ToString();
            userPostCodeLabel.Text = _initialData["postalCode"].ToString();
            userLocationSecLabel.Text = $"{_initialData["city"]}, {_initialData["region"]}";
            personalCode.Text = _initialData["personalCode"].ToString();
            userPassword.Text = _initialData["password"].ToString();
        }

       
        
        // EDITING MODE
        
        //open editing mode
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            SaveInitialData();
            
            //Activate fields to editing
            Style newStyle = this.FindResource("txtBoxUserProfResultStyleActiveToEdit") as Style;

            // 2 section
            userFirstNameLabel.Style = newStyle;
            userLastNameLabel.Style = newStyle;
            userEmailAdressLabel.Style = newStyle;
            userPhoneNumberLabel.Style = newStyle;
            userBDayLabel.Style = newStyle;
            //3 section
            userCountryLabel.Style = newStyle;
            userLocationSecLabel.Style = newStyle;
            userPostCodeLabel.Style = newStyle;

            //4 section
            personalCode.Style = newStyle;

            // Deactivate buttons and combobox
            addNewUserBtn.Visibility = Visibility.Collapsed;
            removeUserBtn.Visibility = Visibility.Collapsed;
            activateEditModeBtn.Visibility = Visibility.Collapsed;
            comboBoxUsersPersonalCode.IsEnabled = false;

            //Show new buttons
            confirmEditBtn.Visibility = Visibility.Visible;
            closeEditingModeBtn.Visibility = Visibility.Visible;
            generatePassword.Visibility = Visibility.Visible;
            generatePersonalCode.Visibility = Visibility.Visible;

            userRoleComboBox.Visibility = Visibility.Visible;
            userPositionLabel.Visibility = Visibility.Collapsed;

            maskUserPhoneNumber.Visibility = Visibility.Visible;
            userPhoneNumberLabel.Visibility = Visibility.Collapsed;
        }
        
        //confirm data editing
        private void confirmEditBtn_Click(object sender, RoutedEventArgs e)
        {
            // 1 section
            string newUserRole = userRoleComboBox.Text;

            // 2 section 
            string newName = userFirstNameLabel.Text;
            string newSurname = userLastNameLabel.Text;
            string newEmail = userEmailAdressLabel.Text;
            string newPhoneNumber = userPhoneNumberLabel.Text;
            DateTime newBirthday = DateTime.ParseExact(userBDayLabel.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            string formattedBday = newBirthday.ToString("yyyy-MM-dd");

            // 3 section
            string newCountry = userCountryLabel.Text;
            int newPostalCode = Convert.ToInt32(userPostCodeLabel.Text);

            string cityAndRegion = userLocationSecLabel.Text;
            string[] parts = cityAndRegion.Split(',');
            string newCity = parts[0].Trim();
            string newRegion = parts[1].Trim();

            // 4 section
            int newPersonalCode = Convert.ToInt32(personalCode.Text);
            string newPassword = userPassword.Text;

            DB_Connect db = new DB_Connect();

            using (db)
            {
                string query = $"UPDATE `users` SET `user_name`=@_name_, `user_surname`=@_surname_, `user_email`=@_email_, " +
                    $"`user_phone`=@_phoneNumber_, `user_birthday`=@_birthday_, `user_country`=@_country_, `user_postal_code`=@_postalCode_, " +
                    $"`user_city`=@_city_, `user_region`=@_region_, `username`=@_personalCode_, `user_password`=@_password_ " +
                    $"WHERE `username`=@_oldPersCode_;";
                MySqlCommand command = new MySqlCommand(query, db.getConnection());

                command.Parameters.AddWithValue("@_name_", newName);
                command.Parameters.AddWithValue("@_surname_", newSurname);
                command.Parameters.AddWithValue("@_email_", newEmail);
                command.Parameters.AddWithValue("@_phoneNumber_", newPhoneNumber);
                command.Parameters.AddWithValue("@_birthday_", formattedBday);
                command.Parameters.AddWithValue("@_country_", newCountry);
                command.Parameters.AddWithValue("@_postalCode_", newPostalCode);
                command.Parameters.AddWithValue("@_city_", newCity);
                command.Parameters.AddWithValue("@_region_", newRegion);
                command.Parameters.AddWithValue("@_personalCode_", newPersonalCode);
                command.Parameters.AddWithValue("@_password_", newPassword);
                command.Parameters.AddWithValue("@_oldPersCode_", comboBoxUsersPersonalCode.Text);

                db.openConnection();

                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Успішно змінено!");

                    // update local values with new data
                    _userRole = newUserRole;
                    _name = newName;
                    _surname = newSurname;
                    _email = newEmail;
                    _phoneNumber = newPhoneNumber;
                    _birthday = newBirthday;
                    _country = newCountry;
                    _postalCode = newPostalCode;
                    _city = newCity;
                    _region = newRegion;
                    _personalCode = newPersonalCode;
                    _password = newPassword;

                    loadUsersPersonalCodeList();

                    // update visual elements with new data
                    userRoleComboBox.Text = _userRole;
                    userFirstNameLabel.Text = _name;
                    userLastNameLabel.Text = _surname;
                    userEmailAdressLabel.Text = _email;
                    userPhoneNumberLabel.Text = _phoneNumber;
                    userBDayLabel.Text = _birthday.ToString("dd.MM.yyyy");
                    userCountryLabel.Text = _country;
                    userPostCodeLabel.Text = _postalCode.ToString();
                    userLocationSecLabel.Text = $"{_city}, {_region}";
                    personalCode.Text = _personalCode.ToString();
                    userPassword.Text = _password;
                }
                else
                {
                    MessageBox.Show("Помилка!");
                }

                db.closeConnection();
            }

            showNewData();
            backToReadMode();
        }
        
        //show new data that was updated after editing
        private void showNewData()
        {
            userRoleComboBox.Text = _userRole;
            userFirstNameLabel.Text = _name;
            userLastNameLabel.Text = _surname;
            userEmailAdressLabel.Text = _email;
            userPhoneNumberLabel.Text = _phoneNumber;
            userBDayLabel.Text = _birthday.ToString("dd.MM.yyyy");
            userCountryLabel.Text = _country;
            userPostCodeLabel.Text = _postalCode.ToString();
            userLocationSecLabel.Text = $"{_city}, {_region}";
            personalCode.Text = _personalCode.ToString();
            userPassword.Text = _password;
        }

        //close editing mode
        private void closeEditingModeBtn_Click(object sender, RoutedEventArgs e)
        {
            RestoreInitialData();
            backToReadMode();
        }

        
        
        // ADDING NEW USER MODE
        
        //activate adding mode
        private void addNewUserBtn_Click(object sender, RoutedEventArgs e)
        {
            activateAddNewUserMode();
        }

        //activate fields for adding mode
        private void activateAddNewUserMode()
        {
            ClearFields();

            // Activate fields to editing
            Style newStyle = this.FindResource("txtBoxUserProfResultStyleActiveToEdit") as Style;

            // 2 section
            userFirstNameLabel.Style = newStyle;
            userLastNameLabel.Style = newStyle;
            userEmailAdressLabel.Style = newStyle;
            userPhoneNumberLabel.Style = newStyle;
            userBDayLabel.Style = newStyle;
            
            // 3 section
            userCountryLabel.Style = newStyle;
            userLocationSecLabel.Style = newStyle;
            userPostCodeLabel.Style = newStyle;
            
            // 4 section
            personalCode.Style = newStyle; // personal code

            // Deactivate buttons and combobox
            addNewUserBtn.Visibility = Visibility.Collapsed;
            removeUserBtn.Visibility = Visibility.Collapsed;
            activateEditModeBtn.Visibility = Visibility.Collapsed;
            comboBoxUsersPersonalCode.IsEnabled = false;

            // Show new buttons
            confirmAddingBtn.Visibility = Visibility.Visible;
            closeAddingMode.Visibility = Visibility.Visible;
            generatePassword.Visibility = Visibility.Visible;
            generatePersonalCode.Visibility = Visibility.Visible;

            userRoleComboBox.Visibility = Visibility.Visible;
            userPositionLabel.Visibility = Visibility.Collapsed;
            maskUserPhoneNumber.Visibility = Visibility.Visible;
            userPhoneNumberLabel.Visibility = Visibility.Collapsed;  
        }

        // Mode to back the reading mode
        private void backToReadMode()
        {
            // Activate buttons and combobox
            addNewUserBtn.Visibility = Visibility.Visible;
            removeUserBtn.Visibility = Visibility.Visible;
            activateEditModeBtn.Visibility = Visibility.Visible;
            comboBoxUsersPersonalCode.IsEnabled = true;

            // Close new buttons
            confirmEditBtn.Visibility = Visibility.Collapsed;
            closeEditingModeBtn.Visibility = Visibility.Collapsed;

            Style newStyle = this.FindResource("txtBoxUserProfResultStyle") as Style;
            // 1 section

            // 2 section
            userFirstNameLabel.Style = newStyle;
            userLastNameLabel.Style = newStyle;
            userEmailAdressLabel.Style = newStyle;
            userPhoneNumberLabel.Style = newStyle;
            userBDayLabel.Style = newStyle;
            
            //3 section
            userCountryLabel.Style = newStyle;
            userLocationSecLabel.Style = newStyle;
            userPostCodeLabel.Style = newStyle;

            //4 section
            personalCode.Style = newStyle;

            userRoleComboBox.Visibility = Visibility.Collapsed;
            generatePassword.Visibility = Visibility.Collapsed;
            userPositionLabel.Visibility = Visibility.Visible;
            generatePersonalCode.Visibility = Visibility.Collapsed;
            confirmAddingBtn.Visibility = Visibility.Collapsed;
            closeAddingMode.Visibility = Visibility.Collapsed;
            

            maskUserBDay.Visibility = Visibility.Collapsed;
            maskUserPhoneNumber.Visibility = Visibility.Collapsed;
            userPhoneNumberLabel.Visibility = Visibility.Visible;
            userBDayLabel.Visibility = Visibility.Visible;
        }

        // confirm adding of new user
        private void confirmAddingBtn_Click(object sender, RoutedEventArgs e)
        {
            // check if all field were filled with data
            if (string.IsNullOrWhiteSpace(userFirstNameLabel.Text) ||
                string.IsNullOrWhiteSpace(userLastNameLabel.Text) ||
                string.IsNullOrWhiteSpace(userEmailAdressLabel.Text) ||
                string.IsNullOrWhiteSpace(maskUserPhoneNumber.Text) ||
                string.IsNullOrWhiteSpace(userBDayLabel.Text) ||
                string.IsNullOrWhiteSpace(userCountryLabel.Text) ||
                string.IsNullOrWhiteSpace(userLocationSecLabel.Text) ||
                string.IsNullOrWhiteSpace(userPostCodeLabel.Text) ||
                string.IsNullOrWhiteSpace(personalCode.Text) ||
                string.IsNullOrWhiteSpace(userPassword.Text))
            {
                MessageBox.Show("Будь ласка, заповніть всі поля.");
                return;
            }

            // Get data from fields
            string _userRole = userRoleComboBox.Text;
            string _name = userFirstNameLabel.Text;
            string _surname = userLastNameLabel.Text;
            string _email = userEmailAdressLabel.Text;
            string _phoneNumber = maskUserPhoneNumber.Text;
            DateTime _birthday = DateTime.ParseExact(userBDayLabel.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            string _formattedBday = _birthday.ToString("yyyy-MM-dd");
            string _country = userCountryLabel.Text;
            int _postalCode = Convert.ToInt32(userPostCodeLabel.Text);
            string cityAndRegion = userLocationSecLabel.Text;
            string[] parts = cityAndRegion.Split(',');
            string _city = parts[0].Trim();
            string _region = parts[1].Trim();
            int _personalCode = Convert.ToInt32(personalCode.Text);
            string _password = userPassword.Text;

            DB_Connect db = new DB_Connect();

            using (db)
            {
                db.openConnection();
                string query = "INSERT INTO `users` (`user_name`, `user_surname`, `user_email`, `user_phone`, " +
                    "`user_birthday`, `user_country`, `user_postal_code`, `user_city`, `user_region`, `username`, `user_password`, `user_role`) " +
                    "VALUES (@_name_, @_surname_, @_email_, @_phoneNumber_, @_birthday_, @_country_, @_postalCode_, @_city_, @_region_, " +
                    "@_personalCode_, @_password_, @_userRole_);";
                MySqlCommand command = new MySqlCommand(query, db.getConnection());

                command.Parameters.AddWithValue("@_name_", _name);
                command.Parameters.AddWithValue("@_surname_", _surname);
                command.Parameters.AddWithValue("@_email_", _email);
                command.Parameters.AddWithValue("@_phoneNumber_", _phoneNumber);
                command.Parameters.AddWithValue("@_birthday_", _formattedBday);
                command.Parameters.AddWithValue("@_country_", _country);
                command.Parameters.AddWithValue("@_postalCode_", _postalCode);
                command.Parameters.AddWithValue("@_city_", _city);
                command.Parameters.AddWithValue("@_region_", _region);
                command.Parameters.AddWithValue("@_personalCode_", _personalCode);
                command.Parameters.AddWithValue("@_password_", _password);
                command.Parameters.AddWithValue("@_userRole_", _userRole);

                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Користувача успішно додано!");

                    // update combobox
                    loadUsersPersonalCodeList();

                    // selekt new user in combobox
                    comboBoxUsersPersonalCode.SelectedItem = personalCode.Text;

                    // exit adding mode
                    backToReadMode();
                }
                else
                {
                    MessageBox.Show("Помилка при додаванні користувача!");
                }

                db.closeConnection();
            }
        }

        private void closeAddingMode_Click(object sender, RoutedEventArgs e)
        {
            cancelAddNewUser();
        }

        private void DisplayUserData(string _personalCode)
        {
            try
            {
                DB_Connect db = new DB_Connect();
                db.openConnection();
                MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `username`=@personalCode", db.getConnection());
                command.Parameters.AddWithValue("@personalCode", _personalCode);
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // Заповнення полів даними користувача
                    userFullNameLabel.Content = reader["user_name"].ToString() + reader["user_surname"].ToString();
                    userLocationLabel.Text = $"{reader["user_city"]}, {reader["user_region"]}" + ", " + reader["user_country"].ToString();
                    userFirstNameLabel.Text = reader["user_name"].ToString();
                    userLastNameLabel.Text = reader["user_surname"].ToString();
                    userEmailAdressLabel.Text = reader["user_email"].ToString();
                    userPhoneNumberLabel.Text = reader["user_phone"].ToString();
                    userBDayLabel.Text = Convert.ToDateTime(reader["user_birthday"]).ToString("dd.MM.yyyy");
                    userCountryLabel.Text = reader["user_country"].ToString();
                    userLocationSecLabel.Text = $"{reader["user_city"]}, {reader["user_region"]}";
                    userPostCodeLabel.Text = reader["user_postal_code"].ToString();
                    personalCode.Text = reader["username"].ToString();
                    userPassword.Text = reader["user_password"].ToString();

                    userRoleComboBox.Text = reader["user_role"].ToString();
                    userPositionLabel.Text = reader["user_role"].ToString();
                    maskUserPhoneNumber.Text = reader["user_phone"].ToString();
                    maskUserBDay.Text = Convert.ToDateTime(reader["user_birthday"]).ToString("dd.MM.yyyy");
                }

                db.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при завантаженні даних користувача: " + ex.Message);
            }
        }

        private void cancelAddNewUser()
        {
            // Повернення до режиму перегляду
            backToReadMode();

            // Відобразити дані користувача, вибраного в комбобоксі
            if (comboBoxUsersPersonalCode.SelectedItem != null)
            {
                string personalCode = comboBoxUsersPersonalCode.Text;
                DisplayUserData(personalCode);
            }
        }



        // GENERATING DATA

        // generate user's password
        private void generatePassword_Click(object sender, RoutedEventArgs e)
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Random random = new Random();
            string password = new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            userPassword.Text = password;
        }

        // generate user's personal code
        private void generatePersonalCode_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            int _personalCode;
            bool exists;

            do
            {
                _personalCode = random.Next(100000, 999999);

                DB_Connect db = new DB_Connect();
                db.openConnection();
                MySqlCommand sc = new MySqlCommand("SELECT * FROM `users` WHERE `username` = @pc", db.getConnection());
                sc.Parameters.Add("@pc", MySqlDbType.Int32).Value = _personalCode;

                MySqlDataReader reader = sc.ExecuteReader();
                exists = reader.HasRows;
                reader.Close();
                db.closeConnection();

            } while (exists);

            personalCode.Text = _personalCode.ToString();
        }
        
        
        // PASSWORD VISIBILITY SETTINGS
        
        //show user's password
        private void showPassword_Click(object sender, RoutedEventArgs e)
        {
            userPasswordBox.Visibility = Visibility.Collapsed;
            userPassword.Visibility = Visibility.Visible;

            showPassword.Visibility = Visibility.Collapsed;
            hidePassword.Visibility = Visibility.Visible;       
        }  

        //hide user's password
        private void hidePassword_Click(object sender, RoutedEventArgs e)
        {          
            userPassword.Visibility = Visibility.Collapsed;
            userPasswordBox.Visibility = Visibility.Visible;

            hidePassword.Visibility = Visibility.Collapsed; 
            showPassword.Visibility = Visibility.Visible;
        }
        

        // DELETING USER

        //delete user
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxUsersPersonalCode.SelectedItem == null)
                return;

            string selectedUsername = comboBoxUsersPersonalCode.Text;

            DB_Connect db = new DB_Connect();

            using (db)
            {
                db.openConnection();
                MySqlCommand sc = new MySqlCommand("DELETE FROM `users` WHERE `username`=@username", db.getConnection());
                sc.Parameters.AddWithValue("@username", selectedUsername);

                if (sc.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Користувача успішно видалено!");

                    // Очищення полів
                    ClearFields();

                    // Оновлення ComboBox
                    loadUsersPersonalCodeList();

                    // Вибір першого користувача в ComboBox
                    if (comboBoxUsersPersonalCode.Items.Count > 0)
                    {
                        comboBoxUsersPersonalCode.SelectedIndex = 0;
                        LoadSelectedUserData();
                    }
                }
                else
                {
                    MessageBox.Show("Помилка при видаленні користувача!");
                }

                db.closeConnection();
            }
        }

        private void ClearFields()
        {
            userFullNameLabel.Content = string.Empty;
            userPositionLabel.Text = string.Empty;
            userLocationLabel.Text = string.Empty;

            userFirstNameLabel.Text = string.Empty;
            userLastNameLabel.Text = string.Empty;
            userEmailAdressLabel.Text = string.Empty;
            userPhoneNumberLabel.Text = string.Empty;
            userBDayLabel.Text = string.Empty;
            maskUserPhoneNumber.Text = string.Empty;
            maskUserBDay.Text = string.Empty;

            userCountryLabel.Text = string.Empty;
            userLocationSecLabel.Text = string.Empty;
            userPostCodeLabel.Text = string.Empty;

            personalCode.Text = string.Empty;
            userPassword.Text = string.Empty;
        }

        private void LoadSelectedUserData()
        {
            if (comboBoxUsersPersonalCode.SelectedItem == null)
                return;

            DataRowView row = (DataRowView)comboBoxUsersPersonalCode.SelectedItem;
            string selectedUsername = row["username"].ToString();

            DB_Connect db = new DB_Connect();

            using (db)
            {
                db.openConnection();
                MySqlCommand sc = new MySqlCommand("SELECT * FROM `users` WHERE `username`=@username", db.getConnection());
                sc.Parameters.AddWithValue("@username", selectedUsername);
                MySqlDataReader reader = sc.ExecuteReader();

                if (reader.Read())
                {
                    string _userName = reader["user_name"].ToString();
                    string _userSurName = reader["user_surname"].ToString();
                    int _personalCode = Convert.ToInt32(reader["username"].ToString());
                    string _userPassword = reader["user_password"].ToString();
                    string _userRole = reader["user_role"].ToString();
                    DateTime _userBirthday = Convert.ToDateTime(reader["user_birthday"].ToString());
                    string _userEmail = reader["user_email"].ToString();
                    string _userPhone = reader["user_phone"].ToString();
                    string _userCountry = reader["user_country"].ToString();
                    string _userCity = reader["user_city"].ToString();
                    string _userRegion = reader["user_region"].ToString();
                    int _userPostalCode = Convert.ToInt32(reader["user_postal_code"].ToString());

                    // 1 section
                    userFullNameLabel.Content = _userName + " " + _userSurName;
                    userPositionLabel.Text = _userRole;
                    userLocationLabel.Text = _userCity + ", " + _userRegion + ", " + _userCountry;

                    // 2 section
                    userFirstNameLabel.Text = _userName;
                    userLastNameLabel.Text = _userSurName;
                    userEmailAdressLabel.Text = _userEmail;
                    userPhoneNumberLabel.Text = _userPhone;
                    userBDayLabel.Text = _userBirthday.ToString("dd.MM.yyyy");
                    maskUserPhoneNumber.Text = _userPhone;
                    maskUserBDay.Text = _userBirthday.ToString();

                    // 3 section
                    userCountryLabel.Text = _userCountry;
                    userLocationSecLabel.Text = _userCity + ", " + _userRegion;
                    userPostCodeLabel.Text = _userPostalCode.ToString();

                    // 4 section
                    personalCode.Text = _personalCode.ToString();
                    userPassword.Text = _userPassword.ToString();
                }

                db.closeConnection();
            }
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

        //Validation for field "city/region"
        private void letterValidationWithDashTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-щьюяїієґА-ЩЬЮЯЇІЄҐ ,\\-]+");
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
    }
}
