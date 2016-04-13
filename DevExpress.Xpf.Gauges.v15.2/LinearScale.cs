// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LinearScale
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A linear scale.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class LinearScale : Scale
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
    public static readonly DependencyProperty LayoutModeProperty = DependencyPropertyManager.Register("LayoutMode", typeof (LinearScaleLayoutMode), typeof (LinearScale), new PropertyMetadata((object) LinearScaleLayoutMode.BottomToTop, new PropertyChangedCallback(Scale.InvalidateLayout)));
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
    public static readonly DependencyProperty LabelOptionsProperty = DependencyPropertyManager.Register("LabelOptions", typeof (LinearScaleLabelOptions), typeof (LinearScale), new PropertyMetadata((object) null, new PropertyChangedCallback(Scale.LabelOptionsPropertyChanged)));
    internal static readonly DependencyPropertyKey MarkersPropertyKey = DependencyPropertyManager.RegisterReadOnly("Markers", typeof (LinearScaleMarkerCollection), typeof (LinearScale), new PropertyMetadata());
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
    public static readonly DependencyProperty MarkersProperty = LinearScale.MarkersPropertyKey.DependencyProperty;
    internal static readonly DependencyPropertyKey RangeBarsPropertyKey = DependencyPropertyManager.RegisterReadOnly("RangeBars", typeof (LinearScaleRangeBarCollection), typeof (LinearScale), new PropertyMetadata());
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
    public static readonly DependencyProperty RangeBarsProperty = LinearScale.RangeBarsPropertyKey.DependencyProperty;
    internal static readonly DependencyPropertyKey LevelBarsPropertyKey = DependencyPropertyManager.RegisterReadOnly("LevelBars", typeof (LinearScaleLevelBarCollection), typeof (LinearScale), new PropertyMetadata());
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
    public static readonly DependencyProperty LevelBarsProperty = LinearScale.LevelBarsPropertyKey.DependencyProperty;
    internal static readonly DependencyPropertyKey LayersPropertyKey = DependencyPropertyManager.RegisterReadOnly("Layers", typeof (LinearScaleLayerCollection), typeof (LinearScale), new PropertyMetadata());
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
    public static readonly DependencyProperty LayersProperty = LinearScale.LayersPropertyKey.DependencyProperty;
    internal static readonly DependencyPropertyKey RangesPropertyKey = DependencyPropertyManager.RegisterReadOnly("Ranges", typeof (LinearScaleRangeCollection), typeof (LinearScale), new PropertyMetadata());
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
    public static readonly DependencyProperty RangesProperty = LinearScale.RangesPropertyKey.DependencyProperty;
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
    public static readonly DependencyProperty LinePresentationProperty = DependencyPropertyManager.Register("LinePresentation", typeof (LinearScaleLinePresentation), typeof (LinearScale), new PropertyMetadata((object) null, new PropertyChangedCallback(Scale.PresentationPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Provides different types of layouts for the Linear Scale.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearScaleLayoutMode"/> enumeration value that specifies the possible ways a linear scale can be positioned.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleLayoutMode")]
    [Category("Layout")]
    public LinearScaleLayoutMode LayoutMode
    {
      get
      {
        return (LinearScaleLayoutMode) this.GetValue(LinearScale.LayoutModeProperty);
      }
      set
      {
        this.SetValue(LinearScale.LayoutModeProperty, (object) value);
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
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearScaleLabelOptions"/> object that contains label settings.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleLabelOptions")]
    [Category("Presentation")]
    public LinearScaleLabelOptions LabelOptions
    {
      get
      {
        return (LinearScaleLabelOptions) this.GetValue(LinearScale.LabelOptionsProperty);
      }
      set
      {
        this.SetValue(LinearScale.LabelOptionsProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to a collection of markers contained in the current  Linear Scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearScaleMarkerCollection"/> object that contains scale markers.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleMarkers")]
    [Category("Elements")]
    public LinearScaleMarkerCollection Markers
    {
      get
      {
        return (LinearScaleMarkerCollection) this.GetValue(LinearScale.MarkersProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to a collection of range bars contained in the current Linear Scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearScaleRangeBarCollection"/> object that contains scale range bars.
    /// 
    /// </value>
    [Category("Elements")]
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleRangeBars")]
    public LinearScaleRangeBarCollection RangeBars
    {
      get
      {
        return (LinearScaleRangeBarCollection) this.GetValue(LinearScale.RangeBarsProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to a collection of level bars contained in the current Linear Scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearScaleLevelBarCollection"/> object that contains scale level bars.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleLevelBars")]
    [Category("Elements")]
    public LinearScaleLevelBarCollection LevelBars
    {
      get
      {
        return (LinearScaleLevelBarCollection) this.GetValue(LinearScale.LevelBarsProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to a collection of layers contained in the current Linear Scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearScaleLayerCollection"/> object that contains scale layers.
    /// 
    /// </value>
    [Category("Elements")]
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleLayers")]
    public LinearScaleLayerCollection Layers
    {
      get
      {
        return (LinearScaleLayerCollection) this.GetValue(LinearScale.LayersProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to a collection of ranges contained in the current Linear Scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearScaleRangeCollection"/> object that contains scale ranges.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleRanges")]
    [Category("Elements")]
    public LinearScaleRangeCollection Ranges
    {
      get
      {
        return (LinearScaleRangeCollection) this.GetValue(LinearScale.RangesProperty);
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
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearScaleLinePresentation"/> object.
    /// 
    /// 
    /// </value>
    [Category("Presentation")]
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleLinePresentation")]
    public LinearScaleLinePresentation LinePresentation
    {
      get
      {
        return (LinearScaleLinePresentation) this.GetValue(LinearScale.LinePresentationProperty);
      }
      set
      {
        this.SetValue(LinearScale.LinePresentationProperty, (object) value);
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
    [DevExpressXpfGaugesLocalizedDescription("LinearScalePredefinedLinePresentations")]
    public static List<PredefinedElementKind> PredefinedLinePresentations
    {
      get
      {
        return PredefinedLinearScaleLinePresentations.PresentationKinds;
      }
    }

    internal override ScaleLinePresentation ActualLinePresentation
    {
      get
      {
        if (this.LinePresentation != null)
          return (ScaleLinePresentation) this.LinePresentation;
        if (this.Model != null && ((LinearScaleModel) this.Model).LinePresentation != null)
          return (ScaleLinePresentation) ((LinearScaleModel) this.Model).LinePresentation;
        return (ScaleLinePresentation) new DefaultLinearScaleLinePresentation();
      }
    }

    protected override ScaleModel Model
    {
      get
      {
        if (this.Gauge == null || this.Gauge.ActualModel == null)
          return (ScaleModel) null;
        return (ScaleModel) this.Gauge.ActualModel.GetScaleModel(this.Gauge.Scales.IndexOf(this));
      }
    }

    protected internal override ScaleLabelOptions ActualLabelOptions
    {
      get
      {
        if (this.LabelOptions != null)
          return (ScaleLabelOptions) this.LabelOptions;
        if (this.Model != null && ((LinearScaleModel) this.Model).LabelOptions != null)
          return (ScaleLabelOptions) ((LinearScaleModel) this.Model).LabelOptions;
        return (ScaleLabelOptions) new LinearScaleLabelOptions();
      }
    }

    protected internal override IEnumerable<IElementInfo> Elements
    {
      get
      {
        foreach (LinearScaleLayer linearScaleLayer in (FreezableCollection<LinearScaleLayer>) this.Layers)
          yield return (IElementInfo) linearScaleLayer.ElementInfo;
        foreach (ValueIndicatorBase valueIndicatorBase in this.Indicators)
        {
          foreach (ValueIndicatorInfo valueIndicatorInfo in valueIndicatorBase.Elements)
            yield return (IElementInfo) valueIndicatorInfo;
        }
        foreach (LinearScaleRange linearScaleRange in (FreezableCollection<LinearScaleRange>) this.Ranges)
          yield return (IElementInfo) linearScaleRange.ElementInfo;
        yield return (IElementInfo) this.LineInfo;
        yield return (IElementInfo) this.MinorTickmarksInfo;
        yield return (IElementInfo) this.MajorTickmarksInfo;
        yield return (IElementInfo) this.LabelsInfo;
        foreach (ScaleCustomLabel scaleCustomLabel in (Collection<ScaleCustomLabel>) this.CustomLabels)
          yield return (IElementInfo) scaleCustomLabel;
        foreach (ScaleCustomElement scaleCustomElement in (Collection<ScaleCustomElement>) this.CustomElements)
          yield return (IElementInfo) scaleCustomElement;
      }
    }

    protected internal override IEnumerable<ValueIndicatorBase> Indicators
    {
      get
      {
        if (this.Markers != null)
        {
          foreach (LinearScaleIndicator linearScaleIndicator in (FreezableCollection<LinearScaleMarker>) this.Markers)
            yield return (ValueIndicatorBase) linearScaleIndicator;
        }
        if (this.RangeBars != null)
        {
          foreach (LinearScaleIndicator linearScaleIndicator in (FreezableCollection<LinearScaleRangeBar>) this.RangeBars)
            yield return (ValueIndicatorBase) linearScaleIndicator;
        }
        if (this.LevelBars != null)
        {
          foreach (LinearScaleIndicator linearScaleIndicator in (FreezableCollection<LinearScaleLevelBar>) this.LevelBars)
            yield return (ValueIndicatorBase) linearScaleIndicator;
        }
      }
    }

    internal LinearScaleMapping Mapping
    {
      get
      {
        return base.Mapping as LinearScaleMapping;
      }
    }

    internal LinearGaugeControl Gauge
    {
      get
      {
        return base.Gauge as LinearGaugeControl;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the LinearScale class with default settings.
    /// 
    /// </para>
    /// 
    /// </summary>
    public LinearScale()
    {
      this.DefaultStyleKey = (object) typeof (LinearScale);
      this.SetValue(LinearScale.MarkersPropertyKey, (object) new LinearScaleMarkerCollection(this));
      this.SetValue(LinearScale.RangeBarsPropertyKey, (object) new LinearScaleRangeBarCollection(this));
      this.SetValue(LinearScale.LevelBarsPropertyKey, (object) new LinearScaleLevelBarCollection(this));
      this.SetValue(LinearScale.LayersPropertyKey, (object) new LinearScaleLayerCollection(this));
      this.SetValue(LinearScale.RangesPropertyKey, (object) new LinearScaleRangeCollection(this));
      this.UpdateElementsInfo();
    }

    protected override ScaleElementLayout CalculateLineLayout()
    {
      if (!this.ActualShowLine || this.Mapping.Layout.IsEmpty)
        return (ScaleElementLayout) null;
      double angle = 0.0;
      switch (this.Mapping.Scale.LayoutMode)
      {
        case LinearScaleLayoutMode.LeftToRight:
          angle = 270.0;
          break;
        case LinearScaleLayoutMode.RightToLeft:
          angle = 90.0;
          break;
        case LinearScaleLayoutMode.BottomToTop:
          angle = 180.0;
          break;
        case LinearScaleLayoutMode.TopToBottom:
          angle = 0.0;
          break;
      }
      Point pointByPercent = this.Mapping.GetPointByPercent(0.0, this.ActualLineOptions.Offset);
      Size size = new Size((double) Math.Max(this.ActualLineOptions.Thickness, 0), Math.Max(MathUtils.CalculateDistance(pointByPercent, this.Mapping.GetPointByPercent(1.0, this.ActualLineOptions.Offset)), 0.0));
      return new ScaleElementLayout(angle, pointByPercent, size);
    }

    protected override void UpdateModel()
    {
      base.UpdateModel();
      foreach (IModelSupported modelSupported in (FreezableCollection<LinearScaleLayer>) this.Layers)
        modelSupported.UpdateModel();
      foreach (IModelSupported modelSupported in (FreezableCollection<LinearScaleRange>) this.Ranges)
        modelSupported.UpdateModel();
    }

    protected internal override void CheckIndicatorEnterLeaveRange(ValueIndicatorBase indicator, double oldValue, double newValue)
    {
      foreach (RangeBase rangeBase in (FreezableCollection<LinearScaleRange>) this.Ranges)
        rangeBase.OnIndicatorEnterLeave(indicator, oldValue, newValue);
    }

    protected override ScaleMapping CalculateMapping(Size constraint)
    {
      return (ScaleMapping) new LinearScaleMapping(this, new Rect(0.0, 0.0, constraint.Width, constraint.Height));
    }
  }
}
