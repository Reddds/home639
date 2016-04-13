// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.LinearScaleMapping
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges;
using System.Windows;

namespace DevExpress.Xpf.Gauges.Native
{
  public class LinearScaleMapping : ScaleMapping
  {
    private bool IsVerticalScale
    {
      get
      {
        if (this.Scale.LayoutMode != LinearScaleLayoutMode.BottomToTop)
          return this.Scale.LayoutMode == LinearScaleLayoutMode.TopToBottom;
        return true;
      }
    }

    public LinearScale Scale
    {
      get
      {
        return base.Scale as LinearScale;
      }
    }

    public LinearScaleLayout Layout
    {
      get
      {
        return base.Layout as LinearScaleLayout;
      }
    }

    public LinearScaleMapping(LinearScale scale, Rect bounds)
      : base((DevExpress.Xpf.Gauges.Scale) scale, (ScaleLayout) new LinearScaleLayoutCalculator(scale, bounds).Calculate())
    {
    }

    private Point GetPointOffset(double offset)
    {
      if (!this.IsVerticalScale)
        return new Point(0.0, offset);
      return new Point(offset, 0.0);
    }

    protected override double GetAngleByPoint(Point point)
    {
      return !this.IsVerticalScale ? 90.0 : 0.0;
    }

    public override Point GetPointByPercent(double percent)
    {
      return this.GetPointByPercent(percent, 0.0);
    }

    public override Point GetPointByPercent(double percent, double offset)
    {
      Point pointOffset = this.GetPointOffset(offset);
      return new Point(this.Layout.AnchorPoint.X + percent * this.Layout.ScaleVector.X + pointOffset.X, this.Layout.AnchorPoint.Y + percent * this.Layout.ScaleVector.Y + pointOffset.Y);
    }

    public override double? GetValueByPoint(Point point)
    {
      if (this.Layout.IsEmpty)
        return new double?();
      double num = this.Scale.StartValue + (this.IsVerticalScale ? (point.Y - this.Layout.AnchorPoint.Y) / this.Layout.ScaleVector.Y : (point.X - this.Layout.AnchorPoint.X) / this.Layout.ScaleVector.X) * this.Scale.ValuesRange;
      if (MathUtils.IsValueInRange(num, this.Scale.StartValue, this.Scale.EndValue))
        return new double?(num);
      return new double?();
    }
  }
}
