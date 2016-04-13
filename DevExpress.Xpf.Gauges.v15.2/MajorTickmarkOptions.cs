// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.MajorTickmarkOptions
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
  /// Contains settings that define the layout and behavior of the major tickmarks along the scale.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class MajorTickmarkOptions : TickmarkOptions
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
    public static readonly DependencyProperty ShowFirstProperty = DependencyPropertyManager.Register("ShowFirst", typeof (bool), typeof (MajorTickmarkOptions), new PropertyMetadata((object) true, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty ShowLastProperty = DependencyPropertyManager.Register("ShowLast", typeof (bool), typeof (MajorTickmarkOptions), new PropertyMetadata((object) true, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty ZIndexProperty = DependencyPropertyManager.Register("ZIndex", typeof (int), typeof (MajorTickmarkOptions), new PropertyMetadata((object) 20, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value indicating whether or not the fitst major tickmark should be shown on a scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> to display the first major tickmark; otherwise <b>false</b>.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("MajorTickmarkOptionsShowFirst")]
    [Category("Behavior")]
    public bool ShowFirst
    {
      get
      {
        return (bool) this.GetValue(MajorTickmarkOptions.ShowFirstProperty);
      }
      set
      {
        this.SetValue(MajorTickmarkOptions.ShowFirstProperty, (object) (bool) (value ? 1 : 0));
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value indicating whether or not the last major tickmark should be shown on a scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> to display the last major tickmark on a scale; otherwise <b>false</b>.
    /// 
    /// </value>
    [Category("Behavior")]
    [DevExpressXpfGaugesLocalizedDescription("MajorTickmarkOptionsShowLast")]
    public bool ShowLast
    {
      get
      {
        return (bool) this.GetValue(MajorTickmarkOptions.ShowLastProperty);
      }
      set
      {
        this.SetValue(MajorTickmarkOptions.ShowLastProperty, (object) (bool) (value ? 1 : 0));
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the z-index of major tickmarks.
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
    [DevExpressXpfGaugesLocalizedDescription("MajorTickmarkOptionsZIndex")]
    public int ZIndex
    {
      get
      {
        return (int) this.GetValue(MajorTickmarkOptions.ZIndexProperty);
      }
      set
      {
        this.SetValue(MajorTickmarkOptions.ZIndexProperty, (object) value);
      }
    }

    protected override bool IsTickVisible(TickmarkInfo elementInfo)
    {
      MajorTickmarkInfo majorTickmarkInfo = elementInfo as MajorTickmarkInfo;
      if (!base.IsTickVisible(elementInfo) || majorTickmarkInfo == null || majorTickmarkInfo.IsFirstTick && !this.ShowFirst)
        return false;
      if (majorTickmarkInfo.IsLastTick)
        return this.ShowLast;
      return true;
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new MajorTickmarkOptions();
    }
  }
}
