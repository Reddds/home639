// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.CircularRedClockModel
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  public class CircularRedClockModel : CircularGaugeModel
  {
    public override string ModelName
    {
      get
      {
        return "Red Clock";
      }
    }

    public CircularRedClockModel()
    {
      this.InnerPadding = new Thickness(0.0, 7.0, 0.0, 4.0);
      this.ModelFull = (PartialCircularGaugeModel) new CircularRedClockFullModel();
      this.ModelHalfTop = (PartialCircularGaugeModel) new CircularRedClockHalfTopModel();
      this.ModelQuarterTopLeft = (PartialCircularGaugeModel) new CircularRedClockQuarterTopLeftModel();
      this.ModelQuarterTopRight = (PartialCircularGaugeModel) new CircularRedClockQuarterTopRightModel();
      this.ModelThreeQuarters = (PartialCircularGaugeModel) new CircularRedClockThreeQuartersModel();
    }
  }
}
