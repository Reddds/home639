// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.MatrixViewInternal
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges;
using System;
using System.Collections.Generic;

namespace DevExpress.Xpf.Gauges.Native
{
  public abstract class MatrixViewInternal : SymbolViewInternal
  {
    private readonly SymbolsStateGenerator symbolsStateGenerator;

    protected internal abstract int SymbolWidth { get; }

    protected internal abstract int SymbolHeight { get; }

    protected internal abstract PixelsArray SymbolsPixelsArray { get; }

    public MatrixViewInternal()
    {
      this.symbolsStateGenerator = new SymbolsStateGenerator(this);
    }

    protected override SymbolsAnimation GetDefaultAnimation()
    {
      return (SymbolsAnimation) new BlinkingAnimation();
    }

    protected override List<SymbolState> GetSymbolsStateByDisplayText(List<string> textBySymbols)
    {
      List<SymbolState> list = new List<SymbolState>();
      foreach (string symbol in textBySymbols)
      {
        SymbolSegmentsMapping customSegmentsMapping = this.GetCustomSegmentsMapping(symbol[0]);
        if (customSegmentsMapping != null)
          list.Add(new SymbolState(symbol, Math.Max(customSegmentsMapping.SegmentsStates.States.Length, this.SymbolHeight * this.SymbolWidth), customSegmentsMapping.SegmentsStates.States));
        else
          list.Add(this.symbolsStateGenerator.GetCachedSymbolState(symbol[0]));
      }
      return list;
    }

    protected internal override List<string> SeparateTextToSymbols(string text)
    {
      List<string> list = new List<string>();
      if (text != null)
      {
        for (int index = 0; index < text.Length; ++index)
          list.Add(text[index].ToString());
      }
      return list;
    }
  }
}
