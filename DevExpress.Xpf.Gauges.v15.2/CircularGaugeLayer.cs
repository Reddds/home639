// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.CircularGaugeLayer
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
  /// A layer of a Circular Gauge.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class CircularGaugeLayer : GaugeLayerBase
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
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (CircularGaugeLayerPresentation), typeof (CircularGaugeLayer), new PropertyMetadata((object) null, new PropertyChangedCallback(LayerBase.PresentationPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the current presentation that specifies the appearance of the layer.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.CircularGaugeLayerPresentation"/> object.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("CircularGaugeLayerPresentation")]
    [Category("Presentation")]
    public CircularGaugeLayerPresentation Presentation
    {
      get
      {
        return (CircularGaugeLayerPresentation) this.GetValue(CircularGaugeLayer.PresentationProperty);
      }
      set
      {
        this.SetValue(CircularGaugeLayer.PresentationProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns a list of predefined presentations for a circular gauge layer.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Collections.Generic.List`1"/> of <see cref="T:DevExpress.Xpf.Gauges.PredefinedElementKind"/> objects.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("CircularGaugeLayerPredefinedPresentations")]
    public static List<PredefinedElementKind> PredefinedPresentations
    {
      get
      {
        return PredefinedCircularGaugeLayerPresentations.PresentationKinds;
      }
    }

    private CircularGaugeControl Gauge
    {
      get
      {
        return base.Gauge as CircularGaugeControl;
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
          LayerModel layerModel = this.Gauge.ActualModel.GetLayerModel(this.Gauge.Layers.IndexOf(this));
          if (layerModel != null && layerModel.Presentation != null)
            return layerModel.Presentation;
        }
        return (LayerPresentation) new DefaultCircularGaugeBackgroundLayerPresentation();
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
          LayerModel layerModel = this.Gauge.ActualModel.GetLayerModel(this.Gauge.Layers.IndexOf(this));
          if (layerModel != null && layerModel.Options != null)
            return layerModel.Options;
        }
        return new LayerOptions();
      }
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new CircularGaugeLayer();
    }
  }
}
