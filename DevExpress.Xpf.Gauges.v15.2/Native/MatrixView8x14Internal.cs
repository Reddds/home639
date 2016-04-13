// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.MatrixView8x14Internal
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges;

namespace DevExpress.Xpf.Gauges.Native
{
  public class MatrixView8x14Internal : MatrixViewInternal
  {
    private static readonly PixelsArray symbolsPixelsArray = PixelsArray.LoadFromCpecificImage(8, 14);
    private const int symbolWidth = 8;
    private const int symbolHeight = 14;

    private Matrix8x14Model Model
    {
      get
      {
        if (this.Gauge == null)
          return (Matrix8x14Model) null;
        return this.Gauge.ActualModel.Matrix8x14Model;
      }
    }

    private Matrix8x14Presentation Presentation
    {
      get
      {
        return ((MatrixView8x14) this.View).Presentation;
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
        return (SymbolPresentation) new DefaultMatrix8x14Presentation();
      }
    }

    internal override double DefaultHeightToWidthRatio
    {
      get
      {
        return 1.75;
      }
    }

    protected internal override int SymbolWidth
    {
      get
      {
        return 8;
      }
    }

    protected internal override int SymbolHeight
    {
      get
      {
        return 14;
      }
    }

    protected internal override PixelsArray SymbolsPixelsArray
    {
      get
      {
        return MatrixView8x14Internal.symbolsPixelsArray;
      }
    }

    protected internal override SymbolState GetEmptySymbolState()
    {
      return new SymbolState(string.Empty, 112, new bool[1]);
    }
  }
}
