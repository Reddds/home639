// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.DigitalGaugeElementsPanel
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DevExpress.Xpf.Gauges
{
  public class DigitalGaugeElementsPanel : Panel
  {
    private const double defaultWidth = 300.0;
    private const double defaultHeight = 300.0;

    private DigitalGaugeModel Model
    {
      get
      {
        if (!(this.DataContext is DigitalGaugeControl))
          return (DigitalGaugeModel) null;
        return ((DigitalGaugeControl) this.DataContext).ActualModel;
      }
    }

    protected override Size MeasureOverride(Size availableSize)
    {
      double num1 = double.IsInfinity(availableSize.Width) ? 300.0 : availableSize.Width;
      double num2 = double.IsInfinity(availableSize.Height) ? 300.0 : availableSize.Height;
      Size availableSize1 = new Size(num1, num2);
      double num3 = 0.0;
      double val1_1 = 0.0;
      foreach (UIElement uiElement in this.Children)
      {
        uiElement.Measure(availableSize1);
        IGaugeLayoutElement gaugeLayoutElement = uiElement as IGaugeLayoutElement;
        if (gaugeLayoutElement != null && gaugeLayoutElement.InfluenceOnGaugeSize)
        {
          val1_1 = Math.Max(val1_1, uiElement.DesiredSize.Height);
          num3 += uiElement.DesiredSize.Width;
        }
      }
      double val1_2 = num3 + (this.Model != null ? this.Model.InnerPadding.Left + this.Model.InnerPadding.Right : 0.0);
      double val1_3 = val1_1 + (this.Model != null ? this.Model.InnerPadding.Top + this.Model.InnerPadding.Bottom : 0.0);
      return new Size(Math.Min(val1_2, num1), Math.Min(val1_3, num2));
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      Rect finalRect = new Rect(0.0, 0.0, finalSize.Width, finalSize.Height);
      foreach (UIElement uiElement in this.Children)
        uiElement.Arrange(finalRect);
      return finalSize;
    }
  }
}
