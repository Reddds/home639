﻿// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ClassicTickmarksPresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class ClassicTickmarksPresentation : PredefinedTickmarksPresentation
  {
    public override string PresentationName
    {
      get
      {
        return "Classic Tickmarks";
      }
    }

    protected override Brush DefaultMajorTickBrush
    {
      get
      {
        return (Brush) new SolidColorBrush(Colors.White);
      }
    }

    protected override Brush DefaultMinorTickBrush
    {
      get
      {
        return (Brush) new SolidColorBrush(Colors.White);
      }
    }

    protected internal override PresentationControl CreateMinorTickPresentationControl()
    {
      return (PresentationControl) new ClassicMinorTickmarkControl();
    }

    protected internal override PresentationControl CreateMajorTickPresentationControl()
    {
      return (PresentationControl) new ClassicMajorTickmarkControl();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new ClassicTickmarksPresentation();
    }
  }
}
