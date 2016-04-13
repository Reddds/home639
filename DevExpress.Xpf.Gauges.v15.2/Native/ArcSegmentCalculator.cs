// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.ArcSegmentCalculator
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges;
using System;
using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges.Native
{
  public static class ArcSegmentCalculator
  {
    private static Geometry CalculateClipGeometry(ElementInfoBase elmentInfo, ArcScaleMapping mapping, double startValue, double endValue, double offset, int thickness)
    {
      double startAlpha = MathUtils.Radian2Degree(mapping.GetAlphaByPercent(mapping.Scale.GetValueInPercent(startValue)));
      double sweepAngle = MathUtils.Radian2Degree(mapping.GetAlphaByPercent(mapping.Scale.GetValueInPercent(endValue))) - startAlpha;
      Geometry geometry = ArcSegmentCalculator.CalculateGeometry(mapping, startAlpha, sweepAngle, offset, thickness);
      geometry.Transform = (Transform) new TranslateTransform()
      {
        X = (elmentInfo.Layout.Width.Value / 2.0 - mapping.Layout.EllipseCenter.X),
        Y = (elmentInfo.Layout.Height.Value / 2.0 - mapping.Layout.EllipseCenter.Y)
      };
      return geometry;
    }

    public static ElementLayout CreateRangeLayout(ArcScaleMapping mapping, double offset, int thickness)
    {
      return new ElementLayout(Math.Max(mapping.Layout.EllipseWidth + offset * 2.0 + (double) thickness, 0.0), Math.Max(mapping.Layout.EllipseHeight + offset * 2.0 + (double) thickness, 0.0));
    }

    public static void CompleteRangeLayout(ElementInfoBase elementInfo, ArcScale scale, double startValue, double endValue, double offset, int thickness)
    {
      Point ellipseCenter = scale.Mapping.Layout.EllipseCenter;
      Point layoutOffset = scale.GetLayoutOffset();
      ellipseCenter.X += layoutOffset.X;
      ellipseCenter.Y += layoutOffset.Y;
      elementInfo.Layout.CompleteLayout(ellipseCenter, (Transform) null, ArcSegmentCalculator.CalculateClipGeometry(elementInfo, scale.Mapping, startValue, endValue, offset, thickness));
    }

    public static Geometry CalculateGeometry(ArcScaleMapping mapping, double startAlpha, double sweepAngle, double offset, int thickness)
    {
      double width1 = Math.Max(0.5 * mapping.Layout.EllipseWidth + offset + Math.Floor((double) thickness / 2.0), 0.0);
      double height1 = Math.Max(0.5 * mapping.Layout.EllipseHeight + offset + Math.Floor((double) thickness / 2.0), 0.0);
      double width2 = Math.Max(0.5 * mapping.Layout.EllipseWidth + offset - Math.Ceiling((double) thickness / 2.0), 0.0);
      double height2 = Math.Max(0.5 * mapping.Layout.EllipseHeight + offset - Math.Ceiling((double) thickness / 2.0), 0.0);
      Geometry geometry;
      if (Math.Abs(sweepAngle) < 360.0)
      {
        Point pointByAlpha1 = mapping.GetPointByAlpha(MathUtils.Degree2Radian(startAlpha), offset + Math.Floor((double) thickness / 2.0));
        Point pointByAlpha2 = mapping.GetPointByAlpha(MathUtils.Degree2Radian(startAlpha + sweepAngle), offset + Math.Floor((double) thickness / 2.0));
        Point pointByAlpha3 = mapping.GetPointByAlpha(MathUtils.Degree2Radian(startAlpha), offset - Math.Ceiling((double) thickness / 2.0));
        Point pointByAlpha4 = mapping.GetPointByAlpha(MathUtils.Degree2Radian(startAlpha + sweepAngle), offset - Math.Ceiling((double) thickness / 2.0));
        PathFigure pathFigure = new PathFigure()
        {
          StartPoint = pointByAlpha3
        };
        pathFigure.Segments.Add((PathSegment) new LineSegment()
        {
          Point = pointByAlpha1
        });
        pathFigure.Segments.Add((PathSegment) new ArcSegment()
        {
          Point = pointByAlpha2,
          Size = new Size(width1, height1),
          IsLargeArc = (sweepAngle > 180.0 || sweepAngle < -180.0),
          SweepDirection = (sweepAngle > 0.0 ? SweepDirection.Clockwise : SweepDirection.Counterclockwise)
        });
        pathFigure.Segments.Add((PathSegment) new LineSegment()
        {
          Point = pointByAlpha4
        });
        pathFigure.Segments.Add((PathSegment) new ArcSegment()
        {
          Point = pointByAlpha3,
          Size = new Size(width2, height2),
          IsLargeArc = (sweepAngle > 180.0 || sweepAngle < -180.0),
          SweepDirection = (sweepAngle < 0.0 ? SweepDirection.Clockwise : SweepDirection.Counterclockwise)
        });
        geometry = (Geometry) new PathGeometry();
        ((PathGeometry) geometry).Figures.Add(pathFigure);
      }
      else
      {
        geometry = (Geometry) new GeometryGroup()
        {
          FillRule = FillRule.EvenOdd
        };
        ((GeometryGroup) geometry).Children.Add((Geometry) new EllipseGeometry()
        {
          Center = mapping.Layout.EllipseCenter,
          RadiusX = width2,
          RadiusY = height2
        });
        ((GeometryGroup) geometry).Children.Add((Geometry) new EllipseGeometry()
        {
          Center = mapping.Layout.EllipseCenter,
          RadiusX = width1,
          RadiusY = height1
        });
      }
      return geometry;
    }
  }
}
