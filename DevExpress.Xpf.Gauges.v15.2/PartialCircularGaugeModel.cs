// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.PartialCircularGaugeModel
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  public abstract class PartialCircularGaugeModel : GaugeModelBase
  {
    public static readonly DependencyProperty ScaleModelsProperty = DependencyPropertyManager.Register("ScaleModels", typeof (ArcScaleModelCollection), typeof (PartialCircularGaugeModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeModelBase.CollectionPropertyChanged)));
    public static readonly DependencyProperty NeedleModelsProperty = DependencyPropertyManager.Register("NeedleModels", typeof (ArcScaleNeedleModelCollection), typeof (PartialCircularGaugeModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeModelBase.CollectionPropertyChanged)));
    public static readonly DependencyProperty MarkerModelsProperty = DependencyPropertyManager.Register("MarkerModels", typeof (ArcScaleMarkerModelCollection), typeof (PartialCircularGaugeModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeModelBase.CollectionPropertyChanged)));
    public static readonly DependencyProperty RangeBarModelsProperty = DependencyPropertyManager.Register("RangeBarModels", typeof (ArcScaleRangeBarModelCollection), typeof (PartialCircularGaugeModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeModelBase.CollectionPropertyChanged)));
    public static readonly DependencyProperty RangeModelsProperty = DependencyPropertyManager.Register("RangeModels", typeof (ArcScaleRangeModelCollection), typeof (PartialCircularGaugeModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeModelBase.CollectionPropertyChanged)));
    public static readonly DependencyProperty LayerModelsProperty = DependencyPropertyManager.Register("LayerModels", typeof (LayerModelCollection), typeof (PartialCircularGaugeModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeModelBase.CollectionPropertyChanged)));
    public static readonly DependencyProperty ScaleLayerModelsProperty = DependencyPropertyManager.Register("ScaleLayerModels", typeof (LayerModelCollection), typeof (PartialCircularGaugeModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeModelBase.CollectionPropertyChanged)));

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ArcScaleModelCollection ScaleModels
    {
      get
      {
        return (ArcScaleModelCollection) this.GetValue(PartialCircularGaugeModel.ScaleModelsProperty);
      }
      set
      {
        this.SetValue(PartialCircularGaugeModel.ScaleModelsProperty, (object) value);
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ArcScaleNeedleModelCollection NeedleModels
    {
      get
      {
        return (ArcScaleNeedleModelCollection) this.GetValue(PartialCircularGaugeModel.NeedleModelsProperty);
      }
      set
      {
        this.SetValue(PartialCircularGaugeModel.NeedleModelsProperty, (object) value);
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public ArcScaleMarkerModelCollection MarkerModels
    {
      get
      {
        return (ArcScaleMarkerModelCollection) this.GetValue(PartialCircularGaugeModel.MarkerModelsProperty);
      }
      set
      {
        this.SetValue(PartialCircularGaugeModel.MarkerModelsProperty, (object) value);
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public ArcScaleRangeBarModelCollection RangeBarModels
    {
      get
      {
        return (ArcScaleRangeBarModelCollection) this.GetValue(PartialCircularGaugeModel.RangeBarModelsProperty);
      }
      set
      {
        this.SetValue(PartialCircularGaugeModel.RangeBarModelsProperty, (object) value);
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ArcScaleRangeModelCollection RangeModels
    {
      get
      {
        return (ArcScaleRangeModelCollection) this.GetValue(PartialCircularGaugeModel.RangeModelsProperty);
      }
      set
      {
        this.SetValue(PartialCircularGaugeModel.RangeModelsProperty, (object) value);
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public LayerModelCollection LayerModels
    {
      get
      {
        return (LayerModelCollection) this.GetValue(PartialCircularGaugeModel.LayerModelsProperty);
      }
      set
      {
        this.SetValue(PartialCircularGaugeModel.LayerModelsProperty, (object) value);
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public LayerModelCollection ScaleLayerModels
    {
      get
      {
        return (LayerModelCollection) this.GetValue(PartialCircularGaugeModel.ScaleLayerModelsProperty);
      }
      set
      {
        this.SetValue(PartialCircularGaugeModel.ScaleLayerModelsProperty, (object) value);
      }
    }

    internal ArcScaleModel GetScaleModel(int index)
    {
      return this.GetModel(PartialCircularGaugeModel.ScaleModelsProperty, index) as ArcScaleModel;
    }

    internal ArcScaleNeedleModel GetNeedleModel(int index)
    {
      return this.GetModel(PartialCircularGaugeModel.NeedleModelsProperty, index) as ArcScaleNeedleModel;
    }

    internal ArcScaleMarkerModel GetMarkerModel(int index)
    {
      return this.GetModel(PartialCircularGaugeModel.MarkerModelsProperty, index) as ArcScaleMarkerModel;
    }

    internal ArcScaleRangeBarModel GetRangeBarModel(int index)
    {
      return this.GetModel(PartialCircularGaugeModel.RangeBarModelsProperty, index) as ArcScaleRangeBarModel;
    }

    internal ArcScaleRangeModel GetRangeModel(int index)
    {
      return this.GetModel(PartialCircularGaugeModel.RangeModelsProperty, index) as ArcScaleRangeModel;
    }

    internal LayerModel GetLayerModel(int index)
    {
      return this.GetModel(PartialCircularGaugeModel.LayerModelsProperty, index) as LayerModel;
    }

    internal LayerModel GetScaleLayerModel(int index)
    {
      return this.GetModel(PartialCircularGaugeModel.ScaleLayerModelsProperty, index) as LayerModel;
    }
  }
}
