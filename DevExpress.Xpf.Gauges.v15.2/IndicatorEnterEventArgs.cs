// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.IndicatorEnterEventArgs
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// Provides data for the <see cref="E:DevExpress.Xpf.Gauges.RangeBase.IndicatorEnter"/> event.
  /// 
  /// </para>
  /// 
  /// </summary>
  [NonCategorized]
  public class IndicatorEnterEventArgs : IndicatorEnterLeaveEventArgs
  {
    internal IndicatorEnterEventArgs(ValueIndicatorBase indicator, double oldValue, double newvalue)
      : base(indicator, oldValue, newvalue)
    {
    }
  }
}
