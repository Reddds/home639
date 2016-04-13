// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.CreepingLineAnimation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// Contains settings to provide a creeping line animation for the digital gauge control.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class CreepingLineAnimation : SymbolsAnimation
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
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Obsolete("The StartSpaces property is now obsolete. Use the InitialMoves property instead.")]
    [Browsable(false)]
    public static readonly DependencyProperty StartSpacesProperty = DependencyPropertyManager.Register("StartSpaces", typeof (int), typeof (CreepingLineAnimation), new PropertyMetadata((object) -1, new PropertyChangedCallback(CreepingLineAnimation.StartSpacesPropertyChanged)));
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
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("The AdditionalSpaces property is now obsolete. Instead, use the RepeatSpaces property for animations with Repeat=True and the FinalMoves property for animations with Repeat=False.")]
    [Browsable(false)]
    public static readonly DependencyProperty AdditionalSpacesProperty = DependencyPropertyManager.Register("AdditionalSpaces", typeof (int), typeof (CreepingLineAnimation), new PropertyMetadata((object) -1, new PropertyChangedCallback(CreepingLineAnimation.AdditionalSpacesPropertyChanged)));
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
    public static readonly DependencyProperty InitialMovesProperty = DependencyPropertyManager.Register("InitialMoves", typeof (int), typeof (CreepingLineAnimation), new PropertyMetadata((object) -1, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty FinalMovesProperty = DependencyPropertyManager.Register("FinalMoves", typeof (int), typeof (CreepingLineAnimation), new PropertyMetadata((object) 0, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)), new ValidateValueCallback(CreepingLineAnimation.FinalMovesValidation));
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
    public static readonly DependencyProperty RepeatSpacesProperty = DependencyPropertyManager.Register("RepeatSpaces", typeof (int), typeof (CreepingLineAnimation), new PropertyMetadata((object) 3, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)), new ValidateValueCallback(CreepingLineAnimation.RepeatSpacesValidation));
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
    public static readonly DependencyProperty RepeatProperty = DependencyPropertyManager.Register("Repeat", typeof (bool), typeof (CreepingLineAnimation), new PropertyMetadata((object) false, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty DirectionProperty = DependencyPropertyManager.Register("Direction", typeof (CreepingLineDirection), typeof (CreepingLineAnimation), new PropertyMetadata((object) CreepingLineDirection.RightToLeft, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
    private const int defaultRepeatSpaces = 3;
    private const int defaultEndSpaces = 0;

    /// <summary>
    /// 
    /// <para>
    /// Specifies the start segment from which the creeping line animation begins on the symbols panel.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An integer value that specifies the start segment of creeping line animation on the symbols panel.
    /// 
    /// </value>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Obsolete("The StartSpaces property is now obsolete. Use the InitialMoves property instead.")]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public int StartSpaces
    {
      get
      {
        return (int) this.GetValue(CreepingLineAnimation.StartSpacesProperty);
      }
      set
      {
        this.SetValue(CreepingLineAnimation.StartSpacesProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Specifies the additional steps which the creeping line animation executes on the symbols panel relative to the inanimate text position.
    /// 
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An integer value that is the additional step of the creeping line animation.
    /// 
    /// </value>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("The AdditionalSpaces property is now obsolete. Instead, use the RepeatSpaces property for animations with Repeat=True and the FinalMoves property for animations with Repeat=False.")]
    [Browsable(false)]
    public int AdditionalSpaces
    {
      get
      {
        return (int) this.GetValue(CreepingLineAnimation.AdditionalSpacesProperty);
      }
      set
      {
        this.SetValue(CreepingLineAnimation.AdditionalSpacesProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets whether or not the creeping line animation should be repeated.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> to repeat the creeping line animation for the digital gauge control; otherwise <b>false</b>.
    /// 
    /// </value>
    [Category("Behavior")]
    public bool Repeat
    {
      get
      {
        return (bool) this.GetValue(CreepingLineAnimation.RepeatProperty);
      }
      set
      {
        this.SetValue(CreepingLineAnimation.RepeatProperty, (object) (bool) (value ? 1 : 0));
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the direction of creeping line animation on the symbols panel.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.CreepingLineDirection"/> enumeration value that specifies the creeping line animation direction.
    /// 
    /// </value>
    [Category("Behavior")]
    public CreepingLineDirection Direction
    {
      get
      {
        return (CreepingLineDirection) this.GetValue(CreepingLineAnimation.DirectionProperty);
      }
      set
      {
        this.SetValue(CreepingLineAnimation.DirectionProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Specifies the start segment from which the creeping line animation begins on the  symbols panel.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An integer value that specifies the start segment of creeping line animation on the symbols panel.
    /// 
    /// </value>
    [Category("Behavior")]
    public int InitialMoves
    {
      get
      {
        return (int) this.GetValue(CreepingLineAnimation.InitialMovesProperty);
      }
      set
      {
        this.SetValue(CreepingLineAnimation.InitialMovesProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Specifies the additional moves which the creeping line animation executes on the symbols panel relative to the inanimate text position.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An integer value that is the additional move of the creeping line animation.
    /// 
    /// </value>
    [Category("Behavior")]
    public int FinalMoves
    {
      get
      {
        return (int) this.GetValue(CreepingLineAnimation.FinalMovesProperty);
      }
      set
      {
        this.SetValue(CreepingLineAnimation.FinalMovesProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Specifies space segments that appear on the symbols panel each time the text animation is repeated.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An integer value that specifies the number of space segments on the symbols panel.
    /// 
    /// </value>
    [Category("Behavior")]
    public int RepeatSpaces
    {
      get
      {
        return (int) this.GetValue(CreepingLineAnimation.RepeatSpacesProperty);
      }
      set
      {
        this.SetValue(CreepingLineAnimation.RepeatSpacesProperty, (object) value);
      }
    }

    protected internal override bool ShouldReplay
    {
      get
      {
        return this.Repeat;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Occurs when the creeping line animation is completed in the digital gauge control.
    /// 
    /// </para>
    /// 
    /// </summary>
    public event CreepingLineAnimationCompletedEventHandler CreepingLineAnimationCompleted;

    private static void StartSpacesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      CreepingLineAnimation creepingLineAnimation = d as CreepingLineAnimation;
      if (creepingLineAnimation == null)
        return;
      creepingLineAnimation.InitialMoves = (int) e.NewValue;
    }

    private static void AdditionalSpacesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      CreepingLineAnimation creepingLineAnimation = d as CreepingLineAnimation;
      if (creepingLineAnimation == null)
        return;
      int num = (int) e.NewValue;
      if (num < 0)
      {
        creepingLineAnimation.RepeatSpaces = 3;
        creepingLineAnimation.FinalMoves = 0;
      }
      else
      {
        creepingLineAnimation.RepeatSpaces = num;
        creepingLineAnimation.FinalMoves = num;
      }
    }

    private static bool RepeatSpacesValidation(object value)
    {
      return (int) value > -1;
    }

    private static bool FinalMovesValidation(object value)
    {
      return (int) value > -1;
    }

    private int GetActualStartSpaces(SymbolViewInternal symbolViewInternal)
    {
      if (this.InitialMoves >= 0)
        return this.InitialMoves;
      if (symbolViewInternal.Gauge.TextDirection == TextDirection.LeftToRight && this.Direction == CreepingLineDirection.LeftToRight || symbolViewInternal.Gauge.TextDirection == TextDirection.RightToLeft && this.Direction == CreepingLineDirection.RightToLeft)
        return symbolViewInternal.SeparateTextToSymbols(symbolViewInternal.Gauge.Text).Count;
      return symbolViewInternal.Gauge.ActualSymbolCount;
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new CreepingLineAnimation();
    }

    protected override int GetAnimationIntervalCount(SymbolViewInternal symbolViewInternal, bool firstPlay)
    {
      List<string> list = symbolViewInternal.SeparateTextToSymbols(symbolViewInternal.Gauge.Text);
      int actualStartSpaces = this.GetActualStartSpaces(symbolViewInternal);
      if (firstPlay)
      {
        if (!this.Repeat)
          return actualStartSpaces + this.FinalMoves;
        return actualStartSpaces;
      }
      int val2 = this.Repeat ? list.Count + this.RepeatSpaces : list.Count;
      return Math.Max(symbolViewInternal.Gauge.ActualSymbolCount, val2);
    }

    protected override void PrepareStatesForFirstPlay(SymbolViewInternal symbolViewInternal, List<SymbolState> states)
    {
      int actualStartSpaces = this.GetActualStartSpaces(symbolViewInternal);
      for (int index = 0; index < actualStartSpaces; ++index)
      {
        if (this.Direction == CreepingLineDirection.LeftToRight)
          states.Add(symbolViewInternal.GetEmptySymbolState());
        else
          states.Insert(0, symbolViewInternal.GetEmptySymbolState());
      }
      for (int index = 0; index < this.FinalMoves; ++index)
      {
        if (this.Direction == CreepingLineDirection.RightToLeft)
          states.Add(symbolViewInternal.GetEmptySymbolState());
        else
          states.Insert(0, symbolViewInternal.GetEmptySymbolState());
      }
    }

    protected override int GetInitialOffset(SymbolViewInternal symbolViewInternal)
    {
      if (this.Direction == CreepingLineDirection.LeftToRight && symbolViewInternal.Gauge.TextDirection == TextDirection.LeftToRight || this.Direction == CreepingLineDirection.RightToLeft && symbolViewInternal.Gauge.TextDirection == TextDirection.RightToLeft)
        return -this.GetActualStartSpaces(symbolViewInternal) - this.FinalMoves;
      return 0;
    }

    protected internal override List<SymbolState> AnimateSymbolsStates(List<SymbolState> states, SymbolViewInternal symbolViewInternal)
    {
      List<SymbolState> list = new List<SymbolState>();
      int num1 = this.Direction == CreepingLineDirection.LeftToRight ? -symbolViewInternal.Progress.ActualIntegerProgress : symbolViewInternal.Progress.ActualIntegerProgress;
      for (int index = 0; index < states.Count; ++index)
      {
        int num2 = index + num1;
        if (num2 < 0)
          num2 += (int) (Math.Ceiling((double) -num2 / (double) states.Count) * (double) states.Count);
        list.Add(states[num2 % states.Count]);
      }
      return list;
    }

    protected internal override void RaiseCompletedEvent()
    {
      if (this.CreepingLineAnimationCompleted == null)
        return;
      this.CreepingLineAnimationCompleted((object) this, new EventArgs());
    }
  }
}
