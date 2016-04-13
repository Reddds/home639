// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ArcScaleMarkerOptions
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// Contains layout options for an arc scale marker.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class ArcScaleMarkerOptions : MarkerOptionsBase
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
    public static readonly DependencyProperty OrientationProperty = DependencyPropertyManager.Register("Orientation", typeof (ArcScaleMarkerOrientation), typeof (ArcScaleMarkerOptions), new PropertyMetadata((object) ArcScaleMarkerOrientation.RadialToCenter, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty ZIndexProperty = DependencyPropertyManager.Register("ZIndex", typeof (int), typeof (ArcScaleMarkerOptions), new PropertyMetadata((object) 100, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Provides different types of orientation for the marker on the Circular scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.ArcScaleMarkerOrientation"/> object that specifies possible ways the marker can be oriented.
    /// 
    /// </value>
    [Category("Layout")]
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleMarkerOptionsOrientation")]
    public ArcScaleMarkerOrientation Orientation
    {
      get
      {
        return (ArcScaleMarkerOrientation) this.GetValue(ArcScaleMarkerOptions.OrientationProperty);
      }
      set
      {
        this.SetValue(ArcScaleMarkerOptions.OrientationProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the z-index of a marker.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An integer value that is the z-index.
    /// 
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleMarkerOptionsZIndex")]
    [Category("Layout")]
    public int ZIndex
    {
      get
      {
        return (int) this.GetValue(ArcScaleMarkerOptions.ZIndexProperty);
      }
      set
      {
        this.SetValue(ArcScaleMarkerOptions.ZIndexProperty, (object) value);
      }
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new ArcScaleMarkerOptions();
    }
  }
}
