// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LinearScaleLayer
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
  /// A layer that contains properties to define the visual presentation of a linear scale.
  /// 
  /// 
  /// 
  /// 
  /// </para>
  /// 
  /// </summary>
  public class LinearScaleLayer : ScaleLayerBase
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
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (LinearScaleLayerPresentation), typeof (LinearScaleLayer), new PropertyMetadata((object) null, new PropertyChangedCallback(LayerBase.PresentationPropertyChanged)));

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
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearScaleLayerPresentation"/> object.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleLayerPresentation")]
    [Category("Presentation")]
    public LinearScaleLayerPresentation Presentation
    {
      get
      {
        return (LinearScaleLayerPresentation) this.GetValue(LinearScaleLayer.PresentationProperty);
      }
      set
      {
        this.SetValue(LinearScaleLayer.PresentationProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns a list of predefined presentations for a linear scale layer.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Collections.Generic.List`1"/> of <see cref="T:DevExpress.Xpf.Gauges.PredefinedElementKind"/> objects.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleLayerPredefinedPresentations")]
    public static List<PredefinedElementKind> PredefinedPresentations
    {
      get
      {
        return PredefinedLinearScaleLayerPresentations.PresentationKinds;
      }
    }

    private LinearScale Scale
    {
      get
      {
        return this.Owner as LinearScale;
      }
    }

    private LinearGaugeControl Gauge
    {
      get
      {
        if (this.Scale == null)
          return (LinearGaugeControl) null;
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
          LayerModel scaleLayerModel = this.Gauge.ActualModel.GetScaleLayerModel(this.Scale.Layers.IndexOf(this));
          if (scaleLayerModel != null && scaleLayerModel.Presentation != null)
            return scaleLayerModel.Presentation;
        }
        return (LayerPresentation) new DefaultLinearScaleBackgroundLayerPresentation();
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
          LayerModel scaleLayerModel = this.Gauge.ActualModel.GetScaleLayerModel(this.Scale.Layers.IndexOf(this));
          if (scaleLayerModel != null && scaleLayerModel.Options != null)
            return scaleLayerModel.Options;
        }
        return new LayerOptions();
      }
    }

    private Point GetArrangePointByScaleLayoutMode()
    {
      double x;
      double y;
      if (this.Scale.LayoutMode == LinearScaleLayoutMode.TopToBottom || this.Scale.LayoutMode == LinearScaleLayoutMode.BottomToTop)
      {
        x = this.Scale.Mapping.Layout.AnchorPoint.X;
        y = this.Scale.Mapping.Layout.AnchorPoint.Y + this.Scale.Mapping.Layout.ScaleVector.Y / 2.0;
      }
      else
      {
        x = this.Scale.Mapping.Layout.AnchorPoint.X + this.Scale.Mapping.Layout.ScaleVector.X / 2.0;
        y = this.Scale.Mapping.Layout.AnchorPoint.Y;
      }
      return new Point(x, y);
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new LinearScaleLayer();
    }

    protected override ElementLayout CreateLayout(Size constraint)
    {
      if (this.Scale != null && this.Scale.Mapping != null && !this.Scale.Mapping.Layout.IsEmpty)
        return new ElementLayout(this.Scale.Mapping.Layout.InitialBounds.Width, this.Scale.Mapping.Layout.InitialBounds.Height);
      return (ElementLayout) null;
    }

    protected override void CompleteLayout(ElementInfoBase elementInfo)
    {
      Point byScaleLayoutMode = this.GetArrangePointByScaleLayoutMode();
      Point layoutOffset = this.Scale.GetLayoutOffset();
      byScaleLayoutMode.X += layoutOffset.X;
      byScaleLayoutMode.Y += layoutOffset.Y;
      elementInfo.Layout.CompleteLayout(byScaleLayoutMode, (Transform) null, (Geometry) null);
    }
  }
}
