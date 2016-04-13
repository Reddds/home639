// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ScaleCustomLabel
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A scale custom label.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class ScaleCustomLabel : ScaleCustomElement
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
    public static readonly DependencyProperty OffsetProperty = DependencyPropertyManager.Register("Offset", typeof (double), typeof (ScaleCustomLabel), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ScaleCustomElement.PropertyChanged)));
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
    public static readonly DependencyProperty ValueProperty = DependencyPropertyManager.Register("Value", typeof (double), typeof (ScaleCustomLabel), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ScaleCustomElement.PropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the offset specifying a custom label's position on a scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that specifies the custom label's offset.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ScaleCustomLabelOffset")]
    [Category("Layout")]
    public double Offset
    {
      get
      {
        return (double) this.GetValue(ScaleCustomLabel.OffsetProperty);
      }
      set
      {
        this.SetValue(ScaleCustomLabel.OffsetProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the value about which the custom label is located on a scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value.
    /// 
    /// </value>
    [Category("Data")]
    [DevExpressXpfGaugesLocalizedDescription("ScaleCustomLabelValue")]
    public double Value
    {
      get
      {
        return (double) this.GetValue(ScaleCustomLabel.ValueProperty);
      }
      set
      {
        this.SetValue(ScaleCustomLabel.ValueProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the ScaleCustomLabel class with default settings.
    /// 
    /// </para>
    /// 
    /// </summary>
    public ScaleCustomLabel()
    {
      this.DefaultStyleKey = (object) typeof (ScaleCustomLabel);
    }

    protected override ElementLayout CreateLayout(ScaleMapping mapping)
    {
      ElementLayout elementLayout = new ElementLayout();
      elementLayout.CompleteLayout(mapping.GetPointByValue(this.Value, this.Offset), (Transform) null, (Geometry) null);
      return elementLayout;
    }
  }
}
