// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.BlinkingAnimation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// Contains settings to provide a blinking animation effect for the digital gauge control.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class BlinkingAnimation : SymbolsAnimation
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
    public static readonly DependencyProperty SymbolsStatesProperty = DependencyPropertyManager.Register("SymbolsStates", typeof (StatesMask), typeof (BlinkingAnimation), new PropertyMetadata((object) new StatesMask(), new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Specifies symbols states to show (hide) blinking animation on the symbols panel.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.StatesMask"/> value that specifies symbols states.
    /// 
    /// 
    /// </value>
    [Category("Behavior")]
    public StatesMask SymbolsStates
    {
      get
      {
        return (StatesMask) this.GetValue(BlinkingAnimation.SymbolsStatesProperty);
      }
      set
      {
        this.SetValue(BlinkingAnimation.SymbolsStatesProperty, (object) value);
      }
    }

    protected internal override bool ShouldReplay
    {
      get
      {
        return true;
      }
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new BlinkingAnimation();
    }

    protected override int GetAnimationIntervalCount(SymbolViewInternal symbolViewInternal, bool firstPlay)
    {
      return 2;
    }

    protected internal override List<SymbolState> AnimateSymbolsStates(List<SymbolState> symbolsState, SymbolViewInternal symbolViewInternal)
    {
      List<SymbolState> list = new List<SymbolState>();
      for (int index = 0; index < symbolsState.Count; ++index)
      {
        if ((this.SymbolsStates.States == null || index >= this.SymbolsStates.States.Length || this.SymbolsStates.States[index]) && symbolViewInternal.Progress.IntegerProgress > 0)
          list.Add(new SymbolState(string.Empty, symbolsState[index].Segments.Length, new bool[1]));
        else
          list.Add(symbolsState[index]);
      }
      return list;
    }
  }
}
