// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.SevenSegmentsView
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A seven segments symbols panel type of digital gauge control.
  /// 
  /// 
  /// </para>
  /// 
  /// </summary>
  public class SevenSegmentsView : SegmentsView
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
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (SevenSegmentsPresentation), typeof (SevenSegmentsView), new PropertyMetadata((object) null, new PropertyChangedCallback(SymbolViewBase.PresentationPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the current presentation that specifies the appearance of the seven segment view type.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.SevenSegmentsPresentation"/> object
    /// 
    /// </value>
    [Category("Presentation")]
    public SevenSegmentsPresentation Presentation
    {
      get
      {
        return (SevenSegmentsPresentation) this.GetValue(SevenSegmentsView.PresentationProperty);
      }
      set
      {
        this.SetValue(SevenSegmentsView.PresentationProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the SevenSegmentsView class with default settings.
    /// 
    /// </para>
    /// 
    /// </summary>
    public SevenSegmentsView()
    {
      this.InitializeMapping();
    }

    private void InitializeMapping()
    {
      StatesMaskConverter statesMaskConverter = new StatesMaskConverter();
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '0',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 1 1 1 1 1 0"),
        SymbolType = SymbolType.Main
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '1',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 1 1 0 0 0 0"),
        SymbolType = SymbolType.Main
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '2',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 1 0 1 1 0 1"),
        SymbolType = SymbolType.Main
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '3',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 1 1 1 0 0 1"),
        SymbolType = SymbolType.Main
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '4',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 1 1 0 0 1 1"),
        SymbolType = SymbolType.Main
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '5',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 0 1 1 0 1 1"),
        SymbolType = SymbolType.Main
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '6',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 0 1 1 1 1 1"),
        SymbolType = SymbolType.Main
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '7',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 1 1 0 0 0 0"),
        SymbolType = SymbolType.Main
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '8',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 1 1 1 1 1 1"),
        SymbolType = SymbolType.Main
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '9',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 1 1 1 0 1 1"),
        SymbolType = SymbolType.Main
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '-',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 0 0 0 0 0 1"),
        SymbolType = SymbolType.Main
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '.',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 0 0 0 0 0 0 1"),
        SymbolType = SymbolType.Additional
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = ',',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 0 0 0 0 0 0 1"),
        SymbolType = SymbolType.Additional
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = ':',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 0 0 0 0 0 0 0 1"),
        SymbolType = SymbolType.Main
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '\'',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 0 0 0 0 0 0 0 0 1"),
        SymbolType = SymbolType.Additional
      });
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new SevenSegmentsView();
    }

    protected internal override SymbolViewInternal CreateInternalView()
    {
      return (SymbolViewInternal) new SevenSegmentsViewInternal();
    }
  }
}
