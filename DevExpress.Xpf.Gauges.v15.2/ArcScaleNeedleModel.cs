// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ArcScaleNeedleModel
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  public class ArcScaleNeedleModel : ModelBase
  {
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (ArcScaleNeedlePresentation), typeof (ArcScaleNeedleModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
    public static readonly DependencyProperty OptionsProperty = DependencyPropertyManager.Register("Options", typeof (ArcScaleNeedleOptions), typeof (ArcScaleNeedleModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public ArcScaleNeedlePresentation Presentation
    {
      get
      {
        return (ArcScaleNeedlePresentation) this.GetValue(ArcScaleNeedleModel.PresentationProperty);
      }
      set
      {
        this.SetValue(ArcScaleNeedleModel.PresentationProperty, (object) value);
      }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ArcScaleNeedleOptions Options
    {
      get
      {
        return (ArcScaleNeedleOptions) this.GetValue(ArcScaleNeedleModel.OptionsProperty);
      }
      set
      {
        this.SetValue(ArcScaleNeedleModel.OptionsProperty, (object) value);
      }
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new ArcScaleNeedleModel();
    }
  }
}
