using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SweetBakeryWpfApp.View;
using SweetBakeryWpfApp.ViewModel.DailyReportChilds;
using System.Windows.Controls;
using System.Windows;

namespace SweetBakeryWpfApp.ViewModel
{
    public class DailyReportViewModel:ViewModelBase
    {
        private ViewModelBase _currentChildView;
        public ICommand ShowBakedCakesViewCommand { get; }
        public ICommand ShowUsedRawViewCommand { get; }
        public ICommand ShowSentCakesViewCommand { get; }

        public DailyReportViewModel()
        {
            //Initialize commands
            ShowBakedCakesViewCommand = new ViewModelCommand(ExecuteShowBakedCakesView_DR_Command);
            ShowUsedRawViewCommand = new ViewModelCommand(ExecuteShowUsedRawView_DR_Command);
            ShowSentCakesViewCommand = new ViewModelCommand(ExecuteShowSentCakesView_DR_Command);

            //Default view
            ExecuteShowBakedCakesView_DR_Command(null);

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
        private void ExecuteShowBakedCakesView_DR_Command(object obj)
        {
            CurrentChildView = new BakedCakesModelView_DR();
        }

        private void ExecuteShowUsedRawView_DR_Command(object obj)
        {
            CurrentChildView = new UsedRawViewModel();
        }
        private void ExecuteShowSentCakesView_DR_Command(object obj)
        {
            CurrentChildView = new SentCakesModelView_DR();
        }


    }
}
