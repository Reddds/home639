﻿// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.FutureArcScaleBackgroundLayerPresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class FutureArcScaleBackgroundLayerPresentation : PredefinedArcScaleLayerPresentation
  {
    public override string PresentationName
    {
      get
      {
        return "Future Background";
      }
    }

    protected override Brush DefaultFill
    {
      get
      {
        RadialGradientBrush radialGradientBrush = new RadialGradientBrush()
        {
          Center = new Point(0.5, 1.0),
          GradientOrigin = new Point(0.5, 1.0),
          RadiusX = 1.0,
          RadiusY = 1.0
        };
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb(byte.MaxValue, (byte) 50, (byte) 47, (byte) 89),
          Offset = 0.0
        });
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb(byte.MaxValue, (byte) 9, (byte) 9, (byte) 14),
          Offset = 0.996
        });
        return (Brush) radialGradientBrush;
      }
    }

    protected internal override PresentationControl CreateLayerPresentationControl()
    {
      return (PresentationControl) new FutureArcScaleBackgroundLayerControl();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new FutureArcScaleBackgroundLayerPresentation();
    }
  }
}
