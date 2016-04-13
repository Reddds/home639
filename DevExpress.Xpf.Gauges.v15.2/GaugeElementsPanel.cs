// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.GaugeElementsPanel
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using System.Windows;
using System.Windows.Controls;

namespace DevExpress.Xpf.Gauges
{
  public class GaugeElementsPanel : Panel
  {
    private const double defaultWidth = 300.0;
    private const double defaultHeight = 300.0;

    protected override Size MeasureOverride(Size availableSize)
    {
      Size availableSize1 = new Size(double.IsInfinity(availableSize.Width) ? 300.0 : availableSize.Width, double.IsInfinity(availableSize.Height) ? 300.0 : availableSize.Height);
      foreach (UIElement uiElement in this.Children)
        uiElement.Measure(availableSize1);
      return availableSize1;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      foreach (UIElement uiElement in this.Children)
      {
        IGaugeLayoutElement gaugeLayoutElement = uiElement as IGaugeLayoutElement;
        if (gaugeLayoutElement != null && gaugeLayoutElement.Layout != null)
        {
          uiElement.RenderTransform = gaugeLayoutElement.Layout.RenderTransform;
          uiElement.RenderTransformOrigin = gaugeLayoutElement.RenderTransformOrigin;
          uiElement.Clip = gaugeLayoutElement.Layout.ClipGeometry;
          Size size = new Size(gaugeLayoutElement.Layout.Width.HasValue ? gaugeLayoutElement.Layout.Width.Value : uiElement.DesiredSize.Width, gaugeLayoutElement.Layout.Height.HasValue ? gaugeLayoutElement.Layout.Height.Value : uiElement.DesiredSize.Height);
          Point location = MathUtils.CorrectLocationByTransformOrigin(gaugeLayoutElement.Layout.AnchorPoint, gaugeLayoutElement.RenderTransformOrigin, size);
          location.X += gaugeLayoutElement.Offset.X;
          location.Y += gaugeLayoutElement.Offset.Y;
          uiElement.Arrange(new Rect(location, size));
        }
        else
          uiElement.Arrange(new Rect(0.0, 0.0, 0.0, 0.0));
      }
      return finalSize;
    }
  }
}
