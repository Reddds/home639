// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.SpindleCap
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges;
using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges.Native
{
  public class SpindleCap : ILayoutCalculator
  {
    private readonly ArcScale scale;
    private readonly LayerInfo info;

    private SpindleCapPresentation Presentation
    {
      get
      {
        return this.scale.ActualSpindleCapPresentation;
      }
    }

    private int ZIndex
    {
      get
      {
        return this.Options.ZIndex;
      }
    }

    private SpindleCapOptions Options
    {
      get
      {
        return this.scale.ActualSpindleCapOptions;
      }
    }

    public LayerInfo ElementInfo
    {
      get
      {
        return this.info;
      }
    }

    public SpindleCap(ArcScale scale)
    {
      this.scale = scale;
      this.info = new LayerInfo((ILayoutCalculator) this, this.ZIndex, this.Presentation.CreateLayerPresentationControl(), (PresentationBase) this.Presentation);
    }

    ElementLayout ILayoutCalculator.CreateLayout(Size constraint)
    {
      if (this.scale.ActualShowSpindleCap && this.scale.Mapping != null && !this.scale.Mapping.Layout.IsEmpty)
        return new ElementLayout();
      return (ElementLayout) null;
    }

    void ILayoutCalculator.CompleteLayout(ElementInfoBase elementInfo)
    {
      Point ellipseCenter = this.scale.Mapping.Layout.EllipseCenter;
      Point layoutOffset = this.scale.GetLayoutOffset();
      ellipseCenter.X += layoutOffset.X;
      ellipseCenter.Y += layoutOffset.Y;
      ScaleTransform scaleTransform = new ScaleTransform()
      {
        ScaleX = this.Options.FactorWidth,
        ScaleY = this.Options.FactorHeight
      };
      elementInfo.Layout.CompleteLayout(ellipseCenter, (Transform) scaleTransform, (Geometry) null);
    }

    public void UpdateModel()
    {
      this.info.Presentation = (PresentationBase) this.Presentation;
      this.info.PresentationControl = this.Presentation.CreateLayerPresentationControl();
      this.info.ZIndex = this.ZIndex;
    }
  }
}
