// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.CustomLinearScaleRangeBarPresentation
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
  public class CustomLinearScaleRangeBarPresentation : LinearScaleRangeBarPresentation
  {
    public static readonly DependencyProperty RangeBarTemplateProperty = DependencyPropertyManager.Register("RangeBarTemplate", typeof (ControlTemplate), typeof (CustomLinearScaleRangeBarPresentation));

    [Category("Common")]
    public ControlTemplate RangeBarTemplate
    {
      get
      {
        return (ControlTemplate) this.GetValue(CustomLinearScaleRangeBarPresentation.RangeBarTemplateProperty);
      }
      set
      {
        this.SetValue(CustomLinearScaleRangeBarPresentation.RangeBarTemplateProperty, (object) value);
      }
    }

    public override string PresentationName
    {
      get
      {
        return "Custom Range Bar";
      }
    }

    protected internal override PresentationControl CreateIndicatorPresentationControl()
    {
      CustomValueIndicatorPresentationControl presentationControl = new CustomValueIndicatorPresentationControl();
      presentationControl.SetBinding(Control.TemplateProperty, (BindingBase) new Binding("RangeBarTemplate")
      {
        Source = (object) this
      });
      return (PresentationControl) presentationControl;
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new CustomLinearScaleRangeBarPresentation();
    }
  }
}
