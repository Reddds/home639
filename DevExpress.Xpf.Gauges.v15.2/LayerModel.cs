// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LayerModel
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  public class LayerModel : ModelBase
  {
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (LayerPresentation), typeof (LayerModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
    public static readonly DependencyProperty OptionsProperty = DependencyPropertyManager.Register("Options", typeof (LayerOptions), typeof (LayerModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public LayerPresentation Presentation
    {
      get
      {
        return (LayerPresentation) this.GetValue(LayerModel.PresentationProperty);
      }
      set
      {
        this.SetValue(LayerModel.PresentationProperty, (object) value);
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public LayerOptions Options
    {
      get
      {
        return (LayerOptions) this.GetValue(LayerModel.OptionsProperty);
      }
      set
      {
        this.SetValue(LayerModel.OptionsProperty, (object) value);
      }
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new LayerModel();
    }
  }
}
