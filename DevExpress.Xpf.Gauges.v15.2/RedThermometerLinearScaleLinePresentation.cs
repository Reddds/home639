// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.RedThermometerLinearScaleLinePresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class RedThermometerLinearScaleLinePresentation : PredefinedLinearScaleLinePresentation
  {
    public override string PresentationName
    {
      get
      {
        return "Red Thermometer Scale Line";
      }
    }

    protected override Brush DefaultFill
    {
      get
      {
        return (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 58, (byte) 56, (byte) 50));
      }
    }

    protected internal override PresentationControl CreateLinePresentationControl()
    {
      return (PresentationControl) new RedThermometerLinearScaleLineControl();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new MagicLightLinearScaleLinePresentation();
    }
  }
}
