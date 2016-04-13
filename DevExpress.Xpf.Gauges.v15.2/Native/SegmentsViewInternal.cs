// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.SegmentsViewInternal
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges;
using System.Collections.Generic;
using System.Windows;

namespace DevExpress.Xpf.Gauges.Native
{
  public abstract class SegmentsViewInternal : SymbolViewInternal
  {
    private SymbolDictionary SymbolMapping
    {
      get
      {
        return ((SegmentsView) this.View).SymbolMapping;
      }
    }

    internal override double DefaultHeightToWidthRatio
    {
      get
      {
        return 2.0;
      }
    }

    private SymbolSegmentsMapping GetSegmentsMapping(char symbol)
    {
      SymbolSegmentsMapping customSegmentsMapping = this.GetCustomSegmentsMapping(symbol);
      if (customSegmentsMapping != null)
        return customSegmentsMapping;
      foreach (SymbolSegmentsMapping symbolSegmentsMapping in (FreezableCollection<SymbolSegmentsMapping>) this.SymbolMapping)
      {
        char ch = char.ToUpper(symbol);
        if ((int) symbolSegmentsMapping.Symbol == (int) ch)
          return symbolSegmentsMapping;
      }
      return (SymbolSegmentsMapping) null;
    }

    private SymbolState GetSymbolStateBySymbolText(string symbolText)
    {
      SymbolState symbolState = (SymbolState) null;
      foreach (char symbol in symbolText)
      {
        SymbolSegmentsMapping segmentsMapping = this.GetSegmentsMapping(symbol);
        symbolState = segmentsMapping == null ? this.GetEmptySymbolState() : (symbolState != null ? symbolState.Unite(new SymbolState(symbol.ToString(), segmentsMapping.SegmentsStates.States)) : new SymbolState(symbol.ToString(), segmentsMapping.SegmentsStates.States));
      }
      return symbolState;
    }

    protected override SymbolsAnimation GetDefaultAnimation()
    {
      return (SymbolsAnimation) new CreepingLineAnimation();
    }

    protected override List<SymbolState> GetSymbolsStateByDisplayText(List<string> textBySymbols)
    {
      List<SymbolState> list = new List<SymbolState>();
      foreach (string symbolText in textBySymbols)
        list.Add(this.GetSymbolStateBySymbolText(symbolText));
      return list;
    }

    protected internal override List<string> SeparateTextToSymbols(string text)
    {
      List<string> list = new List<string>();
      if (text != null)
      {
        string str = "";
        for (int index = 0; index < text.Length; ++index)
        {
          SymbolSegmentsMapping segmentsMapping = this.GetSegmentsMapping(text[index]);
          if (segmentsMapping != null)
          {
            if (segmentsMapping.SymbolType == SymbolType.Main)
            {
              if (str != "")
                list.Add(str);
              str = text[index].ToString();
            }
            else
              str = !(str != "") ? text[index].ToString() : str + (object) text[index];
          }
          else
          {
            if (str != "")
              list.Add(str);
            str = text[index].ToString();
          }
        }
        if (str != "")
          list.Add(str);
      }
      return list;
    }
  }
}
