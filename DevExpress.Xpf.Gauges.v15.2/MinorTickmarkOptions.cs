// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.MinorTickmarkOptions
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
  /// Contains settings that define the layout and behavior of the minor tickmarks along the scale.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class MinorTickmarkOptions : TickmarkOptions
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
    public static readonly DependencyProperty ShowTicksForMajorProperty = DependencyPropertyManager.Register("ShowTicksForMajor", typeof (bool), typeof (MinorTickmarkOptions), new PropertyMetadata((object) false, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty ZIndexProperty = DependencyPropertyManager.Register("ZIndex", typeof (int), typeof (MinorTickmarkOptions), new PropertyMetadata((object) 10, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value indicating whether or not minor tickmarks should be visible at the positions of the corresponding major tickmarks.
    /// 
    /// 
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> to display minor tickmarks at the major tickmark positions; otherwise <b>false</b>.
    /// 
    /// 
    /// </value>
    [Category("Behavior")]
    [DevExpressXpfGaugesLocalizedDescription("MinorTickmarkOptionsShowTicksForMajor")]
    public bool ShowTicksForMajor
    {
      get
      {
        return (bool) this.GetValue(MinorTickmarkOptions.ShowTicksForMajorProperty);
      }
      set
      {
        this.SetValue(MinorTickmarkOptions.ShowTicksForMajorProperty, (object) (bool) (value ? 1 : 0));
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the z-index of minor tickmarks.
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
    [Category("Layout")]
    [DevExpressXpfGaugesLocalizedDescription("MinorTickmarkOptionsZIndex")]
    public int ZIndex
    {
      get
      {
        return (int) this.GetValue(MinorTickmarkOptions.ZIndexProperty);
      }
      set
      {
        this.SetValue(MinorTickmarkOptions.ZIndexProperty, (object) value);
      }
    }

    protected override bool IsTickVisible(TickmarkInfo elementInfo)
    {
      MinorTickmarkInfo minorTickmarkInfo = elementInfo as MinorTickmarkInfo;
      if (!base.IsTickVisible(elementInfo) || minorTickmarkInfo == null)
        return false;
      if (minorTickmarkInfo.BelowMajorTick)
        return this.ShowTicksForMajor;
      return true;
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new MinorTickmarkOptions();
    }
  }
}
