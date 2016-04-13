// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ArrowStateIndicatorModel
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

namespace DevExpress.Xpf.Gauges
{
  public class ArrowStateIndicatorModel : StateIndicatorModel
  {
    public override string ModelName
    {
      get
      {
        return "Arrow";
      }
    }

    public ArrowStateIndicatorModel()
    {
      this.DefaultStyleKey = (object) typeof (ArrowStateIndicatorModel);
      this.defaultState = new State()
      {
        Presentation = (StatePresentation) new ArrowDefaultStatePresentation()
      };
      this.predefinedStates.Add(new State()
      {
        Presentation = (StatePresentation) new ArrowUpStatePresentation()
      });
      this.predefinedStates.Add(new State()
      {
        Presentation = (StatePresentation) new ArrowDownStatePresentation()
      });
      this.predefinedStates.Add(new State()
      {
        Presentation = (StatePresentation) new ArrowLeftStatePresentation()
      });
      this.predefinedStates.Add(new State()
      {
        Presentation = (StatePresentation) new ArrowRightStatePresentation()
      });
      this.predefinedStates.Add(new State()
      {
        Presentation = (StatePresentation) new ArrowLeftUpStatePresentation()
      });
      this.predefinedStates.Add(new State()
      {
        Presentation = (StatePresentation) new ArrowRightUpStatePresentation()
      });
      this.predefinedStates.Add(new State()
      {
        Presentation = (StatePresentation) new ArrowRightDownStatePresentation()
      });
      this.predefinedStates.Add(new State()
      {
        Presentation = (StatePresentation) new ArrowLeftDownStatePresentation()
      });
    }
  }
}
