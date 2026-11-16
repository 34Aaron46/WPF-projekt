using System.Windows.Input;

namespace TuningKatalogusWpf.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action muvelet;

        public RelayCommand(Action muvelet)
        {
            this.muvelet = muvelet;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            muvelet();
        }
    }
}