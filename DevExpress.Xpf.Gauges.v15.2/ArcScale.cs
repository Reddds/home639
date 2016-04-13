// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ArcScale
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Utils;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// An arc scale.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class ArcScale : Scale
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
    public static readonly DependencyProperty SpindleCapPresentationProperty = DependencyPropertyManager.Register("SpindleCapPresentation", typeof (SpindleCapPresentation), typeof (ArcScale), new PropertyMetadata((object) null, new PropertyChangedCallback(Scale.PresentationPropertyChanged)));
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
    public static readonly DependencyProperty LayoutModeProperty = DependencyPropertyManager.Register("LayoutMode", typeof (ArcScaleLayoutMode), typeof (ArcScale), new PropertyMetadata((object) ArcScaleLayoutMode.Auto, new PropertyChangedCallback(ArcScale.LayoutModePropertyChanged)));
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
    public static readonly DependencyProperty StartAngleProperty = DependencyPropertyManager.Register("StartAngle", typeof (double), typeof (ArcScale), new PropertyMetadata((object) -240.0, new PropertyChangedCallback(ArcScale.AnglesPropertyChanged)));
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
    public static readonly DependencyProperty EndAngleProperty = DependencyPropertyManager.Register("EndAngle", typeof (double), typeof (ArcScale), new PropertyMetadata((object) 60.0, new PropertyChangedCallback(ArcScale.AnglesPropertyChanged)));
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
    public static readonly DependencyProperty LabelOptionsProperty = DependencyPropertyManager.Register("LabelOptions", typeof (ArcScaleLabelOptions), typeof (ArcScale), new PropertyMetadata((object) null, new PropertyChangedCallback(Scale.LabelOptionsPropertyChanged)));
    internal static readonly DependencyPropertyKey NeedlesPropertyKey = DependencyPropertyManager.RegisterReadOnly("Needles", typeof (ArcScaleNeedleCollection), typeof (ArcScale), new PropertyMetadata());
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
    public static readonly DependencyProperty NeedlesProperty = ArcScale.NeedlesPropertyKey.DependencyProperty;
    internal static readonly DependencyPropertyKey RangeBarsPropertyKey = DependencyPropertyManager.RegisterReadOnly("RangeBars", typeof (ArcScaleRangeBarCollection), typeof (ArcScale), new PropertyMetadata());
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
    public static readonly DependencyProperty RangeBarsProperty = ArcScale.RangeBarsPropertyKey.DependencyProperty;
    internal static readonly DependencyPropertyKey MarkersPropertyKey = DependencyPropertyManager.RegisterReadOnly("Markers", typeof (ArcScaleMarkerCollection), typeof (ArcScale), new PropertyMetadata());
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
    public static readonly DependencyProperty MarkersProperty = ArcScale.MarkersPropertyKey.DependencyProperty;
    internal static readonly DependencyPropertyKey LayersPropertyKey = DependencyPropertyManager.RegisterReadOnly("Layers", typeof (ArcScaleLayerCollection), typeof (ArcScale), new PropertyMetadata());
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
    public static readonly DependencyProperty LayersProperty = ArcScale.LayersPropertyKey.DependencyProperty;
    internal static readonly DependencyPropertyKey RangesPropertyKey = DependencyPropertyManager.RegisterReadOnly("Ranges", typeof (ArcScaleRangeCollection), typeof (ArcScale), new PropertyMetadata());
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
    public static readonly DependencyProperty RangesProperty = ArcScale.RangesPropertyKey.DependencyProperty;
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
    public static readonly DependencyProperty ShowSpindleCapProperty = DependencyPropertyManager.Register("ShowSpindleCap", typeof (DefaultBoolean), typeof (ArcScale), new PropertyMetadata((object) DefaultBoolean.Default, new PropertyChangedCallback(Scale.InvalidateLayout)));
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
    public static readonly DependencyProperty SpindleCapOptionsProperty = DependencyPropertyManager.Register("SpindleCapOptions", typeof (SpindleCapOptions), typeof (ArcScale), new PropertyMetadata((object) null, new PropertyChangedCallback(ArcScale.SpindleCapOptionsPropertyChanged)));
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
    public static readonly DependencyProperty LinePresentationProperty = DependencyPropertyManager.Register("LinePresentation", typeof (ArcScaleLinePresentation), typeof (ArcScale), new PropertyMetadata((object) null, new PropertyChangedCallback(Scale.PresentationPropertyChanged)));
    private const double halfTopStartAngleLimit1 = 180.0;
    private const double halfTopStartAngleLimit2 = 225.0;
    private const double halfTopEndAngleLimit1 = 315.0;
    private const double halfTopEndAngleLimit2 = 360.0;
    private const double quarterTopLeftStartAngleLimit1 = 180.0;
    private const double quarterTopLeftStartAngleLimit2 = 270.0;
    private const double quarterTopLeftEndAngleLimit1 = 180.0;
    private const double quarterTopLeftEndAngleLimit2 = 270.0;
    private const double quarterTopRightStartAngleLimit1 = 270.0;
    private const double quarterTopRightStartAngleLimit2 = 360.0;
    private const double quarterTopRightEndAngleLimit1 = 270.0;
    private const double quarterTopRightEndAngleLimit2 = 360.0;
    private const double threeQuartersStartAngleLimit1 = 135.0;
    private const double threeQuartersStartAngleLimit2 = 180.0;
    private const double threeQuartersEndAngleLimit1 = 0.0;
    private const double threeQuartersEndAngleLimit2 = 45.0;
    private readonly SpindleCap spindleCap;
    private ArcScaleLayoutMode actualLayoutMode;

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the current presentation that specifies the appearance of the spindle cap.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.SpindleCapPresentation"/> object.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleSpindleCapPresentation")]
    [Category("Presentation")]
    public SpindleCapPresentation SpindleCapPresentation
    {
      get
      {
        return (SpindleCapPresentation) this.GetValue(ArcScale.SpindleCapPresentationProperty);
      }
      set
      {
        this.SetValue(ArcScale.SpindleCapPresentationProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides different types of layouts for the Circular Scale.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.ArcScaleLayoutMode"/> enumeration value that specifies the possible ways a circular scale can be positioned.
    /// 
    /// </value>
    [Category("Layout")]
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleLayoutMode")]
    public ArcScaleLayoutMode LayoutMode
    {
      get
      {
        return (ArcScaleLayoutMode) this.GetValue(ArcScale.LayoutModeProperty);
      }
      set
      {
        this.SetValue(ArcScale.LayoutModeProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the angle that specifies the scale start position.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that is the start angle of the scale.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleStartAngle")]
    [Category("Presentation")]
    public double StartAngle
    {
      get
      {
        return (double) this.GetValue(ArcScale.StartAngleProperty);
      }
      set
      {
        this.SetValue(ArcScale.StartAngleProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the angle that specifies the scale end position.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that is the end angle of the scale.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleEndAngle")]
    [Category("Presentation")]
    public double EndAngle
    {
      get
      {
        return (double) this.GetValue(ArcScale.EndAngleProperty);
      }
      set
      {
        this.SetValue(ArcScale.EndAngleProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to a collection of needles contained in the current Circular Scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.ArcScaleNeedleCollection"/> object that contains scale needles.
    /// 
    /// </value>
    [Category("Elements")]
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleNeedles")]
    public ArcScaleNeedleCollection Needles
    {
      get
      {
        return (ArcScaleNeedleCollection) this.GetValue(ArcScale.NeedlesProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to a collection of range bars contained in the current Circular Scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.ArcScaleRangeBarCollection"/> object that contains scale range bars.
    /// 
    /// </value>
    [Category("Elements")]
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleRangeBars")]
    public ArcScaleRangeBarCollection RangeBars
    {
      get
      {
        return (ArcScaleRangeBarCollection) this.GetValue(ArcScale.RangeBarsProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to a collection of markers contained in the current Circular Scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.ArcScaleMarkerCollection"/> object that contains scale markers.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleMarkers")]
    [Category("Elements")]
    public ArcScaleMarkerCollection Markers
    {
      get
      {
        return (ArcScaleMarkerCollection) this.GetValue(ArcScale.MarkersProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the options that specify the position and format for labels displayed on the scale.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.ArcScaleLabelOptions"/> object that contains label settings.
    /// 
    /// </value>
    [Category("Presentation")]
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleLabelOptions")]
    public ArcScaleLabelOptions LabelOptions
    {
      get
      {
        return (ArcScaleLabelOptions) this.GetValue(ArcScale.LabelOptionsProperty);
      }
      set
      {
        this.SetValue(ArcScale.LabelOptionsProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to a collection of layers contained in the current Circular scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.ArcScaleLayerCollection"/> object that contains scale layers.
    /// 
    /// </value>
    [Category("Elements")]
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleLayers")]
    public ArcScaleLayerCollection Layers
    {
      get
      {
        return (ArcScaleLayerCollection) this.GetValue(ArcScale.LayersProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to a collection of ranges contained in the current Circular Scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.ArcScaleRangeCollection"/>  object that contains scale ranges.
    /// 
    /// </value>
    [Category("Elements")]
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleRanges")]
    public ArcScaleRangeCollection Ranges
    {
      get
      {
        return (ArcScaleRangeCollection) this.GetValue(ArcScale.RangesProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value indicating whether a spindle cap should be shown or not.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> to show a spindle cap; <b>false</b> to hide it.
    /// 
    /// </value>
    [Category("Behavior")]
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleShowSpindleCap")]
    public DefaultBoolean ShowSpindleCap
    {
      get
      {
        return (DefaultBoolean) this.GetValue(ArcScale.ShowSpindleCapProperty);
      }
      set
      {
        this.SetValue(ArcScale.ShowSpindleCapProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the options that specify the position and format for a spindle cap displayed on the scale.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.SpindleCapOptions"/> object that contains spindle cap settings.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleSpindleCapOptions")]
    [Category("Presentation")]
    public SpindleCapOptions SpindleCapOptions
    {
      get
      {
        return (SpindleCapOptions) this.GetValue(ArcScale.SpindleCapOptionsProperty);
      }
      set
      {
        this.SetValue(ArcScale.SpindleCapOptionsProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the current presentation that specifies the appearance of a line.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.ArcScaleLinePresentation"/> object.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleLinePresentation")]
    [Category("Presentation")]
    public ArcScaleLinePresentation LinePresentation
    {
      get
      {
        return (ArcScaleLinePresentation) this.GetValue(ArcScale.LinePresentationProperty);
      }
      set
      {
        this.SetValue(ArcScale.LinePresentationProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Contains the list of predefined presentations for the Spindle Cap element.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Collections.Generic.List`1"/> of <see cref="T:DevExpress.Xpf.Gauges.PredefinedElementKind"/> objects.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ArcScalePredefinedSpindleCapPresentations")]
    public static List<PredefinedElementKind> PredefinedSpindleCapPresentations
    {
      get
      {
        return DevExpress.Xpf.Gauges.Native.PredefinedSpindleCapPresentations.PresentationKinds;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns a list of predefined presentations for lines.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Collections.Generic.List`1"/> of <see cref="T:DevExpress.Xpf.Gauges.PredefinedElementKind"/> objects.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ArcScalePredefinedLinePresentations")]
    public static List<PredefinedElementKind> PredefinedLinePresentations
    {
      get
      {
        return PredefinedArcScaleLinePresentations.PresentationKinds;
      }
    }

    internal override ScaleLinePresentation ActualLinePresentation
    {
      get
      {
        if (this.LinePresentation != null)
          return (ScaleLinePresentation) this.LinePresentation;
        if (this.Model != null && ((ArcScaleModel) this.Model).LinePresentation != null)
          return (ScaleLinePresentation) ((ArcScaleModel) this.Model).LinePresentation;
        return (ScaleLinePresentation) new DefaultArcScaleLinePresentation();
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets an actual value indicating the visibility of a Spindle Cap.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> to show a spindle cap; otherwise, <b>false</b>.
    /// 
    /// </value>
    public bool ActualShowSpindleCap
    {
      get
      {
        if (this.ShowSpindleCap == DefaultBoolean.Default && this.Model != null)
          return ((ArcScaleModel) this.Model).ShowSpindleCap;
        return this.ShowSpindleCap != DefaultBoolean.False;
      }
    }

    protected override ScaleModel Model
    {
      get
      {
        if (this.Gauge == null || this.Gauge.ActualModel == null)
          return (ScaleModel) null;
        return (ScaleModel) this.Gauge.ActualModel.GetScaleModel(this.ActualLayoutMode, this.Gauge.Scales.IndexOf(this));
      }
    }

    internal SpindleCapPresentation ActualSpindleCapPresentation
    {
      get
      {
        if (this.SpindleCapPresentation != null)
          return this.SpindleCapPresentation;
        if (this.Model != null && ((ArcScaleModel) this.Model).SpindleCapPresentation != null)
          return ((ArcScaleModel) this.Model).SpindleCapPresentation;
        return (SpindleCapPresentation) new DefaultSpindleCapPresentation();
      }
    }

    protected internal override ScaleLabelOptions ActualLabelOptions
    {
      get
      {
        if (this.LabelOptions != null)
          return (ScaleLabelOptions) this.LabelOptions;
        if (this.Model != null && ((ArcScaleModel) this.Model).LabelOptions != null)
          return (ScaleLabelOptions) ((ArcScaleModel) this.Model).LabelOptions;
        return (ScaleLabelOptions) new ArcScaleLabelOptions();
      }
    }

    protected internal override IEnumerable<IElementInfo> Elements
    {
      get
      {
        foreach (ArcScaleLayer arcScaleLayer in (FreezableCollection<ArcScaleLayer>) this.Layers)
          yield return (IElementInfo) arcScaleLayer.ElementInfo;
        foreach (ValueIndicatorBase valueIndicatorBase in this.Indicators)
          yield return (IElementInfo) valueIndicatorBase.ElementInfo;
        foreach (ArcScaleRange arcScaleRange in (FreezableCollection<ArcScaleRange>) this.Ranges)
          yield return (IElementInfo) arcScaleRange.ElementInfo;
        yield return (IElementInfo) this.LineInfo;
        yield return (IElementInfo) this.MinorTickmarksInfo;
        yield return (IElementInfo) this.MajorTickmarksInfo;
        yield return (IElementInfo) this.LabelsInfo;
        foreach (ScaleCustomLabel scaleCustomLabel in (Collection<ScaleCustomLabel>) this.CustomLabels)
          yield return (IElementInfo) scaleCustomLabel;
        foreach (ScaleCustomElement scaleCustomElement in (Collection<ScaleCustomElement>) this.CustomElements)
          yield return (IElementInfo) scaleCustomElement;
        yield return (IElementInfo) this.SpindeleCapInfo;
      }
    }

    protected internal override IEnumerable<ValueIndicatorBase> Indicators
    {
      get
      {
        if (this.Needles != null)
        {
          foreach (ArcScaleIndicator arcScaleIndicator in (FreezableCollection<ArcScaleNeedle>) this.Needles)
            yield return (ValueIndicatorBase) arcScaleIndicator;
        }
        if (this.Markers != null)
        {
          foreach (ArcScaleIndicator arcScaleIndicator in (FreezableCollection<ArcScaleMarker>) this.Markers)
            yield return (ValueIndicatorBase) arcScaleIndicator;
        }
        if (this.RangeBars != null)
        {
          foreach (ArcScaleIndicator arcScaleIndicator in (FreezableCollection<ArcScaleRangeBar>) this.RangeBars)
            yield return (ValueIndicatorBase) arcScaleIndicator;
        }
      }
    }

    internal SpindleCapOptions ActualSpindleCapOptions
    {
      get
      {
        if (this.SpindleCapOptions != null)
          return this.SpindleCapOptions;
        if (this.Model != null && ((ArcScaleModel) this.Model).SpindleCapOptions != null)
          return ((ArcScaleModel) this.Model).SpindleCapOptions;
        return new SpindleCapOptions();
      }
    }

    internal LayerInfo SpindeleCapInfo
    {
      get
      {
        return this.spindleCap.ElementInfo;
      }
    }

    internal ArcScaleMapping Mapping
    {
      get
      {
        return base.Mapping as ArcScaleMapping;
      }
    }

    internal CircularGaugeControl Gauge
    {
      get
      {
        return base.Gauge as CircularGaugeControl;
      }
    }

    internal ArcScaleLayoutMode ActualLayoutMode
    {
      get
      {
        return this.actualLayoutMode;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the ArcScale class with default settings.
    /// 
    /// </para>
    /// 
    /// </summary>
    public ArcScale()
    {
      this.DefaultStyleKey = (object) typeof (ArcScale);
      this.spindleCap = new SpindleCap(this);
      this.SetValue(ArcScale.NeedlesPropertyKey, (object) new ArcScaleNeedleCollection(this));
      this.SetValue(ArcScale.RangeBarsPropertyKey, (object) new ArcScaleRangeBarCollection(this));
      this.SetValue(ArcScale.MarkersPropertyKey, (object) new ArcScaleMarkerCollection(this));
      this.SetValue(ArcScale.LayersPropertyKey, (object) new ArcScaleLayerCollection(this));
      this.SetValue(ArcScale.RangesPropertyKey, (object) new ArcScaleRangeCollection(this));
      this.UpdateElementsInfo();
    }

    private static void LayoutModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ArcScale arcScale = d as ArcScale;
      if (arcScale == null)
        return;
      arcScale.CalculateActualLayoutMode();
      arcScale.UpdateModel();
      arcScale.Invalidate();
    }

    private static void AnglesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ArcScale arcScale = d as ArcScale;
      if (arcScale == null)
        return;
      if (arcScale.LayoutMode == ArcScaleLayoutMode.Auto)
      {
        arcScale.CalculateActualLayoutMode();
        arcScale.UpdateModel();
      }
      arcScale.Invalidate();
    }

    private static void SpindleCapOptionsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ArcScale arcScale = d as ArcScale;
      if (arcScale == null)
        return;
      CommonUtils.SubscribePropertyChangedWeakEvent((INotifyPropertyChanged) (e.OldValue as SpindleCapOptions), (INotifyPropertyChanged) (e.NewValue as SpindleCapOptions), (IWeakEventListener) arcScale);
      arcScale.OnSpindleCapOptionsChanged();
    }

    protected override void UpdateModel()
    {
      base.UpdateModel();
      this.OnSpindleCapOptionsChanged();
      foreach (IModelSupported modelSupported in (FreezableCollection<ArcScaleLayer>) this.Layers)
        modelSupported.UpdateModel();
      foreach (IModelSupported modelSupported in (FreezableCollection<ArcScaleRange>) this.Ranges)
        modelSupported.UpdateModel();
    }

    protected override bool PerformWeakEvent(Type managerType, object sender, EventArgs e)
    {
      bool flag = base.PerformWeakEvent(managerType, sender, e);
      if (managerType == typeof (PropertyChangedWeakEventManager))
      {
        if (sender is SpindleCapOptions)
          this.OnSpindleCapOptionsChanged();
        flag = true;
      }
      return flag;
    }

    private void OnSpindleCapOptionsChanged()
    {
      if (this.spindleCap.ElementInfo == null)
        return;
      this.spindleCap.UpdateModel();
      this.Invalidate();
    }

    internal void CalculateActualLayoutMode()
    {
      double startAngle = MathUtils.NormalizeDegree(this.StartAngle);
      double endAngle = MathUtils.NormalizeDegree(this.EndAngle);
      double num = Math.Abs(this.EndAngle - this.StartAngle);
      if (this.LayoutMode != ArcScaleLayoutMode.Auto)
      {
        this.actualLayoutMode = this.LayoutMode;
      }
      else
      {
        this.actualLayoutMode = ArcScaleLayoutMode.Auto;
        if (this.IsAnglesInRanges(startAngle, endAngle, 180.0, 225.0, 315.0, 360.0) && num <= 180.0 && MathUtils.NormalizeDegree((this.StartAngle + this.EndAngle) / 2.0) > 180.0)
          this.actualLayoutMode = ArcScaleLayoutMode.HalfTop;
        if (this.IsAnglesInRanges(startAngle, endAngle, 180.0, 270.0, 180.0, 270.0) && num <= 90.0)
          this.actualLayoutMode = ArcScaleLayoutMode.QuarterTopLeft;
        if (this.IsAnglesInRanges(startAngle, endAngle, 270.0, 360.0, 270.0, 360.0) && num <= 90.0)
          this.actualLayoutMode = ArcScaleLayoutMode.QuarterTopRight;
        if (this.IsAnglesInRanges(startAngle, endAngle, 135.0, 180.0, 0.0, 45.0) && num > 180.0)
          this.actualLayoutMode = ArcScaleLayoutMode.ThreeQuarters;
        if (num < 360.0 && this.actualLayoutMode != ArcScaleLayoutMode.Auto)
          return;
        this.actualLayoutMode = ArcScaleLayoutMode.Circle;
      }
    }

    private bool IsAngleInRange(double angle, double startAngle, double endAngle)
    {
      if (endAngle != 360.0)
      {
        if (angle >= startAngle)
          return angle <= endAngle;
        return false;
      }
      if (angle < startAngle || angle >= endAngle)
        return angle == 0.0;
      return true;
    }

    private bool IsAnglesInRanges(double startAngle, double endAngle, double startAngleLimit1, double startAngleLimit2, double endAngleLimit1, double endAngleLimit2)
    {
      return this.IsAngleInRange(startAngle, startAngleLimit1, startAngleLimit2) && this.IsAngleInRange(endAngle, endAngleLimit1, endAngleLimit2) || this.IsAngleInRange(endAngle, startAngleLimit1, startAngleLimit2) && this.IsAngleInRange(startAngle, endAngleLimit1, endAngleLimit2);
    }

    protected override ScaleElementLayout CalculateLineLayout()
    {
      if (!this.ActualShowLine || this.Mapping.Layout.IsEmpty)
        return (ScaleElementLayout) null;
      Geometry clipGeometry = ArcSegmentCalculator.CalculateGeometry(this.Mapping, this.Mapping.Scale.StartAngle, this.Mapping.Scale.EndAngle - this.Mapping.Scale.StartAngle, this.ActualLineOptions.Offset, this.ActualLineOptions.Thickness);
      Size size = new Size(Math.Max(this.Mapping.Layout.EllipseWidth + 2.0 * this.ActualLineOptions.Offset + (double) this.ActualLineOptions.Thickness, 0.0), Math.Max(this.Mapping.Layout.EllipseHeight + 2.0 * this.ActualLineOptions.Offset + (double) this.ActualLineOptions.Thickness, 0.0));
      clipGeometry.Transform = (Transform) new TranslateTransform()
      {
        X = (size.Width / 2.0 - this.Mapping.Layout.EllipseCenter.X),
        Y = (size.Height / 2.0 - this.Mapping.Layout.EllipseCenter.Y)
      };
      return new ScaleElementLayout(this.Mapping.Layout.EllipseCenter, size, clipGeometry);
    }

    protected override ScaleMapping CalculateMapping(Size constraint)
    {
      return (ScaleMapping) new ArcScaleMapping(this, new Rect(0.0, 0.0, constraint.Width, constraint.Height));
    }

    protected internal override void UpdateElementsInfo()
    {
      base.UpdateElementsInfo();
    }

    protected internal override void CheckIndicatorEnterLeaveRange(ValueIndicatorBase indicator, double oldValue, double newValue)
    {
      foreach (RangeBase rangeBase in (FreezableCollection<ArcScaleRange>) this.Ranges)
        rangeBase.OnIndicatorEnterLeave(indicator, oldValue, newValue);
    }
  }
}
