// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.SymbolsAnimation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A base class for creeping line and blinking animation effects of a digital gauge control.
  /// 
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class SymbolsAnimation : AnimationBase
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
    public static readonly DependencyProperty RefreshTimeProperty = DependencyPropertyManager.Register("RefreshTime", typeof (TimeSpan), typeof (SymbolsAnimation), new PropertyMetadata((object) TimeSpan.FromMilliseconds(400.0), new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the refresh time between two symbol animations.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.TimeSpan"/> value that is the refresh time between two animations.
    /// 
    /// </value>
    [Category("Behavior")]
    public TimeSpan RefreshTime
    {
      get
      {
        return (TimeSpan) this.GetValue(SymbolsAnimation.RefreshTimeProperty);
      }
      set
      {
        this.SetValue(SymbolsAnimation.RefreshTimeProperty, (object) value);
      }
    }

    protected internal abstract bool ShouldReplay { get; }

    private Timeline CreateTimeline(TimeSpan duration)
    {
      DoubleAnimation doubleAnimation = new DoubleAnimation();
      doubleAnimation.From = new double?(0.0);
      doubleAnimation.To = new double?(1.0);
      doubleAnimation.Duration = (Duration) duration;
      doubleAnimation.BeginTime = new TimeSpan?(TimeSpan.Zero);
      return (Timeline) doubleAnimation;
    }

    private void PrepareStoryboard(Storyboard storyboard, SymbolViewInternal symbolViewInternal, int animationInterval, bool firstPlay)
    {
      symbolViewInternal.Progress.IntervalCount = animationInterval;
      symbolViewInternal.Progress.InitialOffset = firstPlay ? this.GetInitialOffset(symbolViewInternal) : 0;
      Timeline timeline = this.CreateTimeline(TimeSpan.FromMilliseconds((double) animationInterval * this.RefreshTime.TotalMilliseconds));
      Storyboard.SetTarget((DependencyObject) timeline, (DependencyObject) symbolViewInternal.Progress);
      Storyboard.SetTargetProperty((DependencyObject) timeline, new PropertyPath(DependencyPropertyExtensions.GetName(AnimationProgress.ProgressProperty), new object[0]));
      storyboard.Children.Add(timeline);
      storyboard.BeginTime = new TimeSpan?(TimeSpan.Zero);
      storyboard.RepeatBehavior = firstPlay ? new RepeatBehavior(1.0) : RepeatBehavior.Forever;
    }

    private void PrepareStates(SymbolViewInternal symbolViewInternal, List<SymbolState> states, int animationIntervalCount, bool firstPlay)
    {
      if (firstPlay)
        this.PrepareStatesForFirstPlay(symbolViewInternal, states);
      while (states.Count < animationIntervalCount)
      {
        if (symbolViewInternal.Gauge.TextDirection == TextDirection.RightToLeft)
          states.Insert(0, symbolViewInternal.GetEmptySymbolState());
        else
          states.Add(symbolViewInternal.GetEmptySymbolState());
      }
    }

    protected virtual int GetInitialOffset(SymbolViewInternal symbolViewInternal)
    {
      return 0;
    }

    protected virtual void PrepareStatesForFirstPlay(SymbolViewInternal symbolViewInternal, List<SymbolState> states)
    {
    }

    protected abstract int GetAnimationIntervalCount(SymbolViewInternal symbolViewInternal, bool firstPlay);

    internal void Prepare(Storyboard storyboard, SymbolViewInternal symbolViewInternal, List<SymbolState> states, bool firstPlay)
    {
      int animationIntervalCount = this.GetAnimationIntervalCount(symbolViewInternal, firstPlay);
      this.PrepareStoryboard(storyboard, symbolViewInternal, animationIntervalCount, firstPlay);
      this.PrepareStates(symbolViewInternal, states, animationIntervalCount, firstPlay);
    }

    protected internal abstract List<SymbolState> AnimateSymbolsStates(List<SymbolState> states, SymbolViewInternal symbolViewInternal);

    protected internal virtual void RaiseCompletedEvent()
    {
    }
  }
}
