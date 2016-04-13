// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.DigitalGaugeModel
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
  /// For internal use.
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class DigitalGaugeModel : GaugeModelBase
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
    public static readonly DependencyProperty SevenSegmentsModelProperty = DependencyPropertyManager.Register("SevenSegmentsModel", typeof (SevenSegmentsModel), typeof (DigitalGaugeModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeModelBase.PropertyChanged)));
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
    public static readonly DependencyProperty FourteenSegmentsModelProperty = DependencyPropertyManager.Register("FourteenSegmentsModel", typeof (FourteenSegmentsModel), typeof (DigitalGaugeModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeModelBase.PropertyChanged)));
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
    public static readonly DependencyProperty Matrix5x8ModelProperty = DependencyPropertyManager.Register("Matrix5x8Model", typeof (Matrix5x8Model), typeof (DigitalGaugeModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeModelBase.PropertyChanged)));
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
    public static readonly DependencyProperty Matrix8x14ModelProperty = DependencyPropertyManager.Register("Matrix8x14Model", typeof (Matrix8x14Model), typeof (DigitalGaugeModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeModelBase.PropertyChanged)));
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
    public static readonly DependencyProperty LayerModelsProperty = DependencyPropertyManager.Register("LayerModels", typeof (LayerModelCollection), typeof (DigitalGaugeModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeModelBase.CollectionPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// For internal use. Gets or sets a model for the seven segments view type of the digital gauge  control that is used to draw its elements.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.SevenSegmentsModel"/> class descendant that is the actual model.
    /// 
    /// </value>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public SevenSegmentsModel SevenSegmentsModel
    {
      get
      {
        return (SevenSegmentsModel) this.GetValue(DigitalGaugeModel.SevenSegmentsModelProperty);
      }
      set
      {
        this.SetValue(DigitalGaugeModel.SevenSegmentsModelProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// For internal use. Gets or sets a model for the fourteen segments view type of the digital gauge  control that is used to draw its elements.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.FourteenSegmentsModel"/> class descendant that is the actual model.
    /// 
    /// </value>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public FourteenSegmentsModel FourteenSegmentsModel
    {
      get
      {
        return (FourteenSegmentsModel) this.GetValue(DigitalGaugeModel.FourteenSegmentsModelProperty);
      }
      set
      {
        this.SetValue(DigitalGaugeModel.FourteenSegmentsModelProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// For internal use. Gets or sets a model for the matrix5x8 view type of the digital gauge control that is used to draw its elements.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.Matrix5x8Model"/> class descendant that is the actual model.
    /// 
    /// </value>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Matrix5x8Model Matrix5x8Model
    {
      get
      {
        return (Matrix5x8Model) this.GetValue(DigitalGaugeModel.Matrix5x8ModelProperty);
      }
      set
      {
        this.SetValue(DigitalGaugeModel.Matrix5x8ModelProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// For internal use. Gets or sets a model for the matrix8x14 view type of the digital gauge control that is used to draw its elements.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.Matrix8x14Model"/> class descendant that is the actual model.
    /// 
    /// </value>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Matrix8x14Model Matrix8x14Model
    {
      get
      {
        return (Matrix8x14Model) this.GetValue(DigitalGaugeModel.Matrix8x14ModelProperty);
      }
      set
      {
        this.SetValue(DigitalGaugeModel.Matrix8x14ModelProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// For internal use. Provides access to a collection of layer models contained in the current Digital Gauge control.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.LayerModelCollection"/> object that contains layer models of a digital gauge.
    /// 
    /// </value>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public LayerModelCollection LayerModels
    {
      get
      {
        return (LayerModelCollection) this.GetValue(DigitalGaugeModel.LayerModelsProperty);
      }
      set
      {
        this.SetValue(DigitalGaugeModel.LayerModelsProperty, (object) value);
      }
    }

    internal LayerModel GetLayerModel(int index)
    {
      return this.GetModel(DigitalGaugeModel.LayerModelsProperty, index) as LayerModel;
    }
  }
}
