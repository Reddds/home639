// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.SymbolsStateGenerator
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges;
using System.Collections.Generic;

namespace DevExpress.Xpf.Gauges.Native
{
  public class SymbolsStateGenerator
  {
    private readonly Dictionary<char, SymbolState> cache = new Dictionary<char, SymbolState>();
    private readonly MatrixViewInternal viewInternal;

    public SymbolsStateGenerator(MatrixViewInternal viewInternal)
    {
      this.viewInternal = viewInternal;
    }

    private SymbolState GetSymbolState(char symbol)
    {
      bool[] flagArray = new bool[this.viewInternal.SymbolWidth * this.viewInternal.SymbolHeight];
      int num1 = 0;
      int num2 = (int) symbol & (int) byte.MaxValue;
      int num3 = (int) symbol >> 8 & (int) byte.MaxValue;
      for (int index1 = 0; index1 < this.viewInternal.SymbolHeight; ++index1)
      {
        for (int index2 = 0; index2 < this.viewInternal.SymbolWidth; ++index2)
        {
          int num4 = (int) this.viewInternal.SymbolsPixelsArray[num2 * this.viewInternal.SymbolWidth + index2, num3 * this.viewInternal.SymbolHeight + index1];
          flagArray[num1++] = num4 > 0;
        }
      }
      return new SymbolState(symbol.ToString(), flagArray.Length, flagArray);
    }

    public SymbolState GetCachedSymbolState(char symbol)
    {
      SymbolState symbolState;
      this.cache.TryGetValue(symbol, out symbolState);
      if (symbolState == null)
      {
        symbolState = this.GetSymbolState(symbol);
        this.cache.Add(symbol, symbolState);
      }
      return symbolState;
    }
  }
}
