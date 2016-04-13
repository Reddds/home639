// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.MatrixView5x8Internal
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges;

namespace DevExpress.Xpf.Gauges.Native
{
  public class MatrixView5x8Internal : MatrixViewInternal
  {
    private static readonly PixelsArray symbolsPixelsArray = PixelsArray.LoadFromCpecificImage(5, 8);
    private const int symbolWidth = 5;
    private const int symbolHeight = 8;

    private Matrix5x8Model Model
    {
      get
      {
        if (this.Gauge == null)
          return (Matrix5x8Model) null;
        return this.Gauge.ActualModel.Matrix5x8Model;
      }
    }

    private Matrix5x8Presentation Presentation
    {
      get
      {
        return ((MatrixView5x8) this.View).Presentation;
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
        return (SymbolPresentation) new DefaultMatrix5x8Presentation();
      }
    }

    internal override double DefaultHeightToWidthRatio
    {
      get
      {
        return 1.6;
      }
    }

    protected internal override int SymbolWidth
    {
      get
      {
        return 5;
      }
    }

    protected internal override int SymbolHeight
    {
      get
      {
        return 8;
      }
    }

    protected internal override PixelsArray SymbolsPixelsArray
    {
      get
      {
        return MatrixView5x8Internal.symbolsPixelsArray;
      }
    }

    protected internal override SymbolState GetEmptySymbolState()
    {
      return new SymbolState(string.Empty, 40, new bool[1]);
    }
  }
}
