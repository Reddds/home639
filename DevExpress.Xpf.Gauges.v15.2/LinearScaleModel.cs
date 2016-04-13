// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LinearScaleModel
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  public class LinearScaleModel : ScaleModel
  {
    public static readonly DependencyProperty LabelOptionsProperty = DependencyPropertyManager.Register("LabelOptions", typeof (LinearScaleLabelOptions), typeof (LinearScaleModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
    public static readonly DependencyProperty LinePresentationProperty = DependencyPropertyManager.Register("LinePresentation", typeof (LinearScaleLinePresentation), typeof (LinearScaleModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public LinearScaleLabelOptions LabelOptions
    {
      get
      {
        return (LinearScaleLabelOptions) this.GetValue(LinearScaleModel.LabelOptionsProperty);
      }
      set
      {
        this.SetValue(LinearScaleModel.LabelOptionsProperty, (object) value);
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public LinearScaleLinePresentation LinePresentation
    {
      get
      {
        return (LinearScaleLinePresentation) this.GetValue(LinearScaleModel.LinePresentationProperty);
      }
      set
      {
        this.SetValue(LinearScaleModel.LinePresentationProperty, (object) value);
      }
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new LinearScaleModel();
    }
  }
}
