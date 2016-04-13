// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LinearScaleLevelBar
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;
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
  /// A linear scale level bar.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class LinearScaleLevelBar : LinearScaleIndicator
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
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (LinearScaleLevelBarPresentation), typeof (LinearScaleLevelBar), new PropertyMetadata((object) null, new PropertyChangedCallback(ValueIndicatorBase.PresentationPropertyChanged)));
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
    public static readonly DependencyProperty OptionsProperty = DependencyPropertyManager.Register("Options", typeof (LinearScaleLevelBarOptions), typeof (LinearScaleLevelBar), new PropertyMetadata(new PropertyChangedCallback(LinearScaleLevelBar.OptionsPropertyChanged)));
    private readonly ValueIndicatorInfo foregroundInfo;

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the current presentation that specifies the appearance of the level bar.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearScaleLevelBarPresentation"/> object.
    /// 
    /// </value>
    [Category("Presentation")]
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleLevelBarPresentation")]
    public LinearScaleLevelBarPresentation Presentation
    {
      get
      {
        return (LinearScaleLevelBarPresentation) this.GetValue(LinearScaleLevelBar.PresentationProperty);
      }
      set
      {
        this.SetValue(LinearScaleLevelBar.PresentationProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the options of a level bar that specify its shape and position on a Linear scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearScaleLevelBarOptions"/> object that contains the settings of the level bar.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleLevelBarOptions")]
    [Category("Presentation")]
    public LinearScaleLevelBarOptions Options
    {
      get
      {
        return (LinearScaleLevelBarOptions) this.GetValue(LinearScaleLevelBar.OptionsProperty);
      }
      set
      {
        this.SetValue(LinearScaleLevelBar.OptionsProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns a list of predefined presentations for a linear gauge level bar.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Collections.Generic.List`1"/> of <see cref="T:DevExpress.Xpf.Gauges.PredefinedElementKind"/> objects.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleLevelBarPredefinedPresentations")]
    public static List<PredefinedElementKind> PredefinedPresentations
    {
      get
      {
        return PredefinedLinearScaleLevelBarPresentations.PresentationKinds;
      }
    }

    private LinearScaleLevelBarModel Model
    {
      get
      {
        if (this.Gauge == null || this.Gauge.ActualModel == null)
          return (LinearScaleLevelBarModel) null;
        return this.Gauge.ActualModel.GetLevelBarModel(this.Scale.LevelBars.IndexOf(this));
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
        return (ValueIndicatorPresentation) new DefaultLinearScaleLevelBarPresentation();
      }
    }

    protected internal override IEnumerable<ValueIndicatorInfo> Elements
    {
      get
      {
        yield return this.ElementInfo;
        yield return this.foregroundInfo;
      }
    }

    internal ValueIndicatorInfo ForegroundInfo
    {
      get
      {
        return this.foregroundInfo;
      }
    }

    internal LinearScaleLevelBarOptions ActualOptions
    {
      get
      {
        if (this.Options != null)
          return this.Options;
        if (this.Model != null && this.Model.Options != null)
          return this.Model.Options;
        return new LinearScaleLevelBarOptions();
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the LinearScaleLevelBar class with default settings.
    /// 
    /// </para>
    /// 
    /// </summary>
    public LinearScaleLevelBar()
    {
      LinearScaleLevelBarPresentation levelBarPresentation = this.ActualPresentation as LinearScaleLevelBarPresentation;
      this.foregroundInfo = levelBarPresentation != null ? new ValueIndicatorInfo((ValueIndicatorBase) this, this.ActualZIndex, levelBarPresentation.CreateForegroundPresentationControl(), (PresentationBase) levelBarPresentation) : (ValueIndicatorInfo) null;
    }

    private static void OptionsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      LinearScaleLevelBar linearScaleLevelBar = d as LinearScaleLevelBar;
      if (linearScaleLevelBar == null)
        return;
      CommonUtils.SubscribePropertyChangedWeakEvent((INotifyPropertyChanged) (e.OldValue as LinearScaleLevelBarOptions), (INotifyPropertyChanged) (e.NewValue as LinearScaleLevelBarOptions), (IWeakEventListener) linearScaleLevelBar);
      linearScaleLevelBar.OnOptionsChanged();
    }

    private double GetClipHeightByScaleLayoutMode(ElementInfoBase elementInfo)
    {
      double num = elementInfo.Layout.Height.Value;
      Rect relativeElementRect = LayoutHelper.GetRelativeElementRect((UIElement) this.ElementInfo.PresentationControl, (UIElement) this.Gauge);
      switch (this.Scale.LayoutMode)
      {
        case LinearScaleLayoutMode.LeftToRight:
          num += relativeElementRect.Left;
          break;
        case LinearScaleLayoutMode.RightToLeft:
          num += this.Gauge.ActualWidth - relativeElementRect.Right;
          break;
        case LinearScaleLayoutMode.BottomToTop:
          num += this.Gauge.ActualHeight - relativeElementRect.Bottom;
          break;
        case LinearScaleLayoutMode.TopToBottom:
          num += relativeElementRect.Top;
          break;
      }
      return num;
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new LinearScaleLevelBar();
    }

    protected override void UpdatePresentation()
    {
      base.UpdatePresentation();
      LinearScaleLevelBarPresentation levelBarPresentation = this.ActualPresentation as LinearScaleLevelBarPresentation;
      if (this.ForegroundInfo == null || levelBarPresentation == null)
        return;
      this.ForegroundInfo.Presentation = (PresentationBase) levelBarPresentation;
      this.ForegroundInfo.PresentationControl = levelBarPresentation.CreateForegroundPresentationControl();
    }

    protected override ElementLayout CreateLayout(Size constraint)
    {
      if (this.Scale == null || this.Scale.Mapping == null || this.Scale.Mapping.Layout.IsEmpty)
        return (ElementLayout) null;
      double width;
      double height;
      if (this.Scale.LayoutMode == LinearScaleLayoutMode.BottomToTop || this.Scale.LayoutMode == LinearScaleLayoutMode.TopToBottom)
      {
        width = constraint.Width;
        height = Math.Abs(this.Scale.Mapping.Layout.ScaleVector.Y);
      }
      else
      {
        width = constraint.Height;
        height = Math.Abs(this.Scale.Mapping.Layout.ScaleVector.X);
      }
      return new ElementLayout(width, height);
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
      TransformGroup transformGroup = new TransformGroup();
      transformGroup.Children.Add((Transform) new ScaleTransform()
      {
        ScaleX = this.ActualOptions.FactorThickness
      });
      transformGroup.Children.Add((Transform) new RotateTransform()
      {
        Angle = byScaleLayoutMode
      });
      RectangleGeometry rectangleGeometry = (RectangleGeometry) null;
      if (elementInfo == this.foregroundInfo)
      {
        Point point2 = new Point(0.0, this.GetPointYByScaleLayoutMode(this.ActualValue));
        Point point1 = new Point(elementInfo.Layout.Width.Value, this.GetClipHeightByScaleLayoutMode(elementInfo));
        rectangleGeometry = new RectangleGeometry();
        rectangleGeometry.Rect = new Rect(point1, point2);
      }
      elementInfo.Layout.CompleteLayout(anchorPoint, (Transform) transformGroup, (Geometry) rectangleGeometry);
    }
  }
}
