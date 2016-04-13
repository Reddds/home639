// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.IndicatorEnterLeaveEventArgs
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A base class for classes that provide data for <see cref="E:DevExpress.Xpf.Gauges.RangeBase.IndicatorEnter"/> and <see cref="E:DevExpress.Xpf.Gauges.RangeBase.IndicatorLeave"/> events.
  /// 
  /// </para>
  /// 
  /// </summary>
  [NonCategorized]
  public class IndicatorEnterLeaveEventArgs : ValueChangedEventArgs
  {
    private ValueIndicatorBase indicator;

    /// <summary>
    /// 
    /// <para>
    /// Gets the object, for which either the <see cref="E:DevExpress.Xpf.Gauges.RangeBase.IndicatorEnter"/> or <see cref="E:DevExpress.Xpf.Gauges.RangeBase.IndicatorLeave"/> event has been raised.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.ValueIndicatorBase"/> class descendant representing the indicator.
    /// 
    /// 
    /// </value>
    public ValueIndicatorBase Indicator
    {
      get
      {
        return this.indicator;
      }
    }

    internal IndicatorEnterLeaveEventArgs(ValueIndicatorBase indicator, double oldValue, double newvalue)
      : base(oldValue, newvalue)
    {
      this.indicator = indicator;
    }
  }
}
