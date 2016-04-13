// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.RedThermometerLinearGaugeBackgroundLayerPresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class RedThermometerLinearGaugeBackgroundLayerPresentation : PredefinedLinearGaugeLayerPresentation
  {
    protected override Brush DefaultFill
    {
      get
      {
        RadialGradientBrush radialGradientBrush = new RadialGradientBrush()
        {
          Center = new Point(0.5, 0.0),
          GradientOrigin = new Point(0.5, 0.0),
          RadiusX = 2.0,
          RadiusY = 1.0
        };
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb((byte) 153, byte.MaxValue, byte.MaxValue, byte.MaxValue)
        });
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Colors.Transparent,
          Offset = 0.3801
        });
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb((byte) 76, byte.MaxValue, byte.MaxValue, byte.MaxValue),
          Offset = 0.38
        });
        return (Brush) radialGradientBrush;
      }
    }

    public override string PresentationName
    {
      get
      {
        return "Red Thermometer Background";
      }
    }

    protected internal override PresentationControl CreateLayerPresentationControl()
    {
      return (PresentationControl) new RedThermometerGaugeBackgroundLayerControl();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new RedThermometerLinearGaugeBackgroundLayerPresentation();
    }
  }
}
