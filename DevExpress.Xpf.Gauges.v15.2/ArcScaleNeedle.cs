// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ArcScaleNeedle
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
  /// An arc scale needle.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class ArcScaleNeedle : ArcScaleIndicator
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
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (ArcScaleNeedlePresentation), typeof (ArcScaleNeedle), new PropertyMetadata((object) null, new PropertyChangedCallback(ValueIndicatorBase.PresentationPropertyChanged)));
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
    public static readonly DependencyProperty OptionsProperty = DependencyPropertyManager.Register("Options", typeof (ArcScaleNeedleOptions), typeof (ArcScaleNeedle), new PropertyMetadata(new PropertyChangedCallback(ArcScaleNeedle.OptionsPropertyChanged)));
    private const int lengthPrecision = 1;

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the current presentation that specifies the appearance of the needle.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.ArcScaleNeedlePresentation"/> object.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleNeedlePresentation")]
    [Category("Presentation")]
    public ArcScaleNeedlePresentation Presentation
    {
      get
      {
        return (ArcScaleNeedlePresentation) this.GetValue(ArcScaleNeedle.PresentationProperty);
      }
      set
      {
        this.SetValue(ArcScaleNeedle.PresentationProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the options that allow you to customize the needle's  shape and position on a Circular scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.ArcScaleNeedleOptions"/> object that contain the settings for the needle.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleNeedleOptions")]
    [Category("Presentation")]
    public ArcScaleNeedleOptions Options
    {
      get
      {
        return (ArcScaleNeedleOptions) this.GetValue(ArcScaleNeedle.OptionsProperty);
      }
      set
      {
        this.SetValue(ArcScaleNeedle.OptionsProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns a list of predefined presentations for an arc scale needle.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Collections.Generic.List`1"/> of <see cref="T:DevExpress.Xpf.Gauges.PredefinedElementKind"/> objects.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleNeedlePredefinedPresentations")]
    public static List<PredefinedElementKind> PredefinedPresentations
    {
      get
      {
        return PredefinedArcScaleNeedlePresentations.PresentationKinds;
      }
    }

    private ArcScaleNeedleModel Model
    {
      get
      {
        if (this.Gauge == null || this.Gauge.ActualModel == null)
          return (ArcScaleNeedleModel) null;
        return this.Gauge.ActualModel.GetNeedleModel(this.Scale.ActualLayoutMode, this.Scale.Needles.IndexOf(this));
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
        return (ValueIndicatorPresentation) new DefaultArcScaleNeedlePresentation();
      }
    }

    internal ArcScaleNeedleOptions ActualOptions
    {
      get
      {
        if (this.Options != null)
          return this.Options;
        if (this.Model != null && this.Model.Options != null)
          return this.Model.Options;
        return new ArcScaleNeedleOptions();
      }
    }

    private static void OptionsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ArcScaleNeedle arcScaleNeedle = d as ArcScaleNeedle;
      if (arcScaleNeedle == null)
        return;
      CommonUtils.SubscribePropertyChangedWeakEvent((INotifyPropertyChanged) (e.OldValue as ArcScaleNeedleOptions), (INotifyPropertyChanged) (e.NewValue as ArcScaleNeedleOptions), (IWeakEventListener) arcScaleNeedle);
      arcScaleNeedle.OnOptionsChanged();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new ArcScaleNeedle();
    }

    protected override ElementLayout CreateLayout(Size constraint)
    {
      if (this.Scale != null && this.Scale.Mapping != null)
      {
        double num = Math.Round(MathUtils.CalculateDistance(this.Scale.Mapping.Layout.EllipseCenter, this.Scale.Mapping.GetPointByValue(this.ActualValue, -this.ActualOptions.EndOffset)) - this.ActualOptions.StartOffset, 1);
        double width = num >= 0.0 ? num : 0.0;
        if (!this.Scale.Mapping.Layout.IsEmpty)
          return new ElementLayout(width);
      }
      return (ElementLayout) null;
    }

    protected override void CompleteLayout(ElementInfoBase elementInfo)
    {
      double height = elementInfo.PresentationControl.DesiredSize.Height;
      double angleByValue = this.Scale.Mapping.GetAngleByValue(this.ActualValue);
      Transform transform = (Transform) new RotateTransform()
      {
        Angle = angleByValue
      };
      Point ellipseCenter = this.Scale.Mapping.Layout.EllipseCenter;
      ellipseCenter.X += this.ActualOptions.StartOffset * Math.Cos(MathUtils.Degree2Radian(angleByValue));
      ellipseCenter.Y += this.ActualOptions.StartOffset * Math.Sin(MathUtils.Degree2Radian(angleByValue));
      Point layoutOffset = this.Scale.GetLayoutOffset();
      ellipseCenter.X += layoutOffset.X;
      ellipseCenter.Y += layoutOffset.Y;
      elementInfo.Layout.CompleteLayout(ellipseCenter, transform, (Geometry) null);
    }
  }
}
