// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ValueIndicatorAutomationPeer
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Localization;
using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace DevExpress.Xpf.Gauges
{
  public class ValueIndicatorAutomationPeer : FrameworkElementAutomationPeer, IRangeValueProvider
  {
    private ValueIndicatorBase ValueIndicator
    {
      get
      {
        ValueIndicatorPresentationControl presentationControl = this.Owner as ValueIndicatorPresentationControl;
        if (presentationControl != null)
          return presentationControl.ValueIndicator;
        return (ValueIndicatorBase) null;
      }
    }

    bool IRangeValueProvider.IsReadOnly
    {
      get
      {
        return false;
      }
    }

    double IRangeValueProvider.LargeChange
    {
      get
      {
        return double.NaN;
      }
    }

    double IRangeValueProvider.Maximum
    {
      get
      {
        if (this.ValueIndicator != null && this.ValueIndicator.Scale != null)
          return Math.Max(this.ValueIndicator.Scale.StartValue, this.ValueIndicator.Scale.EndValue);
        return double.NaN;
      }
    }

    double IRangeValueProvider.Minimum
    {
      get
      {
        if (this.ValueIndicator != null && this.ValueIndicator.Scale != null)
          return Math.Min(this.ValueIndicator.Scale.StartValue, this.ValueIndicator.Scale.EndValue);
        return double.NaN;
      }
    }

    double IRangeValueProvider.SmallChange
    {
      get
      {
        return double.NaN;
      }
    }

    double IRangeValueProvider.Value
    {
      get
      {
        if (this.ValueIndicator != null)
          return this.ValueIndicator.Value;
        return double.NaN;
      }
    }

    public ValueIndicatorAutomationPeer(FrameworkElement owner)
      : base(owner)
    {
    }

    protected override string GetClassNameCore()
    {
      if (this.ValueIndicator is ArcScaleNeedle)
        return "Needle";
      if (this.ValueIndicator is ArcScaleMarker || this.ValueIndicator is LinearScaleMarker)
        return "Marker";
      if (this.ValueIndicator is ArcScaleRangeBar || this.ValueIndicator is LinearScaleRangeBar)
        return "RangeBar";
      return this.ValueIndicator is LinearScaleLevelBar ? "LevelBar" : "ValueIndicatorBase";
    }

    protected override string GetLocalizedControlTypeCore()
    {
      if (this.ValueIndicator is ArcScaleNeedle)
        return GaugeLocalizer.GetString(GaugeStringId.NeedleLocalizedControlType);
      if (this.ValueIndicator is ArcScaleMarker || this.ValueIndicator is LinearScaleMarker)
        return GaugeLocalizer.GetString(GaugeStringId.MarkerLocalizedControlType);
      if (this.ValueIndicator is ArcScaleRangeBar || this.ValueIndicator is LinearScaleRangeBar)
        return GaugeLocalizer.GetString(GaugeStringId.RangeBarLocalizedControlType);
      if (this.ValueIndicator is LinearScaleLevelBar)
        return GaugeLocalizer.GetString(GaugeStringId.LevelBarLocalizedControlType);
      return GaugeLocalizer.GetString(GaugeStringId.ValueIndicatorLocalizedControlType);
    }

    protected override bool IsContentElementCore()
    {
      return false;
    }

    void IRangeValueProvider.SetValue(double value)
    {
      if (this.ValueIndicator == null)
        return;
      this.ValueIndicator.SetValue(ValueIndicatorBase.ValueProperty, (object) value);
    }

    public override object GetPattern(PatternInterface patternInterface)
    {
      if (patternInterface == PatternInterface.RangeValue)
        return (object) this;
      return base.GetPattern(patternInterface);
    }
  }
}
