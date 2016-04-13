// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.FlatLightLinearScaleLevelBarPresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class FlatLightLinearScaleLevelBarPresentation : PredefinedLinearScaleLevelBarPresentation
  {
    protected override Brush DefaultFill
    {
      get
      {
        return (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 231, (byte) 49, (byte) 65));
      }
    }

    public override string PresentationName
    {
      get
      {
        return "Flat Light Level Bar";
      }
    }

    protected internal override PresentationControl CreateIndicatorPresentationControl()
    {
      return (PresentationControl) new FlatLightLinearScaleLevelBarBackgroundControl();
    }

    protected internal override PresentationControl CreateForegroundPresentationControl()
    {
      return (PresentationControl) new FlatLightLinearScaleLevelBarForegroundControl();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new FlatLightLinearScaleLevelBarPresentation();
    }
  }
}
