// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.SymbolPanel
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using System.Windows;
using System.Windows.Controls;

namespace DevExpress.Xpf.Gauges
{
  public class SymbolPanel : Panel
  {
    private const double defaultWidth = 300.0;
    private const double defaultHeight = 300.0;

    protected override Size MeasureOverride(Size availableSize)
    {
      Size constraint = new Size(double.IsInfinity(availableSize.Width) ? 300.0 : availableSize.Width, double.IsInfinity(availableSize.Height) ? 300.0 : availableSize.Height);
      Size availableSize1 = new Size(0.0, 0.0);
      Visibility visibility = Visibility.Collapsed;
      foreach (UIElement uiElement in this.Children)
      {
        FrameworkElement frameworkElement = uiElement as FrameworkElement;
        if (frameworkElement != null)
        {
          ElementInfoBase elementInfoBase = frameworkElement.DataContext as ElementInfoBase;
          if (elementInfoBase != null)
          {
            elementInfoBase.CreateLayout(constraint);
            if (elementInfoBase.Layout != null)
            {
              availableSize1 = new Size(elementInfoBase.Layout.Width.HasValue ? elementInfoBase.Layout.Width.Value : constraint.Width, elementInfoBase.Layout.Height.HasValue ? elementInfoBase.Layout.Height.Value : constraint.Height);
              visibility = Visibility.Visible;
            }
          }
        }
        uiElement.Visibility = visibility;
        uiElement.Measure(availableSize1);
      }
      return constraint;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      foreach (UIElement uiElement in this.Children)
      {
        Rect finalRect = new Rect(0.0, 0.0, 0.0, 0.0);
        FrameworkElement frameworkElement = uiElement as FrameworkElement;
        if (frameworkElement != null)
        {
          ElementInfoBase elementInfoBase = frameworkElement.DataContext as ElementInfoBase;
          if (elementInfoBase != null)
          {
            elementInfoBase.CompleteLayout();
            if (elementInfoBase.Layout != null)
            {
              Size size = new Size(elementInfoBase.Layout.Width.HasValue ? elementInfoBase.Layout.Width.Value : uiElement.DesiredSize.Width, elementInfoBase.Layout.Height.HasValue ? elementInfoBase.Layout.Height.Value : uiElement.DesiredSize.Height);
              Point transformOrigin = elementInfoBase.PresentationControl != null ? elementInfoBase.PresentationControl.GetRenderTransformOrigin() : new Point(0.0, 0.0);
              finalRect = new Rect(MathUtils.CorrectLocationByTransformOrigin(elementInfoBase.Layout.AnchorPoint, transformOrigin, size), size);
              uiElement.RenderTransformOrigin = transformOrigin;
              uiElement.RenderTransform = elementInfoBase.Layout.RenderTransform;
              uiElement.Clip = elementInfoBase.Layout.ClipGeometry;
            }
          }
        }
        uiElement.Arrange(finalRect);
      }
      return finalSize;
    }
  }
}
