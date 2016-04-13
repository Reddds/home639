// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ScaleElementInfoContainer
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System.Windows;
using System.Windows.Controls;

namespace DevExpress.Xpf.Gauges
{
  [NonCategorized]
  public class ScaleElementInfoContainer : Control, IScaleLayoutElement
  {
    public static readonly DependencyProperty ElementInfoProperty = DependencyPropertyManager.Register("ElementInfo", typeof (ScaleElementInfoBase), typeof (ScaleElementInfoContainer));

    public ScaleElementInfoBase ElementInfo
    {
      get
      {
        return (ScaleElementInfoBase) this.GetValue(ScaleElementInfoContainer.ElementInfoProperty);
      }
      set
      {
        this.SetValue(ScaleElementInfoContainer.ElementInfoProperty, (object) value);
      }
    }

    private IScaleLayoutElement LayoutElement
    {
      get
      {
        return (IScaleLayoutElement) this.ElementInfo;
      }
    }

    ScaleElementLayout IScaleLayoutElement.Layout
    {
      get
      {
        if (this.LayoutElement == null)
          return (ScaleElementLayout) null;
        return this.LayoutElement.Layout;
      }
    }

    Point IScaleLayoutElement.RenderTransformOrigin
    {
      get
      {
        if (this.LayoutElement == null)
          return new Point(0.0, 0.0);
        return this.LayoutElement.RenderTransformOrigin;
      }
    }

    public ScaleElementInfoContainer()
    {
      this.DefaultStyleKey = (object) typeof (ScaleElementInfoContainer);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
      return base.MeasureOverride(availableSize);
    }
  }
}
