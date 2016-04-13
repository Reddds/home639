﻿// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.MagicLightTickmarksPresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class MagicLightTickmarksPresentation : PredefinedTickmarksPresentation
  {
    public override string PresentationName
    {
      get
      {
        return "Magic Light Tickmarks";
      }
    }

    protected override Brush DefaultMajorTickBrush
    {
      get
      {
        RadialGradientBrush radialGradientBrush = new RadialGradientBrush();
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb(byte.MaxValue, (byte) 96, (byte) 210, (byte) 226)
        });
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb((byte) 0, (byte) 96, (byte) 210, (byte) 226),
          Offset = 1.0
        });
        return (Brush) radialGradientBrush;
      }
    }

    protected override Brush DefaultMinorTickBrush
    {
      get
      {
        RadialGradientBrush radialGradientBrush = new RadialGradientBrush();
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb(byte.MaxValue, (byte) 96, (byte) 210, (byte) 226)
        });
        radialGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb((byte) 0, (byte) 96, (byte) 210, (byte) 226),
          Offset = 1.0
        });
        return (Brush) radialGradientBrush;
      }
    }

    protected internal override PresentationControl CreateMinorTickPresentationControl()
    {
      return (PresentationControl) new MagicLightMinorTickmarkControl();
    }

    protected internal override PresentationControl CreateMajorTickPresentationControl()
    {
      return (PresentationControl) new MagicLightMajorTickmarkControl();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new MagicLightTickmarksPresentation();
    }
  }
}