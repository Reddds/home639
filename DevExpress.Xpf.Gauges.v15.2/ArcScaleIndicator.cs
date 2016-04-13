// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ArcScaleIndicator
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A class that specifies  value indicators of a circular scale.
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class ArcScaleIndicator : ValueIndicatorBase
  {
    protected CircularGaugeControl Gauge
    {
      get
      {
        return base.Gauge as CircularGaugeControl;
      }
    }

    internal ArcScale Scale
    {
      get
      {
        return base.Scale as ArcScale;
      }
    }
  }
}
