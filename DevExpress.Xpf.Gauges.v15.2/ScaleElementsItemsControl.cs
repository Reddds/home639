﻿// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ScaleElementsItemsControl
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows;
using System.Windows.Controls;

namespace DevExpress.Xpf.Gauges
{
  public class ScaleElementsItemsControl : ItemsControl
  {
    protected override DependencyObject GetContainerForItemOverride()
    {
      return (DependencyObject) new ScaleElementInfoContainer();
    }

    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
      base.PrepareContainerForItemOverride(element, item);
      ScaleElementInfoContainer elementInfoContainer = element as ScaleElementInfoContainer;
      if (elementInfoContainer == null)
        return;
      elementInfoContainer.ElementInfo = item as ScaleElementInfoBase;
    }

    protected override bool IsItemItsOwnContainerOverride(object item)
    {
      return item is UIElement;
    }
  }
}
