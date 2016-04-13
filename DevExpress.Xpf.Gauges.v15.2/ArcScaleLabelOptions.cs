// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ArcScaleLabelOptions
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
  /// Contains appearance and behavior options for arc scale labels.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class ArcScaleLabelOptions : ScaleLabelOptions
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
    public static readonly DependencyProperty OrientationProperty = DependencyPropertyManager.Register("Orientation", typeof (ArcScaleLabelOrientation), typeof (ArcScaleLabelOptions), new PropertyMetadata((object) ArcScaleLabelOrientation.LeftToRight, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Provides different types of orientation for labels on the Circular scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.ArcScaleLabelOrientation"/> object that specifies possible ways labels can be oriented.
    /// 
    /// </value>
    [Category("Layout")]
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleLabelOptionsOrientation")]
    public ArcScaleLabelOrientation Orientation
    {
      get
      {
        return (ArcScaleLabelOrientation) this.GetValue(ArcScaleLabelOptions.OrientationProperty);
      }
      set
      {
        this.SetValue(ArcScaleLabelOptions.OrientationProperty, (object) value);
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
        case ArcScaleLabelOrientation.Radial:
          if (angle <= 90.0 || angle > 270.0)
            return angle;
          return angle + 180.0;
        case ArcScaleLabelOrientation.Tangent:
          if ((angle < 180.0 || angle > 360.0) && angle != 0.0)
            return angle - 90.0;
          return angle + 90.0;
        case ArcScaleLabelOrientation.LeftToRight:
          return 0.0;
        case ArcScaleLabelOrientation.BottomToTop:
          return 270.0;
        case ArcScaleLabelOrientation.TopToBottom:
          return 90.0;
        default:
          DebugHelper.Fail("Unknown the scale label orientation.");
          goto case 0;
      }
    }
  }
}
