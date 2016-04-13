// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.SymbolOptions
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
  /// Contains options that define the layout of symbols inside the symbols panel.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class SymbolOptions : GaugeDependencyObject
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
    public static readonly DependencyProperty MarginProperty = DependencyPropertyManager.Register("Margin", typeof (Thickness), typeof (SymbolOptions), new PropertyMetadata((object) new Thickness(0.0), new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty SkewAngleXProperty = DependencyPropertyManager.Register("SkewAngleX", typeof (double), typeof (SymbolOptions), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty SkewAngleYProperty = DependencyPropertyManager.Register("SkewAngleY", typeof (double), typeof (SymbolOptions), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the margin of a digital gauge symbol.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Windows.Thickness"/> value.
    /// 
    /// </value>
    [Category("Layout")]
    public Thickness Margin
    {
      get
      {
        return (Thickness) this.GetValue(SymbolOptions.MarginProperty);
      }
      set
      {
        this.SetValue(SymbolOptions.MarginProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a skew angle of a symbol  along the X axis.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that is a symbol skew angle along the X-axis.
    /// 
    /// </value>
    [Category("Layout")]
    public double SkewAngleX
    {
      get
      {
        return (double) this.GetValue(SymbolOptions.SkewAngleXProperty);
      }
      set
      {
        this.SetValue(SymbolOptions.SkewAngleXProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a skew angle of a symbol  along the Y-axis.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that is the symbol skew angle along the Y-axis.
    /// 
    /// </value>
    [Category("Layout")]
    public double SkewAngleY
    {
      get
      {
        return (double) this.GetValue(SymbolOptions.SkewAngleYProperty);
      }
      set
      {
        this.SetValue(SymbolOptions.SkewAngleYProperty, (object) value);
      }
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new SymbolOptions();
    }
  }
}
