// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.CustomMatrix5x8Presentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace DevExpress.Xpf.Gauges
{
  public class CustomMatrix5x8Presentation : Matrix5x8Presentation
  {
    public static readonly DependencyProperty ActiveSegmentTemplateProperty = DependencyPropertyManager.Register("ActiveSegmentTemplate", typeof (DataTemplate), typeof (CustomMatrix5x8Presentation));
    public static readonly DependencyProperty InactiveSegmentTemplateProperty = DependencyPropertyManager.Register("InactiveSegmentTemplate", typeof (DataTemplate), typeof (CustomMatrix5x8Presentation));

    [Category("Common")]
    public DataTemplate ActiveSegmentTemplate
    {
      get
      {
        return (DataTemplate) this.GetValue(CustomMatrix5x8Presentation.ActiveSegmentTemplateProperty);
      }
      set
      {
        this.SetValue(CustomMatrix5x8Presentation.ActiveSegmentTemplateProperty, (object) value);
      }
    }

    [Category("Common")]
    public DataTemplate InactiveSegmentTemplate
    {
      get
      {
        return (DataTemplate) this.GetValue(CustomMatrix5x8Presentation.InactiveSegmentTemplateProperty);
      }
      set
      {
        this.SetValue(CustomMatrix5x8Presentation.InactiveSegmentTemplateProperty, (object) value);
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
      CustomMatrix5x8Control matrix5x8Control = new CustomMatrix5x8Control();
      matrix5x8Control.SetBinding(CustomMatrix5x8Control.ActiveSegmentTemplateProperty, (BindingBase) new Binding("ActiveSegmentTemplate")
      {
        Source = (object) this
      });
      matrix5x8Control.SetBinding(CustomMatrix5x8Control.InactiveSegmentTemplateProperty, (BindingBase) new Binding("InactiveSegmentTemplate")
      {
        Source = (object) this
      });
      return (PresentationControl) matrix5x8Control;
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new CustomMatrix5x8Presentation();
    }
  }
}
