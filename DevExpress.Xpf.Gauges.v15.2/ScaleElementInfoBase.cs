// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ScaleElementInfoBase
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Gauges.Native;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  [NonCategorized]
  public abstract class ScaleElementInfoBase : IScaleLayoutElement, INotifyPropertyChanged
  {
    private PresentationControl presentationControl;
    private PresentationBase presentation;
    private ScaleElementLayout layout;

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
        this.RaisePropertyChanged("PresentationControl");
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
        this.presentation = value;
        this.RaisePropertyChanged("Presentation");
      }
    }

    public ScaleElementLayout Layout
    {
      get
      {
        return this.layout;
      }
      internal set
      {
        this.layout = value;
      }
    }

    ScaleElementLayout IScaleLayoutElement.Layout
    {
      get
      {
        return this.Layout;
      }
    }

    Point IScaleLayoutElement.RenderTransformOrigin
    {
      get
      {
        if (this.PresentationControl == null)
          return new Point(0.0, 0.0);
        return this.PresentationControl.GetRenderTransformOrigin();
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    internal ScaleElementInfoBase(PresentationControl presentationControl, PresentationBase presentation)
    {
      this.PresentationControl = presentationControl;
      this.Presentation = presentation;
    }

    protected void RaisePropertyChanged(string propertyName)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
