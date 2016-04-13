// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.SpindleCapOptions
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
  /// Contains layout options for a spindle cap.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class SpindleCapOptions : GaugeDependencyObject
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
    public static readonly DependencyProperty FactorWidthProperty = DependencyPropertyManager.Register("FactorWidth", typeof (double), typeof (SpindleCapOptions), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty FactorHeightProperty = DependencyPropertyManager.Register("FactorHeight", typeof (double), typeof (SpindleCapOptions), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty ZIndexProperty = DependencyPropertyManager.Register("ZIndex", typeof (int), typeof (SpindleCapOptions), new PropertyMetadata((object) 150, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value by which the spindle cap's width should be multiplied.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that is the spindle cap's width multiplier.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("SpindleCapOptionsFactorWidth")]
    [Category("Layout")]
    public double FactorWidth
    {
      get
      {
        return (double) this.GetValue(SpindleCapOptions.FactorWidthProperty);
      }
      set
      {
        this.SetValue(SpindleCapOptions.FactorWidthProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value by which the spindle cap's height should be multiplied.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that is the spindle cap's height multiplier.
    /// 
    /// </value>
    [Category("Layout")]
    [DevExpressXpfGaugesLocalizedDescription("SpindleCapOptionsFactorHeight")]
    public double FactorHeight
    {
      get
      {
        return (double) this.GetValue(SpindleCapOptions.FactorHeightProperty);
      }
      set
      {
        this.SetValue(SpindleCapOptions.FactorHeightProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the z-index of a spindle cap.
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
    [DevExpressXpfGaugesLocalizedDescription("SpindleCapOptionsZIndex")]
    [Category("Layout")]
    public int ZIndex
    {
      get
      {
        return (int) this.GetValue(SpindleCapOptions.ZIndexProperty);
      }
      set
      {
        this.SetValue(SpindleCapOptions.ZIndexProperty, (object) value);
      }
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new SpindleCapOptions();
    }
  }
}
