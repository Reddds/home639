// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.SymbolsModelBase
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  public abstract class SymbolsModelBase : ModelBase
  {
    public static readonly DependencyProperty OptionsProperty = DependencyPropertyManager.Register("Options", typeof (SymbolOptions), typeof (SymbolsModelBase), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public SymbolOptions Options
    {
      get
      {
        return (SymbolOptions) this.GetValue(SymbolsModelBase.OptionsProperty);
      }
      set
      {
        this.SetValue(SymbolsModelBase.OptionsProperty, (object) value);
      }
    }
  }
}
