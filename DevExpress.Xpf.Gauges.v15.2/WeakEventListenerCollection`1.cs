// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.WeakEventListenerCollection`1
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  public abstract class WeakEventListenerCollection<T> : GaugeDependencyObjectCollection<T>, IWeakEventListener where T : DependencyObject
  {
    bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
    {
      return this.PerformWeakEvent(managerType, sender, e);
    }

    protected abstract bool PerformWeakEvent(Type managerType, object sender, EventArgs e);

    protected override void AddChildren(IList children)
    {
      base.AddChildren(children);
      foreach (object obj in (IEnumerable) children)
      {
        if (obj is INotifyPropertyChanged)
          PropertyChangedWeakEventManager.AddListener(obj as INotifyPropertyChanged, (IWeakEventListener) this);
      }
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
  }
}
