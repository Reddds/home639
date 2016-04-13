// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.CleanWhiteTickmarksPresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class CleanWhiteTickmarksPresentation : PredefinedTickmarksPresentation
  {
    public override string PresentationName
    {
      get
      {
        return "Clean White Tickmarks";
      }
    }

    protected override Brush DefaultMajorTickBrush
    {
      get
      {
        return (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 72, (byte) 78, (byte) 90));
      }
    }

    protected override Brush DefaultMinorTickBrush
    {
      get
      {
        return (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 72, (byte) 78, (byte) 90));
      }
    }

    protected internal override PresentationControl CreateMinorTickPresentationControl()
    {
      return (PresentationControl) new CleanWhiteMinorTickmarkControl();
    }

    protected internal override PresentationControl CreateMajorTickPresentationControl()
    {
      return (PresentationControl) new CleanWhiteMajorTickmarkControl();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new CleanWhiteTickmarksPresentation();
    }
  }
}
