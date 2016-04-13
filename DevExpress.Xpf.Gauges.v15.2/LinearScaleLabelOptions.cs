// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LinearScaleLabelOptions
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// Contains behavior, layout and data representation options for linear scale labels.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class LinearScaleLabelOptions : ScaleLabelOptions
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
    public static readonly DependencyProperty OrientationProperty = DependencyPropertyManager.Register("Orientation", typeof (LinearScaleLabelOrientation), typeof (LinearScaleLabelOptions), new PropertyMetadata((object) LinearScaleLabelOrientation.LeftToRight, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Provides different types of orientation for labels on the Linear scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearScaleLabelOrientation"/> object that specifies possible ways labels can be oriented.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleLabelOptionsOrientation")]
    [Category("Layout")]
    public LinearScaleLabelOrientation Orientation
    {
      get
      {
        return (LinearScaleLabelOrientation) this.GetValue(LinearScaleLabelOptions.OrientationProperty);
      }
      set
      {
        this.SetValue(LinearScaleLabelOptions.OrientationProperty, (object) value);
      }
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new ArcScaleLabelOptions();
    }

    protected internal override double CorrectAngleByOrientation(double angle)
    {
      MathUtils.NormalizeDegree(angle);
      switch (this.Orientation)
      {
        case LinearScaleLabelOrientation.LeftToRight:
          return 0.0;
        case LinearScaleLabelOrientation.BottomToTop:
          return 270.0;
        case LinearScaleLabelOrientation.TopToBottom:
          return 90.0;
        default:
          DebugHelper.Fail("Unknown the scale label orientation.");
          goto case 0;
      }
    }
  }
}
