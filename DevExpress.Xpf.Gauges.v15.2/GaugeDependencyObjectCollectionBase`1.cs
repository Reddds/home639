// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.GaugeDependencyObjectCollectionBase`1
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;
using System.Collections.Specialized;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A base class for all collections in the DXGauges Suite.
  /// 
  /// </para>
  /// 
  /// </summary>
  [NonCategorized]
  public abstract class GaugeDependencyObjectCollectionBase<T> : FreezableCollection<T> where T : DependencyObject
  {
    /// <summary>
    /// 
    /// <para>
    /// Provides indexed access to individual items in the collection.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// <param name="index">A zero-based integer specifying the desired item's position within the collection. If it's negative or exceeds the last available index, an exception is raised.
    /// 
    ///             </param>
    /// <value>
    /// An object which represents an item at the specified position.
    /// 
    /// 
    /// </value>
    public new T this[int index]
    {
      get
      {
        return base[index];
      }
      set
      {
        this.SetItem(index, value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the GaugeDependencyObjectCollectionBase class with default settings.
    /// 
    /// </para>
    /// 
    /// </summary>
    public GaugeDependencyObjectCollectionBase()
    {
      this.CollectionChanged += new NotifyCollectionChangedEventHandler(this.BaseCollectionChanged);
    }

    private void BaseCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      this.OnCollectionChanged(e);
    }

    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
    }

    protected virtual void ClearItems()
    {
      base.Clear();
    }

    protected virtual void InsertItem(int index, T item)
    {
      base.Insert(index, item);
    }

    protected virtual void MoveItem(int oldIndex, int newIndex)
    {
      T obj = this[oldIndex];
      base.RemoveAt(oldIndex);
      base.Insert(newIndex, obj);
    }

    protected virtual void RemoveItem(int index)
    {
      base.RemoveAt(index);
    }

    protected virtual void SetItem(int index, T item)
    {
      base[index] = item;
    }

    public new void Add(T item)
    {
      this.InsertItem(this.Count, item);
    }

    public new void Insert(int index, T item)
    {
      this.InsertItem(index, item);
    }

    public new bool Remove(T item)
    {
      int index = this.IndexOf(item);
      if (index < 0)
        return false;
      this.RemoveItem(index);
      return true;
    }

    /// <summary>
    /// 
    /// <para>
    /// Removes an item at the specified position from the collection.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// <param name="index">A zero-based integer specifying the index of the object to remove. If it's negative or exceeds the number of elements, an exception is raised.
    /// 
    ///             </param>
    public new void RemoveAt(int index)
    {
      this.RemoveItem(index);
    }

    /// <summary>
    /// 
    /// <para>
    /// Removes all items from the collection.
    /// 
    /// </para>
    /// 
    /// </summary>
    public new void Clear()
    {
      this.ClearItems();
    }

    /// <summary>
    /// 
    /// <para>
    /// Moves a specific item to another position within the collection.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// <param name="oldIndex">An integer value specifying the zero-based index of an item to be moved.
    /// 
    ///             </param><param name="newIndex">An integer value specifying the zero-based destination index of the moved item.
    /// 
    ///             </param>
    public void Move(int oldIndex, int newIndex)
    {
      this.MoveItem(oldIndex, newIndex);
    }
  }
}
