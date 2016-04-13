// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LayerCollection`1
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Collections.Specialized;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A collection that stores the layers of a particular scale.
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class LayerCollection<T> : GaugeDependencyObjectCollection<T> where T : LayerBase
  {
    private Scale Scale
    {
      get
      {
        return this.Owner as Scale;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the LayerCollection class with the specified owner.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// <param name="scale">A <see cref="T:DevExpress.Xpf.Gauges.Scale"/> class descendant that should be the owner of the created collection.
    /// 
    ///             </param>
    public LayerCollection(Scale scale)
    {
      this.Owner = (object) scale;
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      base.OnCollectionChanged(e);
      if (this.Scale == null || this.Scale.Gauge == null)
        return;
      this.Scale.Gauge.UpdateElements();
    }
  }
}
