// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.CircularGaugeLayerCollection
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A collection that stores the layers of a particular circular gauge.
  /// 
  /// 
  /// 
  /// </para>
  /// 
  /// </summary>
  public class CircularGaugeLayerCollection : GaugeLayerCollection<CircularGaugeLayer>
  {
    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the CircularGaugeLayerCollection class with the specified owner.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// <param name="gauge">A <see cref="T:DevExpress.Xpf.Gauges.CircularGaugeControl"/> object that should be the owner of the created collection.
    /// 
    ///             </param>
    public CircularGaugeLayerCollection(CircularGaugeControl gauge)
      : base((GaugeControlBase) gauge)
    {
    }
  }
}
