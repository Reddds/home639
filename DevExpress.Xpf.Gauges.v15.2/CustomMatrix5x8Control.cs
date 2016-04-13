// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.CustomMatrix5x8Control
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Utils;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  [NonCategorized]
  public class CustomMatrix5x8Control : SymbolPresentationControl
  {
    public static readonly DependencyProperty ActiveSegmentTemplateProperty = DependencyPropertyManager.Register("ActiveSegmentTemplate", typeof (DataTemplate), typeof (CustomMatrix5x8Control));
    public static readonly DependencyProperty InactiveSegmentTemplateProperty = DependencyPropertyManager.Register("InactiveSegmentTemplate", typeof (DataTemplate), typeof (CustomMatrix5x8Control));

    public DataTemplate ActiveSegmentTemplate
    {
      get
      {
        return (DataTemplate) this.GetValue(CustomMatrix5x8Control.ActiveSegmentTemplateProperty);
      }
      set
      {
        this.SetValue(CustomMatrix5x8Control.ActiveSegmentTemplateProperty, (object) value);
      }
    }

    public DataTemplate InactiveSegmentTemplate
    {
      get
      {
        return (DataTemplate) this.GetValue(CustomMatrix5x8Control.InactiveSegmentTemplateProperty);
      }
      set
      {
        this.SetValue(CustomMatrix5x8Control.InactiveSegmentTemplateProperty, (object) value);
      }
    }

    public CustomMatrix5x8Control()
    {
      this.DefaultStyleKey = (object) typeof (CustomMatrix5x8Control);
    }
  }
}
