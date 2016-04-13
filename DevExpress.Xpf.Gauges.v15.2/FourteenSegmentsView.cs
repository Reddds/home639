// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.FourteenSegmentsView
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
  /// A fourteen segments symbols panel type of digital gauge control.
  /// 
  /// 
  /// </para>
  /// 
  /// </summary>
  public class FourteenSegmentsView : SegmentsView
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
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (FourteenSegmentsPresentation), typeof (FourteenSegmentsView), new PropertyMetadata((object) null, new PropertyChangedCallback(SymbolViewBase.PresentationPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the current presentation that specifies the appearance of the fourteen segment view type.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.FourteenSegmentsPresentation"/> object
    /// 
    /// </value>
    [Category("Presentation")]
    public FourteenSegmentsPresentation Presentation
    {
      get
      {
        return (FourteenSegmentsPresentation) this.GetValue(FourteenSegmentsView.PresentationProperty);
      }
      set
      {
        this.SetValue(FourteenSegmentsView.PresentationProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the FourteenSegmentsView class with default settings.
    /// 
    /// </para>
    /// 
    /// </summary>
    public FourteenSegmentsView()
    {
      this.InitializeMapping();
    }

    private void InitializeMapping()
    {
      StatesMaskConverter statesMaskConverter = new StatesMaskConverter();
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '0',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 1 1 1 1 1 0 0 0 0 1 0 1 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '1',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 1 1 0 0 0 0 0 0 0 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '2',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 1 0 1 1 0 0 1 0 1 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '3',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 1 1 1 0 0 0 1 0 1 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '4',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 1 1 0 0 1 0 1 0 1 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '5',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 0 1 1 0 1 0 1 0 1 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '6',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 0 1 1 1 1 0 1 0 1 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '7',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 1 1 0 0 0 0 0 0 0 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '8',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 1 1 1 1 1 0 1 0 1 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '9',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 1 1 1 0 1 0 1 0 1 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '-',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 0 0 0 0 0 0 1 0 1 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '+',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 0 0 0 0 0 1 1 1 1 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '/',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 0 0 0 0 0 0 0 0 0 1 0 1 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '*',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 0 0 0 0 0 1 1 1 1 1 1 1 1")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'A',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 1 1 0 1 1 0 1 0 1 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'B',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 1 1 1 0 0 1 1 1 0 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'C',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 0 0 1 1 1 0 0 0 0 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'D',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 1 1 1 0 0 1 0 1 0 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'E',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 0 0 1 1 1 0 1 0 1 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'F',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 0 0 0 1 1 0 1 0 1 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'G',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 0 1 1 1 1 0 1 0 0 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'H',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 1 1 0 1 1 0 1 0 1 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'I',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 0 0 1 0 0 1 0 1 0 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'J',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 1 1 1 1 0 0 0 0 0 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'K',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 0 0 0 1 1 0 0 0 1 1 1 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'L',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 0 0 1 1 1 0 0 0 0 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'M',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 1 1 0 1 1 0 0 0 0 1 0 0 1")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'N',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 1 1 0 1 1 0 0 0 0 0 1 0 1")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'O',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 1 1 1 1 1 0 0 0 0 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'P',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 1 0 0 1 1 0 1 0 1 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'Q',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 1 1 1 1 1 0 0 0 0 0 1 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'R',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 1 0 0 1 1 0 1 0 1 0 1 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'S',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 0 1 1 0 1 0 1 0 1 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'T',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 0 0 0 0 0 1 0 1 0 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'U',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 1 1 1 1 1 0 0 0 0 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'V',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 0 0 0 1 1 0 0 0 0 1 0 1 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'W',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 1 1 0 1 1 0 0 0 0 0 1 1 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'X',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 0 0 0 0 0 0 0 0 0 1 1 1 1")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'Y',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 1 0 0 0 1 0 1 1 1 0 0 0 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = 'Z',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("1 0 0 1 0 0 0 0 0 0 1 0 1 0")
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '.',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 0 0 0 0 0 0 0 0 0 0 0 0 0 1"),
        SymbolType = SymbolType.Additional
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = ',',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 0 0 0 0 0 0 0 0 0 0 0 0 0 1"),
        SymbolType = SymbolType.Additional
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = ':',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 1"),
        SymbolType = SymbolType.Main
      });
      this.SymbolMapping.Add(new SymbolSegmentsMapping()
      {
        Symbol = '\'',
        SegmentsStates = (StatesMask) statesMaskConverter.ConvertFromString("0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 1"),
        SymbolType = SymbolType.Additional
      });
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new FourteenSegmentsView();
    }

    protected internal override SymbolViewInternal CreateInternalView()
    {
      return (SymbolViewInternal) new FourteenSegmentsViewInternal();
    }
  }
}
