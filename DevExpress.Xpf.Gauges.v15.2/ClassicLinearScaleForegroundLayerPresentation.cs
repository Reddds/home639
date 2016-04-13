// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ClassicLinearScaleForegroundLayerPresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class ClassicLinearScaleForegroundLayerPresentation : PredefinedLinearScaleLayerPresentation
  {
    protected override Brush DefaultFill
    {
      get
      {
        RadialGradientBrush radialGradientBrush = new RadialGradientBrush()
        {
          Center = new Point(0.5, -0.186),
          GradientOrigin = new Point(0.5, -0.186),
          RadiusX = 1.281,
          RadiusY = 0.525
        };
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Offset = 1.0
        });
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb((byte) 96, byte.MaxValue, byte.MaxValue, byte.MaxValue)
        });
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb((byte) 12, byte.MaxValue, byte.MaxValue, byte.MaxValue),
          Offset = 0.9999
        });
        return (Brush) radialGradientBrush;
      }
    }

    public override string PresentationName
    {
      get
      {
        return "Classic Foreground";
      }
    }

    protected internal override PresentationControl CreateLayerPresentationControl()
    {
      return (PresentationControl) new ClassicLinearScaleForegroundLayerControl();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new ClassicLinearScaleForegroundLayerPresentation();
    }
  }
}
