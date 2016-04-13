// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LinearScaleLevelBarModel
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  public class LinearScaleLevelBarModel : ModelBase
  {
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (LinearScaleLevelBarPresentation), typeof (LinearScaleLevelBarModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
    public static readonly DependencyProperty OptionsProperty = DependencyPropertyManager.Register("Options", typeof (LinearScaleLevelBarOptions), typeof (LinearScaleLevelBarModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public LinearScaleLevelBarPresentation Presentation
    {
      get
      {
        return (LinearScaleLevelBarPresentation) this.GetValue(LinearScaleLevelBarModel.PresentationProperty);
      }
      set
      {
        this.SetValue(LinearScaleLevelBarModel.PresentationProperty, (object) value);
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public LinearScaleLevelBarOptions Options
    {
      get
      {
        return (LinearScaleLevelBarOptions) this.GetValue(LinearScaleLevelBarModel.OptionsProperty);
      }
      set
      {
        this.SetValue(LinearScaleLevelBarModel.OptionsProperty, (object) value);
      }
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new LinearScaleLevelBarModel();
    }
  }
}
