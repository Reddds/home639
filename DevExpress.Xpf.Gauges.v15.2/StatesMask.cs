// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.StatesMask
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.ComponentModel;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A states mask that is used to display a custom symbol on a <see cref="T:DevExpress.Xpf.Gauges.DigitalGaugeControl"/>.
  /// 
  /// 
  /// </para>
  /// 
  /// </summary>
  [TypeConverter(typeof (StatesMaskConverter))]
  public struct StatesMask
  {
    private bool[] states;

    /// <summary>
    /// 
    /// <para>
    /// Provides access to the symbol states that are used to provide both custom symbol mapping and specify the blinking animation effect.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Boolean"/> array that contains the symbol states.
    /// 
    /// </value>
    public bool[] States
    {
      get
      {
        return this.states;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the StatesMask class with specified initial states.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// <param name="initialStates">A <see cref="T:System.Boolean"/> array that contains the initial states.
    /// 
    /// 
    /// 
    ///             </param>
    public StatesMask(params bool[] initialStates)
    {
      if (initialStates != null)
        this.states = initialStates;
      else
        this.states = new bool[0];
    }
  }
}
