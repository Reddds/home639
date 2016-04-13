// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.ArcScaleLayoutCalculator
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges;
using System;
using System.Windows;

namespace DevExpress.Xpf.Gauges.Native
{
  public class ArcScaleLayoutCalculator
  {
    private readonly ArcScale scale;
    private readonly Rect initialBounds;

    public ArcScaleLayoutCalculator(ArcScale scale, Rect initialBounds)
    {
      this.scale = scale;
      this.initialBounds = initialBounds;
    }

    private void CalculateLayoutParams(Rect bounds, out Point ellipseCenter, out double ellipseWidth, out double ellipseHeight)
    {
      double x = 0.5 * (bounds.Left + bounds.Right);
      double y = 0.5 * (bounds.Top + bounds.Bottom);
      ellipseCenter = new Point(x, y);
      ellipseWidth = bounds.Width;
      ellipseHeight = bounds.Height;
    }

    private void CalculateCircleLayoutParams(out Rect bounds, out Point ellipseCenter, out double ellipseWidth, out double ellipseHeight)
    {
      Size size = new Size(Math.Min(this.initialBounds.Height, this.initialBounds.Width), Math.Min(this.initialBounds.Height, this.initialBounds.Width));
      bounds = new Rect(0.0, 0.0, size.Width, size.Height);
      this.CalculateLayoutParams(bounds, out ellipseCenter, out ellipseWidth, out ellipseHeight);
    }

    private void CalculateEllipseLayoutParams(out Rect bounds, out Point ellipseCenter, out double ellipseWidth, out double ellipseHeight)
    {
      bounds = this.initialBounds;
      this.CalculateLayoutParams(bounds, out ellipseCenter, out ellipseWidth, out ellipseHeight);
    }

    private void CalculateHalfTopLayoutParams(out Rect bounds, out Point ellipseCenter, out double ellipseWidth, out double ellipseHeight)
    {
      bounds = this.initialBounds;
      Thickness layoutMargin = this.GetLayoutMargin();
      if (2.0 * (this.initialBounds.Height - layoutMargin.Top - layoutMargin.Bottom) + layoutMargin.Left + layoutMargin.Right <= this.initialBounds.Width)
      {
        ellipseWidth = Math.Max(2.0 * (this.initialBounds.Height - layoutMargin.Top - layoutMargin.Bottom), 0.0);
        bounds.Width = ellipseWidth + layoutMargin.Left + layoutMargin.Right;
      }
      else
      {
        ellipseWidth = Math.Max(this.initialBounds.Width - layoutMargin.Left - layoutMargin.Right, 0.0);
        bounds.Height = ellipseWidth / 2.0 + layoutMargin.Top + layoutMargin.Bottom;
      }
      ellipseHeight = ellipseWidth;
      double x = 0.5 * ellipseWidth + layoutMargin.Left;
      double y = 0.5 * ellipseHeight + layoutMargin.Top;
      ellipseCenter = new Point(x, y);
    }

    private void CalculateQuarterLayoutParams(out Rect bounds, out Point ellipseCenter, out double ellipseWidth, out double ellipseHeight, ArcScaleLayoutMode layoutMode)
    {
      Thickness layoutMargin = this.GetLayoutMargin();
      bounds = this.initialBounds;
      if (this.initialBounds.Height - layoutMargin.Top - layoutMargin.Bottom + layoutMargin.Left + layoutMargin.Right <= this.initialBounds.Width)
      {
        ellipseWidth = Math.Max(2.0 * (this.initialBounds.Height - layoutMargin.Top - layoutMargin.Bottom), 0.0);
        bounds.Width = 0.5 * ellipseWidth + layoutMargin.Left + layoutMargin.Right;
      }
      else
      {
        ellipseWidth = Math.Max(2.0 * (this.initialBounds.Width - layoutMargin.Left - layoutMargin.Right), 0.0);
        bounds.Height = 0.5 * ellipseWidth + layoutMargin.Top + layoutMargin.Bottom;
      }
      ellipseHeight = ellipseWidth;
      double x = 0.0;
      switch (layoutMode)
      {
        case ArcScaleLayoutMode.QuarterTopLeft:
          x = 0.5 * ellipseWidth + layoutMargin.Left;
          break;
        case ArcScaleLayoutMode.QuarterTopRight:
          x = layoutMargin.Left;
          break;
      }
      double y = 0.5 * ellipseHeight + layoutMargin.Top;
      ellipseCenter = new Point(x, y);
    }

    private Thickness GetLayoutMargin()
    {
      if (this.scale.Gauge.ActualModel.GetScaleLayerModel(this.scale.ActualLayoutMode, 0) != null)
        return this.scale.Gauge.ActualModel.GetScaleModel(this.scale.ActualLayoutMode, 0).LayoutMargin;
      return new Thickness(0.0);
    }

    public ArcScaleLayout Calculate()
    {
      Rect bounds;
      Point ellipseCenter;
      double ellipseWidth;
      double ellipseHeight;
      switch (this.scale.ActualLayoutMode)
      {
        case ArcScaleLayoutMode.Ellipse:
          this.CalculateEllipseLayoutParams(out bounds, out ellipseCenter, out ellipseWidth, out ellipseHeight);
          break;
        case ArcScaleLayoutMode.HalfTop:
          this.CalculateHalfTopLayoutParams(out bounds, out ellipseCenter, out ellipseWidth, out ellipseHeight);
          break;
        case ArcScaleLayoutMode.QuarterTopLeft:
        case ArcScaleLayoutMode.QuarterTopRight:
          this.CalculateQuarterLayoutParams(out bounds, out ellipseCenter, out ellipseWidth, out ellipseHeight, this.scale.ActualLayoutMode);
          break;
        default:
          this.CalculateCircleLayoutParams(out bounds, out ellipseCenter, out ellipseWidth, out ellipseHeight);
          break;
      }
      return new ArcScaleLayout(bounds, ellipseCenter, ellipseWidth, ellipseHeight);
    }
  }
}
