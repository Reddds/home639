// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.GaugeElementCollection`1
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A base class for collections containing gauge elements.
  /// 
  /// 
  /// </para>
  /// 
  /// </summary>
  public class GaugeElementCollection<T> : ObservableCollection<T>, IOwnedElement
  {
    private object owner;
    private ILogicalParent logicalParent;

    protected object Owner
    {
      get
      {
        return this.owner;
      }
    }

    object IOwnedElement.Owner
    {
      get
      {
        return this.owner;
      }
      set
      {
        this.RemoveChildren((IList) this);
        this.owner = value;
        this.logicalParent = this.owner as ILogicalParent;
        this.AddChildren((IList) this);
      }
    }

    private void SetOwnerForChildren(IList children, object owner)
    {
      foreach (object obj in (IEnumerable) children)
      {
        IOwnedElement ownedElement = obj as IOwnedElement;
        if (ownedElement != null)
          ownedElement.Owner = owner;
      }
    }

    private void AddChildren(IList children)
    {
      this.SetOwnerForChildren(children, this.owner);
      if (this.logicalParent == null)
        return;
      foreach (object child in (IEnumerable) children)
      {
        if (child != null)
          this.logicalParent.AddChild(child);
      }
    }

    private void RemoveChildren(IList children)
    {
      this.SetOwnerForChildren(children, (object) null);
      if (this.logicalParent == null)
        return;
      foreach (object child in (IEnumerable) children)
      {
        if (child != null)
          this.logicalParent.RemoveChild(child);
      }
    }

    protected override void ClearItems()
    {
      this.RemoveChildren((IList) this);
      base.ClearItems();
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      base.OnCollectionChanged(e);
      if (e.OldItems != null && (e.Action == NotifyCollectionChangedAction.Reset || e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace))
        this.RemoveChildren(e.OldItems);
      if (e.NewItems == null || e.Action != NotifyCollectionChangedAction.Add && e.Action != NotifyCollectionChangedAction.Replace)
        return;
      this.AddChildren(e.NewItems);
    }
  }
}
