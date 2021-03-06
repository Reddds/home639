﻿// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ProgressiveLinearScaleForegroundLayerPresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class ProgressiveLinearScaleForegroundLayerPresentation : PredefinedLinearScaleLayerPresentation
  {
    protected override Brush DefaultFill
    {
      get
      {
        RadialGradientBrush radialGradientBrush = new RadialGradientBrush()
        {
          Center = new Point(0.501, 0.007),
          GradientOrigin = new Point(0.501, 0.007),
          RadiusX = 2.862,
          RadiusY = 1.0
        };
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb((byte) 25, (byte) 197, (byte) 211, byte.MaxValue),
          Offset = 0.35
        });
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb((byte) 127, (byte) 197, (byte) 211, byte.MaxValue)
        });
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb((byte) 0, (byte) 197, (byte) 211, byte.MaxValue),
          Offset = 0.35001
        });
        return (Brush) radialGradientBrush;
      }
    }

    public override string PresentationName
    {
      get
      {
        return "Progressive Foreground";
      }
    }

    protected internal override PresentationControl CreateLayerPresentationControl()
    {
      return (PresentationControl) new ProgressiveLinearScaleForegroundLayerControl();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new ProgressiveLinearScaleForegroundLayerPresentation();
    }
  }
}
