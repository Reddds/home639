// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.FlatDarkCircularGaugeBackgroundLayerPresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class FlatDarkCircularGaugeBackgroundLayerPresentation : PredefinedCircularGaugeLayerPresentation
  {
    protected override Brush DefaultFill
    {
      get
      {
        return (Brush) new SolidColorBrush(Colors.Black);
      }
    }

    public override string PresentationName
    {
      get
      {
        return "Flat Dark Background";
      }
    }

    protected internal override PresentationControl CreateLayerPresentationControl()
    {
      return (PresentationControl) new FlatDarkGaugeBackgroundLayerControl();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new FlatDarkCircularGaugeBackgroundLayerPresentation();
    }
  }
}
