// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.StateCollection
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Collections.Specialized;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A collection that stores states of a particular state indicator control.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class StateCollection : GaugeDependencyObjectCollection<State>
  {
    private StateIndicatorControl StateIndicator
    {
      get
      {
        return this.Owner as StateIndicatorControl;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the StateCollection class with the specified owner.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// <param name="stateIndicator">A <see cref="T:DevExpress.Xpf.Gauges.StateIndicatorControl"/> object that should be the owner of the created collection.
    /// 
    ///             </param>
    public StateCollection(StateIndicatorControl stateIndicator)
    {
      this.Owner = (object) stateIndicator;
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      base.OnCollectionChanged(e);
      if (this.StateIndicator == null)
        return;
      this.StateIndicator.UpdateStates();
    }
  }
}
