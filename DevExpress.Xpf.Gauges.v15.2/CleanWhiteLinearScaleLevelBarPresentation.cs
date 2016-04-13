// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.CleanWhiteLinearScaleLevelBarPresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class CleanWhiteLinearScaleLevelBarPresentation : PredefinedLinearScaleLevelBarPresentation
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
          Color = Color.FromArgb(byte.MaxValue, (byte) 110, (byte) 117, (byte) 131),
          Offset = 0.0
        });
        linearGradientBrush.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb(byte.MaxValue, (byte) 90, (byte) 98, (byte) 112),
          Offset = 1.0
        });
        return (Brush) linearGradientBrush;
      }
    }

    public override string PresentationName
    {
      get
      {
        return "Clean White Level Bar";
      }
    }

    protected internal override PresentationControl CreateIndicatorPresentationControl()
    {
      return (PresentationControl) new CleanWhiteLinearScaleLevelBarBackgroundControl();
    }

    protected internal override PresentationControl CreateForegroundPresentationControl()
    {
      return (PresentationControl) new CleanWhiteLinearScaleLevelBarForegroundControl();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new CleanWhiteLinearScaleLevelBarPresentation();
    }
  }
}
