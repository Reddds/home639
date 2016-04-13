// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.SmartLinearScaleLevelBarPresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class SmartLinearScaleLevelBarPresentation : PredefinedLinearScaleLevelBarPresentation
  {
    protected override Brush DefaultFill
    {
      get
      {
        LinearGradientBrush linearGradientBrush = new LinearGradientBrush()
        {
          StartPoint = new Point(0.5, 1.0),
          EndPoint = new Point(0.5, 0.0)
        };
        linearGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb(byte.MaxValue, (byte) 210, (byte) 93, (byte) 82),
          Offset = 0.0
        });
        linearGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb(byte.MaxValue, (byte) 182, (byte) 56, (byte) 44),
          Offset = 1.0
        });
        return (Brush) linearGradientBrush;
      }
    }

    public override string PresentationName
    {
      get
      {
        return "Smart Level Bar";
      }
    }

    protected internal override PresentationControl CreateIndicatorPresentationControl()
    {
      return (PresentationControl) new SmartLinearScaleLevelBarBackgroundControl();
    }

    protected internal override PresentationControl CreateForegroundPresentationControl()
    {
      return (PresentationControl) new SmartLinearScaleLevelBarForegroundControl();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new SmartLinearScaleLevelBarPresentation();
    }
  }
}
