// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LinearScaleMarkerModel
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  public class LinearScaleMarkerModel : ModelBase
  {
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (LinearScaleMarkerPresentation), typeof (LinearScaleMarkerModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
    public static readonly DependencyProperty OptionsProperty = DependencyPropertyManager.Register("Options", typeof (LinearScaleMarkerOptions), typeof (LinearScaleMarkerModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public LinearScaleMarkerPresentation Presentation
    {
      get
      {
        return (LinearScaleMarkerPresentation) this.GetValue(LinearScaleMarkerModel.PresentationProperty);
      }
      set
      {
        this.SetValue(LinearScaleMarkerModel.PresentationProperty, (object) value);
      }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public LinearScaleMarkerOptions Options
    {
      get
      {
        return (LinearScaleMarkerOptions) this.GetValue(LinearScaleMarkerModel.OptionsProperty);
      }
      set
      {
        this.SetValue(LinearScaleMarkerModel.OptionsProperty, (object) value);
      }
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new LinearScaleMarkerModel();
    }
  }
}
