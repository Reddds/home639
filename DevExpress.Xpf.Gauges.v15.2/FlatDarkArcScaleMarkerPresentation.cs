// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.FlatDarkArcScaleMarkerPresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class FlatDarkArcScaleMarkerPresentation : PredefinedArcScaleMarkerPresentation
  {
    public override string PresentationName
    {
      get
      {
        return "Flat Dark Marker";
      }
    }

    protected override Brush DefaultFill
    {
      get
      {
        return (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 110, (byte) 110, (byte) 110));
      }
    }

    protected internal override PresentationControl CreateIndicatorPresentationControl()
    {
      return (PresentationControl) new FlatDarkArcScaleMarkerControl();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new FlatDarkArcScaleMarkerPresentation();
    }
  }
}
