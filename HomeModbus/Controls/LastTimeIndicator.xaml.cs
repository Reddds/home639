using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using HomeModbus.Annotations;

namespace HomeModbus.Controls
{
    /// <summary>
    /// Interaction logic for LastTimeIndicator.xaml
    /// </summary>
    public partial class LastTimeIndicator : UserControl, INotifyPropertyChanged
    {
        private DateTime _value;

        public DateTime Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public LastTimeIndicator()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
