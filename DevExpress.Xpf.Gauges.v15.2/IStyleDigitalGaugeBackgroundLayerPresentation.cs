﻿// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.IStyleDigitalGaugeBackgroundLayerPresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class IStyleDigitalGaugeBackgroundLayerPresentation : PredefinedDigitalGaugeLayerPresentation
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
          Color = Color.FromArgb(byte.MaxValue, (byte) 231, (byte) 228, (byte) 231),
          Offset = 1.0
        });
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Colors.White,
          Offset = 0.0
        });
        return (Brush) radialGradientBrush;
      }
    }

    public override string PresentationName
    {
      get
      {
        return "IStyle Background";
      }
    }

    protected internal override PresentationControl CreateLayerPresentationControl()
    {
      return (PresentationControl) new IStyleDigitalGaugeBackgroundLayerControl();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new IStyleDigitalGaugeBackgroundLayerPresentation();
    }
  }
}
