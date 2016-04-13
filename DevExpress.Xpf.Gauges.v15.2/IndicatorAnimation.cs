// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.IndicatorAnimation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// Contains settings for animating a value indicator when it changes its value.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class IndicatorAnimation : AnimationBase
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
    public static readonly DependencyProperty DurationProperty = DependencyPropertyManager.Register("Duration", typeof (TimeSpan), typeof (IndicatorAnimation), new PropertyMetadata((object) TimeSpan.FromMilliseconds(800.0), new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty EasingFunctionProperty = DependencyPropertyManager.Register("EasingFunction", typeof (IEasingFunction), typeof (IndicatorAnimation), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the duration of an animation effect.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.TimeSpan"/> value that is the duration of an animation effect.
    /// 
    /// 
    /// </value>
    [Category("Behavior")]
    [DevExpressXpfGaugesLocalizedDescription("IndicatorAnimationDuration")]
    public TimeSpan Duration
    {
      get
      {
        return (TimeSpan) this.GetValue(IndicatorAnimation.DurationProperty);
      }
      set
      {
        this.SetValue(IndicatorAnimation.DurationProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets an animation function that defines how indicator values change during animation.
    /// 
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An object implementing the <see cref="T:System.Windows.Media.Animation.IEasingFunction"/> interface.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("IndicatorAnimationEasingFunction")]
    [Category("Behavior")]
    public IEasingFunction EasingFunction
    {
      get
      {
        return (IEasingFunction) this.GetValue(IndicatorAnimation.EasingFunctionProperty);
      }
      set
      {
        this.SetValue(IndicatorAnimation.EasingFunctionProperty, (object) value);
      }
    }

    private IEasingFunction GetDefaultEasingFunction()
    {
      return (IEasingFunction) null;
    }

    private Timeline CreateTimeline()
    {
      DoubleAnimation doubleAnimation = new DoubleAnimation();
      doubleAnimation.From = new double?(0.0);
      doubleAnimation.To = new double?(1.0);
      doubleAnimation.Duration = (System.Windows.Duration) this.Duration;
      doubleAnimation.BeginTime = new TimeSpan?(TimeSpan.Zero);
      doubleAnimation.EasingFunction = this.EasingFunction != null ? this.EasingFunction : this.GetDefaultEasingFunction();
      return (Timeline) doubleAnimation;
    }

    internal void PrepareStoryboard(Storyboard storyboard, ValueIndicatorBase indicator)
    {
      Timeline timeline = this.CreateTimeline();
      Storyboard.SetTarget((DependencyObject) timeline, (DependencyObject) indicator.Progress);
      Storyboard.SetTargetProperty((DependencyObject) timeline, new PropertyPath(DependencyPropertyExtensions.GetName(AnimationProgress.ProgressProperty), new object[0]));
      storyboard.Children.Add(timeline);
      storyboard.BeginTime = new TimeSpan?(TimeSpan.Zero);
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new IndicatorAnimation();
    }
  }
}
