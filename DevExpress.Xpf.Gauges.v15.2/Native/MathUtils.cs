// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.MathUtils
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System;
using System.Windows;

namespace DevExpress.Xpf.Gauges.Native
{
  public static class MathUtils
  {
    public static bool IsValueInRange(double value, double edge1, double edge2)
    {
      if (value >= Math.Min(edge1, edge2))
        return value <= Math.Max(edge1, edge2);
      return false;
    }

    public static double StrongRound(double value)
    {
      return (double) Math.Sign(value) * (double) (int) (Math.Abs(value) + 0.5);
    }

    public static Point StrongRound(Point value)
    {
      return new Point(MathUtils.StrongRound(value.X), MathUtils.StrongRound(value.Y));
    }

    public static double Degree2Radian(double angleDegree)
    {
      return angleDegree * Math.PI / 180.0;
    }

    public static double Radian2Degree(double angleRadian)
    {
      return angleRadian * 180.0 / Math.PI;
    }

    public static double NormalizeRadian(double angleRadian)
    {
      int num = (int) (0.5 * angleRadian / Math.PI);
      if (angleRadian >= 0.0)
        return angleRadian - (double) num * Math.PI * 2.0;
      return 2.0 * Math.PI + angleRadian - (double) num * Math.PI * 2.0;
    }

    public static double NormalizeDegree(double angleDegree)
    {
      return MathUtils.Radian2Degree(MathUtils.NormalizeRadian(MathUtils.Degree2Radian(angleDegree)));
    }

    public static Point CalculateCenter(Rect rect)
    {
      return new Point(0.5 * (rect.Left + rect.Right), 0.5 * (rect.Top + rect.Bottom));
    }

    public static double CalculateBetweenPointsAngle(Point p1, Point p2)
    {
      return MathUtils.NormalizeRadian(Math.Atan2(p2.Y - p1.Y, p2.X - p1.X));
    }

    public static Point CorrectLocationByTransformOrigin(Point location, Point transformOrigin, Size size)
    {
      return new Point(location.X - transformOrigin.X * size.Width, location.Y - transformOrigin.Y * size.Height);
    }

    public static double CalculateDistance(Point startPoint, Point endPoint)
    {
      return Math.Sqrt(Math.Pow(endPoint.X - startPoint.X, 2.0) + Math.Pow(endPoint.Y - startPoint.Y, 2.0));
    }

    public static Point CalculateEllipsePoint(Point ellipseCenter, double ellipseWidth, double ellipseHeight, double alpha)
    {
      if (ellipseWidth <= 0.0 || ellipseHeight <= 0.0)
        return ellipseCenter;
      return new Point(ellipseCenter.X + 0.5 * ellipseWidth * Math.Cos(alpha), ellipseCenter.Y + 0.5 * ellipseHeight * Math.Sin(alpha));
    }
  }
}
