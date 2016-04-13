// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.SymbolsLayoutControl
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DevExpress.Xpf.Gauges
{
  public class SymbolsLayoutControl : Control
  {
    private const double defaultWidth = 300.0;
    private const double defaultHeight = 300.0;

    private DigitalGaugeControl Gauge
    {
      get
      {
        return this.DataContext as DigitalGaugeControl;
      }
    }

    private SymbolViewBase View
    {
      get
      {
        if (this.Gauge == null)
          return (SymbolViewBase) null;
        return this.Gauge.ActualSymbolView;
      }
    }

    private int SymbolCount
    {
      get
      {
        if (this.Gauge == null)
          return 1;
        return this.Gauge.ActualSymbolCount;
      }
    }

    private double ActualHeightToWidthRatio
    {
      get
      {
        if (this.View.Height.Type == SymbolLengthType.Proportional)
        {
          if (this.View.Width.Type == SymbolLengthType.Proportional)
            return this.View.Height.ProportionalLength / this.View.Width.ProportionalLength;
          return this.View.Height.ProportionalLength;
        }
        if (this.View.Width.Type == SymbolLengthType.Proportional)
          return 1.0 / this.View.Width.ProportionalLength;
        return this.Gauge.SymbolViewInternal.DefaultHeightToWidthRatio;
      }
    }

    private bool IsAutoSize
    {
      get
      {
        if (this.View.Height.Type == SymbolLengthType.Auto && this.View.Width.Type == SymbolLengthType.Auto)
          return true;
        if (this.View.Height.Type == SymbolLengthType.Proportional)
          return this.View.Width.Type == SymbolLengthType.Proportional;
        return false;
      }
    }

    private bool IsStretchByHorizontal(Size availableSymbolSize)
    {
      if (this.View.Width.Type == SymbolLengthType.Stretch || this.IsAutoSize && this.IsBasedOnWidth(availableSymbolSize))
        return true;
      if (this.View.Width.Type == SymbolLengthType.Auto)
        return this.IsBasedOnWidth(availableSymbolSize);
      return false;
    }

    private bool IsStretchByVertical(Size availableSymbolSize)
    {
      if (this.View.Height.Type == SymbolLengthType.Stretch || this.IsAutoSize && !this.IsBasedOnWidth(availableSymbolSize))
        return true;
      if (this.View.Height.Type == SymbolLengthType.Auto)
        return !this.IsBasedOnWidth(availableSymbolSize);
      return false;
    }

    private bool IsBasedOnWidth(Size availableSymbolSize)
    {
      if (this.IsAutoSize)
        return availableSymbolSize.Width / (double) this.SymbolCount * this.ActualHeightToWidthRatio < availableSymbolSize.Height;
      if (this.View.Width.Type == SymbolLengthType.Auto)
        return this.View.Height.Type == SymbolLengthType.Proportional;
      return this.View.Width.Type != SymbolLengthType.Proportional;
    }

    private Size CalcSizeByHorizontal(Size availableSymbolSize)
    {
      double width = this.IsStretchByHorizontal(availableSymbolSize) ? availableSymbolSize.Width / (double) this.SymbolCount : this.View.Width.FixedLength;
      double height;
      switch (this.View.Height.Type)
      {
        case SymbolLengthType.Stretch:
          height = availableSymbolSize.Height;
          break;
        case SymbolLengthType.Fixed:
          height = this.View.Height.FixedLength;
          break;
        default:
          height = width * this.ActualHeightToWidthRatio;
          break;
      }
      return new Size(width, height);
    }

    private Size CalcSizeByVertical(Size availableSymbolSize)
    {
      double height = this.IsStretchByVertical(availableSymbolSize) ? availableSymbolSize.Height : this.View.Height.FixedLength;
      double width;
      switch (this.View.Width.Type)
      {
        case SymbolLengthType.Stretch:
          width = availableSymbolSize.Width / (double) this.SymbolCount;
          break;
        case SymbolLengthType.Fixed:
          width = this.View.Width.FixedLength;
          break;
        default:
          width = height / this.ActualHeightToWidthRatio;
          break;
      }
      return new Size(width, height);
    }

    private Size CalcSymbolSize(Size availableSymbolSize)
    {
      if (!this.IsBasedOnWidth(availableSymbolSize))
        return this.CalcSizeByVertical(availableSymbolSize);
      return this.CalcSizeByHorizontal(availableSymbolSize);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
      if (this.Gauge == null)
        return base.MeasureOverride(availableSize);
      double num1 = double.IsInfinity(availableSize.Width) ? 300.0 : availableSize.Width;
      double num2 = double.IsInfinity(availableSize.Height) ? 300.0 : availableSize.Height;
      Size symbolSize = this.CalcSymbolSize(new Size(num1, num2));
      this.Gauge.SymbolsLayout = new SymbolsLayout(this, symbolSize);
      return new Size(Math.Min(symbolSize.Width * (double) this.SymbolCount, num1), Math.Min(symbolSize.Height, num2));
    }
  }
}
