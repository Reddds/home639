// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.CustomMatrix8x14Presentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace DevExpress.Xpf.Gauges
{
  public class CustomMatrix8x14Presentation : Matrix8x14Presentation
  {
    public static readonly DependencyProperty ActiveSegmentTemplateProperty = DependencyPropertyManager.Register("ActiveSegmentTemplate", typeof (DataTemplate), typeof (CustomMatrix8x14Presentation));
    public static readonly DependencyProperty InactiveSegmentTemplateProperty = DependencyPropertyManager.Register("InactiveSegmentTemplate", typeof (DataTemplate), typeof (CustomMatrix8x14Presentation));

    [Category("Common")]
    public DataTemplate ActiveSegmentTemplate
    {
      get
      {
        return (DataTemplate) this.GetValue(CustomMatrix8x14Presentation.ActiveSegmentTemplateProperty);
      }
      set
      {
        this.SetValue(CustomMatrix8x14Presentation.ActiveSegmentTemplateProperty, (object) value);
      }
    }

    [Category("Common")]
    public DataTemplate InactiveSegmentTemplate
    {
      get
      {
        return (DataTemplate) this.GetValue(CustomMatrix8x14Presentation.InactiveSegmentTemplateProperty);
      }
      set
      {
        this.SetValue(CustomMatrix8x14Presentation.InactiveSegmentTemplateProperty, (object) value);
      }
    }

    public override string PresentationName
    {
      get
      {
        return "Custom";
      }
    }

    protected internal override PresentationControl CreateSymbolPresentationControl()
    {
      CustomMatrix8x14Control matrix8x14Control = new CustomMatrix8x14Control();
      matrix8x14Control.SetBinding(CustomMatrix8x14Control.ActiveSegmentTemplateProperty, (BindingBase) new Binding("ActiveSegmentTemplate")
      {
        Source = (object) this
      });
      matrix8x14Control.SetBinding(CustomMatrix8x14Control.InactiveSegmentTemplateProperty, (BindingBase) new Binding("InactiveSegmentTemplate")
      {
        Source = (object) this
      });
      return (PresentationControl) matrix8x14Control;
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new CustomMatrix8x14Presentation();
    }
  }
}
