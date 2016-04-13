// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ScaleElementsPanel
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class ScaleElementsPanel : Panel
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
        IScaleLayoutElement scaleLayoutElement = uiElement as IScaleLayoutElement;
        if (scaleLayoutElement != null && scaleLayoutElement.Layout != null)
        {
          uiElement.RenderTransform = (Transform) new TransformGroup()
          {
            Children = {
              (Transform) new ScaleTransform()
              {
                ScaleX = scaleLayoutElement.Layout.ScaleFactor.X,
                ScaleY = scaleLayoutElement.Layout.ScaleFactor.Y
              },
              (Transform) new RotateTransform()
              {
                Angle = scaleLayoutElement.Layout.Angle
              }
            }
          };
          uiElement.RenderTransformOrigin = scaleLayoutElement.RenderTransformOrigin;
          uiElement.Clip = scaleLayoutElement.Layout.ClipGeometry;
          Size size = scaleLayoutElement.Layout.Size.HasValue ? scaleLayoutElement.Layout.Size.Value : uiElement.DesiredSize;
          Point location = MathUtils.CorrectLocationByTransformOrigin(scaleLayoutElement.Layout.AnchorPoint, scaleLayoutElement.RenderTransformOrigin, size);
          uiElement.Arrange(new Rect(location, size));
        }
        else
          uiElement.Arrange(new Rect(0.0, 0.0, 0.0, 0.0));
      }
      return finalSize;
    }
  }
}
