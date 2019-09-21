using Auriga.Common;
using Auriga.Views;
using System.ComponentModel;
using System.Windows.Input;

namespace Auriga.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ICentralView _currentCentralView;
        public ICentralView CurrentCentralView
        {
            get => _currentCentralView;
            set
            {
                if (_currentCentralView != null)
                {
                    _currentCentralView = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(CurrentCentralView)));
                }
                else
                {
                    _currentCentralView = value;
                }
            }
        }

        public MainViewModel()
        {
            CurrentCentralView = new DefaultView();
        }

        public ICommand ShowGraphViewer => new RelayCommand(p => CurrentCentralView = new GraphViewer());
        public ICommand ShowGraphEditor => new RelayCommand(p => CurrentCentralView = new GraphEditor());

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
