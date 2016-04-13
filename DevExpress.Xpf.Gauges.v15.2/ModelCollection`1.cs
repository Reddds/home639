// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ModelCollection`1
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  public abstract class ModelCollection<T> : GaugeDependencyObjectCollection<T>, INotifyPropertyChanged, IWeakEventListener where T : ModelBase
  {
    public event PropertyChangedEventHandler PropertyChanged;

    bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
    {
      return this.PerformWeakEvent(managerType, sender, e);
    }

    private bool PerformWeakEvent(Type managerType, object sender, EventArgs e)
    {
      bool flag = false;
      if (managerType == typeof (PropertyChangedWeakEventManager))
      {
        if (sender is ModelBase)
          this.NotifyPropertyChanged(sender, e);
        flag = true;
      }
      return flag;
    }

    protected override void RemoveChildren(IList children)
    {
      base.RemoveChildren(children);
      foreach (object obj in (IEnumerable) children)
      {
        if (obj is INotifyPropertyChanged)
          PropertyChangedWeakEventManager.RemoveListener(obj as INotifyPropertyChanged, (IWeakEventListener) this);
      }
    }

    protected override void AddChildren(IList children)
    {
      base.AddChildren(children);
      foreach (object obj in (IEnumerable) children)
      {
        if (obj is INotifyPropertyChanged)
          PropertyChangedWeakEventManager.AddListener(obj as INotifyPropertyChanged, (IWeakEventListener) this);
      }
    }

    protected void NotifyPropertyChanged(object sender, EventArgs e)
    {
      if (this.PropertyChanged == null || !(e is PropertyChangedEventArgs))
        return;
      this.PropertyChanged(sender, e as PropertyChangedEventArgs);
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
      if (e.NewItems != null && (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace))
        this.AddChildren(e.NewItems);
      this.NotifyPropertyChanged((object) this, (EventArgs) new PropertyChangedEventArgs("Collection"));
    }
  }
}
