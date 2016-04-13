// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.SymbolSegmentsMapping
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
  /// Contains properties to define how a custom symbol should be displayed on a digital gauge control.
  /// 
  /// 
  /// </para>
  /// 
  /// </summary>
  public class SymbolSegmentsMapping : GaugeDependencyObject
  {
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
    public static readonly DependencyProperty SymbolProperty = DependencyPropertyManager.Register("Symbol", typeof (char), typeof (SymbolSegmentsMapping), new PropertyMetadata(new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty SegmentsStatesProperty = DependencyPropertyManager.Register("SegmentsStates", typeof (StatesMask), typeof (SymbolSegmentsMapping), new PropertyMetadata(new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));
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
    public static readonly DependencyProperty SymbolTypeProperty = DependencyPropertyManager.Register("SymbolType", typeof (SymbolType), typeof (SymbolSegmentsMapping), new PropertyMetadata((object) SymbolType.Main, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Specifies a custom symbol that can be displayed on the symbols panel using symbol segments mapping.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Char"/> value that specifies a custom symbol.
    /// 
    /// </value>
    [Category("Data")]
    [TypeConverter(typeof (StringToCharConverter))]
    public char Symbol
    {
      get
      {
        return (char) this.GetValue(SymbolSegmentsMapping.SymbolProperty);
      }
      set
      {
        this.SetValue(SymbolSegmentsMapping.SymbolProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Specifies appropriate segment states for a desired character.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.StatesMask"/> value that specifies segments states.
    /// 
    /// </value>
    [Category("Data")]
    public StatesMask SegmentsStates
    {
      get
      {
        return (StatesMask) this.GetValue(SymbolSegmentsMapping.SegmentsStatesProperty);
      }
      set
      {
        this.SetValue(SymbolSegmentsMapping.SegmentsStatesProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a symbol type that is used for displaying a custom symbol on a digital gauge control.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.SymbolType"/> enumeration value that specifies a symbol type for custom symbol mapping.
    /// 
    /// </value>
    [Category("Data")]
    public SymbolType SymbolType
    {
      get
      {
        return (SymbolType) this.GetValue(SymbolSegmentsMapping.SymbolTypeProperty);
      }
      set
      {
        this.SetValue(SymbolSegmentsMapping.SymbolTypeProperty, (object) value);
      }
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new SymbolSegmentsMapping();
    }
  }
}
