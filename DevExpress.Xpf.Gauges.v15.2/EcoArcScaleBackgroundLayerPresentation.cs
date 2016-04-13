// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.EcoArcScaleBackgroundLayerPresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class EcoArcScaleBackgroundLayerPresentation : PredefinedArcScaleLayerPresentation
  {
    public override string PresentationName
    {
      get
      {
        return "Eco Background";
      }
    }

    protected override Brush DefaultFill
    {
      get
      {
        RadialGradientBrush radialGradientBrush = new RadialGradientBrush()
        {
          RadiusY = 0.519,
          RadiusX = 0.519
        };
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb((byte) 64, (byte) 33, (byte) 29, (byte) 21),
          Offset = 1.0
        });
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb((byte) 0, (byte) 33, (byte) 29, (byte) 21),
          Offset = 0.9
        });
        return (Brush) radialGradientBrush;
      }
    }

    protected internal override PresentationControl CreateLayerPresentationControl()
    {
      return (PresentationControl) new EcoArcScaleBackgroundLayerControl();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new EcoArcScaleBackgroundLayerPresentation();
    }
  }
}
