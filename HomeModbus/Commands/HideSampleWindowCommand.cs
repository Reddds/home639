﻿using System.Windows;
using System.Windows.Input;

namespace HomeModbus.Commands
{
    /// <summary>
    /// Hides the main window.
    /// </summary>
    public class HideSampleWindowCommand : CommandBase<HideSampleWindowCommand>
    {
        public override void Execute(object parameter)
        {
            GetTaskbarWindow(parameter).Hide();
            CommandManager.InvalidateRequerySuggested();
        }


        public override bool CanExecute(object parameter)
        {
            var win = GetTaskbarWindow(parameter);
            return win != null && win.IsVisible;
        }
    }
}