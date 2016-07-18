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
    /// Interaction logic for BarometerIndicator.xaml
    /// </summary>
    public partial class BarometerIndicator : UserControl, INotifyPropertyChanged
    {
        private double _value;

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                ChangeWeatherPicture();
                OnPropertyChanged(nameof(Value));
            }
        }

        private double _temperature;

        public double Temperature
        {
            get { return _temperature; }
            set
            {
                _temperature = value;
                ChangeWeatherPicture();
                OnPropertyChanged(nameof(Temperature));
            }
        }

        private double _hymidity;

        public double Hymidity
        {
            get { return _hymidity; }
            set
            {
                _hymidity = value;
                ChangeWeatherPicture();
                OnPropertyChanged(nameof(Hymidity));
            }
        }

        void ChangeWeatherPicture()
        {
            foreach (var key in _weathers.Keys)
            {
                if (key.InRange(Value, Hymidity, Temperature))
                {
                    WeatherIndicator.Source = MainWindow.GetImageSource(System.IO.Path.Combine("Weather", _weathers[key]));
                }
            }
        }

        /// <summary>
        /// Нижнее значение включается в диапазон, верхнее нет
        /// </summary>
        class WeatherRange
        {
            public double MinPressure { get; set; }
            public double MaxPressure { get; set; }
            public double MinHymidity { get; set; }
            public double MaxHymidity { get; set; }
            public double MinTemperature { get; set; }
            public double MaxTemperature { get; set; }

            public bool InRange(double pressure, double hymidity, double temp)
            {
                return pressure >= MinPressure && pressure < MaxPressure
                       && (MinHymidity == MaxHymidity || (hymidity >= MinHymidity && hymidity < MaxHymidity))
                       && (MinTemperature == MaxTemperature || (temp >= MinTemperature && temp < MaxTemperature));
            }
        }

        private readonly Dictionary<WeatherRange, string> _weathers; 

        public BarometerIndicator()
        {
            InitializeComponent();

            _weathers = new Dictionary<WeatherRange, string>
            {
                {new WeatherRange {MinPressure = 700, MaxPressure = 710, MinTemperature = -40, MaxTemperature = 0},  "cloud_snow.png"},
                {new WeatherRange {MinPressure = 700, MaxPressure = 710, MinTemperature = 0, MaxTemperature = 40},  "rain.png"},

                {new WeatherRange {MinPressure = 710, MaxPressure = 730, MinTemperature = -40, MaxTemperature = 0},  "cloud_snow.png"},
                {new WeatherRange {MinPressure = 710, MaxPressure = 730, MinTemperature = 0, MaxTemperature = 40},  "rain.png"},

                {new WeatherRange {MinPressure = 730, MaxPressure = 750, MinHymidity = 0, MaxHymidity = 80},  "sun_cloudy.png"},
                {new WeatherRange {MinPressure = 730, MaxPressure = 750, MinHymidity = 80, MaxHymidity = 100, MinTemperature = -40, MaxTemperature = 0},  "cloud_snow.png"},
                {new WeatherRange {MinPressure = 730, MaxPressure = 750, MinHymidity = 80, MaxHymidity = 100, MinTemperature = 0, MaxTemperature = 40},  "rain.png"},

                {new WeatherRange {MinPressure = 750, MaxPressure = 765},  "sunny.png"},
                {new WeatherRange {MinPressure = 765, MaxPressure = 785},  "sunny.png"},
                {new WeatherRange {MinPressure = 785, MaxPressure = 800},  "sun.png"},
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
