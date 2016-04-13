// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.NavigationController
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace DevExpress.Xpf.Gauges.Native
{
  public class NavigationController
  {
    private readonly AnalogGaugeControl gauge;
    private readonly HitTestController hitTestController;
    private bool draggingInProcess;
    private object selectedObject;

    private Scale Scale
    {
      get
      {
        return this.selectedObject as Scale;
      }
    }

    private ValueIndicatorBase Indicator
    {
      get
      {
        return this.selectedObject as ValueIndicatorBase;
      }
    }

    public NavigationController(AnalogGaugeControl gauge)
    {
      this.gauge = gauge;
      this.hitTestController = new HitTestController(gauge);
    }

    private void SelectObject(Point point)
    {
      foreach (IHitTestableElement hitTestableElement in this.hitTestController.FindElements(point))
      {
        if (hitTestableElement.Element is Scale || hitTestableElement.Element is ValueIndicatorBase)
        {
          this.selectedObject = hitTestableElement.Element;
          break;
        }
      }
    }

    private void ResetSelection()
    {
      this.gauge.ReleaseMouseCapture();
      this.draggingInProcess = false;
      this.selectedObject = (object) null;
    }

    private void StartDragging()
    {
      Mouse.Capture((IInputElement) this.gauge, CaptureMode.SubTree);
      this.draggingInProcess = true;
    }

    private void MoveIndicatorsToPoint(Scale scale, MouseEventArgs e)
    {
      foreach (ValueIndicatorBase indicator in scale.Indicators)
      {
        if (indicator.IsInteractive)
          this.MoveIndicatorToPoint(indicator, false, e);
      }
    }

    private void MoveIndicator(ValueIndicatorBase indicator, bool shouldLockAnimation, double? value)
    {
      if (!value.HasValue)
        return;
      try
      {
        if (shouldLockAnimation)
          indicator.IsLockedAnimation = true;
        indicator.SetValue(ValueIndicatorBase.ValueProperty, (object) value.Value);
      }
      finally
      {
        indicator.IsLockedAnimation = false;
      }
    }

    private void MoveIndicatorToPoint(ValueIndicatorBase indicator, bool shouldLockAnimation, MouseEventArgs e)
    {
      double? valueByMousePosition = indicator.Scale.GetValueByMousePosition(e);
      this.MoveIndicator(indicator, shouldLockAnimation, valueByMousePosition);
    }

    private List<IManipulator> ManipulatorsList(IEnumerable<IManipulator> collection)
    {
      List<IManipulator> list = new List<IManipulator>();
      foreach (IManipulator manipulator in collection)
        list.Add(manipulator);
      return list;
    }

    public void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      this.ResetSelection();
      this.SelectObject(e.GetPosition((IInputElement) this.gauge));
      if (this.Indicator == null || !this.Indicator.IsInteractive)
        return;
      this.StartDragging();
      e.Handled = true;
    }

    public void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (!this.draggingInProcess)
      {
        this.SelectObject(e.GetPosition((IInputElement) this.gauge));
        if (this.Scale != null)
          this.MoveIndicatorsToPoint(this.Scale, (MouseEventArgs) e);
      }
      this.ResetSelection();
    }

    public void MouseMove(object sender, MouseEventArgs e)
    {
      if (!this.draggingInProcess)
        return;
      e.Handled = true;
      this.MoveIndicatorToPoint(this.Indicator, true, e);
    }

    public void MouseLeave(object sender, MouseEventArgs e)
    {
      this.ResetSelection();
    }

    public void ManipulationStarted(object sender, ManipulationStartedEventArgs e)
    {
      List<IManipulator> list = this.ManipulatorsList(e.Manipulators);
      if (list.Count != 1)
        return;
      this.ResetSelection();
      this.SelectObject(list[0].GetPosition((IInputElement) this.gauge));
      if (this.Indicator == null || !this.Indicator.IsInteractive)
        return;
      this.StartDragging();
      e.Handled = true;
    }

    public void ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
    {
      if (!this.draggingInProcess)
        return;
      List<IManipulator> list = this.ManipulatorsList(e.Manipulators);
      if (list.Count == 1)
        this.MoveIndicator(this.Indicator, true, this.Indicator.Scale.GetValueByManipulatorPosition(list[0]));
      e.Handled = true;
    }

    public void ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
    {
      this.ResetSelection();
    }
  }
}
