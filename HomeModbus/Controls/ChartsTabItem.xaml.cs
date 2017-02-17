using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DevExpress.Xpf.Charts;
using HomeModbus.Models;
using HomeModbus.Models.Base;

namespace HomeModbus.Controls
{
    /// <summary>
    /// Interaction logic for ChartsTabItem.xaml
    /// </summary>
    public partial class ChartsTabItem
    {
        public ChartsTabItem()
        {
            InitializeComponent();
        }

        public class DatePoint
        {
            public DateTime Argument { get; private set; }
            public double Value { get; private set; }

            public DatePoint(DateTime argument, double value)
            {
                this.Argument = argument;
                this.Value = value;
            }
        }

        private void ShowCharts(DateTime starTime, DateTime endTime)
        {
            if (HomeClientSettings.Instance == null || HomeClientSettings.Instance.Chart == null
                || HomeClientSettings.Instance.Chart.Groups == null
                || HomeClientSettings.Instance.Chart.Groups.Length == 0)
            {
                MessageBox.Show("Настройки для графиков не заданы!", "Ошибка построения графиков", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {

           

            using (var de = new homeserverEntities())
            {
                var dat = (from di in de.DataLog
                           where di.Time >= starTime && di.Time < endTime
                           group di by di.ParameterId into g
                           select g).ToArray();

                //group di by di.ParameterId into g
                //                           select g


                //GcTest.ItemsSource = dat;
                var diag = (XYDiagram2D)MainChart.Diagram;

                diag.Series.Clear();
                diag.SecondaryAxesY.Clear();
                diag.Panes.Clear();

                var firstPaneUsed = false;
                foreach (var chartGroup in HomeClientSettings.Instance.Chart.Groups)
                {
                    Pane pane;
                    SecondaryAxisY2D secY = null;
                    if (firstPaneUsed)
                    {
                        pane = new Pane();
                        diag.Panes.Add(pane);
                        secY = new SecondaryAxisY2D();
                        diag.SecondaryAxesY.Add(secY);
//                        secY.VisualRange = new Range();
//                        secY.VisualRange.SetAuto();
                        secY.WholeRange = new Range();
                        AxisY2D.SetAlwaysShowZeroLevel(secY.WholeRange, false);
                        //secY.WholeRange.SetAuto();
                        secY.Title = new AxisTitle { Content = chartGroup.YAxisTitle };
                        secY.Alignment = AxisAlignment.Near;
                    }
                    else
                    {
                        firstPaneUsed = true;
                        pane = diag.DefaultPane;
                        diag.AxisY.Title = new AxisTitle { Content = chartGroup.YAxisTitle };
                        //diag.AxisY.WholeRange = new Range();
                    }


                    var steckOffset = 0;
                    var minY = double.MaxValue;
                    foreach (var parameter in chartGroup.Parameters)
                    {
                        var dataLog = (from d in dat
                                       where d.Key == parameter.Id
                                       select d).FirstOrDefault();
                        if (dataLog == null)
                            continue;



                        if (chartGroup.DataType == HomeClientSettings.ChartClass.ChartGroup.DataTypes.Bool)
                        {
                            var serie = new LineStepSeries2D(); // new LineStepSeries2D LineSeries2D();
                            if (!ReferenceEquals(pane, diag.DefaultPane))
                                XYDiagram2D.SetSeriesPane(serie, pane);
                            if (secY != null)
                                XYDiagram2D.SetSeriesAxisY(serie, secY);

                            serie.ArgumentDataMember = "Argument";
                            serie.ValueDataMember = "Value";
                            serie.DisplayName = $"{chartGroup.Title} ({parameter.Legend})";
                            var points = new List<DatePoint>();
                            foreach (var log in dataLog)
                            {
                                var val = steckOffset + (log.BoolValue != null && log.BoolValue.Value ? 1 : 0);
                                points.Add(new DatePoint(log.Time, val));
                                //                                var point = new SeriesPoint(log.Time, val);
                                //                                serie.Points.Add(point);
                            }
                            serie.DataSource = points;
                            diag.Series.Add(serie);
                            minY = 0;
                        }
                        else
                        {
                            var serie = new LineSeries2D();

                            XYDiagram2D.SetSeriesPane(serie, pane);
                            XYDiagram2D.SetSeriesAxisY(serie, secY);

                            serie.DisplayName = $"{chartGroup.Title} ({parameter.Legend})";
                            serie.ArgumentDataMember = "Argument";
                            serie.ValueDataMember = "Value";
                            var points = new List<DatePoint>();

                            foreach (var log in dataLog)
                            {
                                var val = 0d;
                                if (log.IntValue != null)
                                    val = log.IntValue.Value;
                                else if (log.DoubleValue != null)
                                    val = log.DoubleValue.Value;
                                if (val < minY)
                                    minY = val;
                                points.Add(new DatePoint(log.Time, val));
                            }
                            serie.DataSource = points;
                            diag.Series.Add(serie);

                        }
//                        if (secY != null)
//                            secY.WholeRange.MinValue = minY;
//                        else
//                            diag.AxisY.WholeRange.MinValue = minY;
                        steckOffset++;
                    }

                }

                diag.AxisX.VisibilityInPanes.Clear();
                if (diag.Panes.Count > 0)
                {
                    diag.DefaultPane.AxisXScrollBarOptions = new ScrollBarOptions {Visible = false};
                    diag.AxisX.VisibilityInPanes.Add(new VisibilityInPane
                    {
                        Pane = diag.DefaultPane,
                        Visible = false
                    });
                    for (var i = 0; i < diag.Panes.Count - 1; i++)
                    {
                        diag.Panes[i].AxisXScrollBarOptions = new ScrollBarOptions { Visible = false };
                        diag.AxisX.VisibilityInPanes.Add(new VisibilityInPane
                        {
                            Pane = diag.Panes[i],
                            Visible = false
                        });
                    }
                    diag.Panes[diag.Panes.Count - 1].AxisXScrollBarOptions = new ScrollBarOptions { Visible = true };
                }
                else
                {
                    diag.DefaultPane.AxisXScrollBarOptions = new ScrollBarOptions { Visible = true };
                }
            }
            }
            catch (Exception ee)
            {

                MessageBox.Show(ee.ToString(), "Ошибка получения данных", MessageBoxButton.OK, MessageBoxImage.Error);;
            }

        }

        private void ShowLastDay(object sender, RoutedEventArgs e)
        {
            var startTime = DateTime.Today;
            ShowCharts(startTime, startTime.AddDays(1));
        }

        private void ShowLastWeek(object sender, RoutedEventArgs e)
        {
            var endTime = DateTime.Today.AddDays(1);
            ShowCharts(endTime.AddDays(-7), endTime);
        }
    }
}
