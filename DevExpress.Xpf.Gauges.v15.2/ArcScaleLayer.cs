// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ArcScaleLayer
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A layer that contains properties to define the visual presentation of a circular scale.
  /// 
  /// 
  /// </para>
  /// 
  /// </summary>
  public class ArcScaleLayer : ScaleLayerBase
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
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (ArcScaleLayerPresentation), typeof (ArcScaleLayer), new PropertyMetadata((object) null, new PropertyChangedCallback(LayerBase.PresentationPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the current presentation that specifies the appearance of the layer.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.ArcScaleLayerPresentation"/> object.
    /// 
    /// </value>
    [Category("Presentation")]
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleLayerPresentation")]
    public ArcScaleLayerPresentation Presentation
    {
      get
      {
        return (ArcScaleLayerPresentation) this.GetValue(ArcScaleLayer.PresentationProperty);
      }
      set
      {
        this.SetValue(ArcScaleLayer.PresentationProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns a list of predefined presentations for an arc scale layer.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Collections.Generic.List`1"/> of <see cref="T:DevExpress.Xpf.Gauges.PredefinedElementKind"/> objects.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleLayerPredefinedPresentations")]
    public static List<PredefinedElementKind> PredefinedPresentations
    {
      get
      {
        return PredefinedArcScaleLayerPresentations.PresentationKinds;
      }
    }

    private ArcScale Scale
    {
      get
      {
        return this.Owner as ArcScale;
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

    protected override LayerPresentation ActualPresentation
    {
      get
      {
        if (this.Presentation != null)
          return (LayerPresentation) this.Presentation;
        if (this.Gauge != null && this.Gauge.ActualModel != null)
        {
          LayerModel scaleLayerModel = this.Gauge.ActualModel.GetScaleLayerModel(this.Scale.ActualLayoutMode, this.Scale.Layers.IndexOf(this));
          if (scaleLayerModel != null && scaleLayerModel.Presentation != null)
            return scaleLayerModel.Presentation;
        }
        return (LayerPresentation) new DefaultArcScaleBackgroundLayerPresentation();
      }
    }

    protected override int ActualZIndex
    {
      get
      {
        return this.ActualOptions.ZIndex;
      }
    }

    internal LayerOptions ActualOptions
    {
      get
      {
        if (this.Options != null)
          return this.Options;
        if (this.Gauge != null && this.Gauge.ActualModel != null)
        {
          LayerModel scaleLayerModel = this.Gauge.ActualModel.GetScaleLayerModel(this.Scale.ActualLayoutMode, this.Scale.Layers.IndexOf(this));
          if (scaleLayerModel != null && scaleLayerModel.Options != null)
            return scaleLayerModel.Options;
        }
        return new LayerOptions();
      }
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new ArcScaleLayer();
    }

    protected override ElementLayout CreateLayout(Size constraint)
    {
      if (this.Scale != null && this.Scale.Mapping != null && !this.Scale.Mapping.Layout.IsEmpty)
        return new ElementLayout(this.Scale.Mapping.Layout.EllipseWidth, this.Scale.Mapping.Layout.EllipseHeight);
      return (ElementLayout) null;
    }

    protected override void CompleteLayout(ElementInfoBase elementInfo)
    {
      Point ellipseCenter = this.Scale.Mapping.Layout.EllipseCenter;
      Point layoutOffset = this.Scale.GetLayoutOffset();
      ellipseCenter.X += layoutOffset.X;
      ellipseCenter.Y += layoutOffset.Y;
      elementInfo.Layout.CompleteLayout(ellipseCenter, (Transform) null, (Geometry) null);
    }
  }
}
