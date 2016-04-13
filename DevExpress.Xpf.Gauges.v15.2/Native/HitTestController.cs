// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.HitTestController
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges.Native
{
  public class HitTestController
  {
    private readonly AnalogGaugeControl gauge;
    private List<IHitTestableElement> currentElements;

    public HitTestController(AnalogGaugeControl gauge)
    {
      this.gauge = gauge;
    }

    private void AddUniqueHitTestableElement(IHitTestableElement hitTestableElement)
    {
      if (hitTestableElement == null || this.currentElements.Contains(hitTestableElement))
        return;
      this.currentElements.Add(hitTestableElement);
    }

    private IHitTestableElement GetParentHitTestableElement(DependencyObject obj)
    {
      for (DependencyObject reference = obj; reference != null && !(reference is CircularGaugeControl); reference = VisualTreeHelper.GetParent(reference))
      {
        IHitTestableElement hitTestableElement = reference as IHitTestableElement;
        if (hitTestableElement != null)
          return hitTestableElement;
      }
      return (IHitTestableElement) null;
    }

    private HitTestResultBehavior OnHitTestResult(HitTestResult result)
    {
      this.AddUniqueHitTestableElement(this.GetParentHitTestableElement(result.VisualHit));
      return HitTestResultBehavior.Continue;
    }

    private void PrepareHitTestableElements(Point point)
    {
      VisualTreeHelper.HitTest((Visual) this.gauge, (HitTestFilterCallback) null, new HitTestResultCallback(this.OnHitTestResult), (HitTestParameters) new PointHitTestParameters(point));
    }

    public List<IHitTestableElement> FindElements(Point point)
    {
      this.currentElements = new List<IHitTestableElement>();
      this.PrepareHitTestableElements(point);
      return this.currentElements;
    }
  }
}
