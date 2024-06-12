using ConfectSoft.ViewModel.NavigationMenuChilds;
using FontAwesome.Sharp;
using SweetBakeryWpfApp.ViewModel;
using System;
using System.Windows.Input;

namespace ConfectSoft.ViewModel
{
    class Dashboard_Admin_ViewModel:ViewModelBase
    {
        private ViewModelBase _currentChildView;
        private string _caption;
        private IconChar _icon;

        public string UserName { get; set; }
        public string UserSurname { get; set; }

        //--> Commands
        public ICommand ShowAdminProfileViewCommand { get; }
        public ICommand ShowUsersDataViewCommand { get; }
        public ICommand ShowUsersHistoryActivityViewCommand { get; }
        
        public Dashboard_Admin_ViewModel()
        {
            ShowAdminProfileViewCommand = new ViewModelCommand(ExecuteShowAdminProfileViewCommand);
            ShowUsersDataViewCommand = new ViewModelCommand(ExecuteShowUsersDataViewCommand);
            ShowUsersHistoryActivityViewCommand = new ViewModelCommand(ExecuteShowUsersHistoryActivityViewCommand);

            ExecuteShowUsersHistoryActivityViewCommand(null);
        }

        private void ExecuteShowAdminProfileViewCommand(object obj)
        {
            CurrentChildView = new UserProfileViewModel();
            Caption = "Мій профіль";
            Icon = IconChar.UserAlt;
        }

        private void ExecuteShowUsersDataViewCommand(object obj)
        {            
            CurrentChildView = new UsersData_ViewModel();
            Caption = "Користувачі";
            Icon = IconChar.Users;
        }

        private void ExecuteShowUsersHistoryActivityViewCommand(object obj)
        {
            CurrentChildView = new UsersHistoryActivity_ViewModel();
            Caption = "Історія дій";
            Icon = IconChar.FileContract;
        }

        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            }
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }
        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }

        public IconChar Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }
    }
}
