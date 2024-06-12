using ConfectSoft.View;
using System;
using System.ComponentModel;
using ConfectSoft.ViewModel.NavigationMenuChilds;

namespace SweetBakeryWpfApp.ViewModel
{
        public abstract class ViewModelBase : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
}
