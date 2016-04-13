// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.LinearScaleLayoutCalculator
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges;
using System.Windows;

namespace DevExpress.Xpf.Gauges.Native
{
  public class LinearScaleLayoutCalculator
  {
    private readonly LinearScale scale;
    private readonly Rect bounds;

    private Point StartPoint
    {
      get
      {
        switch (this.scale.LayoutMode)
        {
          case LinearScaleLayoutMode.LeftToRight:
            return new Point(this.bounds.Left, 0.5 * (this.bounds.Top + this.bounds.Bottom));
          case LinearScaleLayoutMode.RightToLeft:
            return new Point(this.bounds.Right, 0.5 * (this.bounds.Top + this.bounds.Bottom));
          case LinearScaleLayoutMode.TopToBottom:
            return new Point(0.5 * (this.bounds.Left + this.bounds.Right), this.bounds.Top);
          default:
            return new Point(0.5 * (this.bounds.Left + this.bounds.Right), this.bounds.Bottom);
        }
      }
    }

    private Point EndPoint
    {
      get
      {
        switch (this.scale.LayoutMode)
        {
          case LinearScaleLayoutMode.LeftToRight:
            return new Point(this.bounds.Right, 0.5 * (this.bounds.Top + this.bounds.Bottom));
          case LinearScaleLayoutMode.RightToLeft:
            return new Point(this.bounds.Left, 0.5 * (this.bounds.Top + this.bounds.Bottom));
          case LinearScaleLayoutMode.TopToBottom:
            return new Point(0.5 * (this.bounds.Left + this.bounds.Right), this.bounds.Bottom);
          default:
            return new Point(0.5 * (this.bounds.Left + this.bounds.Right), this.bounds.Top);
        }
      }
    }

    public LinearScaleLayoutCalculator(LinearScale scale, Rect bounds)
    {
      this.scale = scale;
      this.bounds = bounds;
    }

    public LinearScaleLayout Calculate()
    {
      return new LinearScaleLayout(this.bounds, this.StartPoint, new Point(this.EndPoint.X - this.StartPoint.X, this.EndPoint.Y - this.StartPoint.Y));
    }
  }
}
