// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LinearScaleRangeBarOptions
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
  /// Contains appearance and layout options for a linear scale range bar.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class LinearScaleRangeBarOptions : RangeBarOptionsBase
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
    public static readonly DependencyProperty ZIndexProperty = DependencyPropertyManager.Register("ZIndex", typeof (int), typeof (LinearScaleRangeBarOptions), new PropertyMetadata((object) 10, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the z-index of a range bar.
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
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleRangeBarOptionsZIndex")]
    [Category("Layout")]
    public int ZIndex
    {
      get
      {
        return (int) this.GetValue(LinearScaleRangeBarOptions.ZIndexProperty);
      }
      set
      {
        this.SetValue(LinearScaleRangeBarOptions.ZIndexProperty, (object) value);
      }
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new LinearScaleRangeBarOptions();
    }
  }
}
