﻿// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.RedThermometerLinearScaleMarkerPresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class RedThermometerLinearScaleMarkerPresentation : PredefinedLinearScaleMarkerPresentation
  {
    protected override Brush DefaultFill
    {
      get
      {
        return (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 131, (byte) 151, (byte) 209));
      }
    }

    public override string PresentationName
    {
      get
      {
        return "Red Thermometer Marker";
      }
    }

    protected internal override PresentationControl CreateIndicatorPresentationControl()
    {
      return (PresentationControl) new RedThermometerLinearScaleMarkerControl();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new RedThermometerLinearScaleMarkerPresentation();
    }
  }
}
