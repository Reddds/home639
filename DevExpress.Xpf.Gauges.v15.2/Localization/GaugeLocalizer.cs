// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Localization.GaugeLocalizer
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Utils.Localization;
using DevExpress.Utils.Localization.Internal;

namespace DevExpress.Xpf.Gauges.Localization
{
  /// <summary>
  /// 
  /// <para>
  /// A base class that provides necessary functionality for custom localizers of the Gauge Controls.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class GaugeLocalizer : XtraLocalizer<GaugeStringId>
  {
    static GaugeLocalizer()
    {
      XtraLocalizer<GaugeStringId>.SetActiveLocalizerProvider((ActiveLocalizerProvider<GaugeStringId>) new DefaultActiveLocalizerProvider<GaugeStringId>(GaugeLocalizer.CreateDefaultLocalizer()));
    }

    protected override void PopulateStringTable()
    {
      this.AddString(GaugeStringId.CircularGaugeLocalizedControlType, "circular gauge");
      this.AddString(GaugeStringId.LinearGaugeLocalizedControlType, "linear gauge");
      this.AddString(GaugeStringId.DigitalGaugeLocalizedControlType, "digital gauge");
      this.AddString(GaugeStringId.StateIndicatorLocalizedControlType, "state indicator");
      this.AddString(GaugeStringId.ScaleLocalizedControlType, "scale");
      this.AddString(GaugeStringId.NeedleLocalizedControlType, "needle");
      this.AddString(GaugeStringId.MarkerLocalizedControlType, "marker");
      this.AddString(GaugeStringId.RangeBarLocalizedControlType, "range bar");
      this.AddString(GaugeStringId.LevelBarLocalizedControlType, "level bar");
      this.AddString(GaugeStringId.ValueIndicatorLocalizedControlType, "value indicator");
      this.AddString(GaugeStringId.CircularGaugeAutomationPeerHelpText, "A gauge control that indicates values on circular scales");
      this.AddString(GaugeStringId.LinearGaugeAutomationPeerHelpText, "A gauge control that indicates values on linear scales");
      this.AddString(GaugeStringId.DigitalGaugeAutomationPeerHelpText, "A gauge control that displays values using digital symbols");
      this.AddString(GaugeStringId.StateIndicatorAutomationPeerHelpText, "A state indicator control that displays an image corresponds to its current state");
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns a localizer object, which provides resources based on the thread's language and regional settings (culture).
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// An <see cref="T:DevExpress.Utils.Localization.XtraLocalizer`1"/> object representing resources based on the thread's culture.
    /// 
    /// </returns>
    public static XtraLocalizer<GaugeStringId> CreateDefaultLocalizer()
    {
      return (XtraLocalizer<GaugeStringId>) new GaugeResLocalizer();
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns a localized string for the given string identifier.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// <param name="id">A <see cref="T:DevExpress.Xpf.Gauges.Localization.GaugeStringId"/> enumeration value identifying the string to localize.
    /// 
    ///             </param>
    /// <returns>
    /// A <see cref="T:System.String"/> corresponding to the specified identifier.
    /// 
    /// </returns>
    public static string GetString(GaugeStringId id)
    {
      return XtraLocalizer<GaugeStringId>.Active.GetLocalizedString(id);
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns a localizer object, which provides resources based on the thread's language and regional settings (culture).
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:DevExpress.Utils.Localization.XtraLocalizer`1"/> object, which provides resources based on the thread's culture.
    /// 
    /// </returns>
    public override XtraLocalizer<GaugeStringId> CreateResXLocalizer()
    {
      return (XtraLocalizer<GaugeStringId>) new GaugeResLocalizer();
    }
  }
}
