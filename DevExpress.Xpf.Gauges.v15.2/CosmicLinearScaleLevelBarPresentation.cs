// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.CosmicLinearScaleLevelBarPresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class CosmicLinearScaleLevelBarPresentation : PredefinedLinearScaleLevelBarPresentation
  {
    protected override Brush DefaultFill
    {
      get
      {
        LinearGradientBrush linearGradientBrush1 = new LinearGradientBrush();
        linearGradientBrush1.StartPoint = new Point(0.5, 1.0);
        linearGradientBrush1.EndPoint = new Point(0.5, 0.0);
        linearGradientBrush1.MappingMode = BrushMappingMode.RelativeToBoundingBox;
        LinearGradientBrush linearGradientBrush2 = linearGradientBrush1;
        linearGradientBrush2.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb(byte.MaxValue, (byte) 22, (byte) 28, (byte) 31)
        });
        linearGradientBrush2.GradientStops.Add(new GradientStop()
        {
          Color = Color.FromArgb(byte.MaxValue, (byte) 50, (byte) 65, (byte) 70),
          Offset = 1.0
        });
        return (Brush) linearGradientBrush2;
      }
    }

    public override string PresentationName
    {
      get
      {
        return "Cosmic Level Bar";
      }
    }

    protected internal override PresentationControl CreateIndicatorPresentationControl()
    {
      return (PresentationControl) new CosmicLinearScaleLevelBarBackgroundControl();
    }

    protected internal override PresentationControl CreateForegroundPresentationControl()
    {
      return (PresentationControl) new CosmicLinearScaleLevelBarForegroundControl();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new CosmicLinearScaleLevelBarPresentation();
    }
  }
}
