// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ScaleLabelOptions
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
  /// Serves as a base for classes that contain appearance and behavior options for scale labels.
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class ScaleLabelOptions : GaugeDependencyObject
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
    public static readonly DependencyProperty OffsetProperty = DependencyPropertyManager.Register("Offset", typeof (double), typeof (ScaleLabelOptions), new PropertyMetadata((object) -50.0, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty ShowFirstProperty = DependencyPropertyManager.Register("ShowFirst", typeof (bool), typeof (ScaleLabelOptions), new PropertyMetadata((object) true, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty ShowLastProperty = DependencyPropertyManager.Register("ShowLast", typeof (bool), typeof (ScaleLabelOptions), new PropertyMetadata((object) true, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty AddendProperty = DependencyPropertyManager.Register("Addend", typeof (double), typeof (ScaleLabelOptions), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty MultiplierProperty = DependencyPropertyManager.Register("Multiplier", typeof (double), typeof (ScaleLabelOptions), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty FormatStringProperty = DependencyPropertyManager.Register("FormatString", typeof (string), typeof (ScaleLabelOptions), new PropertyMetadata((object) "{0:0}", new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty ZIndexProperty = DependencyPropertyManager.Register("ZIndex", typeof (int), typeof (ScaleLabelOptions), new PropertyMetadata((object) 30, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the offset specifying a label's position on a scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that specifies the label's offset.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ScaleLabelOptionsOffset")]
    [Category("Layout")]
    public double Offset
    {
      get
      {
        return (double) this.GetValue(ScaleLabelOptions.OffsetProperty);
      }
      set
      {
        this.SetValue(ScaleLabelOptions.OffsetProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value indicating whether or not the first label should be shown on a scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> to display the first label on a scale; otherwise <b>false</b>.
    /// 
    /// </value>
    [Category("Behavior")]
    [DevExpressXpfGaugesLocalizedDescription("ScaleLabelOptionsShowFirst")]
    public bool ShowFirst
    {
      get
      {
        return (bool) this.GetValue(ScaleLabelOptions.ShowFirstProperty);
      }
      set
      {
        this.SetValue(ScaleLabelOptions.ShowFirstProperty, (object) (bool) (value ? 1 : 0));
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value indicating whether or not the last label should be shown on a scale.
    /// 
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> to display the last label on a scale; otherwise <b>false</b>.
    /// 
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ScaleLabelOptionsShowLast")]
    [Category("Behavior")]
    public bool ShowLast
    {
      get
      {
        return (bool) this.GetValue(ScaleLabelOptions.ShowLastProperty);
      }
      set
      {
        this.SetValue(ScaleLabelOptions.ShowLastProperty, (object) (bool) (value ? 1 : 0));
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value that should be added to every label's value.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value which is added to every label's value on a scale.
    /// 
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ScaleLabelOptionsAddend")]
    [Category("Data")]
    public double Addend
    {
      get
      {
        return (double) this.GetValue(ScaleLabelOptions.AddendProperty);
      }
      set
      {
        this.SetValue(ScaleLabelOptions.AddendProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value by which every label's value should be multiplied.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value which is a multiplier applied to every label's value on a scale.
    /// 
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ScaleLabelOptionsMultiplier")]
    [Category("Data")]
    public double Multiplier
    {
      get
      {
        return (double) this.GetValue(ScaleLabelOptions.MultiplierProperty);
      }
      set
      {
        this.SetValue(ScaleLabelOptions.MultiplierProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value that specifies the format string for the display text on a scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.String"/> value that is the format string of a label.
    /// 
    /// 
    /// </value>
    [Category("Data")]
    [DevExpressXpfGaugesLocalizedDescription("ScaleLabelOptionsFormatString")]
    public string FormatString
    {
      get
      {
        return (string) this.GetValue(ScaleLabelOptions.FormatStringProperty);
      }
      set
      {
        this.SetValue(ScaleLabelOptions.FormatStringProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the z-index of scale labels.
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
    [DevExpressXpfGaugesLocalizedDescription("ScaleLabelOptionsZIndex")]
    [Category("Layout")]
    public int ZIndex
    {
      get
      {
        return (int) this.GetValue(ScaleLabelOptions.ZIndexProperty);
      }
      set
      {
        this.SetValue(ScaleLabelOptions.ZIndexProperty, (object) value);
      }
    }

    protected internal abstract double CorrectAngleByOrientation(double angle);

    internal ScaleElementLayout CalculateLayout(ScaleLabelInfo elementInfo, ScaleMapping mapping)
    {
      if ((!elementInfo.Tickmark.IsFirstTick || this.ShowFirst) && (!elementInfo.Tickmark.IsLastTick || this.ShowLast) && !mapping.Layout.IsEmpty)
        return new ScaleElementLayout(this.CorrectAngleByOrientation(mapping.GetAngleByPercent(elementInfo.Tickmark.Alpha)), mapping.GetPointByPercent(elementInfo.Tickmark.Alpha, this.Offset));
      return (ScaleElementLayout) null;
    }
  }
}
