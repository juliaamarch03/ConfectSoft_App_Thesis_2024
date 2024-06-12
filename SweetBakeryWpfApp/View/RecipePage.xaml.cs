using MySql.Data.MySqlClient;
using SweetBakeryWpfApp.Repositories;
using SweetBakeryWpfApp.View;
using SweetBakeryWpfApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static SweetBakeryWpfApp.View.RecipesView;

namespace ConfectSoft.View
{  
    public partial class RecipePage : Page
    {
        public Recipe Recipe { get; set; }
        public RecipePage(RecipesView.Recipe recipe)
        {
            InitializeComponent();

            this.DataContext = this;
            Recipe = recipe;
            recipeTitle.Text = recipe.Name;
            txtIngredients.Text = recipe.Ingredients;
            imageCake.ImageSource = new BitmapImage(new Uri(recipe.Image)); ;
            prepTimeTxt.Text = recipe.PrepTime;
            cookTimeTxt.Text = recipe.CookTime;
            genTimeTxt.Text = recipe.TotalTime;
            txtCreamInfo.Text = recipe.Cream;
            headerRecipeImg.ImageSource = new BitmapImage(new Uri(recipe.HeaderIMG));
            txtHowToCook.Text = recipe.Cooking;

            bindStatusActivity("Відкрито сторінку 'Рецепти'");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           var recipesView = new RecipesView();
           this.Content = recipesView;
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
