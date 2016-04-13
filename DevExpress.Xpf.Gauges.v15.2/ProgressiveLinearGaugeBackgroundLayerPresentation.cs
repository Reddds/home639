﻿// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ProgressiveLinearGaugeBackgroundLayerPresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class ProgressiveLinearGaugeBackgroundLayerPresentation : PredefinedLinearGaugeLayerPresentation
  {
    protected override Brush DefaultFill
    {
      get
      {
        RadialGradientBrush radialGradientBrush = new RadialGradientBrush()
        {
          Center = new Point(0.5, 0.0),
          GradientOrigin = new Point(0.5, 0.0),
          RadiusX = 2.862,
          RadiusY = 1.0
        };
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb(byte.MaxValue, (byte) 6, (byte) 28, (byte) 43),
          Offset = 1.0
        });
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb(byte.MaxValue, (byte) 93, (byte) 102, (byte) 125),
          Offset = 0.012
        });
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb(byte.MaxValue, (byte) 17, (byte) 27, (byte) 37),
          Offset = 0.32
        });
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 2, (byte) 7),
          Offset = 0.32001
        });
        return (Brush) radialGradientBrush;
      }
    }

    public override string PresentationName
    {
      get
      {
        return "Progressive Background";
      }
    }

    protected internal override PresentationControl CreateLayerPresentationControl()
    {
      return (PresentationControl) new ProgressiveGaugeBackgroundLayerControl();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new ProgressiveLinearGaugeBackgroundLayerPresentation();
    }
  }
}
