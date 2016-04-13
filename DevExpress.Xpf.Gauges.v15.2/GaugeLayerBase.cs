// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.GaugeLayerBase
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// Serves as the base class for gauge-related layers.
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class GaugeLayerBase : LayerBase
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
    public static readonly DependencyProperty OptionsProperty = DependencyPropertyManager.Register("Options", typeof (LayerOptions), typeof (GaugeLayerBase), new PropertyMetadata(new PropertyChangedCallback(LayerBase.OptionsPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Provides access to the settings that specify the shape and position of the current gauge layer.
    /// 
    /// 
    /// 
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.LayerOptions"/> object that contains the settings of the layer.
    /// 
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("GaugeLayerBaseOptions")]
    [Category("Presentation")]
    public LayerOptions Options
    {
      get
      {
        return (LayerOptions) this.GetValue(GaugeLayerBase.OptionsProperty);
      }
      set
      {
        this.SetValue(GaugeLayerBase.OptionsProperty, (object) value);
      }
    }

    protected GaugeControlBase Gauge
    {
      get
      {
        return this.Owner as GaugeControlBase;
      }
    }

    protected override ElementLayout CreateLayout(Size constraint)
    {
      return new ElementLayout(constraint.Width, constraint.Height);
    }

    protected override void CompleteLayout(ElementInfoBase elementInfo)
    {
      elementInfo.Layout.CompleteLayout(new Point(0.0, 0.0), (Transform) null, (Geometry) null);
    }
  }
}
