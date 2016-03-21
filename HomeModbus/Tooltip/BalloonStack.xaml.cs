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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hardcodet.Wpf.TaskbarNotification;

namespace HomeModbus.Tooltip
{
    /// <summary>
    /// Interaction logic for BaloonStack.xaml
    /// </summary>
    public partial class BalloonStack : UserControl
    {
        public UIElementCollection Balloons => SpMain.Children;


        public BalloonStack()
        {
            InitializeComponent();

            SpMain.LayoutUpdated += SpMain_LayoutUpdated;
        }

        public void AddBaloon(FancyBalloon balloon)
        {
            SpMain.Children.Add(balloon);
            balloon.Closing += (sender, args) =>
            {
                SpMain.Children.Remove(balloon);
                if (SpMain.Children.Count == 0)
                {
                    var taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
                    taskbarIcon?.CloseBalloon();
                }
            };
 
        }

        private void SpMain_LayoutUpdated(object sender, EventArgs e)
        {
//            if (SpMain.Children.Count == 0)
//            {
//                var taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
//                taskbarIcon.CloseBalloon();
//            }
        }
    }
}
