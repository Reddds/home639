// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.ArcScaleLayout
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges.Native
{
  public class ArcScaleLayout : ScaleLayout
  {
    private readonly double ellipseHeight;
    private readonly double ellipseWidth;
    private readonly Point ellipseCenter;

    public double EllipseHeight
    {
      get
      {
        return this.ellipseHeight;
      }
    }

    public double EllipseWidth
    {
      get
      {
        return this.ellipseWidth;
      }
    }

    public Point EllipseCenter
    {
      get
      {
        return this.ellipseCenter;
      }
    }

    public override bool IsEmpty
    {
      get
      {
        if (this.ellipseHeight > 0.0)
          return this.ellipseWidth <= 0.0;
        return true;
      }
    }

    public override Geometry Clip
    {
      get
      {
        if (this.IsEmpty)
          return (Geometry) null;
        return (Geometry) new EllipseGeometry()
        {
          Center = this.EllipseCenter,
          RadiusX = (0.5 * this.EllipseWidth),
          RadiusY = (0.5 * this.EllipseHeight)
        };
      }
    }

    public ArcScaleLayout(Rect initialBounds, Point ellipseCenter, double ellipseWidth, double ellipseHeight)
      : base(initialBounds)
    {
      this.ellipseCenter = ellipseCenter;
      this.ellipseWidth = ellipseWidth;
      this.ellipseHeight = ellipseHeight;
    }
  }
}
