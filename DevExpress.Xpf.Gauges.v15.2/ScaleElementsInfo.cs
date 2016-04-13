// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ScaleElementsInfo
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Gauges.Native;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DevExpress.Xpf.Gauges
{
  [NonCategorized]
  public class ScaleElementsInfo : ElementInfoBase
  {
    private readonly Scale scale;
    private ObservableCollection<object> elements;

    protected internal override object HitTestableObject
    {
      get
      {
        return (object) null;
      }
    }

    protected internal override object HitTestableParent
    {
      get
      {
        return (object) this.scale;
      }
    }

    protected internal override bool IsHitTestVisible
    {
      get
      {
        return this.scale.IsHitTestVisible;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ObservableCollection<object> Elements
    {
      get
      {
        return this.elements;
      }
      private set
      {
        if (this.elements == value)
          return;
        this.elements = value;
        this.NotifyPropertyChanged("Elements");
      }
    }

    internal ScaleElementsInfo(Scale scale, int zIndex)
      : base((ILayoutCalculator) scale, zIndex, (PresentationControl) new ScaleElementsPresentationControl(), (PresentationBase) null)
    {
      this.scale = scale;
      this.Elements = new ObservableCollection<object>();
    }

    protected internal override void Invalidate()
    {
      base.Invalidate();
      ScaleElementsPresentationControl presentationControl = this.PresentationControl as ScaleElementsPresentationControl;
      if (presentationControl == null || presentationControl.Panel == null)
        return;
      presentationControl.Panel.InvalidateMeasure();
    }
  }
}
