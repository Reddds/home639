// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ArcScaleModel
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  public class ArcScaleModel : ScaleModel
  {
    public static readonly DependencyProperty SpindleCapPresentationProperty = DependencyPropertyManager.Register("SpindleCapPresentation", typeof (SpindleCapPresentation), typeof (ArcScaleModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
    public static readonly DependencyProperty LabelOptionsProperty = DependencyPropertyManager.Register("LabelOptions", typeof (ArcScaleLabelOptions), typeof (ArcScaleModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
    public static readonly DependencyProperty SpindleCapOptionsProperty = DependencyPropertyManager.Register("SpindleCapOptions", typeof (SpindleCapOptions), typeof (ArcScaleModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
    public static readonly DependencyProperty ShowSpindleCapProperty = DependencyPropertyManager.Register("ShowSpindleCap", typeof (bool), typeof (ArcScaleModel), new PropertyMetadata((object) false, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
    public static readonly DependencyProperty LinePresentationProperty = DependencyPropertyManager.Register("LinePresentation", typeof (ArcScaleLinePresentation), typeof (ArcScaleModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
    public static readonly DependencyProperty LayoutMarginProperty = DependencyPropertyManager.Register("LayoutMargin", typeof (Thickness), typeof (ArcScaleModel), new PropertyMetadata((object) new Thickness(), new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public SpindleCapPresentation SpindleCapPresentation
    {
      get
      {
        return (SpindleCapPresentation) this.GetValue(ArcScaleModel.SpindleCapPresentationProperty);
      }
      set
      {
        this.SetValue(ArcScaleModel.SpindleCapPresentationProperty, (object) value);
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ArcScaleLabelOptions LabelOptions
    {
      get
      {
        return (ArcScaleLabelOptions) this.GetValue(ArcScaleModel.LabelOptionsProperty);
      }
      set
      {
        this.SetValue(ArcScaleModel.LabelOptionsProperty, (object) value);
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public SpindleCapOptions SpindleCapOptions
    {
      get
      {
        return (SpindleCapOptions) this.GetValue(ArcScaleModel.SpindleCapOptionsProperty);
      }
      set
      {
        this.SetValue(ArcScaleModel.SpindleCapOptionsProperty, (object) value);
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool ShowSpindleCap
    {
      get
      {
        return (bool) this.GetValue(ArcScaleModel.ShowSpindleCapProperty);
      }
      set
      {
        this.SetValue(ArcScaleModel.ShowSpindleCapProperty, (object) (bool) (value ? 1 : 0));
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public ArcScaleLinePresentation LinePresentation
    {
      get
      {
        return (ArcScaleLinePresentation) this.GetValue(ArcScaleModel.LinePresentationProperty);
      }
      set
      {
        this.SetValue(ArcScaleModel.LinePresentationProperty, (object) value);
      }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Thickness LayoutMargin
    {
      get
      {
        return (Thickness) this.GetValue(ArcScaleModel.LayoutMarginProperty);
      }
      set
      {
        this.SetValue(ArcScaleModel.LayoutMarginProperty, (object) value);
      }
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new ArcScaleModel();
    }
  }
}
