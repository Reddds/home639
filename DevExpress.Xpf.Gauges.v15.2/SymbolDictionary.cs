// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.SymbolDictionary
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;
using System;
using System.Collections.Specialized;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A dictionary that stores elements for custom symbol mapping.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class SymbolDictionary : WeakEventListenerCollection<SymbolSegmentsMapping>
  {
    private readonly SymbolViewBase symbolView;

    private DigitalGaugeControl Gauge
    {
      get
      {
        if (this.symbolView == null)
          return (DigitalGaugeControl) null;
        return this.symbolView.Gauge;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the SymbolDictionary class with the specified owner.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// <param name="symbolView">A <see cref="T:DevExpress.Xpf.Gauges.SymbolViewBase"/> class descendant that should be the owner of the created collection.
    /// 
    ///             </param>
    public SymbolDictionary(SymbolViewBase symbolView)
    {
      this.symbolView = symbolView;
    }

    protected override bool PerformWeakEvent(Type managerType, object sender, EventArgs e)
    {
      bool flag = false;
      if (managerType == typeof (PropertyChangedWeakEventManager))
      {
        if (sender is GaugeDependencyObject && this.Gauge != null)
          this.Gauge.UpdateViewSymbols();
        flag = true;
      }
      return flag;
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      base.OnCollectionChanged(e);
      if (this.Gauge == null)
        return;
      this.Gauge.UpdateViewSymbols();
    }
  }
}
