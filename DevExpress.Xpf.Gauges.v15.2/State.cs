// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.State
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
  /// A state of a state indicator control.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class State : GaugeDependencyObject
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
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (StatePresentation), typeof (State), new PropertyMetadata((object) null, new PropertyChangedCallback(State.PresentationPropertyChanged)));
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
    public static readonly DependencyProperty ElementInfoProperty = DependencyPropertyManager.Register("ElementInfo", typeof (StateInfo), typeof (State), new PropertyMetadata((PropertyChangedCallback) null));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the current presentation that specifies the appearance of the state indicator control.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.StatePresentation"/> object.
    /// 
    /// </value>
    [Category("Presentation")]
    public StatePresentation Presentation
    {
      get
      {
        return (StatePresentation) this.GetValue(State.PresentationProperty);
      }
      set
      {
        this.SetValue(State.PresentationProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// This property is hidden and intended for internal use only.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.StateInfo"/> object.
    /// 
    /// </value>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public StateInfo ElementInfo
    {
      get
      {
        return (StateInfo) this.GetValue(State.ElementInfoProperty);
      }
      set
      {
        this.SetValue(State.ElementInfoProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns a list of predefined presentations for a state.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Collections.Generic.List`1"/> of <see cref="T:DevExpress.Xpf.Gauges.PredefinedElementKind"/> objects.
    /// 
    /// </value>
    public static List<PredefinedElementKind> PredefinedPresentations
    {
      get
      {
        return PredefinedStatePresentations.PresentationKinds;
      }
    }

    private static void PresentationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      State state = d as State;
      if (state == null || state.ElementInfo == null)
        return;
      StatePresentation statePresentation = e.NewValue as StatePresentation;
      state.ElementInfo.Presentation = (PresentationBase) statePresentation;
      state.ElementInfo.PresentationControl = statePresentation != null ? statePresentation.CreateStatePresentationControl() : (PresentationControl) null;
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new State();
    }
  }
}
