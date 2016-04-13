// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.StateIndicatorModel
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Collections.Generic;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// For internal use.
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class StateIndicatorModel : GaugeModelBase
  {
    protected List<State> predefinedStates = new List<State>();
    protected State defaultState;

    internal List<State> PredefinedStates
    {
      get
      {
        return this.predefinedStates;
      }
    }

    internal State DefaultState
    {
      get
      {
        return this.defaultState;
      }
    }
  }
}
