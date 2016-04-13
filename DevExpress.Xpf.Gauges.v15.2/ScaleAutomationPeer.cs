// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ScaleAutomationPeer
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Localization;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation.Peers;

namespace DevExpress.Xpf.Gauges
{
  public class ScaleAutomationPeer : FrameworkElementAutomationPeer
  {
    public ScaleAutomationPeer(FrameworkElement owner)
      : base(owner)
    {
    }

    protected override string GetClassNameCore()
    {
      return "Scale";
    }

    protected override string GetLocalizedControlTypeCore()
    {
      return GaugeLocalizer.GetString(GaugeStringId.ScaleLocalizedControlType);
    }

    protected override bool IsContentElementCore()
    {
      return false;
    }

    protected override List<AutomationPeer> GetChildrenCore()
    {
      List<AutomationPeer> list = new List<AutomationPeer>();
      Scale scale = this.Owner as Scale;
      if (scale != null)
      {
        foreach (ValueIndicatorBase valueIndicatorBase in scale.Indicators)
        {
          AutomationPeer peerForElement = UIElementAutomationPeer.CreatePeerForElement((UIElement) valueIndicatorBase.ElementInfo.PresentationControl);
          if (peerForElement != null)
            list.Add(peerForElement);
        }
      }
      return list;
    }
  }
}
