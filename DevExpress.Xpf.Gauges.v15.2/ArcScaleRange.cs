// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ArcScaleRange
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
  /// An arc scale range.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class ArcScaleRange : RangeBase
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
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (ArcScaleRangePresentation), typeof (ArcScaleRange), new PropertyMetadata((object) null, new PropertyChangedCallback(LayerBase.PresentationPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the current presentation that specifies the appearance of the range.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.ArcScaleRangePresentation"/> object.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleRangePresentation")]
    [Category("Presentation")]
    public ArcScaleRangePresentation Presentation
    {
      get
      {
        return (ArcScaleRangePresentation) this.GetValue(ArcScaleRange.PresentationProperty);
      }
      set
      {
        this.SetValue(ArcScaleRange.PresentationProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns a list of predefined presentations for an arc scale range.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Collections.Generic.List`1"/> of <see cref="T:DevExpress.Xpf.Gauges.PredefinedElementKind"/> objects.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleRangePredefinedPresentations")]
    public static List<PredefinedElementKind> PredefinedPresentations
    {
      get
      {
        return PredefinedArcScaleRangePresentations.PresentationKinds;
      }
    }

    private ArcScale Scale
    {
      get
      {
        return base.Scale as ArcScale;
      }
    }

    private CircularGaugeControl Gauge
    {
      get
      {
        if (this.Scale == null)
          return (CircularGaugeControl) null;
        return this.Scale.Gauge;
      }
    }

    private ArcScaleRangeModel Model
    {
      get
      {
        if (this.Gauge == null || this.Gauge.ActualModel == null)
          return (ArcScaleRangeModel) null;
        return this.Gauge.ActualModel.GetRangeModel(this.Scale.Ranges.IndexOf(this));
      }
    }

    protected override LayerPresentation ActualPresentation
    {
      get
      {
        if (this.Presentation != null)
          return (LayerPresentation) this.Presentation;
        if (this.Model != null && this.Model.Presentation != null)
          return (LayerPresentation) this.Model.Presentation;
        return (LayerPresentation) new DefaultArcScaleRangePresentation();
      }
    }

    protected internal override RangeOptions ActualOptions
    {
      get
      {
        if (this.Options != null)
          return this.Options;
        if (this.Model != null && this.Model.Options != null)
          return this.Model.Options;
        return new RangeOptions();
      }
    }

    protected override ElementLayout CreateLayout(Size constraint)
    {
      if (this.Scale != null && this.Scale.Mapping != null && !this.Scale.Mapping.Layout.IsEmpty)
        return ArcSegmentCalculator.CreateRangeLayout(this.Scale.Mapping, this.ActualOptions.Offset, this.ActualOptions.Thickness);
      return (ElementLayout) null;
    }

    protected override void CompleteLayout(ElementInfoBase elementInfo)
    {
      double limitedValue1 = this.Scale.GetLimitedValue(this.StartValueAbsolute);
      double limitedValue2 = this.Scale.GetLimitedValue(this.EndValueAbsolute);
      ArcSegmentCalculator.CompleteRangeLayout(elementInfo, this.Scale, limitedValue1, limitedValue2, this.ActualOptions.Offset, this.ActualOptions.Thickness);
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new ArcScaleRange();
    }
  }
}
