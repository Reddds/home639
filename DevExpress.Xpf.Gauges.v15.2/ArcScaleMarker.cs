// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ArcScaleMarker
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
  /// An arc scale marker.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class ArcScaleMarker : ArcScaleIndicator
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
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (ArcScaleMarkerPresentation), typeof (ArcScaleMarker), new PropertyMetadata((object) null, new PropertyChangedCallback(ValueIndicatorBase.PresentationPropertyChanged)));
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
    public static readonly DependencyProperty OptionsProperty = DependencyPropertyManager.Register("Options", typeof (ArcScaleMarkerOptions), typeof (ArcScaleMarker), new PropertyMetadata(new PropertyChangedCallback(ArcScaleMarker.OptionsPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the current presentation that specifies the appearance of the marker.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.ArcScaleMarkerPresentation"/> object.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleMarkerPresentation")]
    [Category("Presentation")]
    public ArcScaleMarkerPresentation Presentation
    {
      get
      {
        return (ArcScaleMarkerPresentation) this.GetValue(ArcScaleMarker.PresentationProperty);
      }
      set
      {
        this.SetValue(ArcScaleMarker.PresentationProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the options of a marker that specify its shape and position on a Circular scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.ArcScaleMarkerOptions"/> object that contains the settings of the marker.
    /// 
    /// 
    /// </value>
    [Category("Presentation")]
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleMarkerOptions")]
    public ArcScaleMarkerOptions Options
    {
      get
      {
        return (ArcScaleMarkerOptions) this.GetValue(ArcScaleMarker.OptionsProperty);
      }
      set
      {
        this.SetValue(ArcScaleMarker.OptionsProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns a list of predefined presentations for an arc scale marker.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Collections.Generic.List`1"/> of <see cref="T:DevExpress.Xpf.Gauges.PredefinedElementKind"/> objects.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleMarkerPredefinedPresentations")]
    public static List<PredefinedElementKind> PredefinedPresentations
    {
      get
      {
        return PredefinedArcScaleMarkerPresentations.PresentationKinds;
      }
    }

    private ArcScaleMarkerModel Model
    {
      get
      {
        if (this.Gauge == null || this.Gauge.ActualModel == null)
          return (ArcScaleMarkerModel) null;
        return this.Gauge.ActualModel.GetMarkerModel(this.Scale.Markers.IndexOf(this));
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
        return (ValueIndicatorPresentation) new DefaultArcScaleMarkerPresentation();
      }
    }

    internal ArcScaleMarkerOptions ActualOptions
    {
      get
      {
        if (this.Options != null)
          return this.Options;
        if (this.Model != null && this.Model.Options != null)
          return this.Model.Options;
        return new ArcScaleMarkerOptions();
      }
    }

    private static void OptionsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ArcScaleMarker arcScaleMarker = d as ArcScaleMarker;
      if (arcScaleMarker == null)
        return;
      CommonUtils.SubscribePropertyChangedWeakEvent((INotifyPropertyChanged) (e.OldValue as ArcScaleMarkerOptions), (INotifyPropertyChanged) (e.NewValue as ArcScaleMarkerOptions), (IWeakEventListener) arcScaleMarker);
      arcScaleMarker.OnOptionsChanged();
    }

    private double CorrectAngleByOrientation(double angle)
    {
      switch (this.ActualOptions.Orientation)
      {
        case ArcScaleMarkerOrientation.RadialToCenter:
          return angle;
        case ArcScaleMarkerOrientation.RadialFromCenter:
          return angle + 180.0;
        case ArcScaleMarkerOrientation.Tangent:
          return angle - 90.0;
        case ArcScaleMarkerOrientation.Normal:
          return 0.0;
        case ArcScaleMarkerOrientation.UpsideDown:
          return 180.0;
        case ArcScaleMarkerOrientation.RotateClockwise:
          return 90.0;
        case ArcScaleMarkerOrientation.RotateCounterclockwise:
          return -90.0;
        default:
          return angle;
      }
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new ArcScaleMarker();
    }

    protected override ElementLayout CreateLayout(Size constraint)
    {
      if (this.Scale == null || this.Scale.Mapping == null || this.Scale.Mapping.Layout.IsEmpty)
        return (ElementLayout) null;
      return new ElementLayout();
    }

    protected override void CompleteLayout(ElementInfoBase elementInfo)
    {
      double num = this.CorrectAngleByOrientation(this.Scale.Mapping.GetAngleByValue(this.ActualValue));
      TransformGroup transformGroup = new TransformGroup();
      transformGroup.Children.Add((Transform) new ScaleTransform()
      {
        ScaleX = this.ActualOptions.FactorWidth,
        ScaleY = this.ActualOptions.FactorHeight
      });
      transformGroup.Children.Add((Transform) new RotateTransform()
      {
        Angle = num
      });
      Point pointByValue = this.Scale.Mapping.GetPointByValue(this.ActualValue, this.ActualOptions.Offset);
      Point layoutOffset = this.Scale.GetLayoutOffset();
      pointByValue.X += layoutOffset.X;
      pointByValue.Y += layoutOffset.Y;
      elementInfo.Layout.CompleteLayout(pointByValue, (Transform) transformGroup, (Geometry) null);
    }
  }
}
