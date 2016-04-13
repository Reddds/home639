// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LinearScaleIndicator
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// Serves as the base class for a linear scale's value indicators.
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class LinearScaleIndicator : ValueIndicatorBase
  {
    protected LinearGaugeControl Gauge
    {
      get
      {
        return base.Gauge as LinearGaugeControl;
      }
    }

    internal LinearScale Scale
    {
      get
      {
        return base.Scale as LinearScale;
      }
    }

    protected double GetRotationAngleByScaleLayoutMode()
    {
      double num = 0.0;
      switch (this.Scale.LayoutMode)
      {
        case LinearScaleLayoutMode.LeftToRight:
          num = 90.0;
          break;
        case LinearScaleLayoutMode.RightToLeft:
          num = -90.0;
          break;
        case LinearScaleLayoutMode.TopToBottom:
          num = 180.0;
          break;
      }
      return num;
    }

    protected double GetPointYByScaleLayoutMode(double value)
    {
      double num1;
      if (this.Scale.LayoutMode == LinearScaleLayoutMode.TopToBottom || this.Scale.LayoutMode == LinearScaleLayoutMode.BottomToTop)
      {
        double num2 = Math.Abs(this.Scale.Mapping.Layout.ScaleVector.Y);
        num1 = this.Scale.Mapping.Layout.AnchorPoint.Y + (double) Math.Sign(this.Scale.Mapping.Layout.ScaleVector.Y) * (num2 - this.Scale.Mapping.GetPointByValue(value).Y);
        if (num1 > num2)
          num1 = num2;
      }
      else
      {
        double num2 = Math.Abs(this.Scale.Mapping.Layout.ScaleVector.X);
        num1 = this.Scale.Mapping.Layout.AnchorPoint.X + (double) Math.Sign(this.Scale.Mapping.Layout.ScaleVector.X) * (num2 - this.Scale.Mapping.GetPointByValue(value).X);
        if (num1 > num2)
          num1 = num2;
      }
      if (num1 < 0.0)
        num1 = 0.0;
      return num1;
    }
  }
}
