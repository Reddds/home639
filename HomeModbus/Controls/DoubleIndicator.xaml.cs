using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HomeModbus.Annotations;

namespace HomeModbus.Controls
{
    /// <summary>
    /// Interaction logic for DoubleIndicator.xaml
    /// </summary>
    public partial class DoubleIndicator : INotifyPropertyChanged
    {
        private string _hiValue;
        private string _loValue;

        public string HiValue
        {
            get { return _hiValue; }
            set
            {
                _hiValue = value;
                OnPropertyChanged(nameof(HiValue));
            }
        }

        public string LoValue
        {
            get { return _loValue; }
            set
            {
                _loValue = value;
                OnPropertyChanged(nameof(LoValue));
            }
        }

        public DoubleIndicator()
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
