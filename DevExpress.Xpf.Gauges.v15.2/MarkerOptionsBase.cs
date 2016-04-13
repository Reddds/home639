// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.MarkerOptionsBase
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
  /// Serves as the base class for all marker options.
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class MarkerOptionsBase : GaugeDependencyObject
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
    public static readonly DependencyProperty OffsetProperty = DependencyPropertyManager.Register("Offset", typeof (double), typeof (MarkerOptionsBase), new PropertyMetadata((object) -32.0, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty FactorWidthProperty = DependencyPropertyManager.Register("FactorWidth", typeof (double), typeof (MarkerOptionsBase), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)), new ValidateValueCallback(MarkerOptionsBase.FactorsValidation));
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
    public static readonly DependencyProperty FactorHeightProperty = DependencyPropertyManager.Register("FactorHeight", typeof (double), typeof (MarkerOptionsBase), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)), new ValidateValueCallback(MarkerOptionsBase.FactorsValidation));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the offset specifying a marker's position on a scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that specifies the marker's offset.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("MarkerOptionsBaseOffset")]
    [Category("Layout")]
    public double Offset
    {
      get
      {
        return (double) this.GetValue(MarkerOptionsBase.OffsetProperty);
      }
      set
      {
        this.SetValue(MarkerOptionsBase.OffsetProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value by which the marker's width should be multiplied.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that is the marker's width multiplier.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("MarkerOptionsBaseFactorWidth")]
    [Category("Layout")]
    public double FactorWidth
    {
      get
      {
        return (double) this.GetValue(MarkerOptionsBase.FactorWidthProperty);
      }
      set
      {
        this.SetValue(MarkerOptionsBase.FactorWidthProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value by which the marker's height should be multiplied.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that is the marker's height multiplier.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("MarkerOptionsBaseFactorHeight")]
    [Category("Layout")]
    public double FactorHeight
    {
      get
      {
        return (double) this.GetValue(MarkerOptionsBase.FactorHeightProperty);
      }
      set
      {
        this.SetValue(MarkerOptionsBase.FactorHeightProperty, (object) value);
      }
    }

    private static bool FactorsValidation(object value)
    {
      return (double) value > 0.0;
    }
  }
}
