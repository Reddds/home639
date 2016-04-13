// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.MajorTickmarkInfo
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

namespace DevExpress.Xpf.Gauges
{
  public class MajorTickmarkInfo : TickmarkInfo
  {
    private readonly double value;
    private readonly bool isFirstTick;
    private readonly bool isLastTick;

    internal double Value
    {
      get
      {
        return this.value;
      }
    }

    internal bool IsFirstTick
    {
      get
      {
        return this.isFirstTick;
      }
    }

    internal bool IsLastTick
    {
      get
      {
        return this.isLastTick;
      }
    }

    internal MajorTickmarkInfo(PresentationControl presentationControl, PresentationBase presentation, double alpha, double value, bool isFirstTick, bool isLastTick)
      : base(presentationControl, presentation, alpha)
    {
      this.value = value;
      this.isFirstTick = isFirstTick;
      this.isLastTick = isLastTick;
    }
  }
}
