// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.SevenSegmentsViewInternal
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges;

namespace DevExpress.Xpf.Gauges.Native
{
  public class SevenSegmentsViewInternal : SegmentsViewInternal
  {
    private SevenSegmentsModel Model
    {
      get
      {
        if (this.Gauge == null)
          return (SevenSegmentsModel) null;
        return this.Gauge.ActualModel.SevenSegmentsModel;
      }
    }

    private SevenSegmentsPresentation Presentation
    {
      get
      {
        return ((SevenSegmentsView) this.View).Presentation;
      }
    }

    internal override double DefaultHeightToWidthRatio
    {
      get
      {
        return 61.0 / 37.0;
      }
    }

    protected override SymbolsModelBase ModelBase
    {
      get
      {
        return (SymbolsModelBase) this.Model;
      }
    }

    protected override SymbolPresentation ActualPresentation
    {
      get
      {
        if (this.Presentation != null)
          return (SymbolPresentation) this.Presentation;
        if (this.Model != null && this.Model.Presentation != null)
          return (SymbolPresentation) this.Model.Presentation;
        return (SymbolPresentation) new DefaultSevenSegmentsPresentation();
      }
    }
  }
}
