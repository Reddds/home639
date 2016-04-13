// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.GaugeBaseLayoutElement
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows;
using System.Windows.Controls;

namespace DevExpress.Xpf.Gauges
{
  public class GaugeBaseLayoutElement : Panel
  {
    private Size arrangeSize = Size.Empty;
    private bool measureInvalidated;

    private void ArrangeChildren(Size finalSize)
    {
      foreach (UIElement uiElement in this.Children)
        uiElement.Arrange(new Rect(new Point(0.0, 0.0), finalSize));
    }

    protected override Size MeasureOverride(Size availableSize)
    {
      Size availableSize1 = this.measureInvalidated ? this.arrangeSize : availableSize;
      foreach (UIElement uiElement in this.Children)
        uiElement.Measure(availableSize1);
      return availableSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      if (this.arrangeSize != finalSize)
      {
        this.arrangeSize = finalSize;
        this.measureInvalidated = true;
        this.InvalidateMeasure();
      }
      else
      {
        this.arrangeSize = Size.Empty;
        this.measureInvalidated = false;
      }
      this.ArrangeChildren(finalSize);
      return finalSize;
    }
  }
}
