// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LinearScaleRange
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A linear scale range.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class LinearScaleRange : RangeBase
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
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (LinearScaleRangePresentation), typeof (LinearScaleRange), new PropertyMetadata((object) null, new PropertyChangedCallback(LayerBase.PresentationPropertyChanged)));

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
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearScaleRangePresentation"/> object.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleRangePresentation")]
    [Category("Presentation")]
    public LinearScaleRangePresentation Presentation
    {
      get
      {
        return (LinearScaleRangePresentation) this.GetValue(LinearScaleRange.PresentationProperty);
      }
      set
      {
        this.SetValue(LinearScaleRange.PresentationProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns a list of predefined presentations for a linear scale range.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Collections.Generic.List`1"/> of <see cref="T:DevExpress.Xpf.Gauges.PredefinedElementKind"/> objects.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleRangePredefinedPresentations")]
    public static List<PredefinedElementKind> PredefinedPresentations
    {
      get
      {
        return PredefinedLinearScaleRangePresentations.PresentationKinds;
      }
    }

    private LinearScale Scale
    {
      get
      {
        return base.Scale as LinearScale;
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

    private LinearScaleRangeModel Model
    {
      get
      {
        if (this.Gauge == null || this.Gauge.ActualModel == null)
          return (LinearScaleRangeModel) null;
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
        return (LayerPresentation) new DefaultLinearScaleRangePresentation();
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

    private double GetRotationAngleByScaleLayoutMode()
    {
      double num = 0.0;
      switch (this.Scale.LayoutMode)
      {
        case LinearScaleLayoutMode.LeftToRight:
          num = 90.0;
          break;
        case LinearScaleLayoutMode.RightToLeft:
          num = -90.0;
          break;
        case LinearScaleLayoutMode.TopToBottom:
          num = 180.0;
          break;
      }
      return num;
    }

    private double GetClipPointYByScaleLayoutMode(double value)
    {
      return this.Scale.LayoutMode == LinearScaleLayoutMode.TopToBottom || this.Scale.LayoutMode == LinearScaleLayoutMode.BottomToTop ? Math.Abs(this.Scale.Mapping.GetPointByValue(value, true).Y - (this.Scale.Mapping.Layout.AnchorPoint.Y + this.Scale.Mapping.Layout.ScaleVector.Y)) : Math.Abs(this.Scale.Mapping.GetPointByValue(value, true).X - (this.Scale.Mapping.Layout.AnchorPoint.X + this.Scale.Mapping.Layout.ScaleVector.X));
    }

    protected override ElementLayout CreateLayout(Size constraint)
    {
      if (this.Scale != null && this.Scale.Mapping != null && !this.Scale.Mapping.Layout.IsEmpty)
        return new ElementLayout((double) this.ActualOptions.Thickness, this.Scale.LayoutMode == LinearScaleLayoutMode.BottomToTop || this.Scale.LayoutMode == LinearScaleLayoutMode.TopToBottom ? this.Scale.Mapping.Layout.InitialBounds.Height : this.Scale.Mapping.Layout.InitialBounds.Width);
      return (ElementLayout) null;
    }

    protected override void CompleteLayout(ElementInfoBase elementInfo)
    {
      Point anchorPoint = this.Scale.Mapping.Layout.AnchorPoint;
      if (this.Scale.LayoutMode == LinearScaleLayoutMode.BottomToTop || this.Scale.LayoutMode == LinearScaleLayoutMode.TopToBottom)
        anchorPoint.X += this.ActualOptions.Offset;
      else
        anchorPoint.Y += this.ActualOptions.Offset;
      Point layoutOffset = this.Scale.GetLayoutOffset();
      anchorPoint.X += layoutOffset.X;
      anchorPoint.Y += layoutOffset.Y;
      double byScaleLayoutMode = this.GetRotationAngleByScaleLayoutMode();
      RotateTransform rotateTransform = new RotateTransform()
      {
        Angle = byScaleLayoutMode
      };
      RectangleGeometry rectangleGeometry = new RectangleGeometry();
      Point point1 = new Point(0.0, this.GetClipPointYByScaleLayoutMode(this.StartValueAbsolute));
      Point point2 = new Point((double) this.ActualOptions.Thickness, this.GetClipPointYByScaleLayoutMode(this.EndValueAbsolute));
      rectangleGeometry.Rect = new Rect(point1, point2);
      elementInfo.Layout.CompleteLayout(anchorPoint, (Transform) rotateTransform, (Geometry) rectangleGeometry);
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new LinearScaleRange();
    }
  }
}
