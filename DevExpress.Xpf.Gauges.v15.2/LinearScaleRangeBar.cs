// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LinearScaleRangeBar
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
  /// A linear scale range bar.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class LinearScaleRangeBar : LinearScaleIndicator
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
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (LinearScaleRangeBarPresentation), typeof (LinearScaleRangeBar), new PropertyMetadata((object) null, new PropertyChangedCallback(ValueIndicatorBase.PresentationPropertyChanged)));
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
    public static readonly DependencyProperty AnchorValueProperty = DependencyPropertyManager.Register("AnchorValue", typeof (double), typeof (LinearScaleRangeBar), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ValueIndicatorBase.IndicatorPropertyChanged)));
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
    public static readonly DependencyProperty OptionsProperty = DependencyPropertyManager.Register("Options", typeof (LinearScaleRangeBarOptions), typeof (LinearScaleRangeBar), new PropertyMetadata(new PropertyChangedCallback(LinearScaleRangeBar.OptionsPropertyChanged)));

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
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearScaleRangeBarPresentation"/> object.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleRangeBarPresentation")]
    [Category("Presentation")]
    public LinearScaleRangeBarPresentation Presentation
    {
      get
      {
        return (LinearScaleRangeBarPresentation) this.GetValue(LinearScaleRangeBar.PresentationProperty);
      }
      set
      {
        this.SetValue(LinearScaleRangeBar.PresentationProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the value on a scale that specifies the fixed edge of the range bar.
    /// 
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
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleRangeBarAnchorValue")]
    public double AnchorValue
    {
      get
      {
        return (double) this.GetValue(LinearScaleRangeBar.AnchorValueProperty);
      }
      set
      {
        this.SetValue(LinearScaleRangeBar.AnchorValueProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the options of a range bar that specify its shape and position on a Linear scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.LinearScaleRangeBarOptions"/> object that contains the settings of the range bar.
    /// 
    /// 
    /// </value>
    [Category("Presentation")]
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleRangeBarOptions")]
    public LinearScaleRangeBarOptions Options
    {
      get
      {
        return (LinearScaleRangeBarOptions) this.GetValue(LinearScaleRangeBar.OptionsProperty);
      }
      set
      {
        this.SetValue(LinearScaleRangeBar.OptionsProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns a list of predefined presentations for a linear scale range bar.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Collections.Generic.List`1"/> of <see cref="T:DevExpress.Xpf.Gauges.PredefinedElementKind"/> objects.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleRangeBarPredefinedPresentations")]
    public static List<PredefinedElementKind> PredefinedPresentations
    {
      get
      {
        return PredefinedLinearScaleRangeBarPresentations.PresentationKinds;
      }
    }

    private LinearScaleRangeBarModel Model
    {
      get
      {
        if (this.Gauge == null || this.Gauge.ActualModel == null)
          return (LinearScaleRangeBarModel) null;
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
        return (ValueIndicatorPresentation) new DefaultLinearScaleRangeBarPresentation();
      }
    }

    internal LinearScaleRangeBarOptions ActualOptions
    {
      get
      {
        if (this.Options != null)
          return this.Options;
        if (this.Model != null && this.Model.Options != null)
          return this.Model.Options;
        return new LinearScaleRangeBarOptions();
      }
    }

    private static void OptionsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      LinearScaleRangeBar linearScaleRangeBar = d as LinearScaleRangeBar;
      if (linearScaleRangeBar == null)
        return;
      CommonUtils.SubscribePropertyChangedWeakEvent((INotifyPropertyChanged) (e.OldValue as LinearScaleRangeBarOptions), (INotifyPropertyChanged) (e.NewValue as LinearScaleRangeBarOptions), (IWeakEventListener) linearScaleRangeBar);
      linearScaleRangeBar.OnOptionsChanged();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new LinearScaleRangeBar();
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
      Point point1 = new Point(0.0, this.GetPointYByScaleLayoutMode(this.AnchorValue));
      Point point2 = new Point((double) this.ActualOptions.Thickness, this.GetPointYByScaleLayoutMode(this.ActualValue));
      rectangleGeometry.Rect = new Rect(point1, point2);
      elementInfo.Layout.CompleteLayout(anchorPoint, (Transform) rotateTransform, (Geometry) rectangleGeometry);
    }
  }
}
