// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.RangeInfo
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;

namespace DevExpress.Xpf.Gauges
{
  public class RangeInfo : LayerInfo
  {
    private readonly RangeBase range;

    protected internal override object HitTestableObject
    {
      get
      {
        return (object) this.range;
      }
    }

    protected internal override object HitTestableParent
    {
      get
      {
        return (object) this.range.Scale;
      }
    }

    protected internal override bool IsHitTestVisible
    {
      get
      {
        return this.range.IsHitTestVisible;
      }
    }

    internal RangeInfo(RangeBase range, int zIndex, PresentationControl presentationControl, PresentationBase presentation)
      : base((ILayoutCalculator) range, zIndex, presentationControl, presentation)
    {
      this.range = range;
    }
  }
}
