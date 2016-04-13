// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.GaugeLayerCollection`1
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Collections.Specialized;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A base class for collections containing gauge layers.
  /// 
  /// 
  /// </para>
  /// 
  /// </summary>
  public class GaugeLayerCollection<T> : GaugeDependencyObjectCollection<T> where T : GaugeLayerBase
  {
    private GaugeControlBase Gauge
    {
      get
      {
        return this.Owner as GaugeControlBase;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the GaugeLayerCollection class with the specified owner.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// <param name="gauge">A <see cref="T:DevExpress.Xpf.Gauges.GaugeControlBase"/> class descendant that should be the owner of the created collection.
    /// 
    ///             </param>
    public GaugeLayerCollection(GaugeControlBase gauge)
    {
      this.Owner = (object) gauge;
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      base.OnCollectionChanged(e);
      if (this.Gauge == null)
        return;
      this.Gauge.UpdateElements();
    }
  }
}
