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
    public enum UpdateModes { None, Updating, Updated, Fail}
    /// <summary>
    /// Interaction logic for SimpleSetter.xaml
    /// </summary>
    public partial class SimpleSetter : INotifyPropertyChanged
    {
        
        public UpdateModes Mode { get; set; }

        public SimpleSetter()
        {
            InitializeComponent();
        }

        private void BMain_Click(object sender, RoutedEventArgs e)
        {
            Mode = UpdateModes.Updating;
            OnPropertyChanged(nameof(Mode));
        }

        /// <summary>
        /// Получен результат установки
        /// </summary>
        /// <param name="status">true - успешно, false - неудачно</param>
        public void Result(bool status)
        {
            Mode = status ? UpdateModes.Updated : UpdateModes.Fail;
            OnPropertyChanged(nameof(Mode));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
