// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.SegmentsView
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A base class for all segment view types of a digital gauge control.
  /// 
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class SegmentsView : SymbolViewBase
  {
    internal static readonly DependencyPropertyKey SymbolMappingPropertyKey = DependencyPropertyManager.RegisterReadOnly("SymbolMapping", typeof (SymbolDictionary), typeof (SegmentsView), new PropertyMetadata());
    /// <summary>
    /// 
    /// <para>
    /// Identifies the  dependency property.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <returns/>
    public static readonly DependencyProperty SymbolMappingProperty = SegmentsView.SymbolMappingPropertyKey.DependencyProperty;

    /// <summary>
    /// 
    /// <para>
    /// For internal use. Provides the information about elements that are used in symbol mapping.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.SymbolDictionary"/> object that stores the element that defines a symbol view of a digital gauge control.
    /// 
    /// 
    /// </value>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public SymbolDictionary SymbolMapping
    {
      get
      {
        return (SymbolDictionary) this.GetValue(SegmentsView.SymbolMappingProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the SegmentsView class with default settings.
    /// 
    /// </para>
    /// 
    /// </summary>
    public SegmentsView()
    {
      this.SetValue(SegmentsView.SymbolMappingPropertyKey, (object) new SymbolDictionary((SymbolViewBase) this));
    }
  }
}
