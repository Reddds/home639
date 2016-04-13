// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ScaleElementLayout
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class ScaleElementLayout
  {
    private readonly Point scaleFactor;
    private readonly double angle;
    private readonly Point anchorPoint;
    private readonly Geometry clipGeometry;
    private readonly System.Windows.Size? size;

    public double Angle
    {
      get
      {
        return this.angle;
      }
    }

    public Point AnchorPoint
    {
      get
      {
        return this.anchorPoint;
      }
    }

    public Point ScaleFactor
    {
      get
      {
        return this.scaleFactor;
      }
    }

    public Geometry ClipGeometry
    {
      get
      {
        return this.clipGeometry;
      }
    }

    public System.Windows.Size? Size
    {
      get
      {
        return this.size;
      }
    }

    public ScaleElementLayout(double angle, Point anchorPoint, Point scaleFactor, Geometry clipGeometry, System.Windows.Size? size)
    {
      this.angle = angle;
      this.anchorPoint = anchorPoint;
      this.scaleFactor = scaleFactor;
      this.clipGeometry = clipGeometry;
      this.size = size;
    }

    public ScaleElementLayout(double angle, Point anchorPoint, Point scaleFactor)
      : this(angle, anchorPoint, scaleFactor, (Geometry) null, new System.Windows.Size?())
    {
    }

    public ScaleElementLayout(double angle, Point anchorPoint)
      : this(angle, anchorPoint, new Point(1.0, 1.0))
    {
    }

    public ScaleElementLayout(double angle, Point anchorPoint, System.Windows.Size size)
      : this(angle, anchorPoint, new Point(1.0, 1.0), (Geometry) null, new System.Windows.Size?(size))
    {
    }

    public ScaleElementLayout(Point anchorPoint, System.Windows.Size size, Geometry clipGeometry)
      : this(0.0, anchorPoint, new Point(1.0, 1.0), clipGeometry, new System.Windows.Size?(size))
    {
    }
  }
}
