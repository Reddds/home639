// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.ScaleMapping
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges;
using System.Windows;

namespace DevExpress.Xpf.Gauges.Native
{
  public abstract class ScaleMapping
  {
    private readonly Scale scale;
    private readonly ScaleLayout layout;

    protected double ValuesRange
    {
      get
      {
        return this.Scale.ValuesRange;
      }
    }

    public Scale Scale
    {
      get
      {
        return this.scale;
      }
    }

    public ScaleLayout Layout
    {
      get
      {
        return this.layout;
      }
    }

    public ScaleMapping(Scale scale, ScaleLayout layout)
    {
      this.scale = scale;
      this.layout = layout;
    }

    protected abstract double GetAngleByPoint(Point point);

    public double GetAngleByPercent(double percent)
    {
      return this.GetAngleByPoint(this.GetPointByPercent(percent));
    }

    public double GetAngleByValue(double value, bool clamp)
    {
      if (clamp)
        return this.GetAngleByPoint(this.GetPointByValue(this.Scale.GetLimitedValue(value)));
      return this.GetAngleByPoint(this.GetPointByValue(value));
    }

    public double GetAngleByValue(double value)
    {
      return this.GetAngleByValue(value, false);
    }

    public Point GetPointByValue(double value, bool clamp)
    {
      if (clamp)
        return this.GetPointByPercent(this.Scale.GetLimitedValueInPercent(value));
      return this.GetPointByPercent(this.Scale.GetValueInPercent(value));
    }

    public Point GetPointByValue(double value)
    {
      return this.GetPointByValue(value, false);
    }

    public Point GetPointByValue(double value, double offset, bool clamp)
    {
      if (clamp)
        return this.GetPointByPercent(this.Scale.GetValueInPercent(this.Scale.GetLimitedValue(value)), offset);
      return this.GetPointByPercent(this.Scale.GetValueInPercent(value), offset);
    }

    public Point GetPointByValue(double value, double offset)
    {
      return this.GetPointByValue(value, offset, false);
    }

    public abstract Point GetPointByPercent(double percent);

    public abstract Point GetPointByPercent(double percent, double offset);

    public abstract double? GetValueByPoint(Point point);
  }
}
