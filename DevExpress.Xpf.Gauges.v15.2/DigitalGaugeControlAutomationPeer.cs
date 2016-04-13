// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.DigitalGaugeControlAutomationPeer
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Localization;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace DevExpress.Xpf.Gauges
{
  public class DigitalGaugeControlAutomationPeer : FrameworkElementAutomationPeer, IValueProvider
  {
    private DigitalGaugeControl Gauge
    {
      get
      {
        return this.Owner as DigitalGaugeControl;
      }
    }

    bool IValueProvider.IsReadOnly
    {
      get
      {
        return false;
      }
    }

    string IValueProvider.Value
    {
      get
      {
        if (this.Gauge != null)
          return this.Gauge.Text;
        return string.Empty;
      }
    }

    public DigitalGaugeControlAutomationPeer(FrameworkElement owner)
      : base(owner)
    {
    }

    protected override string GetClassNameCore()
    {
      return "DigitalGaugeControl";
    }

    protected override string GetLocalizedControlTypeCore()
    {
      return GaugeLocalizer.GetString(GaugeStringId.DigitalGaugeLocalizedControlType);
    }

    protected override bool IsContentElementCore()
    {
      return false;
    }

    protected override string GetHelpTextCore()
    {
      string helpTextCore = base.GetHelpTextCore();
      if (string.IsNullOrEmpty(helpTextCore))
        return GaugeLocalizer.GetString(GaugeStringId.DigitalGaugeAutomationPeerHelpText);
      return helpTextCore;
    }

    void IValueProvider.SetValue(string value)
    {
      if (this.Gauge == null)
        return;
      this.Gauge.Text = value;
    }

    public override object GetPattern(PatternInterface patternInterface)
    {
      if (patternInterface == PatternInterface.Value)
        return (object) this;
      return base.GetPattern(patternInterface);
    }
  }
}
