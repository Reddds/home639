// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.AnalogGaugeControl
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A base class for all analog gauges shipped in the DXGauge Suite.
  /// 
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class AnalogGaugeControl : GaugeControlBase, ILogicalParent
  {
    /// <summary>
    /// 
    /// <para>
    /// Identifies the  dependency property.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <returns/>
    public static readonly DependencyProperty ScalePanelTemplateProperty = DependencyPropertyManager.Register("ScalePanelTemplate", typeof (ItemsPanelTemplate), typeof (AnalogGaugeControl), new PropertyMetadata());
    /// <summary>
    /// 
    /// <para>
    /// Identifies the  attached property.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <returns/>
    public static readonly DependencyProperty ValueIndicatorProperty = DependencyPropertyManager.RegisterAttached("ValueIndicator", typeof (ValueIndicatorBase), typeof (AnalogGaugeControl), new PropertyMetadata((object) null, new PropertyChangedCallback(StateIndicatorControl.ValueIndicatorPropertyChanged)));
    private readonly List<object> logicalChildren = new List<object>();
    private readonly NavigationController navigationController;

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a panel template that specifies how to arrange scales within a Gauge control.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Windows.Controls.ItemsPanelTemplate"/>  object that is a panel template.
    /// 
    /// 
    /// 
    /// </value>
    [Category("Presentation")]
    public ItemsPanelTemplate ScalePanelTemplate
    {
      get
      {
        return (ItemsPanelTemplate) this.GetValue(AnalogGaugeControl.ScalePanelTemplateProperty);
      }
      set
      {
        this.SetValue(AnalogGaugeControl.ScalePanelTemplateProperty, (object) value);
      }
    }

    protected override IEnumerator LogicalChildren
    {
      get
      {
        return (IEnumerator) this.logicalChildren.GetEnumerator();
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the AnalogGaugeControl class with default settings.
    /// 
    /// </para>
    /// 
    /// </summary>
    public AnalogGaugeControl()
    {
      this.navigationController = new NavigationController(this);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(this.navigationController.MouseLeftButtonUp);
      this.MouseLeftButtonDown += new MouseButtonEventHandler(this.navigationController.MouseLeftButtonDown);
      this.MouseMove += new MouseEventHandler(this.navigationController.MouseMove);
      this.MouseLeave += new MouseEventHandler(this.navigationController.MouseLeave);
      this.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(this.navigationController.ManipulationStarted);
      this.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(this.navigationController.ManipulationDelta);
      this.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(this.navigationController.ManipulationCompleted);
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets the value of the <see cref="P:DevExpress.Xpf.Gauges.AnalogGaugeControl.ValueIndicator"/> attached property for the specified  <see cref="T:DevExpress.Xpf.Gauges.StateIndicatorControl"/>.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// <param name="stateControl">A <see cref="T:DevExpress.Xpf.Gauges.StateIndicatorControl"/> object whose <see cref="P:DevExpress.Xpf.Gauges.AnalogGaugeControl.ValueIndicator"/> property's value is to be returned.
    /// 
    ///             </param>
    /// <returns>
    /// The value of the <see cref="P:DevExpress.Xpf.Gauges.AnalogGaugeControl.ValueIndicator"/> property for the specified state indicator.
    /// 
    /// </returns>
    [Category("Common")]
    public static ValueIndicatorBase GetValueIndicator(StateIndicatorControl stateControl)
    {
      return (ValueIndicatorBase) stateControl.GetValue(AnalogGaugeControl.ValueIndicatorProperty);
    }

    /// <summary>
    /// 
    /// <para>
    /// Sets the value of the <see cref="P:DevExpress.Xpf.Gauges.AnalogGaugeControl.ValueIndicator"/> attached property for the specified <see cref="T:DevExpress.Xpf.Gauges.StateIndicatorControl"/>.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// <param name="stateControl">The state indicator control from which the property value is read.
    /// 
    ///             </param><param name="value">The required <see cref="T:DevExpress.Xpf.Gauges.ValueIndicatorBase"/> class descendant.
    /// 
    ///             </param>
    public static void SetValueIndicator(StateIndicatorControl stateControl, ValueIndicatorBase value)
    {
      stateControl.SetValue(AnalogGaugeControl.ValueIndicatorProperty, (object) value);
    }

    void ILogicalParent.AddChild(object child)
    {
      if (this.logicalChildren.Contains(child))
        return;
      this.logicalChildren.Add(child);
      this.AddLogicalChild(child);
    }

    void ILogicalParent.RemoveChild(object child)
    {
      if (!this.logicalChildren.Contains(child))
        return;
      this.logicalChildren.Remove(child);
      this.RemoveLogicalChild(child);
    }
  }
}
