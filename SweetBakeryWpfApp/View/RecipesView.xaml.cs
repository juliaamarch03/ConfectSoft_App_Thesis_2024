using ConfectSoft.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SweetBakeryWpfApp.View
{
    public partial class RecipesView : UserControl
    {
        public class Recipe
        {
            public string Name { get; set; }
            public string Image { get; set; }
            public string PrepTime { get; set; }
            public string CookTime { get; set; }
            public string TotalTime { get; set; }
            public string Ingredients { get; set; }
            public string Cooking { get; set; }
            public string Cream { get; set; }
            public string HeaderIMG { get; set; }
        }

        public ObservableCollection<Recipe> RecipeCollection { get; set; }

        public RecipesView()
        {
            InitializeComponent();

            RecipeCollection = new ObservableCollection<Recipe>
            {
                new Recipe { Name = "Бісквіт з маскарпоне",
                             Image = "D:/OTHER/University/4 курс/Дипломна робота/Практична реалізація/WPF APP/SweetBakeryWpfApp/Images/Recipes/cake_1.jpg",
                             PrepTime = "30 хв",
                             CookTime = "40 хв",
                             TotalTime = "70 хв",
                             Ingredients = $"• 4 яйця\n\n" +
                                           $"• 120 г цукру\n\n" +
                                           $"• 120 г борошна\n\n" +
                                           $"• 1/2 чайної ложки розпушувача\n\n" +
                                           $"• 250 г маскарпоне\n\n" +
                                           $"• 100 г вершків\n\n" +
                                           $"• 50 г цукрової пудри",
                             Cooking = $"1. Збийте яйця з цукром до пишної піни.\n\n" +
                                       $"2. Додайте борошно з розпушувачем, обережно перемішайте.\n\n" +
                                       $"3. Вилийте тісто у форму для випічки.\n\n" +
                                       $"4. Випікайте при 180°C 40 хвилин.\n\n" +
                                       $"5. Збийте маскарпоне з вершками та цукровою пудрою.\n\n" +
                                       $"6. Охолоджений бісквіт розріжте на 2 коржі.\n\n" +
                                       $"7. Змастіть коржі кремом, з'єднайте.\n\n" +
                                       $"8. Прикрасьте торт за бажанням.",
                             Cream = "• Крем маскарпоне",
                             HeaderIMG = "D:/OTHER/University/4 курс/Дипломна робота/Практична реалізація/WPF APP/SweetBakeryWpfApp/Images/Recipes/cake1_header.png"},
                new Recipe { Name = "Торт Медовик", 
                             Image = "D:/OTHER/University/4 курс/Дипломна робота/Практична реалізація/WPF APP/SweetBakeryWpfApp/Images/Recipes/cake_2.jpg",
                             PrepTime = "45 хв",
                             CookTime = "1 год",
                             TotalTime = "110 хв",
                             Ingredients = $"• 200 г борошна\n\n" +
                                           $"• 100 г вершкового масла\n\n" +
                                           $"• 100 г цукру\n\n" +
                                           $"• 2 яйця\n\n" +
                                           $"• 1 столова ложка меду\n\n" +
                                           $"• 1/2 чайної ложки соди",
                             Cooking = $"1. Змішайте борошно з маслом, цукром, яйцями, медом та содою.\n\n" +
                                       $"2. Замісіть тісто.\n\n" +
                                       $"3. Розділіть тісто на 8 частин.\n\n" +
                                       $"4. Випікайте коржі при 180°C по 10 хвилин.\n\n" +
                                       $"5. Збийте сметану з цукром до густоти.\n\n" +
                                       $"6. Змастіть коржі кремом, з'єднайте.\n\n" +
                                       $"7. Прикрасьте торт за бажанням.\n\n",
                             Cream = "• Сметанний крем",
                             HeaderIMG = "D:/OTHER/University/4 курс/Дипломна робота/Практична реалізація/WPF APP/SweetBakeryWpfApp/Images/Recipes/cake2_header.png"},
                new Recipe { Name = "Торт Снікерс", 
                             Image = "D:/OTHER/University/4 курс/Дипломна робота/Практична реалізація/WPF APP/SweetBakeryWpfApp/Images/Recipes/cake_3.jpg",
                             PrepTime = "30 хв",
                             CookTime = "50 хв",
                             TotalTime = "80 хв",
                             Ingredients = $"• 200 г шоколадного печива\n\n" +
                                           $"• 100 г вершкового масла\n\n" +
                                           $"• 1 банка вареного згущеного молока\n\n" +
                                           $"• 200 г арахісу\n\n" +
                                           $"• 100 г шоколаду",
                             Cooking = $"1. Подрібніть печиво в крихту.\n\n" +
                                       $"2. Змішайте крихту з розтопленим маслом.\n\n" +
                                       $"3. Викладіть масу у форму для випічки, утрамбуйте.\n\n" +
                                       $"4. Змастіть корж вареним згущеним молоком.\n\n" +
                                       $"5. Викладіть на нього арахіс.Розтопіть шоколад, вилийте на торт.\n\n" +
                                       $"6. Поставте торт у холодильник на 2 години.\n\n",
                             Cream = "• Використовується варене згущене молоко",
                             HeaderIMG = "D:/OTHER/University/4 курс/Дипломна робота/Практична реалізація/WPF APP/SweetBakeryWpfApp/Images/Recipes/cake3_header.png"},
                new Recipe { Name = "Кавовий брауні з маскарпоне", 
                             Image = "D:/OTHER/University/4 курс/Дипломна робота/Практична реалізація/WPF APP/SweetBakeryWpfApp/Images/Recipes/cake_4.jpg",
                             PrepTime = "20 хв",
                             CookTime = "30 хв",
                             TotalTime = "50 хв",
                             Ingredients = $"• 100 г вершкового масла\n\n" +
                                           $"• 100 г темного шоколаду\n\n" +
                                           $"• 2 яйця\n\n" +
                                           $"• 100 г цукру\n\n" +
                                           $"• 100 г борошна\n\n" +
                                           $"• 1 столова ложка кави\n\n" +
                                           $"• 250 г маскарпоне\n\n" +
                                           $"• 50 г цукрової пудри",
                             Cooking = $"1. Розтопіть масло з шоколадом.\n\n" +
                                       $"2. Збийте яйця з цукром до пишної піни.\n\n" +
                                       $"3. Додайте до яєць шоколадну...",
                             Cream = "• Крем з маскарпоне",
                             HeaderIMG = "D:/OTHER/University/4 курс/Дипломна робота/Практична реалізація/WPF APP/SweetBakeryWpfApp/Images/Recipes/cake4_header.png"},
                new Recipe { Name = "Шоколадно-малиновий торт", 
                             Image = "D:/OTHER/University/4 курс/Дипломна робота/Практична реалізація/WPF APP/SweetBakeryWpfApp/Images/Recipes/cake_6.jpg",
                             PrepTime = "30 хв",
                             CookTime = "40 хв",
                             TotalTime = "70 хв",
                             Ingredients = $"• 200 г темного шоколаду\n\n" +
                                           $"• 150 г вершкового масла\n\n " +
                                           $"• 4 яйця\n\n" +
                                           $"• 100 г цукру\n\n" +
                                           $"• 100 г борошна\n\n" +
                                           $"• 1/2 чайної ложки розпушувача\n\n" +
                                           $"• 250 г малини\n\n" +
                                           $"• 200 г вершків\n\n" +
                                           $"• 50 г цукрової пудри",
                             Cooking = $"1. Розтопіть масло з шоколадом.\n\n" +
                                       $"2. Збийте яйця з цукром до пишної піни.\n\n" +
                                       $"3. Додайте до яєць шоколадну...",
                             Cream = "• Вершковий крем з малиною",
                             HeaderIMG = "D:/OTHER/University/4 курс/Дипломна робота/Практична реалізація/WPF APP/SweetBakeryWpfApp/Images/Recipes/cake6_header.png"},
                new Recipe { Name = "Морквяний торт з кремом", 
                             Image = "D:/OTHER/University/4 курс/Дипломна робота/Практична реалізація/WPF APP/SweetBakeryWpfApp/Images/Recipes/cake_7.jpg",
                             PrepTime = "40 хв",
                             CookTime = "45 хв",
                             TotalTime = "95 хв",
                             Ingredients = $"• 200 г моркви\n\n" +
                                           $"• 100 г вершкового масла\n\n" +
                                           $"• 100 г цукру\n\n" +
                                           $"• 2 яйця\n\n" +
                                           $"• 100 г борошна\n\n" +
                                           $"• 1/2 чайної ложки розпушувача\n\n" +
                                           $"• 1/2 чайної ложки кориці\n\n" +
                                           $"• 1/4 чайної ложки мускатного горіха\n\n" +
                                           $"• 250 г вершкового сиру\n\n" +
                                           $"• 100 г цукрової пудри",
                             Cooking = $"1. Розтопіть масло з шоколадом.\n\n" +
                                       $"2. Збийте яйця з цукром до пишної піни.\n\n" +
                                       $"3. Додайте до яєць шоколадну...",
                             Cream = "• Крем з вершкового сиру",
                             HeaderIMG = "D:/OTHER/University/4 курс/Дипломна робота/Практична реалізація/WPF APP/SweetBakeryWpfApp/Images/Recipes/cake7_header.png"}
            };
            this.DataContext = this;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Отримуємо кнопку, на яку було натиснуто
            var button = (Button)sender;

            // Отримуємо рецепт, пов'язаний з цією кнопкою
            var recipe = (Recipe)button.DataContext;

            // Створюємо нову сторінку рецепту і передаємо їй рецепт
            var recipePage = new RecipePage(recipe);
            recipePage.DataContext = recipe;

            // Створюємо новий Frame
            var frame = new Frame();

            // Вставляємо сторінку рецепту в Frame
            frame.Content = recipePage;

            // Змінюємо вміст UserControl на новий Frame
            this.Content = frame;
        }
    }
}
