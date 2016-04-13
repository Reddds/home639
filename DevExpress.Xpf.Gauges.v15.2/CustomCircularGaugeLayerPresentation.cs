// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.CustomCircularGaugeLayerPresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DevExpress.Xpf.Gauges
{
  public class CustomCircularGaugeLayerPresentation : CircularGaugeLayerPresentation
  {
    public static readonly DependencyProperty GaugeLayerTemplateProperty = DependencyPropertyManager.Register("GaugeLayerTemplate", typeof (ControlTemplate), typeof (CustomCircularGaugeLayerPresentation));

    [Category("Common")]
    public ControlTemplate GaugeLayerTemplate
    {
      get
      {
        return (ControlTemplate) this.GetValue(CustomCircularGaugeLayerPresentation.GaugeLayerTemplateProperty);
      }
      set
      {
        this.SetValue(CustomCircularGaugeLayerPresentation.GaugeLayerTemplateProperty, (object) value);
      }
    }

    public override string PresentationName
    {
      get
      {
        return "Custom";
      }
    }

    protected internal override PresentationControl CreateLayerPresentationControl()
    {
      CustomPresentationControl presentationControl = new CustomPresentationControl();
      presentationControl.SetBinding(Control.TemplateProperty, (BindingBase) new Binding("GaugeLayerTemplate")
      {
        Source = (object) this
      });
      return (PresentationControl) presentationControl;
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new CustomCircularGaugeLayerPresentation();
    }
  }
}
