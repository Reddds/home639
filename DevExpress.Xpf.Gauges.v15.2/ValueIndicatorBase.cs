// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ValueIndicatorBase
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;
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
  /// Serves as the base class for all value indicators.
  /// 
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class ValueIndicatorBase : GaugeDependencyObject, IOwnedElement, IWeakEventListener, IAnimatableElement, IModelSupported, ILayoutCalculator
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
    public static readonly DependencyProperty ValueProperty = DependencyPropertyManager.Register("Value", typeof (double), typeof (ValueIndicatorBase), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ValueIndicatorBase.ValuePropertyChanged)));
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
    public static readonly DependencyProperty AnimationProperty = DependencyPropertyManager.Register("Animation", typeof (IndicatorAnimation), typeof (ValueIndicatorBase), new PropertyMetadata((object) null, new PropertyChangedCallback(ValueIndicatorBase.AnimationPropertyChanged)));
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
    public static readonly DependencyProperty IsHitTestVisibleProperty = DependencyPropertyManager.Register("IsHitTestVisible", typeof (bool), typeof (ValueIndicatorBase), new PropertyMetadata((object) true));
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
    public static readonly DependencyProperty IsInteractiveProperty = DependencyPropertyManager.Register("IsInteractive", typeof (bool), typeof (ValueIndicatorBase), new PropertyMetadata((object) false));
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
    public static readonly DependencyProperty VisibleProperty = DependencyPropertyManager.Register("Visible", typeof (bool), typeof (ValueIndicatorBase), new PropertyMetadata((object) true, new PropertyChangedCallback(ValueIndicatorBase.IndicatorPropertyChanged)));
    private static double defaultAnchorValue = 0.0;
    private double anchorValue = ValueIndicatorBase.defaultAnchorValue;
    private readonly ValueIndicatorInfo info;
    private object owner;
    private Storyboard storyboard;
    private AnimationProgress progress;
    private bool animationInProgress;
    private bool isLockedAnimation;
    private StateIndicatorControl stateIndicator;

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the value of a scale indicator.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that specifies the position of a value indicator on the scale.
    /// 
    /// </value>
    [Category("Data")]
    [DevExpressXpfGaugesLocalizedDescription("ValueIndicatorBaseValue")]
    public double Value
    {
      get
      {
        return (double) this.GetValue(ValueIndicatorBase.ValueProperty);
      }
      set
      {
        this.SetValue(ValueIndicatorBase.ValueProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to the animation object that allows you to customize animation for the current value indicator.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.IndicatorAnimation"/> object.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ValueIndicatorBaseAnimation")]
    [Category("Animation")]
    public IndicatorAnimation Animation
    {
      get
      {
        return (IndicatorAnimation) this.GetValue(ValueIndicatorBase.AnimationProperty);
      }
      set
      {
        this.SetValue(ValueIndicatorBase.AnimationProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value that defines whether or not an indicator can be returned as a hit-testing result.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> in case the indicator can be shown as the result of hit testing; otherwise <b>false</b>.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ValueIndicatorBaseIsHitTestVisible")]
    [Category("Behavior")]
    public bool IsHitTestVisible
    {
      get
      {
        return (bool) this.GetValue(ValueIndicatorBase.IsHitTestVisibleProperty);
      }
      set
      {
        this.SetValue(ValueIndicatorBase.IsHitTestVisibleProperty, (object) (bool) (value ? 1 : 0));
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value that indicates whether interactivity is enabled for the current value indicator or not.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> to enable interactivity; otherwise, <b>false</b>.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ValueIndicatorBaseIsInteractive")]
    [Category("Behavior")]
    public bool IsInteractive
    {
      get
      {
        return (bool) this.GetValue(ValueIndicatorBase.IsInteractiveProperty);
      }
      set
      {
        this.SetValue(ValueIndicatorBase.IsInteractiveProperty, (object) (bool) (value ? 1 : 0));
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets whether the value indicator is visible.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> if the value indicator is visible; otherwise, <b>false</b>.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ValueIndicatorBaseVisible")]
    [Category("Behavior")]
    public bool Visible
    {
      get
      {
        return (bool) this.GetValue(ValueIndicatorBase.VisibleProperty);
      }
      set
      {
        this.SetValue(ValueIndicatorBase.VisibleProperty, (object) (bool) (value ? 1 : 0));
      }
    }

    private IndicatorAnimation ActualAnimation
    {
      get
      {
        if (this.Animation != null)
          return this.Animation;
        return new IndicatorAnimation();
      }
    }

    protected object Owner
    {
      get
      {
        return this.owner;
      }
    }

    protected AnalogGaugeControl Gauge
    {
      get
      {
        if (this.Scale == null)
          return (AnalogGaugeControl) null;
        return this.Scale.Gauge;
      }
    }

    protected abstract ValueIndicatorPresentation ActualPresentation { get; }

    protected abstract int ActualZIndex { get; }

    protected internal virtual IEnumerable<ValueIndicatorInfo> Elements
    {
      get
      {
        yield return this.info;
      }
    }

    internal double ActualValue
    {
      get
      {
        return this.CalcActualValue(this.Value);
      }
    }

    internal bool ShouldAnimate
    {
      get
      {
        if (this.isLockedAnimation)
          return false;
        if (this.Animation != null && this.Scale != null)
          return this.Animation.Enable;
        if (this.Gauge == null)
          return false;
        return this.Gauge.EnableAnimation;
      }
    }

    internal AnimationProgress Progress
    {
      get
      {
        return this.progress;
      }
    }

    internal Storyboard Storyboard
    {
      get
      {
        if (this.storyboard == null && this.Scale != null)
        {
          this.storyboard = new Storyboard();
          this.storyboard.Completed += new EventHandler(this.OnStoryboardCompleted);
          this.Scale.AddStoryboard(this.storyboard, this.GetHashCode());
        }
        return this.storyboard;
      }
    }

    internal Scale Scale
    {
      get
      {
        return this.Owner as Scale;
      }
    }

    internal bool IsLockedAnimation
    {
      get
      {
        return this.isLockedAnimation;
      }
      set
      {
        this.isLockedAnimation = value;
      }
    }

    internal ValueIndicatorInfo ElementInfo
    {
      get
      {
        return this.info;
      }
    }

    internal StateIndicatorControl StateIndicator
    {
      get
      {
        return this.stateIndicator;
      }
      set
      {
        this.stateIndicator = value;
      }
    }

    object IOwnedElement.Owner
    {
      get
      {
        return this.owner;
      }
      set
      {
        if (this.StateIndicator != null)
        {
          this.stateIndicator.SubscribeRangeCollectionEvents(value as Scale, this.owner as Scale);
          this.stateIndicator.UpdateStateIndexByValueIndicator(this);
        }
        this.owner = value;
        this.ChangeOwner();
      }
    }

    bool IAnimatableElement.InProgress
    {
      get
      {
        return this.animationInProgress;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Occurs when the <see cref="P:DevExpress.Xpf.Gauges.ValueIndicatorBase.Value"/> property has been changed.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    public event ValueChangedEventHandler ValueChanged;

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the ValueIndicatorBase class with default settings.
    /// 
    /// </para>
    /// 
    /// </summary>
    public ValueIndicatorBase()
    {
      this.progress = new AnimationProgress((IAnimatableElement) this);
      this.info = new ValueIndicatorInfo(this, this.ActualZIndex, this.ActualPresentation.CreateIndicatorPresentationControl(), (PresentationBase) this.ActualPresentation);
    }

    private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ValueIndicatorBase indicator = d as ValueIndicatorBase;
      if (indicator == null)
        return;
      indicator.anchorValue = indicator.CalcActualValue((double) e.OldValue);
      indicator.RequestAnimation(false);
      ValueIndicatorBase.IndicatorPropertyChanged(d, e);
      indicator.OnValueChanged((double) e.OldValue, (double) e.NewValue);
      if (indicator.Scale == null)
        return;
      indicator.Scale.CheckIndicatorEnterLeaveRange(indicator, (double) e.OldValue, (double) e.NewValue);
    }

    private static void AnimationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ArcScaleIndicator arcScaleIndicator = d as ArcScaleIndicator;
      if (arcScaleIndicator == null)
        return;
      CommonUtils.SubscribePropertyChangedWeakEvent((INotifyPropertyChanged) (e.OldValue as IndicatorAnimation), (INotifyPropertyChanged) (e.NewValue as IndicatorAnimation), (IWeakEventListener) arcScaleIndicator);
      arcScaleIndicator.OnAnimationChanged();
    }

    protected static void IndicatorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ValueIndicatorBase valueIndicatorBase = d as ValueIndicatorBase;
      if (valueIndicatorBase == null)
        return;
      valueIndicatorBase.Invalidate();
    }

    protected static void PresentationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ValueIndicatorBase valueIndicatorBase = d as ValueIndicatorBase;
      if (valueIndicatorBase == null || object.Equals(e.NewValue, e.OldValue))
        return;
      valueIndicatorBase.UpdatePresentation();
    }

    bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
    {
      return this.PerformWeakEvent(managerType, sender, e);
    }

    void IAnimatableElement.ProgressChanged()
    {
      this.Invalidate();
    }

    void IModelSupported.UpdateModel()
    {
      this.UpdatePresentation();
      this.OnOptionsChanged();
    }

    ElementLayout ILayoutCalculator.CreateLayout(Size constraint)
    {
      if (!this.Visible)
        return (ElementLayout) null;
      return this.CreateLayout(constraint);
    }

    void ILayoutCalculator.CompleteLayout(ElementInfoBase elementInfo)
    {
      this.CompleteLayout(elementInfo);
    }

    private double CalcActualValue(double value)
    {
      return this.anchorValue + ((this.Scale != null ? this.Scale.GetLimitedValue(value) : value) - this.anchorValue) * this.progress.ActualProgress;
    }

    private void StopAnimation()
    {
      this.animationInProgress = false;
    }

    private void RequestAnimation(bool shouldResetValue)
    {
      this.StopAnimation();
      if (!this.ShouldAnimate)
        return;
      this.Animate(shouldResetValue);
    }

    private void PrepareStoryboard()
    {
      if (this.Storyboard.Children.Count > 0)
      {
        this.Storyboard.Stop();
        this.Storyboard.Children.Clear();
      }
      this.ActualAnimation.PrepareStoryboard(this.Storyboard, this);
    }

    private void OnStoryboardCompleted(object sender, EventArgs e)
    {
      this.anchorValue = this.Value;
      this.StopAnimation();
    }

    private void OnAnimationChanged()
    {
      if (this.Scale == null || !DesignerProperties.GetIsInDesignMode((DependencyObject) this.Scale))
        return;
      this.RequestAnimation(true);
    }

    private void OnValueChanged(double oldValue, double newValue)
    {
      if (this.ValueChanged == null)
        return;
      this.ValueChanged((object) this, new ValueChangedEventArgs(oldValue, newValue));
    }

    protected virtual void UpdatePresentation()
    {
      if (this.ElementInfo != null)
      {
        this.ElementInfo.Presentation = (PresentationBase) this.ActualPresentation;
        this.ElementInfo.PresentationControl = this.ActualPresentation.CreateIndicatorPresentationControl();
      }
      this.Invalidate();
    }

    private void Invalidate()
    {
      foreach (ElementInfoBase elementInfoBase in this.Elements)
        elementInfoBase.Invalidate();
    }

    protected virtual void ChangeOwner()
    {
      ((IModelSupported) this).UpdateModel();
    }

    protected virtual bool PerformWeakEvent(Type managerType, object sender, EventArgs e)
    {
      bool flag = false;
      if (managerType == typeof (PropertyChangedWeakEventManager))
      {
        if (sender is GaugeDependencyObject)
        {
          this.OnOptionsChanged();
          flag = true;
        }
        if (sender is IndicatorAnimation)
        {
          this.OnAnimationChanged();
          flag = true;
        }
      }
      return flag;
    }

    protected void OnOptionsChanged()
    {
      foreach (ElementInfoBase elementInfoBase in this.Elements)
        elementInfoBase.ZIndex = this.ActualZIndex;
      this.Invalidate();
    }

    protected abstract ElementLayout CreateLayout(Size constraint);

    protected abstract void CompleteLayout(ElementInfoBase elementInfo);

    internal void Animate(bool shouldResetValue)
    {
      if (shouldResetValue)
        this.anchorValue = ValueIndicatorBase.defaultAnchorValue;
      this.StopAnimation();
      if (!this.ShouldAnimate)
        return;
      this.PrepareStoryboard();
      this.progress.Start();
      this.animationInProgress = true;
      this.Storyboard.Begin();
    }

    internal void ClearAnimation()
    {
      if (this.storyboard == null)
        return;
      this.storyboard.Stop();
      this.storyboard.Completed -= new EventHandler(this.OnStoryboardCompleted);
      this.Scale.RemoveStoryboard(this.GetHashCode());
      this.storyboard = (Storyboard) null;
    }
  }
}
