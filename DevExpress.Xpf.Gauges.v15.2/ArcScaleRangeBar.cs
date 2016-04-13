// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ArcScaleRangeBar
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
  /// An arc scale range bar.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class ArcScaleRangeBar : ArcScaleIndicator
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
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (ArcScaleRangeBarPresentation), typeof (ArcScaleRangeBar), new PropertyMetadata((object) null, new PropertyChangedCallback(ValueIndicatorBase.PresentationPropertyChanged)));
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
    public static readonly DependencyProperty AnchorValueProperty = DependencyPropertyManager.Register("AnchorValue", typeof (double), typeof (ArcScaleRangeBar), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ValueIndicatorBase.IndicatorPropertyChanged)));
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
    public static readonly DependencyProperty OptionsProperty = DependencyPropertyManager.Register("Options", typeof (ArcScaleRangeBarOptions), typeof (ArcScaleRangeBar), new PropertyMetadata(new PropertyChangedCallback(ArcScaleRangeBar.OptionsPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the current presentation that specifies the appearance of the range bar.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.ArcScaleRangeBarPresentation"/> object.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleRangeBarPresentation")]
    [Category("Presentation")]
    public ArcScaleRangeBarPresentation Presentation
    {
      get
      {
        return (ArcScaleRangeBarPresentation) this.GetValue(ArcScaleRangeBar.PresentationProperty);
      }
      set
      {
        this.SetValue(ArcScaleRangeBar.PresentationProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the value on a scale that specifies a fixed edge of the range bar.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value on a scale.
    /// 
    /// </value>
    [Category("Data")]
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleRangeBarAnchorValue")]
    public double AnchorValue
    {
      get
      {
        return (double) this.GetValue(ArcScaleRangeBar.AnchorValueProperty);
      }
      set
      {
        this.SetValue(ArcScaleRangeBar.AnchorValueProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the options of a range bar that specify its shape and position on a Circular scale.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.ArcScaleRangeBarOptions"/> object that contains the settings of the range bar.
    /// 
    /// </value>
    [Category("Presentation")]
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleRangeBarOptions")]
    public ArcScaleRangeBarOptions Options
    {
      get
      {
        return (ArcScaleRangeBarOptions) this.GetValue(ArcScaleRangeBar.OptionsProperty);
      }
      set
      {
        this.SetValue(ArcScaleRangeBar.OptionsProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns a list of predefined presentations for an arc scale range bar.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Collections.Generic.List`1"/> of <see cref="T:DevExpress.Xpf.Gauges.PredefinedElementKind"/> objects.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleRangeBarPredefinedPresentations")]
    public static List<PredefinedElementKind> PredefinedPresentations
    {
      get
      {
        return PredefinedArcScaleRangeBarPresentations.PresentationKinds;
      }
    }

    private ArcScaleRangeBarModel Model
    {
      get
      {
        if (this.Gauge == null || this.Gauge.ActualModel == null)
          return (ArcScaleRangeBarModel) null;
        return this.Gauge.ActualModel.GetRangeBarModel(this.Scale.RangeBars.IndexOf(this));
      }
    }

    protected override int ActualZIndex
    {
      get
      {
        return this.ActualOptions.ZIndex;
      }
    }

    protected override ValueIndicatorPresentation ActualPresentation
    {
      get
      {
        if (this.Presentation != null)
          return (ValueIndicatorPresentation) this.Presentation;
        if (this.Model != null && this.Model.Presentation != null)
          return (ValueIndicatorPresentation) this.Model.Presentation;
        return (ValueIndicatorPresentation) new DefaultArcScaleRangeBarPresentation();
      }
    }

    internal ArcScaleRangeBarOptions ActualOptions
    {
      get
      {
        if (this.Options != null)
          return this.Options;
        if (this.Model != null && this.Model.Options != null)
          return this.Model.Options;
        return new ArcScaleRangeBarOptions();
      }
    }

    private static void OptionsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ArcScaleRangeBar arcScaleRangeBar = d as ArcScaleRangeBar;
      if (arcScaleRangeBar == null)
        return;
      CommonUtils.SubscribePropertyChangedWeakEvent((INotifyPropertyChanged) (e.OldValue as ArcScaleRangeBarOptions), (INotifyPropertyChanged) (e.NewValue as ArcScaleRangeBarOptions), (IWeakEventListener) arcScaleRangeBar);
      arcScaleRangeBar.OnOptionsChanged();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new ArcScaleRangeBar();
    }

    protected override ElementLayout CreateLayout(Size constraint)
    {
      if (this.Scale != null && this.Scale.Mapping != null && !this.Scale.Mapping.Layout.IsEmpty)
        return ArcSegmentCalculator.CreateRangeLayout(this.Scale.Mapping, this.ActualOptions.Offset, this.ActualOptions.Thickness);
      return (ElementLayout) null;
    }

    protected override void CompleteLayout(ElementInfoBase elementInfo)
    {
      double limitedValue = this.Scale.GetLimitedValue(this.AnchorValue);
      ArcSegmentCalculator.CompleteRangeLayout(elementInfo, this.Scale, limitedValue, this.ActualValue, this.ActualOptions.Offset, this.ActualOptions.Thickness);
    }
  }
}
