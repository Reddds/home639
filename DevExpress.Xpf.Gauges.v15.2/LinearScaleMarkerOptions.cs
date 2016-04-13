// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LinearScaleMarkerOptions
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
  /// Contains layout options for a linear scale marker.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class LinearScaleMarkerOptions : MarkerOptionsBase
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
    public static readonly DependencyProperty OrientationProperty = DependencyPropertyManager.Register("Orientation", typeof (LinearScaleMarkerOrientation), typeof (LinearScaleMarkerOptions), new PropertyMetadata((object) LinearScaleMarkerOrientation.Normal, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty ZIndexProperty = DependencyPropertyManager.Register("ZIndex", typeof (int), typeof (LinearScaleMarkerOptions), new PropertyMetadata((object) 100, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Provides different types of orientation for the marker on the Linear scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearScaleMarkerOrientation"/> object that specifies possible ways the marker can be oriented.
    /// 
    /// </value>
    [Category("Layout")]
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleMarkerOptionsOrientation")]
    public LinearScaleMarkerOrientation Orientation
    {
      get
      {
        return (LinearScaleMarkerOrientation) this.GetValue(LinearScaleMarkerOptions.OrientationProperty);
      }
      set
      {
        this.SetValue(LinearScaleMarkerOptions.OrientationProperty, (object) value);
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
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleMarkerOptionsZIndex")]
    [Category("Layout")]
    public int ZIndex
    {
      get
      {
        return (int) this.GetValue(LinearScaleMarkerOptions.ZIndexProperty);
      }
      set
      {
        this.SetValue(LinearScaleMarkerOptions.ZIndexProperty, (object) value);
      }
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new LinearScaleMarkerOptions();
    }
  }
}
