// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ElementInfoBase
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Gauges.Native;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace DevExpress.Xpf.Gauges
{
  [NonCategorized]
  public abstract class ElementInfoBase : IElementInfo, INotifyPropertyChanged
  {
    private readonly ILayoutCalculator layoutCalculator;
    private int zIndex;
    private ElementInfoContainer container;
    private ElementLayout layout;
    private PresentationControl presentationControl;
    private PresentationBase presentation;

    protected internal virtual object HitTestableObject
    {
      get
      {
        return (object) null;
      }
    }

    protected internal virtual object HitTestableParent
    {
      get
      {
        return (object) null;
      }
    }

    protected internal virtual bool IsHitTestVisible
    {
      get
      {
        return false;
      }
    }

    protected internal virtual bool InfluenceOnGaugeSize
    {
      get
      {
        return false;
      }
    }

    internal ElementInfoContainer Container
    {
      get
      {
        return this.container;
      }
      set
      {
        this.container = value;
        this.UpdateZIndex(this.ZIndex);
      }
    }

    public ElementLayout Layout
    {
      get
      {
        return this.layout;
      }
    }

    public PresentationBase Presentation
    {
      get
      {
        return this.presentation;
      }
      set
      {
        if (this.presentation == value)
          return;
        this.PresentationChanging(this.presentation, value);
        this.presentation = value;
        this.PresentationChanged();
      }
    }

    public PresentationControl PresentationControl
    {
      get
      {
        return this.presentationControl;
      }
      set
      {
        if (this.presentationControl == value)
          return;
        this.presentationControl = value;
        this.NotifyPropertyChanged("PresentationControl");
      }
    }

    public int ZIndex
    {
      get
      {
        return this.zIndex;
      }
      set
      {
        if (this.zIndex == value)
          return;
        this.zIndex = value;
        this.NotifyPropertyChanged("ZIndex");
        this.UpdateZIndex(this.zIndex);
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    internal ElementInfoBase(ILayoutCalculator layoutCalculator, int zIndex, PresentationControl presentationControl, PresentationBase presentation)
    {
      this.layoutCalculator = layoutCalculator;
      this.ZIndex = zIndex;
      this.PresentationControl = presentationControl;
      this.Presentation = presentation;
    }

    void IElementInfo.Invalidate()
    {
      this.Invalidate();
    }

    protected void NotifyPropertyChanged(string propertyName)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    private void UpdateZIndex(int value)
    {
      if (this.container == null)
        return;
      Panel.SetZIndex((UIElement) this.container, value);
    }

    protected virtual void PresentationChanging(PresentationBase oldValue, PresentationBase newValue)
    {
    }

    protected void PresentationChanged()
    {
      this.NotifyPropertyChanged("Presentation");
    }

    protected internal virtual void Invalidate()
    {
      if (this.Container == null || this.Container.PresentationContainer == null)
        return;
      this.Container.PresentationContainer.InvalidateMeasure();
    }

    internal void CreateLayout(Size constraint)
    {
      this.layout = this.layoutCalculator.CreateLayout(constraint);
    }

    internal void CompleteLayout()
    {
      if (this.layout == null)
        return;
      this.layoutCalculator.CompleteLayout(this);
    }
  }
}
