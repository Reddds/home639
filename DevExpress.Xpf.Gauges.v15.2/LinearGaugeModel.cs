// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LinearGaugeModel
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  public abstract class LinearGaugeModel : GaugeModelBase
  {
    public static readonly DependencyProperty ScaleModelsProperty = DependencyPropertyManager.Register("ScaleModels", typeof (LinearScaleModelCollection), typeof (LinearGaugeModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeModelBase.CollectionPropertyChanged)));
    public static readonly DependencyProperty MarkerModelsProperty = DependencyPropertyManager.Register("MarkerModels", typeof (LinearScaleMarkerModelCollection), typeof (LinearGaugeModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeModelBase.CollectionPropertyChanged)));
    public static readonly DependencyProperty RangeBarModelsProperty = DependencyPropertyManager.Register("RangeBarModels", typeof (LinearScaleRangeBarModelCollection), typeof (LinearGaugeModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeModelBase.CollectionPropertyChanged)));
    public static readonly DependencyProperty LevelBarModelsProperty = DependencyPropertyManager.Register("LevelBarModels", typeof (LinearScaleLevelBarModelCollection), typeof (LinearGaugeModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeModelBase.CollectionPropertyChanged)));
    public static readonly DependencyProperty RangeModelsProperty = DependencyPropertyManager.Register("RangeModels", typeof (LinearScaleRangeModelCollection), typeof (LinearGaugeModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeModelBase.CollectionPropertyChanged)));
    public static readonly DependencyProperty LayerModelsProperty = DependencyPropertyManager.Register("LayerModels", typeof (LayerModelCollection), typeof (LinearGaugeModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeModelBase.CollectionPropertyChanged)));
    public static readonly DependencyProperty ScaleLayerModelsProperty = DependencyPropertyManager.Register("ScaleLayerModels", typeof (LayerModelCollection), typeof (LinearGaugeModel), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeModelBase.CollectionPropertyChanged)));

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public LinearScaleModelCollection ScaleModels
    {
      get
      {
        return (LinearScaleModelCollection) this.GetValue(LinearGaugeModel.ScaleModelsProperty);
      }
      set
      {
        this.SetValue(LinearGaugeModel.ScaleModelsProperty, (object) value);
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public LinearScaleMarkerModelCollection MarkerModels
    {
      get
      {
        return (LinearScaleMarkerModelCollection) this.GetValue(LinearGaugeModel.MarkerModelsProperty);
      }
      set
      {
        this.SetValue(LinearGaugeModel.MarkerModelsProperty, (object) value);
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public LinearScaleRangeBarModelCollection RangeBarModels
    {
      get
      {
        return (LinearScaleRangeBarModelCollection) this.GetValue(LinearGaugeModel.RangeBarModelsProperty);
      }
      set
      {
        this.SetValue(LinearGaugeModel.RangeBarModelsProperty, (object) value);
      }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public LinearScaleLevelBarModelCollection LevelBarModels
    {
      get
      {
        return (LinearScaleLevelBarModelCollection) this.GetValue(LinearGaugeModel.LevelBarModelsProperty);
      }
      set
      {
        this.SetValue(LinearGaugeModel.LevelBarModelsProperty, (object) value);
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public LinearScaleRangeModelCollection RangeModels
    {
      get
      {
        return (LinearScaleRangeModelCollection) this.GetValue(LinearGaugeModel.RangeModelsProperty);
      }
      set
      {
        this.SetValue(LinearGaugeModel.RangeModelsProperty, (object) value);
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public LayerModelCollection LayerModels
    {
      get
      {
        return (LayerModelCollection) this.GetValue(LinearGaugeModel.LayerModelsProperty);
      }
      set
      {
        this.SetValue(LinearGaugeModel.LayerModelsProperty, (object) value);
      }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public LayerModelCollection ScaleLayerModels
    {
      get
      {
        return (LayerModelCollection) this.GetValue(LinearGaugeModel.ScaleLayerModelsProperty);
      }
      set
      {
        this.SetValue(LinearGaugeModel.ScaleLayerModelsProperty, (object) value);
      }
    }

    internal LinearScaleModel GetScaleModel(int index)
    {
      return this.GetModel(LinearGaugeModel.ScaleModelsProperty, index) as LinearScaleModel;
    }

    internal LinearScaleMarkerModel GetMarkerModel(int index)
    {
      return this.GetModel(LinearGaugeModel.MarkerModelsProperty, index) as LinearScaleMarkerModel;
    }

    internal LinearScaleRangeBarModel GetRangeBarModel(int index)
    {
      return this.GetModel(LinearGaugeModel.RangeBarModelsProperty, index) as LinearScaleRangeBarModel;
    }

    internal LinearScaleLevelBarModel GetLevelBarModel(int index)
    {
      return this.GetModel(LinearGaugeModel.LevelBarModelsProperty, index) as LinearScaleLevelBarModel;
    }

    internal LinearScaleRangeModel GetRangeModel(int index)
    {
      return this.GetModel(LinearGaugeModel.RangeModelsProperty, index) as LinearScaleRangeModel;
    }

    internal LayerModel GetLayerModel(int index)
    {
      return this.GetModel(LinearGaugeModel.LayerModelsProperty, index) as LayerModel;
    }

    internal LayerModel GetScaleLayerModel(int index)
    {
      return this.GetModel(LinearGaugeModel.ScaleLayerModelsProperty, index) as LayerModel;
    }
  }
}
