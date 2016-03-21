using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Hardcodet.Wpf.TaskbarNotification;
using NAudio.Wave;

namespace HomeModbus.Tooltip
{
    /// <summary>
    /// Interaction logic for FancyBalloon.xaml
    /// </summary>
    public partial class FancyBalloon : UserControl
    {
        private readonly BaloonStyles _style;
        private bool isClosing = false;

        private WaveOut _dynamics;

        public enum BaloonStyles
        {
            Normal,
            Info,
            Warning,
            Exclamation,
            Alarm,
            Error
        }

        #region BalloonText dependency property

        /// <summary>
        /// Description
        /// </summary>
        public static readonly DependencyProperty BalloonTextProperty =
            DependencyProperty.Register("BalloonText",
                typeof (string),
                typeof (FancyBalloon),
                new FrameworkPropertyMetadata(""));

        /// <summary>
        /// A property wrapper for the <see cref="BalloonTextProperty"/>
        /// dependency property:<br/>
        /// Description
        /// </summary>
        public string BalloonText
        {
            get { return (string) GetValue(BalloonTextProperty); }
            set { SetValue(BalloonTextProperty, value); }
        }

        #endregion

        public FancyBalloon(string text, BaloonStyles style = BaloonStyles.Normal)
        {
            _style = style;
            InitializeComponent();

/*
            var devices = string.Empty;
            for (int deviceId = 0; deviceId < WaveOut.DeviceCount; deviceId++)
            {
                var capabilities = WaveOut.GetCapabilities(deviceId);
                devices += capabilities.ProductName + Environment.NewLine;
//                comboBoxWaveOutDevice.Items.Add(capabilities.ProductName);
            }

            MessageBox.Show(devices);
*/

            //TaskbarIcon.AddBalloonClosingHandler(this, OnBalloonClosing);
            switch (style)
            {
                case BaloonStyles.Normal:
                    break;
                case BaloonStyles.Info:
                    break;
                case BaloonStyles.Warning:
                    break;
                case BaloonStyles.Exclamation:
                    Style = FindResource("ExclamationStyle") as Style;

                    break;
                case BaloonStyles.Alarm:
                    Style = FindResource("AlarmStyle") as Style;
                    break;
                case BaloonStyles.Error:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(style), style, null);
            }

            BalloonText = text;
        }


        /// <summary>
        /// By subscribing to the <see cref="TaskbarIcon.BalloonClosingEvent"/>
        /// and setting the "Handled" property to true, we suppress the popup
        /// from being closed in order to display the custom fade-out animation.
        /// </summary>
        private void OnBalloonClosing(object sender, RoutedEventArgs e)
        {
            e.Handled = true; //suppresses the popup from being closed immediately
            isClosing = true;
        }


        /// <summary>
        /// Resolves the <see cref="TaskbarIcon"/> that displayed
        /// the balloon and requests a close action.
        /// </summary>
        private void imgClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //the tray icon assigned this attached property to simplify access
            RaiseClosingEvent();
//            var taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
//            taskbarIcon.CloseBalloon();
        }

        public static readonly RoutedEvent ClosingEvent = EventManager.RegisterRoutedEvent(
    "Closing", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FancyBalloon));

        // Provide CLR accessors for the event
        public event RoutedEventHandler Closing
        {
            add { AddHandler(ClosingEvent, value); }
            remove { RemoveHandler(ClosingEvent, value); }
        }

        void RaiseClosingEvent()
        {
            var newEventArgs = new RoutedEventArgs(ClosingEvent);
            RaiseEvent(newEventArgs);
        }
        public void Close()
        {
            RaiseClosingEvent();
        }

        /// <summary>
        /// If the users hovers over the balloon, we don't close it.
        /// </summary>
        private void grid_MouseEnter(object sender, MouseEventArgs e)
        {
            //if we're already running the fade-out animation, do not interrupt anymore
            //(makes things too complicated for the sample)
            if (isClosing) return;

            //the tray icon assigned this attached property to simplify access
            TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
            taskbarIcon.ResetBalloonCloseTimer();
        }


        /// <summary>
        /// Closes the popup once the fade-out animation completed.
        /// The animation was triggered in XAML through the attached
        /// BalloonClosing event.
        /// </summary>
        private void OnFadeOutCompleted(object sender, EventArgs e)
        {
            Popup pp = (Popup) Parent;
            pp.IsOpen = false;
        }
    }
}