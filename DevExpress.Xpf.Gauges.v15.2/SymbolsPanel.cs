// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.SymbolsPanel
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System;
using System.Windows;
using System.Windows.Controls;

namespace DevExpress.Xpf.Gauges
{
  public class SymbolsPanel : Panel
  {
    private const double defaultWidth = 300.0;
    private const double defaultHeight = 300.0;

    private DigitalGaugeControl Gauge
    {
      get
      {
        return this.DataContext as DigitalGaugeControl;
      }
    }

    private TextHorizontalAlignment TextHorizontalAlignment
    {
      get
      {
        if (this.Gauge == null)
          return TextHorizontalAlignment.Center;
        return this.Gauge.TextHorizontalAlignment;
      }
    }

    private TextVerticalAlignment TextVerticalAlignment
    {
      get
      {
        if (this.Gauge == null)
          return TextVerticalAlignment.Center;
        return this.Gauge.TextVerticalAlignment;
      }
    }

    protected override Size MeasureOverride(Size availableSize)
    {
      Size availableSize1 = new Size(double.IsInfinity(availableSize.Width) ? 300.0 : availableSize.Width, double.IsInfinity(availableSize.Height) ? 300.0 : availableSize.Height);
      double num1 = 0.0;
      double num2 = 0.0;
      foreach (UIElement uiElement in this.Children)
      {
        uiElement.Measure(availableSize1);
        num1 = Math.Max(num1, uiElement.DesiredSize.Width);
        num2 = Math.Max(num2, uiElement.DesiredSize.Height);
      }
      return new Size(num1, num2);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      foreach (UIElement uiElement in this.Children)
      {
        double val2_1;
        switch (this.TextHorizontalAlignment)
        {
          case TextHorizontalAlignment.Left:
            val2_1 = 0.0;
            break;
          case TextHorizontalAlignment.Right:
            val2_1 = finalSize.Width - uiElement.DesiredSize.Width;
            break;
          default:
            val2_1 = 0.5 * (finalSize.Width - uiElement.DesiredSize.Width);
            break;
        }
        double val2_2;
        switch (this.TextVerticalAlignment)
        {
          case TextVerticalAlignment.Top:
            val2_2 = 0.0;
            break;
          case TextVerticalAlignment.Bottom:
            val2_2 = finalSize.Height - uiElement.DesiredSize.Height;
            break;
          default:
            val2_2 = 0.5 * (finalSize.Height - uiElement.DesiredSize.Height);
            break;
        }
        uiElement.Arrange(new Rect(Math.Max(0.0, val2_1), Math.Max(0.0, val2_2), uiElement.DesiredSize.Width, uiElement.DesiredSize.Height));
      }
      return finalSize;
    }
  }
}
