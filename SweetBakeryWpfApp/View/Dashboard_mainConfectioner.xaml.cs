using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SweetBakeryWpfApp.ViewModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using ConfectSoft.View;

namespace SweetBakeryWpfApp.View
{
    public partial class Dashboard_mainConfectioner : Window
    {
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserInfo { get; set; }

        public Dashboard_mainConfectioner()
        {
            InitializeComponent();
            bindUserProfilePic(userProfPicEllipse, LoginViewModel.UserInfo);     
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

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            SwitchUserRequestWindowView closeWindow = new SwitchUserRequestWindowView();
            closeWindow.Show();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if(this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        public void bindUserProfilePic(Ellipse userEllipse, string userName)
        {
            // Перевірка імені користувача
            switch (userName)
            {
                case "Таня Шевченко":
                    userEllipse.Fill = new ImageBrush(new BitmapImage(new Uri("D:/OTHER/University/4 курс/Дипломна робота/Практична реалізація/WPF APP/SweetBakeryWpfApp/Images/UsersPics/122032.jpg")));
                    break;
                case "Аня Трачук":
                    userEllipse.Fill = new ImageBrush(new BitmapImage(new Uri("D:/OTHER/University/4 курс/Дипломна робота/Практична реалізація/WPF APP/SweetBakeryWpfApp/Images/UsersPics/120120.jpg")));
                    break;
                case "Давид Лукінчук":
                    userEllipse.Fill = new ImageBrush(new BitmapImage(new Uri("D:/OTHER/University/4 курс/Дипломна робота/Практична реалізація/WPF APP/SweetBakeryWpfApp/Images/UsersPics/100101.jpg")));
                    break;
                // додайте більше випадків за потреби
                default:
                    userEllipse.Fill = new ImageBrush(new BitmapImage(new Uri("D:/OTHER/University/4 курс/Дипломна робота/Практична реалізація/WPF APP/SweetBakeryWpfApp/Images/UsersPics/000.png")));
                    break;
            }
        }

        private void btnSwitchUser_Click(object sender, RoutedEventArgs e)
        {
            OpenModalWindow();
        }

        public void SwitchToLoginView()
        {
            LoginView loginView = new LoginView();
            loginView.Show();
            this.Close();
        }

        private void OpenModalWindow()
        {
            SwitchUserRequestWindowView modalWindow = new SwitchUserRequestWindowView(this);
            modalWindow.ShowDialog();
        }

    }
}
