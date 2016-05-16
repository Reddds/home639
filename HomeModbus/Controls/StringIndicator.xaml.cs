using System.ComponentModel;
using System.Runtime.CompilerServices;
using HomeModbus.Annotations;

namespace HomeModbus.Controls
{
    /// <summary>
    /// Interaction logic for StringIndicator.xaml
    /// </summary>
    public partial class StringIndicator : INotifyPropertyChanged
    {
        private string _value;

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public StringIndicator()
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
