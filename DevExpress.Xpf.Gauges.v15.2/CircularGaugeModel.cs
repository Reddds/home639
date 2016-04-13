// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.CircularGaugeModel
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  public abstract class CircularGaugeModel : GaugeModelBase
  {
    public static readonly DependencyProperty ModelFullProperty = DependencyPropertyManager.Register("ModelFull", typeof (PartialCircularGaugeModel), typeof (CircularGaugeModel), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty ModelHalfTopProperty = DependencyPropertyManager.Register("ModelHalfTop", typeof (PartialCircularGaugeModel), typeof (CircularGaugeModel));
    public static readonly DependencyProperty ModelQuarterTopLeftProperty = DependencyPropertyManager.Register("ModelQuarterTopLeft", typeof (PartialCircularGaugeModel), typeof (CircularGaugeModel));
    public static readonly DependencyProperty ModelQuarterTopRightProperty = DependencyPropertyManager.Register("ModelQuarterTopRight", typeof (PartialCircularGaugeModel), typeof (CircularGaugeModel));
    public static readonly DependencyProperty ModelThreeQuartersProperty = DependencyPropertyManager.Register("ModelThreeQuarters", typeof (PartialCircularGaugeModel), typeof (CircularGaugeModel));

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public PartialCircularGaugeModel ModelFull
    {
      get
      {
        return (PartialCircularGaugeModel) this.GetValue(CircularGaugeModel.ModelFullProperty);
      }
      set
      {
        this.SetValue(CircularGaugeModel.ModelFullProperty, (object) value);
      }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public PartialCircularGaugeModel ModelHalfTop
    {
      get
      {
        return (PartialCircularGaugeModel) this.GetValue(CircularGaugeModel.ModelHalfTopProperty);
      }
      set
      {
        this.SetValue(CircularGaugeModel.ModelHalfTopProperty, (object) value);
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public PartialCircularGaugeModel ModelQuarterTopLeft
    {
      get
      {
        return (PartialCircularGaugeModel) this.GetValue(CircularGaugeModel.ModelQuarterTopLeftProperty);
      }
      set
      {
        this.SetValue(CircularGaugeModel.ModelQuarterTopLeftProperty, (object) value);
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public PartialCircularGaugeModel ModelQuarterTopRight
    {
      get
      {
        return (PartialCircularGaugeModel) this.GetValue(CircularGaugeModel.ModelQuarterTopRightProperty);
      }
      set
      {
        this.SetValue(CircularGaugeModel.ModelQuarterTopRightProperty, (object) value);
      }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public PartialCircularGaugeModel ModelThreeQuarters
    {
      get
      {
        return (PartialCircularGaugeModel) this.GetValue(CircularGaugeModel.ModelThreeQuartersProperty);
      }
      set
      {
        this.SetValue(CircularGaugeModel.ModelThreeQuartersProperty, (object) value);
      }
    }

    internal ArcScaleModel GetScaleModel(ArcScaleLayoutMode scaleLayoutMode, int index)
    {
      switch (scaleLayoutMode)
      {
        case ArcScaleLayoutMode.HalfTop:
          return this.ModelHalfTop.GetScaleModel(index);
        case ArcScaleLayoutMode.QuarterTopLeft:
          return this.ModelQuarterTopLeft.GetScaleModel(index);
        case ArcScaleLayoutMode.QuarterTopRight:
          return this.ModelQuarterTopRight.GetScaleModel(index);
        case ArcScaleLayoutMode.ThreeQuarters:
          return this.ModelThreeQuarters.GetScaleModel(index);
        default:
          return this.ModelFull.GetScaleModel(index);
      }
    }

    internal ArcScaleNeedleModel GetNeedleModel(ArcScaleLayoutMode scaleLayoutMode, int index)
    {
      switch (scaleLayoutMode)
      {
        case ArcScaleLayoutMode.HalfTop:
          return this.ModelHalfTop.GetNeedleModel(index);
        case ArcScaleLayoutMode.QuarterTopLeft:
          return this.ModelQuarterTopLeft.GetNeedleModel(index);
        case ArcScaleLayoutMode.QuarterTopRight:
          return this.ModelQuarterTopRight.GetNeedleModel(index);
        case ArcScaleLayoutMode.ThreeQuarters:
          return this.ModelThreeQuarters.GetNeedleModel(index);
        default:
          return this.ModelFull.GetNeedleModel(index);
      }
    }

    internal ArcScaleMarkerModel GetMarkerModel(int index)
    {
      return this.ModelFull.GetMarkerModel(index);
    }

    internal ArcScaleRangeBarModel GetRangeBarModel(int index)
    {
      return this.ModelFull.GetRangeBarModel(index);
    }

    internal ArcScaleRangeModel GetRangeModel(int index)
    {
      return this.ModelFull.GetRangeModel(index);
    }

    internal LayerModel GetLayerModel(int index)
    {
      return this.ModelFull.GetLayerModel(index);
    }

    internal LayerModel GetScaleLayerModel(ArcScaleLayoutMode scaleLayoutMode, int index)
    {
      switch (scaleLayoutMode)
      {
        case ArcScaleLayoutMode.HalfTop:
          return this.ModelHalfTop.GetScaleLayerModel(index);
        case ArcScaleLayoutMode.QuarterTopLeft:
          return this.ModelQuarterTopLeft.GetScaleLayerModel(index);
        case ArcScaleLayoutMode.QuarterTopRight:
          return this.ModelQuarterTopRight.GetScaleLayerModel(index);
        case ArcScaleLayoutMode.ThreeQuarters:
          return this.ModelThreeQuarters.GetScaleLayerModel(index);
        default:
          return this.ModelFull.GetScaleLayerModel(index);
      }
    }

    protected override void OwnerChanged()
    {
      base.OwnerChanged();
      IOwnedElement ownedElement1 = (IOwnedElement) this.ModelFull;
      if (ownedElement1 != null)
        ownedElement1.Owner = this.Owner;
      IOwnedElement ownedElement2 = (IOwnedElement) this.ModelHalfTop;
      if (ownedElement2 != null)
        ownedElement2.Owner = this.Owner;
      IOwnedElement ownedElement3 = (IOwnedElement) this.ModelQuarterTopLeft;
      if (ownedElement3 != null)
        ownedElement3.Owner = this.Owner;
      IOwnedElement ownedElement4 = (IOwnedElement) this.ModelQuarterTopRight;
      if (ownedElement4 != null)
        ownedElement4.Owner = this.Owner;
      IOwnedElement ownedElement5 = (IOwnedElement) this.ModelThreeQuarters;
      if (ownedElement5 == null)
        return;
      ownedElement5.Owner = this.Owner;
    }
  }
}
