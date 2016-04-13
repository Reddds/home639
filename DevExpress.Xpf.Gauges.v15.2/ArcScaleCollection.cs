// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ArcScaleCollection
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A collection of arc scales.
  /// 
  /// 
  /// </para>
  /// 
  /// </summary>
  public class ArcScaleCollection : ScaleCollection<ArcScale>
  {
    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the ArcScaleCollection class with the specified owner.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// <param name="gauge">A <see cref="T:DevExpress.Xpf.Gauges.CircularGaugeControl"/> object that should be the owner of the created collection.
    /// 
    ///             </param>
    public ArcScaleCollection(CircularGaugeControl gauge)
      : base((AnalogGaugeControl) gauge)
    {
    }
  }
}
