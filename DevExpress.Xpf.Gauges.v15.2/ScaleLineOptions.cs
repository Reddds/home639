// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ScaleLineOptions
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
  /// Contains layout and appearance options for a scale line.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class ScaleLineOptions : GaugeDependencyObject
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
    public static readonly DependencyProperty OffsetProperty = DependencyPropertyManager.Register("Offset", typeof (double), typeof (ScaleLineOptions), new PropertyMetadata((object) -27.0, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty ThicknessProperty = DependencyPropertyManager.Register("Thickness", typeof (int), typeof (ScaleLineOptions), new PropertyMetadata((object) 1, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)), new ValidateValueCallback(ScaleLineOptions.ThicknessValidation));
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
    public static readonly DependencyProperty ZIndexProperty = DependencyPropertyManager.Register("ZIndex", typeof (int), typeof (ScaleLineOptions), new PropertyMetadata((object) 0, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the offset specifying a line's position on a scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that specifies the line's offset.
    /// 
    /// </value>
    [Category("Layout")]
    [DevExpressXpfGaugesLocalizedDescription("ScaleLineOptionsOffset")]
    public double Offset
    {
      get
      {
        return (double) this.GetValue(ScaleLineOptions.OffsetProperty);
      }
      set
      {
        this.SetValue(ScaleLineOptions.OffsetProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value that specifies the thickness of the line on a scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Int32"/> value that is the thickness of the line.
    /// 
    /// </value>
    [Category("Layout")]
    [DevExpressXpfGaugesLocalizedDescription("ScaleLineOptionsThickness")]
    public int Thickness
    {
      get
      {
        return (int) this.GetValue(ScaleLineOptions.ThicknessProperty);
      }
      set
      {
        this.SetValue(ScaleLineOptions.ThicknessProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the z-index of a scale line.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Int32"/> value that is the z-index.
    /// 
    /// 
    /// </value>
    [Category("Layout")]
    [DevExpressXpfGaugesLocalizedDescription("ScaleLineOptionsZIndex")]
    public int ZIndex
    {
      get
      {
        return (int) this.GetValue(ScaleLineOptions.ZIndexProperty);
      }
      set
      {
        this.SetValue(ScaleLineOptions.ZIndexProperty, (object) value);
      }
    }

    private static bool ThicknessValidation(object value)
    {
      return (int) value > 0;
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new ScaleLineOptions();
    }
  }
}
