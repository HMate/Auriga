using Auriga.Common;
using Auriga.Views;
using System.ComponentModel;
using System.Windows.Input;

namespace Auriga.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ICentralView? _currentCentralView;
        public ICentralView? CurrentCentralView
        {
            get => _currentCentralView;
            set
            {
                _currentCentralView = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentCentralView)));
            }
        }

        public MainViewModel()
        {
            CurrentCentralView = new DefaultView();
        }

        public ICommand ShowGraphEditor => new RelayCommand(p => CurrentCentralView = new GraphEditor());

        public ICommand ShowDotEditor => new RelayCommand(p => CurrentCentralView = new DotFileEditorView());

        public ICommand ShowBrainfuckVisualizer => new RelayCommand(p => CurrentCentralView = new BrainfuckVisualizer());

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
