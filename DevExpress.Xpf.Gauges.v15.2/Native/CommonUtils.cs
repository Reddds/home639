// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.CommonUtils
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace DevExpress.Xpf.Gauges.Native
{
  public static class CommonUtils
  {
    public static Panel GetChildPanel(ItemsControl itemsControl)
    {
      return LayoutHelper.FindElement((FrameworkElement) itemsControl, (Predicate<FrameworkElement>) (element => element is Panel)) as Panel;
    }

    public static void SubscribePropertyChangedWeakEvent(INotifyPropertyChanged oldSource, INotifyPropertyChanged newSource, IWeakEventListener listener)
    {
      if (listener == null)
        return;
      if (oldSource != null)
        PropertyChangedWeakEventManager.RemoveListener(oldSource, listener);
      if (newSource == null)
        return;
      PropertyChangedWeakEventManager.AddListener(newSource, listener);
    }
  }
}
