// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ValueChangedEventArgs
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;
using System;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// Provides data for the <see cref="E:DevExpress.Xpf.Gauges.ValueIndicatorBase.ValueChanged"/> event.
  /// 
  /// </para>
  /// 
  /// </summary>
  [NonCategorized]
  public class ValueChangedEventArgs : EventArgs
  {
    private double oldValue;
    private double newValue;

    /// <summary>
    /// 
    /// <para>
    /// Gets the old value of a property.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> that is the old value.
    /// 
    /// 
    /// </value>
    public double OldValue
    {
      get
      {
        return this.oldValue;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets the new value of a property.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> that is the new value.
    /// 
    /// 
    /// </value>
    public double NewValue
    {
      get
      {
        return this.newValue;
      }
    }

    internal ValueChangedEventArgs(double oldValue, double newValue)
    {
      this.oldValue = oldValue;
      this.newValue = newValue;
    }
  }
}
