using SweetBakeryWpfApp.View;
using System.Windows;

namespace ConfectSoft.View
{
    public partial class SwitchUserRequestWindowView : Window
    {
        private Dashboard_mainConfectioner _mainConfect;
        private Dashboard_Admin _admin;

        public SwitchUserRequestWindowView(Dashboard_mainConfectioner mainConfect)
        {
            InitializeComponent();
            _mainConfect = mainConfect;
            swithUserBtnConfect.Visibility = Visibility.Visible;
            swithUserBtnAdmin.Visibility = Visibility.Collapsed;
            switchUserModeGrid.Visibility = Visibility.Visible;
        }

        public SwitchUserRequestWindowView()
        {
            InitializeComponent();
            closeAppModeGrid.Visibility = Visibility.Visible;
            closeAppBtn.Visibility = Visibility.Visible;
        }

        public SwitchUserRequestWindowView(Dashboard_Admin admin)
        {
            InitializeComponent();
            _admin = admin;
            swithUserBtnAdmin.Visibility = Visibility.Visible;
            swithUserBtnConfect.Visibility = Visibility.Collapsed;
            switchUserModeGrid.Visibility = Visibility.Visible;
        }

        private void cancelSwitchUser_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void swithUserBtnConfect_Click(object sender, RoutedEventArgs e)
        {
            _mainConfect.SwitchToLoginView();
            this.Close();
        }

        private void swithUserBtnAdmin_Click(object sender, RoutedEventArgs e)
        {
            _admin.SwitchToLoginView();
            this.Close();
        }

        private void cancelClosingApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void closeAppBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
