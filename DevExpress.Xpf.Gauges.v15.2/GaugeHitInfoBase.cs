// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.GaugeHitInfoBase
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using System;
using System.Collections.Generic;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A base class for classes that contains information about a specific point within a gauge.
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class GaugeHitInfoBase
  {
    private readonly List<IHitTestableElement> hitTestableElements = new List<IHitTestableElement>();
    private readonly AnalogGaugeControl gauge;

    internal GaugeHitInfoBase(AnalogGaugeControl gauge, Point point)
    {
      this.gauge = gauge;
      this.hitTestableElements = new HitTestController(gauge).FindElements(point);
    }

    protected object GetElementByType(Type elementType)
    {
      foreach (IHitTestableElement hitTestableElement in this.hitTestableElements)
      {
        if (hitTestableElement.IsHitTestVisible && hitTestableElement.Element != null && elementType.IsAssignableFrom(hitTestableElement.Element.GetType()))
          return hitTestableElement.Element;
      }
      return (object) null;
    }

    protected object GetElementParentByType(Type elementType)
    {
      object obj = (object) null;
      foreach (IHitTestableElement hitTestableElement in this.hitTestableElements)
      {
        if (hitTestableElement.IsHitTestVisible && hitTestableElement.Parent != null && elementType.IsAssignableFrom(hitTestableElement.Parent.GetType()))
        {
          obj = hitTestableElement.Parent;
          break;
        }
      }
      return obj ?? this.GetElementByType(elementType);
    }
  }
}
