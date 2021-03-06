﻿// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.TickmarkInfo
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;

namespace DevExpress.Xpf.Gauges
{
  [NonCategorized]
  public abstract class TickmarkInfo : ScaleElementInfoBase
  {
    private readonly double alpha;

    internal double Alpha
    {
      get
      {
        return this.alpha;
      }
    }

    internal TickmarkInfo(PresentationControl presentationControl, PresentationBase presentation, double alpha)
      : base(presentationControl, presentation)
    {
      this.alpha = alpha;
    }
  }
}
