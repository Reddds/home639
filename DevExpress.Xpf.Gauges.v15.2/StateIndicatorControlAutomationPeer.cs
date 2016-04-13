// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.StateIndicatorControlAutomationPeer
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Localization;
using System.Windows;
using System.Windows.Automation.Peers;

namespace DevExpress.Xpf.Gauges
{
  public class StateIndicatorControlAutomationPeer : FrameworkElementAutomationPeer
  {
    private StateIndicatorControl StateIndicator
    {
      get
      {
        return this.Owner as StateIndicatorControl;
      }
    }

    public StateIndicatorControlAutomationPeer(FrameworkElement owner)
      : base(owner)
    {
    }

    protected override string GetClassNameCore()
    {
      return "StateIndicatorControl";
    }

    protected override string GetLocalizedControlTypeCore()
    {
      return GaugeLocalizer.GetString(GaugeStringId.StateIndicatorLocalizedControlType);
    }

    protected override bool IsContentElementCore()
    {
      return false;
    }

    protected override string GetHelpTextCore()
    {
      string helpTextCore = base.GetHelpTextCore();
      if (string.IsNullOrEmpty(helpTextCore))
        return GaugeLocalizer.GetString(GaugeStringId.StateIndicatorAutomationPeerHelpText);
      return helpTextCore;
    }
  }
}
