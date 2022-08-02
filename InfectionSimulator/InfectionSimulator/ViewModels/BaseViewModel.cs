using System.ComponentModel;

namespace InfectionSimulator.ViewModels
{
    internal class BaseViewModel : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events
    }
}