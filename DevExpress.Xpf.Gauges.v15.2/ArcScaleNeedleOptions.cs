// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ArcScaleNeedleOptions
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
  /// Contains layout options for arc scale needles.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class ArcScaleNeedleOptions : GaugeDependencyObject
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
    public static readonly DependencyProperty StartOffsetProperty = DependencyPropertyManager.Register("StartOffset", typeof (double), typeof (ArcScaleNeedleOptions), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty EndOffsetProperty = DependencyPropertyManager.Register("EndOffset", typeof (double), typeof (ArcScaleNeedleOptions), new PropertyMetadata((object) 37.0, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty ZIndexProperty = DependencyPropertyManager.Register("ZIndex", typeof (int), typeof (ArcScaleNeedleOptions), new PropertyMetadata((object) 150, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the offset of the needle's starting point  from the Circular scale center.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that specifies the start offset of the needle.
    /// 
    /// </value>
    [Category("Layout")]
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleNeedleOptionsStartOffset")]
    public double StartOffset
    {
      get
      {
        return (double) this.GetValue(ArcScaleNeedleOptions.StartOffsetProperty);
      }
      set
      {
        this.SetValue(ArcScaleNeedleOptions.StartOffsetProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the offset of the needle's end point  from the edge of the Circular scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that specifies the end offset of the needle.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleNeedleOptionsEndOffset")]
    [Category("Layout")]
    public double EndOffset
    {
      get
      {
        return (double) this.GetValue(ArcScaleNeedleOptions.EndOffsetProperty);
      }
      set
      {
        this.SetValue(ArcScaleNeedleOptions.EndOffsetProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the z-index of a needle.
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
    [Category("Layout")]
    [DevExpressXpfGaugesLocalizedDescription("ArcScaleNeedleOptionsZIndex")]
    public int ZIndex
    {
      get
      {
        return (int) this.GetValue(ArcScaleNeedleOptions.ZIndexProperty);
      }
      set
      {
        this.SetValue(ArcScaleNeedleOptions.ZIndexProperty, (object) value);
      }
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new ArcScaleNeedleOptions();
    }
  }
}
