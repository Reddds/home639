using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chip45Programmer.Windows
{
    /// <summary>
    /// Interaction logic for ByteFromBitsWindow.xaml
    /// </summary>
    public partial class ByteFromBitsWindow
    {
        class ByteFromBits
        {
            public bool On { get; set; }
        }

        readonly ByteFromBits[] _oneByte;

        public ByteFromBitsWindow()
        {
            InitializeComponent();

            _oneByte = new ByteFromBits[8];
            for (var i = 0; i < _oneByte.Length; i++)
            {
                _oneByte[i] = new ByteFromBits();
            }
            DgByte.ItemsSource = _oneByte;
        }
        /// <summary>
        /// Нумерация с конца
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgByte_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var dg = (DataGrid) sender;
            var b = (ByteFromBits[]) dg.ItemsSource;

            e.Row.Header = (b.Length - e.Row.GetIndex() - 1).ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public byte GetResultByte()
        {
            byte b = 0;
            for (var i = 0; i < _oneByte.Length; i++)
            {
                b |= (byte)((_oneByte[i].On ? 1 : 0) << (_oneByte.Length - i - 1));
            }
            return b;
        }

        public int? Address => IudAddress.Value;
    }
}
