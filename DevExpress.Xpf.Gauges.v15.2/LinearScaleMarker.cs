// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LinearScaleMarker
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
  /// A linear scale marker.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class LinearScaleMarker : LinearScaleIndicator
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
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (LinearScaleMarkerPresentation), typeof (LinearScaleMarker), new PropertyMetadata((object) null, new PropertyChangedCallback(ValueIndicatorBase.PresentationPropertyChanged)));
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
    public static readonly DependencyProperty OptionsProperty = DependencyPropertyManager.Register("Options", typeof (LinearScaleMarkerOptions), typeof (LinearScaleMarker), new PropertyMetadata(new PropertyChangedCallback(LinearScaleMarker.OptionsPropertyChanged)));

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
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearScaleMarkerPresentation"/> object.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleMarkerPresentation")]
    [Category("Presentation")]
    public LinearScaleMarkerPresentation Presentation
    {
      get
      {
        return (LinearScaleMarkerPresentation) this.GetValue(LinearScaleMarker.PresentationProperty);
      }
      set
      {
        this.SetValue(LinearScaleMarker.PresentationProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the options of a marker that specify its shape and position on a Linear scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearScaleMarkerOptions"/> object that contains the settings of the marker.
    /// 
    /// 
    /// </value>
    [Category("Presentation")]
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleMarkerOptions")]
    public LinearScaleMarkerOptions Options
    {
      get
      {
        return (LinearScaleMarkerOptions) this.GetValue(LinearScaleMarker.OptionsProperty);
      }
      set
      {
        this.SetValue(LinearScaleMarker.OptionsProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns a list of predefined presentations for a linear scale marker.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Collections.Generic.List`1"/> of <see cref="T:DevExpress.Xpf.Gauges.PredefinedElementKind"/> objects.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleMarkerPredefinedPresentations")]
    public static List<PredefinedElementKind> PredefinedPresentations
    {
      get
      {
        return PredefinedLinearScaleMarkerPresentations.PresentationKinds;
      }
    }

    private LinearScaleMarkerModel Model
    {
      get
      {
        if (this.Gauge == null || this.Gauge.ActualModel == null)
          return (LinearScaleMarkerModel) null;
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
        return (ValueIndicatorPresentation) new DefaultLinearScaleMarkerPresentation();
      }
    }

    internal LinearScaleMarkerOptions ActualOptions
    {
      get
      {
        if (this.Options != null)
          return this.Options;
        if (this.Model != null && this.Model.Options != null)
          return this.Model.Options;
        return new LinearScaleMarkerOptions();
      }
    }

    private static void OptionsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      LinearScaleMarker linearScaleMarker = d as LinearScaleMarker;
      if (linearScaleMarker == null)
        return;
      CommonUtils.SubscribePropertyChangedWeakEvent((INotifyPropertyChanged) (e.OldValue as LinearScaleMarkerOptions), (INotifyPropertyChanged) (e.NewValue as LinearScaleMarkerOptions), (IWeakEventListener) linearScaleMarker);
      linearScaleMarker.OnOptionsChanged();
    }

    private double GetActualFactorWidth()
    {
      if (this.Scale.LayoutMode != LinearScaleLayoutMode.BottomToTop && this.Scale.LayoutMode != LinearScaleLayoutMode.TopToBottom)
        return this.ActualOptions.FactorHeight;
      return this.ActualOptions.FactorWidth;
    }

    private double GetActualFactorHeight()
    {
      if (this.Scale.LayoutMode != LinearScaleLayoutMode.BottomToTop && this.Scale.LayoutMode != LinearScaleLayoutMode.TopToBottom)
        return this.ActualOptions.FactorWidth;
      return this.ActualOptions.FactorHeight;
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new LinearScaleMarker();
    }

    protected override ElementLayout CreateLayout(Size constraint)
    {
      if (this.Scale == null || this.Scale.Mapping == null || this.Scale.Mapping.Layout.IsEmpty)
        return (ElementLayout) null;
      return new ElementLayout();
    }

    protected override void CompleteLayout(ElementInfoBase elementInfo)
    {
      double num1 = this.ActualOptions.Orientation == LinearScaleMarkerOrientation.Normal ? this.GetActualFactorWidth() : -this.GetActualFactorWidth();
      double num2 = this.Scale.LayoutMode == LinearScaleLayoutMode.LeftToRight || this.Scale.LayoutMode == LinearScaleLayoutMode.RightToLeft ? 90.0 : 0.0;
      TransformGroup transformGroup = new TransformGroup();
      transformGroup.Children.Add((Transform) new ScaleTransform()
      {
        ScaleX = num1,
        ScaleY = this.GetActualFactorHeight()
      });
      transformGroup.Children.Add((Transform) new RotateTransform()
      {
        Angle = num2
      });
      Point pointByValue = this.Scale.Mapping.GetPointByValue(this.ActualValue, this.ActualOptions.Offset);
      Point layoutOffset = this.Scale.GetLayoutOffset();
      pointByValue.X += layoutOffset.X;
      pointByValue.Y += layoutOffset.Y;
      elementInfo.Layout.CompleteLayout(pointByValue, (Transform) transformGroup, (Geometry) null);
    }
  }
}
