// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.CircularGaugeHitInfo
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// Contains information about a specific point within a circular gauge.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class CircularGaugeHitInfo : GaugeHitInfoBase
  {
    /// <summary>
    /// 
    /// <para>
    /// Gets a value indicating whether the test point is within a scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> if the test point is within a scale; otherwise, <b>false</b>.
    /// 
    /// </value>
    public bool InScale
    {
      get
      {
        return this.Scale != null;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets a value indicating whether the test point is within a needle.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> if the test point is within a needle; otherwise, <b>false</b>.
    /// 
    /// </value>
    public bool InNeedle
    {
      get
      {
        return this.Needle != null;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets a value indicating whether the test point is within a marker.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> if the test point is within a marker; otherwise, <b>false</b>.
    /// 
    /// </value>
    public bool InMarker
    {
      get
      {
        return this.Marker != null;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets a value indicating whether the test point is within a range bar.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> if the test point is within a range bar; otherwise, <b>false</b>.
    /// 
    /// </value>
    public bool InRangeBar
    {
      get
      {
        return this.RangeBar != null;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets a value indicating whether the test point is within a range.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> if the test point is within a range; otherwise, <b>false</b>.
    /// 
    /// </value>
    public bool InRange
    {
      get
      {
        return this.Range != null;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets a scale which is located under the test point.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.ArcScale"/> object that is the scale located under the test point.
    /// 
    /// </value>
    public ArcScale Scale
    {
      get
      {
        return this.GetElementParentByType(typeof (ArcScale)) as ArcScale;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets a needle which is located under the test point.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.ArcScaleNeedle"/> object that is the needle located under the test point.
    /// 
    /// </value>
    public ArcScaleNeedle Needle
    {
      get
      {
        return this.GetElementByType(typeof (ArcScaleNeedle)) as ArcScaleNeedle;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets a marker which is located under the test point.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.ArcScaleMarker"/> object that is the marker located under the test point.
    /// 
    /// </value>
    public ArcScaleMarker Marker
    {
      get
      {
        return this.GetElementByType(typeof (ArcScaleMarker)) as ArcScaleMarker;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets a range bar which is located under the test point.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.ArcScaleRangeBar"/> object that is the range bar located under the test point.
    /// 
    /// </value>
    public ArcScaleRangeBar RangeBar
    {
      get
      {
        return this.GetElementByType(typeof (ArcScaleRangeBar)) as ArcScaleRangeBar;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets a range which is located under the test point.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.ArcScaleRange"/> object that is the range located under the test point.
    /// 
    /// </value>
    public ArcScaleRange Range
    {
      get
      {
        return this.GetElementByType(typeof (ArcScaleRange)) as ArcScaleRange;
      }
    }

    internal CircularGaugeHitInfo(CircularGaugeControl gauge, Point point)
      : base((AnalogGaugeControl) gauge, point)
    {
    }
  }
}
