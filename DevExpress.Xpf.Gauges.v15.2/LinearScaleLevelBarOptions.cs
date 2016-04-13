// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LinearScaleLevelBarOptions
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
  /// Contains layout  options for a linear scale's level bar.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class LinearScaleLevelBarOptions : GaugeDependencyObject
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
    public static readonly DependencyProperty OffsetProperty = DependencyPropertyManager.Register("Offset", typeof (double), typeof (LinearScaleLevelBarOptions), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty FactorThicknessProperty = DependencyPropertyManager.Register("FactorThickness", typeof (double), typeof (LinearScaleLevelBarOptions), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)), new ValidateValueCallback(LinearScaleLevelBarOptions.FactorValidation));
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
    public static readonly DependencyProperty ZIndexProperty = DependencyPropertyManager.Register("ZIndex", typeof (int), typeof (LinearScaleLevelBarOptions), new PropertyMetadata((object) 50, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the offset specifying a level bar's position on a Linear scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that specifies the level bar's offset.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleLevelBarOptionsOffset")]
    [Category("Layout")]
    public double Offset
    {
      get
      {
        return (double) this.GetValue(LinearScaleLevelBarOptions.OffsetProperty);
      }
      set
      {
        this.SetValue(LinearScaleLevelBarOptions.OffsetProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value by which the level bar's thickness should be multiplied.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that is the level bar's thickness multiplier.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleLevelBarOptionsFactorThickness")]
    [Category("Layout")]
    public double FactorThickness
    {
      get
      {
        return (double) this.GetValue(LinearScaleLevelBarOptions.FactorThicknessProperty);
      }
      set
      {
        this.SetValue(LinearScaleLevelBarOptions.FactorThicknessProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the z-index of a level bar.
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
    [DevExpressXpfGaugesLocalizedDescription("LinearScaleLevelBarOptionsZIndex")]
    [Category("Layout")]
    public int ZIndex
    {
      get
      {
        return (int) this.GetValue(LinearScaleLevelBarOptions.ZIndexProperty);
      }
      set
      {
        this.SetValue(LinearScaleLevelBarOptions.ZIndexProperty, (object) value);
      }
    }

    private static bool FactorValidation(object value)
    {
      return (double) value > 0.0;
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new LinearScaleLevelBarOptions();
    }
  }
}
