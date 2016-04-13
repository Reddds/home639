﻿// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LinearGaugeControlAutomationPeer
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Localization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Automation.Peers;

namespace DevExpress.Xpf.Gauges
{
  public class LinearGaugeControlAutomationPeer : FrameworkElementAutomationPeer
  {
    public LinearGaugeControlAutomationPeer(FrameworkElement owner)
      : base(owner)
    {
    }

    protected override string GetClassNameCore()
    {
      return "LinearGaugeControl";
    }

    protected override string GetLocalizedControlTypeCore()
    {
      return GaugeLocalizer.GetString(GaugeStringId.LinearGaugeLocalizedControlType);
    }

    protected override bool IsContentElementCore()
    {
      return false;
    }

    protected override string GetHelpTextCore()
    {
      string helpTextCore = base.GetHelpTextCore();
      if (string.IsNullOrEmpty(helpTextCore))
        return GaugeLocalizer.GetString(GaugeStringId.LinearGaugeAutomationPeerHelpText);
      return helpTextCore;
    }

    protected override List<AutomationPeer> GetChildrenCore()
    {
      List<AutomationPeer> list = new List<AutomationPeer>();
      LinearGaugeControl linearGaugeControl = this.Owner as LinearGaugeControl;
      if (linearGaugeControl != null)
      {
        foreach (UIElement element in (Collection<LinearScale>) linearGaugeControl.Scales)
        {
          AutomationPeer peerForElement = UIElementAutomationPeer.CreatePeerForElement(element);
          if (peerForElement != null)
            list.Add(peerForElement);
        }
      }
      return list;
    }
  }
}
