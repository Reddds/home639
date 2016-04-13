// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LinearScaleRangeBarModel
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  public class LinearScaleRangeBarModel : ModelBase
  {
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (LinearScaleRangeBarPresentation), typeof (LinearScaleRangeBarModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
    public static readonly DependencyProperty OptionsProperty = DependencyPropertyManager.Register("Options", typeof (LinearScaleRangeBarOptions), typeof (LinearScaleRangeBarModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public LinearScaleRangeBarPresentation Presentation
    {
      get
      {
        return (LinearScaleRangeBarPresentation) this.GetValue(LinearScaleRangeBarModel.PresentationProperty);
      }
      set
      {
        this.SetValue(LinearScaleRangeBarModel.PresentationProperty, (object) value);
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public LinearScaleRangeBarOptions Options
    {
      get
      {
        return (LinearScaleRangeBarOptions) this.GetValue(LinearScaleRangeBarModel.OptionsProperty);
      }
      set
      {
        this.SetValue(LinearScaleRangeBarModel.OptionsProperty, (object) value);
      }
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new LinearScaleRangeBarModel();
    }
  }
}
