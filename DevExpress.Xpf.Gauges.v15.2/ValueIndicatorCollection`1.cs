// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ValueIndicatorCollection`1
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Collections;
using System.Collections.Specialized;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A collection that stores the value indicators of a particular scale.
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class ValueIndicatorCollection<T> : GaugeDependencyObjectCollection<T> where T : ValueIndicatorBase
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
    /// Initializes a new instance of the ValueIndicatorCollection class with the specified owner.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// <param name="scale">A <see cref="T:DevExpress.Xpf.Gauges.Scale"/> class descendant that should be the owner of the created collection.
    /// 
    ///             </param>
    public ValueIndicatorCollection(Scale scale)
    {
      this.Owner = (object) scale;
    }

    protected override void ClearItems()
    {
      if (this.Scale != null)
      {
        foreach (T obj in (FreezableCollection<T>) this)
          this.Scale.RemoveStoryboard(obj.GetHashCode());
      }
      base.ClearItems();
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      base.OnCollectionChanged(e);
      if (this.Scale == null)
        return;
      if ((e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace) && e.OldItems != null)
      {
        foreach (object obj in (IEnumerable) e.OldItems)
          this.Scale.RemoveStoryboard(obj.GetHashCode());
      }
      if (this.Scale.Gauge == null)
        return;
      this.Scale.Gauge.UpdateElements();
    }
  }
}
