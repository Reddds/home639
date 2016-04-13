// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ScaleModel
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  public abstract class ScaleModel : ModelBase
  {
    public static readonly DependencyProperty LabelPresentationProperty = DependencyPropertyManager.Register("LabelPresentation", typeof (ScaleLabelPresentation), typeof (ScaleModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
    public static readonly DependencyProperty TickmarksPresentationProperty = DependencyPropertyManager.Register("TickmarksPresentation", typeof (TickmarksPresentation), typeof (ScaleModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
    public static readonly DependencyProperty LineOptionsProperty = DependencyPropertyManager.Register("LineOptions", typeof (ScaleLineOptions), typeof (ScaleModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
    public static readonly DependencyProperty ShowLineProperty = DependencyPropertyManager.Register("ShowLine", typeof (bool), typeof (ScaleModel), new PropertyMetadata((object) false, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
    public static readonly DependencyProperty ShowLabelsProperty = DependencyPropertyManager.Register("ShowLabels", typeof (bool), typeof (ScaleModel), new PropertyMetadata((object) true, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
    public static readonly DependencyProperty ShowMajorTickmarksProperty = DependencyPropertyManager.Register("ShowMajorTickmarks", typeof (bool), typeof (ScaleModel), new PropertyMetadata((object) true, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
    public static readonly DependencyProperty ShowMinorTickmarksProperty = DependencyPropertyManager.Register("ShowMinorTickmarks", typeof (bool), typeof (ScaleModel), new PropertyMetadata((object) true, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
    public static readonly DependencyProperty MinorTickmarkOptionsProperty = DependencyPropertyManager.Register("MinorTickmarkOptions", typeof (MinorTickmarkOptions), typeof (ScaleModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
    public static readonly DependencyProperty MajorTickmarkOptionsProperty = DependencyPropertyManager.Register("MajorTickmarkOptions", typeof (MajorTickmarkOptions), typeof (ScaleModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public ScaleLabelPresentation LabelPresentation
    {
      get
      {
        return (ScaleLabelPresentation) this.GetValue(ScaleModel.LabelPresentationProperty);
      }
      set
      {
        this.SetValue(ScaleModel.LabelPresentationProperty, (object) value);
      }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TickmarksPresentation TickmarksPresentation
    {
      get
      {
        return (TickmarksPresentation) this.GetValue(ScaleModel.TickmarksPresentationProperty);
      }
      set
      {
        this.SetValue(ScaleModel.TickmarksPresentationProperty, (object) value);
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ScaleLineOptions LineOptions
    {
      get
      {
        return (ScaleLineOptions) this.GetValue(ScaleModel.LineOptionsProperty);
      }
      set
      {
        this.SetValue(ScaleModel.LineOptionsProperty, (object) value);
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool ShowLine
    {
      get
      {
        return (bool) this.GetValue(ScaleModel.ShowLineProperty);
      }
      set
      {
        this.SetValue(ScaleModel.ShowLineProperty, (object) (bool) (value ? 1 : 0));
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool ShowLabels
    {
      get
      {
        return (bool) this.GetValue(ScaleModel.ShowLabelsProperty);
      }
      set
      {
        this.SetValue(ScaleModel.ShowLabelsProperty, (object) (bool) (value ? 1 : 0));
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public bool ShowMajorTickmarks
    {
      get
      {
        return (bool) this.GetValue(ScaleModel.ShowMajorTickmarksProperty);
      }
      set
      {
        this.SetValue(ScaleModel.ShowMajorTickmarksProperty, (object) (bool) (value ? 1 : 0));
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool ShowMinorTickmarks
    {
      get
      {
        return (bool) this.GetValue(ScaleModel.ShowMinorTickmarksProperty);
      }
      set
      {
        this.SetValue(ScaleModel.ShowMinorTickmarksProperty, (object) (bool) (value ? 1 : 0));
      }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public MinorTickmarkOptions MinorTickmarkOptions
    {
      get
      {
        return (MinorTickmarkOptions) this.GetValue(ScaleModel.MinorTickmarkOptionsProperty);
      }
      set
      {
        this.SetValue(ScaleModel.MinorTickmarkOptionsProperty, (object) value);
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public MajorTickmarkOptions MajorTickmarkOptions
    {
      get
      {
        return (MajorTickmarkOptions) this.GetValue(ScaleModel.MajorTickmarkOptionsProperty);
      }
      set
      {
        this.SetValue(ScaleModel.MajorTickmarkOptionsProperty, (object) value);
      }
    }
  }
}
