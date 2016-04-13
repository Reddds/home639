// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.StateIndicatorControl
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using DevExpress.Xpf.Utils.About;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A <b>state indicator</b> control shipped with the DXGauges Suite.
  /// 
  /// </para>
  /// 
  /// </summary>
  [DXToolboxBrowsable]
  [LicenseProvider(typeof (DX_WPF_LicenseProvider))]
  public class StateIndicatorControl : Control, IModelSupported, ILayoutCalculator
  {
    private readonly List<State> actualStates = new List<State>();
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
    public static readonly DependencyProperty ModelProperty = DependencyPropertyManager.Register("Model", typeof (StateIndicatorModel), typeof (StateIndicatorControl), new PropertyMetadata((object) null, new PropertyChangedCallback(StateIndicatorControl.ModelPropertyChanged)));
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
    public static readonly DependencyProperty StateIndexProperty = DependencyPropertyManager.Register("StateIndex", typeof (int), typeof (StateIndicatorControl), new PropertyMetadata((object) 0, new PropertyChangedCallback(StateIndicatorControl.StateIndexPropertyChanged)));
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
    public static readonly DependencyProperty DefaultStateProperty = DependencyPropertyManager.Register("DefaultState", typeof (State), typeof (StateIndicatorControl), new PropertyMetadata((object) null, new PropertyChangedCallback(StateIndicatorControl.DefaultStatePropertyChanged)));
    private static readonly DependencyPropertyKey StatePropertyKey = DependencyPropertyManager.RegisterReadOnly("State", typeof (State), typeof (StateIndicatorControl), new PropertyMetadata((PropertyChangedCallback) null));
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
    public static readonly DependencyProperty StateProperty = StateIndicatorControl.StatePropertyKey.DependencyProperty;
    internal static readonly DependencyPropertyKey AdditionalStatesPropertyKey = DependencyPropertyManager.RegisterReadOnly("AdditionalStates", typeof (StateCollection), typeof (StateIndicatorControl), new PropertyMetadata((PropertyChangedCallback) null));
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
    public static readonly DependencyProperty AdditionalStatesProperty = StateIndicatorControl.AdditionalStatesPropertyKey.DependencyProperty;
    private static readonly DependencyPropertyKey ActualModelPropertyKey = DependencyPropertyManager.RegisterReadOnly("ActualModel", typeof (StateIndicatorModel), typeof (StateIndicatorControl), new PropertyMetadata((object) null, new PropertyChangedCallback(StateIndicatorControl.ActualModelPropertyChanged)));
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
    public static readonly DependencyProperty ActualModelProperty = StateIndicatorControl.ActualModelPropertyKey.DependencyProperty;
    private static readonly DependencyPropertyKey StateCountPropertyKey = DependencyPropertyManager.RegisterReadOnly("StateCount", typeof (int), typeof (StateIndicatorControl), new PropertyMetadata((object) 0));
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
    public static readonly DependencyProperty StateCountProperty = StateIndicatorControl.StateCountPropertyKey.DependencyProperty;
    private const double defaultWidth = 250.0;
    private const double defaultHeight = 250.0;
    private IList currentRangeCollection;

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a model for the state indicator control that is used to draw its elements.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.StateIndicatorModel"/> class descendant that is the actual model.
    /// 
    /// </value>
    [Category("Presentation")]
    public StateIndicatorModel Model
    {
      get
      {
        return (StateIndicatorModel) this.GetValue(StateIndicatorControl.ModelProperty);
      }
      set
      {
        this.SetValue(StateIndicatorControl.ModelProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the index of a state image that is currently displayed on the <see cref="T:DevExpress.Xpf.Gauges.StateIndicatorControl"/>.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An integer value that is the current state index.
    /// 
    /// 
    /// </value>
    [Category("Data")]
    public int StateIndex
    {
      get
      {
        return (int) this.GetValue(StateIndicatorControl.StateIndexProperty);
      }
      set
      {
        this.SetValue(StateIndicatorControl.StateIndexProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the default state that specifies the <see cref="T:DevExpress.Xpf.Gauges.StateIndicatorControl"/> appearance when the state index is out of the predefined model states or additional states.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.State"/> object that specifies the default state of the State Indicator control.
    /// 
    /// </value>
    [Category("Data")]
    public State DefaultState
    {
      get
      {
        return (State) this.GetValue(StateIndicatorControl.DefaultStateProperty);
      }
      set
      {
        this.SetValue(StateIndicatorControl.DefaultStateProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to the collection of state indicator additional states.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.StateCollection"/> object that is the collection of state indicator additional states.
    /// 
    /// 
    /// </value>
    [Category("Presentation")]
    public StateCollection AdditionalStates
    {
      get
      {
        return (StateCollection) this.GetValue(StateIndicatorControl.AdditionalStatesProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to the current state of the <see cref="T:DevExpress.Xpf.Gauges.StateIndicatorControl"/>.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.State"/> object containing the state image.
    /// 
    /// </value>
    [Category("Presentation")]
    public State State
    {
      get
      {
        return (State) this.GetValue(StateIndicatorControl.StateProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets the actual model used to draw elements of a State Indicator.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.StateIndicatorControl"/> class descendant that is the actual model.
    /// 
    /// </value>
    [Category("Presentation")]
    public StateIndicatorModel ActualModel
    {
      get
      {
        return (StateIndicatorModel) this.GetValue(StateIndicatorControl.ActualModelProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns the total number of all states (both predefined and additional) that are currently available in the State Indicator control.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An integer value that is the number of the state indicator's states.
    /// 
    /// </value>
    [Category("Data")]
    public int StateCount
    {
      get
      {
        return (int) this.GetValue(StateIndicatorControl.StateCountProperty);
      }
    }

    private IEnumerable<StateInfo> StatesInfo
    {
      get
      {
        foreach (State state in this.actualStates)
          yield return state.ElementInfo;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns a list of predefined models for a State Indicator control.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Collections.Generic.List`1"/> of <see cref="T:DevExpress.Xpf.Gauges.PredefinedElementKind"/> objects.
    /// 
    /// </value>
    public static List<PredefinedElementKind> PredefinedModels
    {
      get
      {
        return PredefinedStateIndicatorModels.ModelKinds;
      }
    }

    private State ActualDefaultState
    {
      get
      {
        if (this.DefaultState == null)
          return this.ActualModel.DefaultState;
        return this.DefaultState;
      }
    }

    static StateIndicatorControl()
    {
      About.CheckLicenseShowNagScreen(typeof (StateIndicatorControl));
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the StateIndicatorControl class with default settings.
    /// 
    /// </para>
    /// 
    /// </summary>
    public StateIndicatorControl()
    {
      this.DefaultStyleKey = (object) typeof (StateIndicatorControl);
      this.SetValue(StateIndicatorControl.AdditionalStatesPropertyKey, (object) new StateCollection(this));
      this.SetValue(StateIndicatorControl.ActualModelPropertyKey, (object) new EmptyStateIndicatorModel());
    }

    private static void ModelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      StateIndicatorControl indicatorControl = d as StateIndicatorControl;
      if (indicatorControl == null)
        return;
      StateIndicatorModel stateIndicatorModel = e.NewValue as StateIndicatorModel;
      if (stateIndicatorModel == null)
        indicatorControl.SetValue(StateIndicatorControl.ActualModelPropertyKey, (object) new EmptyStateIndicatorModel());
      else
        indicatorControl.SetValue(StateIndicatorControl.ActualModelPropertyKey, (object) stateIndicatorModel);
    }

    private static void ActualModelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      IModelSupported modelSupported = d as IModelSupported;
      IOwnedElement ownedElement = e.NewValue as IOwnedElement;
      if (ownedElement != null)
        ownedElement.Owner = (object) (d as StateIndicatorControl);
      if (modelSupported == null)
        return;
      modelSupported.UpdateModel();
    }

    private static void StateIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      StateIndicatorControl indicatorControl = d as StateIndicatorControl;
      if (indicatorControl == null)
        return;
      indicatorControl.UpdateActualState();
    }

    private static void DefaultStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      StateIndicatorControl indicatorControl = d as StateIndicatorControl;
      if (indicatorControl == null || indicatorControl.StateIndex >= 0 && indicatorControl.StateIndex < indicatorControl.StateCount)
        return;
      indicatorControl.UpdateActualState();
    }

    internal static void ValueIndicatorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      StateIndicatorControl stateControl = d as StateIndicatorControl;
      ValueIndicatorBase valueIndicatorBase1 = e.OldValue as ValueIndicatorBase;
      if (valueIndicatorBase1 != null)
        valueIndicatorBase1.StateIndicator = (StateIndicatorControl) null;
      ValueIndicatorBase valueIndicatorBase2 = e.NewValue as ValueIndicatorBase;
      if (valueIndicatorBase2 != null)
        valueIndicatorBase2.StateIndicator = stateControl;
      if (stateControl == null)
        return;
      stateControl.SubscribeRangeCollectionEvents(valueIndicatorBase2 != null ? valueIndicatorBase2.Scale : (Scale) null, valueIndicatorBase1 != null ? valueIndicatorBase1.Scale : (Scale) null);
      stateControl.UpdateStateIndexByValueIndicator(AnalogGaugeControl.GetValueIndicator(stateControl));
    }

    void IModelSupported.UpdateModel()
    {
      this.UpdateStates();
    }

    ElementLayout ILayoutCalculator.CreateLayout(Size constraint)
    {
      return new ElementLayout();
    }

    void ILayoutCalculator.CompleteLayout(ElementInfoBase elementInfo)
    {
      if (elementInfo.Layout == null)
        return;
      elementInfo.Layout.CompleteLayout(new Point(0.0, 0.0), (Transform) null, (Geometry) null);
    }

    internal void SubscribeRangeCollectionEvents(Scale newScale, Scale oldScale)
    {
      if (oldScale != null)
      {
        IList list = oldScale is ArcScale ? (IList) ((ArcScale) oldScale).Ranges : (IList) ((LinearScale) oldScale).Ranges;
        INotifyCollectionChanged collectionChanged = list as INotifyCollectionChanged;
        if (collectionChanged != null)
          collectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.RangeCollectionChanged);
        foreach (object obj in (IEnumerable) list)
          this.UnsubscribeRangeEvents(obj as RangeBase);
      }
      if (newScale != null)
      {
        this.currentRangeCollection = newScale is ArcScale ? (IList) ((ArcScale) newScale).Ranges : (IList) ((LinearScale) newScale).Ranges;
        INotifyCollectionChanged collectionChanged = this.currentRangeCollection as INotifyCollectionChanged;
        if (collectionChanged != null)
          collectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(this.RangeCollectionChanged);
        foreach (object obj in (IEnumerable) this.currentRangeCollection)
          this.SubscribeRangeEvents(obj as RangeBase);
      }
      else
        this.currentRangeCollection = (IList) null;
    }

    private void SubscribeRangeEvents(RangeBase range)
    {
      if (range == null)
        return;
      range.IndicatorEnter += new IndicatorEnterEventHandler(this.RangeIndicatorEnter);
      range.IndicatorLeave += new IndicatorLeaveEventHandler(this.RangeIndicatorLeave);
    }

    private void UnsubscribeRangeEvents(RangeBase range)
    {
      if (range == null)
        return;
      range.IndicatorEnter -= new IndicatorEnterEventHandler(this.RangeIndicatorEnter);
      range.IndicatorLeave -= new IndicatorLeaveEventHandler(this.RangeIndicatorLeave);
    }

    private void RangeIndicatorLeave(object sender, IndicatorLeaveEventArgs e)
    {
      ValueIndicatorBase valueIndicator = AnalogGaugeControl.GetValueIndicator(this);
      if (this.currentRangeCollection == null || valueIndicator == null || (!object.ReferenceEquals((object) valueIndicator, (object) e.Indicator) || this.StateIndex != this.currentRangeCollection.IndexOf(sender)))
        return;
      this.UpdateStateIndexByValueIndicator(AnalogGaugeControl.GetValueIndicator(this));
    }

    private void RangeIndicatorEnter(object sender, IndicatorEnterEventArgs e)
    {
      ValueIndicatorBase valueIndicator = AnalogGaugeControl.GetValueIndicator(this);
      if (this.currentRangeCollection == null || valueIndicator == null || !object.ReferenceEquals((object) valueIndicator, (object) e.Indicator))
        return;
      this.UpdateStateIndexByValueIndicator(AnalogGaugeControl.GetValueIndicator(this));
    }

    private void RangeCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.OldItems != null && (e.Action == NotifyCollectionChangedAction.Reset || e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace))
      {
        foreach (object obj in (IEnumerable) e.OldItems)
          this.UnsubscribeRangeEvents(obj as RangeBase);
      }
      if (e.NewItems != null && (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace))
      {
        foreach (object obj in (IEnumerable) e.NewItems)
          this.SubscribeRangeEvents(obj as RangeBase);
      }
      this.UpdateStateIndexByValueIndicator(AnalogGaugeControl.GetValueIndicator(this));
    }

    private void UpdateActualState()
    {
      if (this.StateIndex >= 0 && this.StateIndex < this.actualStates.Count)
        this.SetValue(StateIndicatorControl.StatePropertyKey, (object) this.actualStates[this.StateIndex]);
      else
        this.SetValue(StateIndicatorControl.StatePropertyKey, (object) this.ActualDefaultState);
      if (this.State.ElementInfo == null && this.State.Presentation != null)
        this.State.ElementInfo = new StateInfo((ILayoutCalculator) this, 0, this.State.Presentation.CreateStatePresentationControl(), (PresentationBase) this.State.Presentation);
      if (this.State.Presentation == null)
        return;
      this.State.ElementInfo.Invalidate();
    }

    internal void UpdateStateIndexByValueIndicator(ValueIndicatorBase indicator)
    {
      bool flag = false;
      if (this.currentRangeCollection != null && indicator != null)
      {
        foreach (object obj in (IEnumerable) this.currentRangeCollection)
        {
          RangeBase rangeBase = obj as RangeBase;
          if (indicator.Value >= Math.Min(rangeBase.EndValue.Value, rangeBase.StartValue.Value) && indicator.Value <= Math.Max(rangeBase.EndValue.Value, rangeBase.StartValue.Value))
          {
            this.SetValue(StateIndicatorControl.StateIndexProperty, (object) this.currentRangeCollection.IndexOf((object) rangeBase));
            flag = true;
            break;
          }
        }
      }
      if (this.currentRangeCollection != null && flag)
        return;
      this.SetValue(StateIndicatorControl.StateIndexProperty, (object) -1);
    }

    internal void UpdateStates()
    {
      this.actualStates.Clear();
      if (this.ActualModel != null)
      {
        foreach (State state in this.ActualModel.PredefinedStates)
          this.actualStates.Add(state);
      }
      if (this.AdditionalStates != null)
      {
        foreach (State state in (FreezableCollection<State>) this.AdditionalStates)
          this.actualStates.Add(state);
      }
      this.SetValue(StateIndicatorControl.StateCountPropertyKey, (object) this.actualStates.Count);
      this.UpdateActualState();
    }

    protected override Size MeasureOverride(Size constraint)
    {
      return base.MeasureOverride(new Size(double.IsInfinity(constraint.Width) ? 250.0 : constraint.Width, double.IsInfinity(constraint.Height) ? 250.0 : constraint.Height));
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
      return (AutomationPeer) new StateIndicatorControlAutomationPeer((FrameworkElement) this);
    }
  }
}
