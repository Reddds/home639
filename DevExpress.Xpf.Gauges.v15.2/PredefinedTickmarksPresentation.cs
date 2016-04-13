// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.PredefinedTickmarksPresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public abstract class PredefinedTickmarksPresentation : TickmarksPresentation
  {
    public static readonly DependencyProperty MajorTickBrushProperty = DependencyPropertyManager.Register("MajorTickBrush", typeof (Brush), typeof (PredefinedTickmarksPresentation), new PropertyMetadata((object) null, new PropertyChangedCallback(PredefinedTickmarksPresentation.MajorTickBrushPropertyChanged)));
    public static readonly DependencyProperty MinorTickBrushProperty = DependencyPropertyManager.Register("MinorTickBrush", typeof (Brush), typeof (PredefinedTickmarksPresentation), new PropertyMetadata((object) null, new PropertyChangedCallback(PredefinedTickmarksPresentation.MinorTickBrushPropertyChanged)));

    [Category("Presentation")]
    public Brush MajorTickBrush
    {
      get
      {
        return (Brush) this.GetValue(PredefinedTickmarksPresentation.MajorTickBrushProperty);
      }
      set
      {
        this.SetValue(PredefinedTickmarksPresentation.MajorTickBrushProperty, (object) value);
      }
    }

    [Category("Presentation")]
    public Brush MinorTickBrush
    {
      get
      {
        return (Brush) this.GetValue(PredefinedTickmarksPresentation.MinorTickBrushProperty);
      }
      set
      {
        this.SetValue(PredefinedTickmarksPresentation.MinorTickBrushProperty, (object) value);
      }
    }

    protected abstract Brush DefaultMajorTickBrush { get; }

    protected abstract Brush DefaultMinorTickBrush { get; }

    [Category("Presentation")]
    public Brush ActualMajorTickBrush
    {
      get
      {
        if (this.MajorTickBrush == null)
          return this.DefaultMajorTickBrush;
        return this.MajorTickBrush;
      }
    }

    [Category("Presentation")]
    public Brush ActualMinorTickBrush
    {
      get
      {
        if (this.MinorTickBrush == null)
          return this.DefaultMinorTickBrush;
        return this.MinorTickBrush;
      }
    }

    private static void MajorTickBrushPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      PredefinedTickmarksPresentation tickmarksPresentation = d as PredefinedTickmarksPresentation;
      if (tickmarksPresentation == null)
        return;
      tickmarksPresentation.ActualMajorTickBrushChanged();
    }

    private static void MinorTickBrushPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      PredefinedTickmarksPresentation tickmarksPresentation = d as PredefinedTickmarksPresentation;
      if (tickmarksPresentation == null)
        return;
      tickmarksPresentation.ActualMinorTickBrushChanged();
    }

    private void ActualMinorTickBrushChanged()
    {
      this.NotifyPropertyChanged("ActualMinorTickBrush");
    }

    private void ActualMajorTickBrushChanged()
    {
      this.NotifyPropertyChanged("ActualMajorTickBrush");
    }
  }
}
