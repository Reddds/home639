// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.ArcScaleMapping
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges;
using System;
using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges.Native
{
  public class ArcScaleMapping : ScaleMapping
  {
    public double AnglesRange
    {
      get
      {
        return this.Scale.EndAngle - this.Scale.StartAngle;
      }
    }

    public ArcScale Scale
    {
      get
      {
        return base.Scale as ArcScale;
      }
    }

    public ArcScaleLayout Layout
    {
      get
      {
        return base.Layout as ArcScaleLayout;
      }
    }

    public ArcScaleMapping(ArcScale scale, Rect bounds)
      : base((DevExpress.Xpf.Gauges.Scale) scale, (ScaleLayout) new ArcScaleLayoutCalculator(scale, bounds).Calculate())
    {
    }

    private double GetSecondaryAlpha(double primaryAlpha, double offset)
    {
      double num1 = this.Layout.EllipseWidth / 2.0;
      double num2 = this.Layout.EllipseHeight / 2.0;
      return Math.Atan2(num2 * (num1 + offset) * Math.Sin(primaryAlpha), num1 * (num2 + offset) * Math.Cos(primaryAlpha));
    }

    protected override double GetAngleByPoint(Point point)
    {
      return MathUtils.Radian2Degree(MathUtils.CalculateBetweenPointsAngle(this.Layout.EllipseCenter, point));
    }

    public double GetAlphaByPercent(double percent)
    {
      return MathUtils.Degree2Radian(this.Scale.StartAngle + percent * this.AnglesRange);
    }

    public Point GetPointByAlpha(double alpha)
    {
      return MathUtils.CalculateEllipsePoint(this.Layout.EllipseCenter, this.Layout.EllipseWidth, this.Layout.EllipseHeight, alpha);
    }

    public Point GetPointByAlpha(double alpha, double offset)
    {
      double secondaryAlpha = this.GetSecondaryAlpha(alpha, offset);
      return MathUtils.CalculateEllipsePoint(this.Layout.EllipseCenter, this.Layout.EllipseWidth + 2.0 * offset, this.Layout.EllipseHeight + 2.0 * offset, secondaryAlpha);
    }

    public override Point GetPointByPercent(double percent)
    {
      return this.GetPointByAlpha(this.GetAlphaByPercent(percent));
    }

    public override Point GetPointByPercent(double percent, double offset)
    {
      return this.GetPointByAlpha(this.GetAlphaByPercent(percent), offset);
    }

    public override double? GetValueByPoint(Point point)
    {
      if (this.Layout.IsEmpty || point == this.Layout.EllipseCenter)
        return new double?();
      double num1 = point.X - this.Layout.EllipseCenter.X;
      double num2 = point.Y - this.Layout.EllipseCenter.Y;
      SweepDirection sweepDirection = this.Scale.StartAngle < this.Scale.EndAngle ? SweepDirection.Clockwise : SweepDirection.Counterclockwise;
      double num3 = MathUtils.Radian2Degree(MathUtils.NormalizeRadian(Math.Atan2(0.5 * this.Layout.EllipseWidth * num2, 0.5 * this.Layout.EllipseHeight * num1)));
      double num4 = MathUtils.NormalizeDegree(this.Scale.StartAngle);
      double num5 = this.Scale.StartValue + (sweepDirection != SweepDirection.Clockwise ? (num3 > num4 ? (360.0 - num3 + num4) / (this.Scale.StartAngle - this.Scale.EndAngle) : (num4 - num3) / (this.Scale.StartAngle - this.Scale.EndAngle)) : (num3 > num4 ? (num3 - num4) / (this.Scale.EndAngle - this.Scale.StartAngle) : (360.0 - num4 + num3) / (this.Scale.EndAngle - this.Scale.StartAngle))) * (this.Scale.EndValue - this.Scale.StartValue);
      if (MathUtils.IsValueInRange(num5, this.Scale.StartValue, this.Scale.EndValue))
        return new double?(num5);
      return new double?();
    }
  }
}
