// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ElementInfoPanel
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
  public class ElementInfoPanel : Panel
  {
    public static readonly DependencyProperty StretchToAvailableSizeProperty = DependencyPropertyManager.Register("StretchToAvailableSize", typeof (bool), typeof (ElementInfoPanel), new PropertyMetadata((object) true));
    private const double defaultWidth = 300.0;
    private const double defaultHeight = 300.0;

    public bool StretchToAvailableSize
    {
      get
      {
        return (bool) this.GetValue(ElementInfoPanel.StretchToAvailableSizeProperty);
      }
      set
      {
        this.SetValue(ElementInfoPanel.StretchToAvailableSizeProperty, (object) (bool) (value ? 1 : 0));
      }
    }

    private ElementInfoBase ElementInfo
    {
      get
      {
        return this.DataContext as ElementInfoBase;
      }
    }

    private Size GetMeasureSize(Size constraint)
    {
      if (this.StretchToAvailableSize || this.ElementInfo == null || this.Children.Count == 0)
        return constraint;
      if (this.ElementInfo.Layout == null)
        return this.Children[0].DesiredSize;
      return new Size(this.ElementInfo.Layout.Width.HasValue ? this.ElementInfo.Layout.Width.Value : this.Children[0].DesiredSize.Width, this.ElementInfo.Layout.Height.HasValue ? this.ElementInfo.Layout.Height.Value : this.Children[0].DesiredSize.Height);
    }

    private Size GetArrangeSize(UIElement child, ElementLayout layout, Size finalSize)
    {
      double height = layout.Height.HasValue ? layout.Height.Value : (this.StretchToAvailableSize ? child.DesiredSize.Height : finalSize.Height);
      return new Size(layout.Width.HasValue ? layout.Width.Value : (this.StretchToAvailableSize ? child.DesiredSize.Width : finalSize.Width), height);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
      Size constraint = new Size(double.IsInfinity(availableSize.Width) ? 300.0 : availableSize.Width, double.IsInfinity(availableSize.Height) ? 300.0 : availableSize.Height);
      Size availableSize1 = new Size(0.0, 0.0);
      Visibility visibility = Visibility.Collapsed;
      if (this.ElementInfo != null)
      {
        this.ElementInfo.CreateLayout(constraint);
        if (this.ElementInfo.Layout != null)
        {
          availableSize1 = new Size(this.ElementInfo.Layout.Width.HasValue ? this.ElementInfo.Layout.Width.Value : constraint.Width, this.ElementInfo.Layout.Height.HasValue ? this.ElementInfo.Layout.Height.Value : constraint.Height);
          visibility = Visibility.Visible;
        }
      }
      foreach (UIElement uiElement in this.Children)
      {
        uiElement.Visibility = visibility;
        uiElement.Measure(availableSize1);
      }
      return this.GetMeasureSize(constraint);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      if (this.ElementInfo != null)
        this.ElementInfo.CompleteLayout();
      foreach (UIElement child in this.Children)
      {
        Rect finalRect = new Rect(0.0, 0.0, 0.0, 0.0);
        if (this.ElementInfo != null && this.ElementInfo.Layout != null)
        {
          Size arrangeSize = this.GetArrangeSize(child, this.ElementInfo.Layout, finalSize);
          Point transformOrigin = this.ElementInfo.PresentationControl != null ? this.ElementInfo.PresentationControl.GetRenderTransformOrigin() : new Point(0.0, 0.0);
          finalRect = new Rect(MathUtils.CorrectLocationByTransformOrigin(this.ElementInfo.Layout.AnchorPoint, transformOrigin, arrangeSize), arrangeSize);
          child.RenderTransformOrigin = transformOrigin;
          child.RenderTransform = this.ElementInfo.Layout.RenderTransform;
          child.Clip = this.ElementInfo.Layout.ClipGeometry;
        }
        child.Arrange(finalRect);
      }
      return finalSize;
    }
  }
}
