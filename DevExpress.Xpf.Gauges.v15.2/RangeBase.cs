// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.RangeBase
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// Serves as the base class for all ranges.
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class RangeBase : LayerBase
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
    public static readonly DependencyProperty OptionsProperty = DependencyPropertyManager.Register("Options", typeof (RangeOptions), typeof (RangeBase), new PropertyMetadata(new PropertyChangedCallback(LayerBase.OptionsPropertyChanged)));
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
    [TypeConverter(typeof (RangeValueConverter))]
    public static readonly DependencyProperty StartValueProperty = DependencyPropertyManager.Register("StartValue", typeof (RangeValue), typeof (RangeBase), new PropertyMetadata((object) new RangeValue(0.0, RangeValueType.Percent), new PropertyChangedCallback(RangeBase.RangePropertyChanged)));
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
    [TypeConverter(typeof (RangeValueConverter))]
    public static readonly DependencyProperty EndValueProperty = DependencyPropertyManager.Register("EndValue", typeof (RangeValue), typeof (RangeBase), new PropertyMetadata((object) new RangeValue(25.0, RangeValueType.Percent), new PropertyChangedCallback(RangeBase.RangePropertyChanged)));
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
    public static readonly DependencyProperty IsHitTestVisibleProperty = DependencyPropertyManager.Register("IsHitTestVisible", typeof (bool), typeof (RangeBase), new PropertyMetadata((object) true));
    private readonly RangeInfo info;

    /// <summary>
    /// 
    /// <para>
    /// Provides access to the settings that specify the shape and position of the current range.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.RangeOptions"/> object that contains the settings of the range.
    /// 
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("RangeBaseOptions")]
    [Category("Presentation")]
    public RangeOptions Options
    {
      get
      {
        return (RangeOptions) this.GetValue(RangeBase.OptionsProperty);
      }
      set
      {
        this.SetValue(RangeBase.OptionsProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the start position of the range on a scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.RangeValue"/> object that allows setting the start position of the range in either absolute or relative units.
    /// 
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("RangeBaseStartValue")]
    [Category("Data")]
    [TypeConverter(typeof (RangeValueConverter))]
    public RangeValue StartValue
    {
      get
      {
        return (RangeValue) this.GetValue(RangeBase.StartValueProperty);
      }
      set
      {
        this.SetValue(RangeBase.StartValueProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the end position of the range on a scale.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.RangeValue"/> object that allows to set the end position of the range in absolute or relative units.
    /// 
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("RangeBaseEndValue")]
    [TypeConverter(typeof (RangeValueConverter))]
    [Category("Data")]
    public RangeValue EndValue
    {
      get
      {
        return (RangeValue) this.GetValue(RangeBase.EndValueProperty);
      }
      set
      {
        this.SetValue(RangeBase.EndValueProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value that defines whether or not a range can be returned as a hit-testing result.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> in case the range can be shown as the result of hit testing; otherwise <b>false</b>.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("RangeBaseIsHitTestVisible")]
    [Category("Behavior")]
    public bool IsHitTestVisible
    {
      get
      {
        return (bool) this.GetValue(RangeBase.IsHitTestVisibleProperty);
      }
      set
      {
        this.SetValue(RangeBase.IsHitTestVisibleProperty, (object) (bool) (value ? 1 : 0));
      }
    }

    internal Scale Scale
    {
      get
      {
        return this.Owner as Scale;
      }
    }

    internal override LayerInfo ElementInfo
    {
      get
      {
        return (LayerInfo) this.info;
      }
    }

    protected internal abstract RangeOptions ActualOptions { get; }

    protected override int ActualZIndex
    {
      get
      {
        return this.ActualOptions.ZIndex;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets a value that specifies the start range position in absolute units.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that is the start range position in absolute units.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("RangeBaseStartValueAbsolute")]
    public double StartValueAbsolute
    {
      get
      {
        if (!this.StartValue.IsPercent)
          return this.StartValue.Value;
        return this.PercentToAbsolute(this.StartValue.Value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets a value that specifies the end range position on a scale in absolute units.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that is the end range position in absolute units.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("RangeBaseEndValueAbsolute")]
    public double EndValueAbsolute
    {
      get
      {
        if (!this.EndValue.IsPercent)
          return this.EndValue.Value;
        return this.PercentToAbsolute(this.EndValue.Value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Occurs when any value indicator enters the current range.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    public event IndicatorEnterEventHandler IndicatorEnter;

    /// <summary>
    /// 
    /// <para>
    /// Occurs when any value indicator leaves the current range.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    public event IndicatorLeaveEventHandler IndicatorLeave;

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the RangeBase class with default settings.
    /// 
    /// </para>
    /// 
    /// </summary>
    public RangeBase()
    {
      this.info = new RangeInfo(this, this.ActualZIndex, this.ActualPresentation.CreateLayerPresentationControl(), (PresentationBase) this.ActualPresentation);
    }

    private static void RangePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      RangeBase rangeBase = d as RangeBase;
      if (rangeBase == null)
        return;
      rangeBase.Invalidate();
      bool startValueChanged = e.Property == RangeBase.StartValueProperty;
      RangeValue rangeValue = (RangeValue) e.OldValue;
      double oldRangeValue = rangeValue.IsPercent ? rangeBase.PercentToAbsolute(rangeValue.Value) : rangeValue.Value;
      rangeBase.CheckRangeEnterLeaveIndicator(oldRangeValue, startValueChanged);
    }

    private bool IsInRange(double value)
    {
      return MathUtils.IsValueInRange(value, this.StartValueAbsolute, this.EndValueAbsolute);
    }

    private bool IsEnter(double value, double oldRangeValue, bool startValueChanged)
    {
      double edge1 = startValueChanged ? this.EndValueAbsolute : this.StartValueAbsolute;
      if (!MathUtils.IsValueInRange(value, edge1, oldRangeValue))
        return this.IsInRange(value);
      return false;
    }

    private bool IsLeave(double value, double oldRangeValue, bool startValueChanged)
    {
      double edge1 = startValueChanged ? this.EndValueAbsolute : this.StartValueAbsolute;
      if (MathUtils.IsValueInRange(value, edge1, oldRangeValue))
        return !this.IsInRange(value);
      return false;
    }

    private double PercentToAbsolute(double percentValue)
    {
      if (this.Scale == null)
        return percentValue;
      return this.Scale.StartValue + (this.Scale.EndValue - this.Scale.StartValue) * percentValue / 100.0;
    }

    private void CheckRangeEnterLeaveIndicator(double oldRangeValue, bool startValueChanged)
    {
      if (this.Scale == null)
        return;
      foreach (ValueIndicatorBase indicator in this.Scale.Indicators)
      {
        if (this.IsEnter(indicator.Value, oldRangeValue, startValueChanged) && this.IndicatorEnter != null)
          this.IndicatorEnter((object) this, new IndicatorEnterEventArgs(indicator, indicator.Value, indicator.Value));
        if (this.IsLeave(indicator.Value, oldRangeValue, startValueChanged) && this.IndicatorLeave != null)
          this.IndicatorLeave((object) this, new IndicatorLeaveEventArgs(indicator, indicator.Value, indicator.Value));
      }
    }

    internal void OnIndicatorEnterLeave(ValueIndicatorBase indicator, double oldValue, double newValue)
    {
      if (indicator == null)
        return;
      if (!this.IsInRange(oldValue) && this.IsInRange(newValue) && this.IndicatorEnter != null)
        this.IndicatorEnter((object) this, new IndicatorEnterEventArgs(indicator, oldValue, newValue));
      if (!this.IsInRange(oldValue) || this.IsInRange(newValue) || this.IndicatorLeave == null)
        return;
      this.IndicatorLeave((object) this, new IndicatorLeaveEventArgs(indicator, oldValue, newValue));
    }
  }
}
