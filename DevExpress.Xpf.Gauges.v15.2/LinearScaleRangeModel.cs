// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LinearScaleRangeModel
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  public class LinearScaleRangeModel : ModelBase
  {
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (LinearScaleRangePresentation), typeof (LinearScaleRangeModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
    public static readonly DependencyProperty OptionsProperty = DependencyPropertyManager.Register("Options", typeof (RangeOptions), typeof (LinearScaleRangeModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public LinearScaleRangePresentation Presentation
    {
      get
      {
        return (LinearScaleRangePresentation) this.GetValue(LinearScaleRangeModel.PresentationProperty);
      }
      set
      {
        this.SetValue(LinearScaleRangeModel.PresentationProperty, (object) value);
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public RangeOptions Options
    {
      get
      {
        return (RangeOptions) this.GetValue(LinearScaleRangeModel.OptionsProperty);
      }
      set
      {
        this.SetValue(LinearScaleRangeModel.OptionsProperty, (object) value);
      }
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new LinearScaleRangeModel();
    }
  }
}
