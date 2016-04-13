// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.RangeBarOptionsBase
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
  /// Serves as the base class for range bar options.
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class RangeBarOptionsBase : GaugeDependencyObject
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
    public static readonly DependencyProperty OffsetProperty = DependencyPropertyManager.Register("Offset", typeof (double), typeof (RangeBarOptionsBase), new PropertyMetadata((object) -90.0, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty ThicknessProperty = DependencyPropertyManager.Register("Thickness", typeof (int), typeof (RangeBarOptionsBase), new PropertyMetadata((object) 10, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)), new ValidateValueCallback(RangeBarOptionsBase.ThicknessValidation));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the offset specifying a range bar's position on a scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that specifies the range bar's offset.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("RangeBarOptionsBaseOffset")]
    [Category("Layout")]
    public double Offset
    {
      get
      {
        return (double) this.GetValue(RangeBarOptionsBase.OffsetProperty);
      }
      set
      {
        this.SetValue(RangeBarOptionsBase.OffsetProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value that specifies the thickness of the range bar on a scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Int32"/> value that is the thickness of the range bar.
    /// 
    /// </value>
    [Category("Layout")]
    [DevExpressXpfGaugesLocalizedDescription("RangeBarOptionsBaseThickness")]
    public int Thickness
    {
      get
      {
        return (int) this.GetValue(RangeBarOptionsBase.ThicknessProperty);
      }
      set
      {
        this.SetValue(RangeBarOptionsBase.ThicknessProperty, (object) value);
      }
    }

    private static bool ThicknessValidation(object value)
    {
      return (int) value > 0;
    }
  }
}
