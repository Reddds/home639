// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ElementLayout
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;
using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  [NonCategorized]
  public class ElementLayout
  {
    private double? height = new double?();
    private double? width = new double?();
    private Transform renderTransform;
    private Point anchorPoint;
    private Geometry clipGeometry;

    public Point AnchorPoint
    {
      get
      {
        return this.anchorPoint;
      }
    }

    public Transform RenderTransform
    {
      get
      {
        return this.renderTransform;
      }
    }

    public Geometry ClipGeometry
    {
      get
      {
        return this.clipGeometry;
      }
    }

    public double? Height
    {
      get
      {
        return this.height;
      }
    }

    public double? Width
    {
      get
      {
        return this.width;
      }
    }

    internal ElementLayout()
    {
    }

    internal ElementLayout(double width)
    {
      this.width = new double?(width);
    }

    internal ElementLayout(double width, double height)
      : this(width)
    {
      this.height = new double?(height);
    }

    internal void CompleteLayout(Point location, Transform transform, Geometry clipGeometry)
    {
      this.anchorPoint = location;
      this.renderTransform = transform;
      this.clipGeometry = clipGeometry;
    }
  }
}
