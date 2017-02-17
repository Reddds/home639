using System;
using System.Windows;
using System.Windows.Threading;

namespace HomeModbus.Controls
{
    /// <summary>
    /// Interaction logic for RoomTab.xaml
    /// </summary>
    public partial class RoomTab
    {
        public RoomTab()
        {
            InitializeComponent();
        }

        public string ControllerId { get; set; }

        private void ExecDispatched(Action action)
        {
            Application.Current.Dispatcher.BeginInvoke(
              DispatcherPriority.Background,
              action);
        }
        public bool? State
        {
            set
            {
                ExecDispatched(() =>
                {
                    switch (value)
                    {
                        case true:
                            SiControllerState.StateIndex = 1;
                            break;
                        case false:
                            SiControllerState.StateIndex = 3;
                            break;
                        default:
                            SiControllerState.StateIndex = 0;
                            break;
                    }
                });
            }
        }

        public string Title
        {
            set { TbTitle.Content = value; }
        }
    }
}
