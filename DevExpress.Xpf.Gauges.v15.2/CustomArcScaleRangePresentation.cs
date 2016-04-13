// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.CustomArcScaleRangePresentation
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
  public class CustomArcScaleRangePresentation : ArcScaleRangePresentation
  {
    public static readonly DependencyProperty RangeTemplateProperty = DependencyPropertyManager.Register("RangeTemplate", typeof (ControlTemplate), typeof (CustomArcScaleRangePresentation));

    [Category("Common")]
    public ControlTemplate RangeTemplate
    {
      get
      {
        return (ControlTemplate) this.GetValue(CustomArcScaleRangePresentation.RangeTemplateProperty);
      }
      set
      {
        this.SetValue(CustomArcScaleRangePresentation.RangeTemplateProperty, (object) value);
      }
    }

    public override string PresentationName
    {
      get
      {
        return "Custom Range";
      }
    }

    protected internal override PresentationControl CreateLayerPresentationControl()
    {
      CustomPresentationControl presentationControl = new CustomPresentationControl();
      presentationControl.SetBinding(Control.TemplateProperty, (BindingBase) new Binding("RangeTemplate")
      {
        Source = (object) this
      });
      return (PresentationControl) presentationControl;
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new CustomArcScaleRangePresentation();
    }
  }
}
