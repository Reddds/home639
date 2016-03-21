using System.Windows.Input;

namespace HomeModbus.Commands
{
    /// <summary>
    /// Closes the current window.
    /// </summary>
    public class CloseWindowCommand : CommandBase<CloseWindowCommand>
    {
        public override void Execute(object parameter)
        {
            var win = GetTaskbarWindow(parameter);
            win.Tag = 1;
            win.Close();
            CommandManager.InvalidateRequerySuggested();
        }


        public override bool CanExecute(object parameter)
        {
            var win = GetTaskbarWindow(parameter);
            return win != null;
        }
    }
}