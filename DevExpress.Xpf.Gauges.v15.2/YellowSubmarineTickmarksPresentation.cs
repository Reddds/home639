// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.YellowSubmarineTickmarksPresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class YellowSubmarineTickmarksPresentation : PredefinedTickmarksPresentation
  {
    public override string PresentationName
    {
      get
      {
        return "Yellow Submarine Tickmarks";
      }
    }

    protected override Brush DefaultMajorTickBrush
    {
      get
      {
        return (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 53, (byte) 59, (byte) 74));
      }
    }

    protected override Brush DefaultMinorTickBrush
    {
      get
      {
        return (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 53, (byte) 59, (byte) 74));
      }
    }

    protected internal override PresentationControl CreateMinorTickPresentationControl()
    {
      return (PresentationControl) new YellowSubmarineMinorTickmarkControl();
    }

    protected internal override PresentationControl CreateMajorTickPresentationControl()
    {
      return (PresentationControl) new YellowSubmarineMajorTickmarkControl();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new YellowSubmarineTickmarksPresentation();
    }
  }
}
