// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.SymbolInfo
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Gauges.Native;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  [NonCategorized]
  public class SymbolInfo : ElementInfoBase, IWeakEventListener
  {
    private string displayText = string.Empty;
    private readonly int symbolIndex;
    private Thickness margin;
    private Transform renderTransform;
    private SymbolState symbolState;

    internal int SymbolIndex
    {
      get
      {
        return this.symbolIndex;
      }
    }

    protected internal override bool InfluenceOnGaugeSize
    {
      get
      {
        return true;
      }
    }

    public string DisplayText
    {
      get
      {
        return this.displayText;
      }
      set
      {
        if (!(this.displayText != value))
          return;
        this.displayText = value;
        this.NotifyPropertyChanged("DisplayText");
      }
    }

    public Thickness Margin
    {
      get
      {
        return this.margin;
      }
      set
      {
        if (!(this.margin != value))
          return;
        this.margin = value;
        this.NotifyPropertyChanged("Margin");
      }
    }

    public Transform RenderTransform
    {
      get
      {
        return this.renderTransform;
      }
      set
      {
        if (this.renderTransform == value)
          return;
        this.renderTransform = value;
        this.NotifyPropertyChanged("RenderTransform");
      }
    }

    public SymbolState SymbolState
    {
      get
      {
        return this.symbolState;
      }
      set
      {
        if (this.symbolState == value)
          return;
        this.symbolState = value;
        this.NotifyPropertyChanged("SymbolState");
        this.UpdateSegments();
      }
    }

    public SymbolInfo(ILayoutCalculator layoutCalculator, PresentationControl presentationControl, PresentationBase presentation, int symbolIndex)
      : base(layoutCalculator, 0, presentationControl, presentation)
    {
      this.symbolIndex = symbolIndex;
    }

    bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
    {
      bool flag = false;
      if (managerType == typeof (PropertyChangedWeakEventManager) && sender is PresentationBase)
      {
        this.PresentationChanged();
        flag = true;
      }
      return flag;
    }

    protected virtual void UpdateSegments()
    {
    }

    protected override void PresentationChanging(PresentationBase oldValue, PresentationBase newValue)
    {
      base.PresentationChanging(oldValue, newValue);
      CommonUtils.SubscribePropertyChangedWeakEvent((INotifyPropertyChanged) oldValue, (INotifyPropertyChanged) newValue, (IWeakEventListener) this);
    }
  }
}
