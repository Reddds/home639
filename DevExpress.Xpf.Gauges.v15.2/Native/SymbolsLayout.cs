// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.SymbolsLayout
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Gauges;
using System.Windows;

namespace DevExpress.Xpf.Gauges.Native
{
  public class SymbolsLayout
  {
    private readonly SymbolsLayoutControl layoutControl;
    private readonly Size symbolSize;

    public Size SymbolSize
    {
      get
      {
        return this.symbolSize;
      }
    }

    public SymbolsLayoutControl LayoutControl
    {
      get
      {
        return this.layoutControl;
      }
    }

    public SymbolsLayout(SymbolsLayoutControl layoutControl, Size symbolSize)
    {
      this.layoutControl = layoutControl;
      this.symbolSize = symbolSize;
    }

    public Point GetSymbolLocation(UIElement baseLayoutElement, SymbolInfo symbolInfo)
    {
      Rect relativeElementRect = LayoutHelper.GetRelativeElementRect((UIElement) this.layoutControl, baseLayoutElement);
      return new Point(relativeElementRect.Left + this.SymbolSize.Width * (double) symbolInfo.SymbolIndex, relativeElementRect.Top);
    }

    public Rect GetClipBounds(UIElement baseLayoutElement)
    {
      return LayoutHelper.GetRelativeElementRect((UIElement) this.layoutControl, baseLayoutElement);
    }

    public void Invalidate()
    {
      this.layoutControl.InvalidateMeasure();
      UIElement uiElement = LayoutHelper.GetParent((DependencyObject) this.layoutControl, false) as UIElement;
      if (uiElement == null)
        return;
      uiElement.InvalidateMeasure();
    }
  }
}
