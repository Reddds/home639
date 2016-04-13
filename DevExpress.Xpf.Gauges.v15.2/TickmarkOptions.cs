// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.TickmarkOptions
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// Serves as a base for classes that contain appearance and behavior options for tickmarks.
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class TickmarkOptions : GaugeDependencyObject
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
    public static readonly DependencyProperty OffsetProperty = DependencyPropertyManager.Register("Offset", typeof (double), typeof (TickmarkOptions), new PropertyMetadata((object) -27.0, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty FactorLengthProperty = DependencyPropertyManager.Register("FactorLength", typeof (double), typeof (TickmarkOptions), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty FactorThicknessProperty = DependencyPropertyManager.Register("FactorThickness", typeof (double), typeof (TickmarkOptions), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the offset of tickmarks shown on a scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that specifies the tickmark position on a scale.
    /// 
    /// 
    /// 
    /// </value>
    [Category("Layout")]
    [DevExpressXpfGaugesLocalizedDescription("TickmarkOptionsOffset")]
    public double Offset
    {
      get
      {
        return (double) this.GetValue(TickmarkOptions.OffsetProperty);
      }
      set
      {
        this.SetValue(TickmarkOptions.OffsetProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value by which the tickmarks' length should be multiplied.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that is the tickmarks' length multiplier.
    /// 
    /// </value>
    [Category("Layout")]
    [DevExpressXpfGaugesLocalizedDescription("TickmarkOptionsFactorLength")]
    public double FactorLength
    {
      get
      {
        return (double) this.GetValue(TickmarkOptions.FactorLengthProperty);
      }
      set
      {
        this.SetValue(TickmarkOptions.FactorLengthProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value by which the tickmarks' thickness should be multiplied.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that is the tickmarks' thickness multiplier.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("TickmarkOptionsFactorThickness")]
    [Category("Layout")]
    public double FactorThickness
    {
      get
      {
        return (double) this.GetValue(TickmarkOptions.FactorThicknessProperty);
      }
      set
      {
        this.SetValue(TickmarkOptions.FactorThicknessProperty, (object) value);
      }
    }

    protected virtual bool IsTickVisible(TickmarkInfo info)
    {
      return true;
    }

    internal ScaleElementLayout CalculateLayout(TickmarkInfo elementInfo, ScaleMapping mapping)
    {
      if (this.IsTickVisible(elementInfo) && !mapping.Layout.IsEmpty)
        return new ScaleElementLayout(mapping.GetAngleByPercent(elementInfo.Alpha), mapping.GetPointByPercent(elementInfo.Alpha, this.Offset), new Point(this.FactorLength, this.FactorThickness));
      return (ScaleElementLayout) null;
    }
  }
}
