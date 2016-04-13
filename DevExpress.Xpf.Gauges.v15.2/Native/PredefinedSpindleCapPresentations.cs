// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.PredefinedSpindleCapPresentations
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges;
using System.Collections.Generic;

namespace DevExpress.Xpf.Gauges.Native
{
  public class PredefinedSpindleCapPresentations : PredefinedElementKinds
  {
    private static List<PredefinedElementKind> kinds = new List<PredefinedElementKind>();

    public static List<PredefinedElementKind> PresentationKinds
    {
      get
      {
        return PredefinedSpindleCapPresentations.kinds;
      }
    }

    static PredefinedSpindleCapPresentations()
    {
      PredefinedElementKinds.FillKindList(PredefinedSpindleCapPresentations.kinds, typeof (PredefinedSpindleCapPresentation));
    }
  }
}
