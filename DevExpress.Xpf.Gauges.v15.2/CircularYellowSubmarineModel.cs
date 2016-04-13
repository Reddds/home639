// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.CircularYellowSubmarineModel
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

namespace DevExpress.Xpf.Gauges
{
  public class CircularYellowSubmarineModel : CircularGaugeModel
  {
    public override string ModelName
    {
      get
      {
        return "Yellow Submarine";
      }
    }

    public CircularYellowSubmarineModel()
    {
      this.ModelFull = (PartialCircularGaugeModel) new CircularYellowSubmarineFullModel();
      this.ModelHalfTop = (PartialCircularGaugeModel) new CircularYellowSubmarineHalfTopModel();
      this.ModelQuarterTopLeft = (PartialCircularGaugeModel) new CircularYellowSubmarineQuarterTopLeftModel();
      this.ModelQuarterTopRight = (PartialCircularGaugeModel) new CircularYellowSubmarineQuarterTopRightModel();
      this.ModelThreeQuarters = (PartialCircularGaugeModel) new CircularYellowSubmarineThreeQuartersModel();
    }
  }
}
