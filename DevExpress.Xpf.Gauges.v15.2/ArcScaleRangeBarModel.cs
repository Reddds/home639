// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ArcScaleRangeBarModel
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  public class ArcScaleRangeBarModel : ModelBase
  {
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (ArcScaleRangeBarPresentation), typeof (ArcScaleRangeBarModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
    public static readonly DependencyProperty OptionsProperty = DependencyPropertyManager.Register("Options", typeof (ArcScaleRangeBarOptions), typeof (ArcScaleRangeBarModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ArcScaleRangeBarPresentation Presentation
    {
      get
      {
        return (ArcScaleRangeBarPresentation) this.GetValue(ArcScaleRangeBarModel.PresentationProperty);
      }
      set
      {
        this.SetValue(ArcScaleRangeBarModel.PresentationProperty, (object) value);
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public ArcScaleRangeBarOptions Options
    {
      get
      {
        return (ArcScaleRangeBarOptions) this.GetValue(ArcScaleRangeBarModel.OptionsProperty);
      }
      set
      {
        this.SetValue(ArcScaleRangeBarModel.OptionsProperty, (object) value);
      }
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new ArcScaleRangeBarModel();
    }
  }
}
