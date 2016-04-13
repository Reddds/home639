// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.SymbolState
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;
using System;

namespace DevExpress.Xpf.Gauges
{
  [NonCategorized]
  public class SymbolState
  {
    private const int defaultSegmentCount = 17;
    private readonly bool[] segments;
    private readonly string symbol;

    internal string Symbol
    {
      get
      {
        return this.symbol;
      }
    }

    public bool[] Segments
    {
      get
      {
        return this.segments;
      }
    }

    internal SymbolState(params bool[] values)
      : this(string.Empty, values)
    {
    }

    internal SymbolState(string symbol, params bool[] values)
      : this(symbol, 17, values)
    {
    }

    internal SymbolState(string symbol, int segmentCount, params bool[] values)
    {
      this.symbol = symbol;
      this.segments = new bool[segmentCount];
      if (values == null)
        return;
      int num = Math.Min(segmentCount, values.Length);
      for (int index = 0; index < num; ++index)
        this.segments[index] = values[index];
    }

    internal SymbolState Unite(SymbolState unitedSymbolState)
    {
      bool[] flagArray = new bool[Math.Min(this.segments.Length, unitedSymbolState.segments.Length)];
      for (int index = 0; index < flagArray.Length; ++index)
        flagArray[index] = this.Segments[index] || unitedSymbolState.Segments[index];
      return new SymbolState(this.symbol + unitedSymbolState.symbol, flagArray.Length, flagArray);
    }
  }
}
