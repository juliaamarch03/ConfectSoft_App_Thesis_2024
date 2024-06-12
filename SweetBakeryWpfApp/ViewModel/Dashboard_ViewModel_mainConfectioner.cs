using System.Windows.Input;
using ConfectSoft.View;
using System;
using FontAwesome.Sharp;
using ConfectSoft.ViewModel.NavigationMenuChilds;

namespace SweetBakeryWpfApp.ViewModel
{
    public class Dashboard_ViewModel_mainConfectioner : ViewModelBase
    {
        private ViewModelBase _currentChildView;
        private string _caption;
        private IconChar _icon;

        public string UserName { get; set; }
        public string UserSurname { get; set; }


        //--> Commands
        public ICommand ShowDashboardDataViewCommand { get; } // Show "Dashboard" page
        public ICommand ShowEditDataViewCommand { get; }      // Show "Edit data" page
        public ICommand ShowSalaryDataViewCommand { get; }    // Show "Salary" page
        public ICommand ShowDailyReportViewCommand { get; }   // Show "Daily report" page
        public ICommand ShowUsedRawViewCommand { get; }       // Show "Used raw" page
        public ICommand ShowRecipesViewCommand { get; }       // Show "Recipes" page
        public ICommand ShowUserProfileViewCommand { get; }   // Show "User profile" page

        public Dashboard_ViewModel_mainConfectioner()
        {
            //Initialize commands
            ShowDashboardDataViewCommand = new ViewModelCommand(ExecuteShowDashboardDataViewCommand);
            ShowEditDataViewCommand = new ViewModelCommand(ExecuteShowEditViewCommand);
            ShowSalaryDataViewCommand = new ViewModelCommand(ExecuteShowSalaryDataViewCommand);
            ShowDailyReportViewCommand = new ViewModelCommand(ExecuteShowDailyReportViewCommand);
            ShowUsedRawViewCommand = new ViewModelCommand(ExecuteShowUsedRawViewCommand);
            ShowRecipesViewCommand = new ViewModelCommand(ExecuteShowRecipesViewCommand);
            ShowUserProfileViewCommand = new ViewModelCommand(ExecuteShowUserProfileViewCommand);
            
            //Default view
            ExecuteShowDashboardDataViewCommand(null);
        }

        // For setting "Current page" when the user is clicking the menu
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
        
        // Page title 
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
        
        // Icon
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

        // Method to execute command "Show Dashboard page"
        private void ExecuteShowDashboardDataViewCommand(object obj)
        {
            CurrentChildView = new DashboardDataViewModel();
            Caption = "Головна";
            Icon = IconChar.Microsoft;
        }
        private void ExecuteShowUserProfileViewCommand(object obj)
        {
            CurrentChildView = new UserProfileViewModel();
            Caption = "Мій профіль";
            Icon = IconChar.UserAlt;
        }
        private void ExecuteShowEditViewCommand(object obj)
        {
            CurrentChildView = new EditDataViewModel();
            Caption = "Редагувати дані";
            Icon = IconChar.PenClip;
        }
        private void ExecuteShowSalaryDataViewCommand(object obj)
        {
            CurrentChildView = new SalaryViewModel();
            Caption = "Зарплата";
            Icon = IconChar.MoneyBills;
        }
        private void ExecuteShowDailyReportViewCommand(object obj)
        {
            CurrentChildView = new DailyReportViewModel();
            Caption = "Денний звіт";
            Icon = IconChar.FilePen;
        }
        private void ExecuteShowRecipesViewCommand(object obj)
        {
            CurrentChildView = new RecipesViewModel();
            Caption = "Рецепти";
            Icon = IconChar.CakeCandles;
        }
        private void ExecuteShowUsedRawViewCommand(object obj)
        {
            CurrentChildView = new UsedRawViewModel();
            Caption = "Сировина";
            Icon = IconChar.CartFlatbed;
        }
    }
}
