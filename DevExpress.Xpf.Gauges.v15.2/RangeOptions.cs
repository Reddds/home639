// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.RangeOptions
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
  /// Contains  layout and appearance options for a range.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class RangeOptions : GaugeDependencyObject
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
    public static readonly DependencyProperty OffsetProperty = DependencyPropertyManager.Register("Offset", typeof (double), typeof (RangeOptions), new PropertyMetadata((object) -19.0, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty ThicknessProperty = DependencyPropertyManager.Register("Thickness", typeof (int), typeof (RangeOptions), new PropertyMetadata((object) 5, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)), new ValidateValueCallback(RangeOptions.ThicknessValidation));
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
    public static readonly DependencyProperty ZIndexProperty = DependencyPropertyManager.Register("ZIndex", typeof (int), typeof (RangeOptions), new PropertyMetadata((object) -10, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the offset specifying a range's position on a scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that specifies the range's offset.
    /// 
    /// </value>
    [Category("Layout")]
    [DevExpressXpfGaugesLocalizedDescription("RangeOptionsOffset")]
    public double Offset
    {
      get
      {
        return (double) this.GetValue(RangeOptions.OffsetProperty);
      }
      set
      {
        this.SetValue(RangeOptions.OffsetProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value that specifies the thickness of the range on a scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Int32"/> value that is the thickness of the range.
    /// 
    /// </value>
    [Category("Layout")]
    [DevExpressXpfGaugesLocalizedDescription("RangeOptionsThickness")]
    public int Thickness
    {
      get
      {
        return (int) this.GetValue(RangeOptions.ThicknessProperty);
      }
      set
      {
        this.SetValue(RangeOptions.ThicknessProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the z-index of a range.
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
    [DevExpressXpfGaugesLocalizedDescription("RangeOptionsZIndex")]
    public int ZIndex
    {
      get
      {
        return (int) this.GetValue(RangeOptions.ZIndexProperty);
      }
      set
      {
        this.SetValue(RangeOptions.ZIndexProperty, (object) value);
      }
    }

    private static bool ThicknessValidation(object value)
    {
      return (int) value > 0;
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new RangeOptions();
    }
  }
}
