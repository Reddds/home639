// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ProgressiveArcScaleLinePresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class ProgressiveArcScaleLinePresentation : PredefinedArcScaleLinePresentation
  {
    public override string PresentationName
    {
      get
      {
        return "Progressive Scale Line";
      }
    }

    protected override Brush DefaultFill
    {
      get
      {
        RadialGradientBrush radialGradientBrush = new RadialGradientBrush()
        {
          Center = new Point(0.5, 0.0),
          GradientOrigin = new Point(0.5, 0.0),
          RadiusX = 1.0,
          RadiusY = 1.0
        };
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb(byte.MaxValue, (byte) 101, (byte) 203, (byte) 246)
        });
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb(byte.MaxValue, (byte) 43, (byte) 90, (byte) 112),
          Offset = 1.0
        });
        return (Brush) radialGradientBrush;
      }
    }

    protected internal override PresentationControl CreateLinePresentationControl()
    {
      return (PresentationControl) new ProgressiveArcScaleLineControl();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new ProgressiveArcScaleLinePresentation();
    }
  }
}
