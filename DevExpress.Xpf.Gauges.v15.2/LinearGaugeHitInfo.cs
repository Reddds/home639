// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LinearGaugeHitInfo
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// Contains information about a specific point within a linear gauge.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class LinearGaugeHitInfo : GaugeHitInfoBase
  {
    /// <summary>
    /// 
    /// <para>
    /// Gets a value indicating whether the test point is within a linear scale.
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
    /// Gets a value indicating whether the test point is within a level bar.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> if the test point is within a level bar; otherwise, <b>false</b>.
    /// 
    /// </value>
    public bool InLevelBar
    {
      get
      {
        return this.LevelBar != null;
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
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearScale"/> object that is the scale located under the test point.
    /// 
    /// </value>
    public LinearScale Scale
    {
      get
      {
        return this.GetElementParentByType(typeof (LinearScale)) as LinearScale;
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
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearScaleMarker"/> object that is the marker located under the test point.
    /// 
    /// </value>
    public LinearScaleMarker Marker
    {
      get
      {
        return this.GetElementByType(typeof (LinearScaleMarker)) as LinearScaleMarker;
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
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearScaleRangeBar"/> object that is the range bar located under the test point.
    /// 
    /// </value>
    public LinearScaleRangeBar RangeBar
    {
      get
      {
        return this.GetElementByType(typeof (LinearScaleRangeBar)) as LinearScaleRangeBar;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets a level bar which is located under the test point.
    /// 
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearScaleLevelBar"/> object that is the level bar located under the test point.
    /// 
    /// </value>
    public LinearScaleLevelBar LevelBar
    {
      get
      {
        return this.GetElementByType(typeof (LinearScaleLevelBar)) as LinearScaleLevelBar;
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
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearScaleRange"/> object that is the range located under the test point.
    /// 
    /// </value>
    public LinearScaleRange Range
    {
      get
      {
        return this.GetElementByType(typeof (LinearScaleRange)) as LinearScaleRange;
      }
    }

    internal LinearGaugeHitInfo(LinearGaugeControl gauge, Point point)
      : base((AnalogGaugeControl) gauge, point)
    {
    }
  }
}
