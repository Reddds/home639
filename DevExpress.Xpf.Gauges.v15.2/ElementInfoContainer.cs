// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ElementInfoContainer
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace DevExpress.Xpf.Gauges
{
  [NonCategorized]
  [TemplatePart(Name = "PART_ElementPresentationContainer", Type = typeof (Panel))]
  public class ElementInfoContainer : Control, IHitTestableElement, IGaugeLayoutElement
  {
    public static readonly DependencyProperty ElementInfoProperty = DependencyPropertyManager.Register("ElementInfo", typeof (object), typeof (ElementInfoContainer), new PropertyMetadata(new PropertyChangedCallback(ElementInfoContainer.ElementInfoPropertyChanged)));
    public static readonly DependencyProperty StretchToAvailableSizeProperty = DependencyPropertyManager.Register("StretchToAvailableSize", typeof (bool), typeof (ElementInfoContainer), new PropertyMetadata((object) true));

    public object ElementInfo
    {
      get
      {
        return this.GetValue(ElementInfoContainer.ElementInfoProperty);
      }
      set
      {
        this.SetValue(ElementInfoContainer.ElementInfoProperty, value);
      }
    }

    public bool StretchToAvailableSize
    {
      get
      {
        return (bool) this.GetValue(ElementInfoContainer.StretchToAvailableSizeProperty);
      }
      set
      {
        this.SetValue(ElementInfoContainer.StretchToAvailableSizeProperty, (object) (bool) (value ? 1 : 0));
      }
    }

    private ElementInfoBase ActualElementInfo
    {
      get
      {
        return this.ElementInfo as ElementInfoBase;
      }
    }

    internal UIElement PresentationContainer
    {
      get
      {
        return this.GetTemplateChild("PART_ElementPresentationContainer") as UIElement;
      }
    }

    object IHitTestableElement.Element
    {
      get
      {
        if (this.ElementInfo == null)
          return (object) null;
        return this.ActualElementInfo.HitTestableObject;
      }
    }

    object IHitTestableElement.Parent
    {
      get
      {
        if (this.ElementInfo == null)
          return (object) null;
        return this.ActualElementInfo.HitTestableParent;
      }
    }

    bool IHitTestableElement.IsHitTestVisible
    {
      get
      {
        if (this.ElementInfo == null)
          return false;
        return this.ActualElementInfo.IsHitTestVisible;
      }
    }

    ElementLayout IGaugeLayoutElement.Layout
    {
      get
      {
        return new ElementLayout();
      }
    }

    Point IGaugeLayoutElement.Offset
    {
      get
      {
        return new Point(0.0, 0.0);
      }
    }

    bool IGaugeLayoutElement.InfluenceOnGaugeSize
    {
      get
      {
        if (this.ActualElementInfo == null)
          return false;
        return this.ActualElementInfo.InfluenceOnGaugeSize;
      }
    }

    public ElementInfoContainer()
    {
      this.DefaultStyleKey = (object) typeof (ElementInfoContainer);
    }

    private static void ElementInfoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ElementInfoBase elementInfoBase1 = e.NewValue as ElementInfoBase;
      if (elementInfoBase1 != null)
        elementInfoBase1.Container = d as ElementInfoContainer;
      ElementInfoBase elementInfoBase2 = e.OldValue as ElementInfoBase;
      if (elementInfoBase2 == null)
        return;
      elementInfoBase2.Container = (ElementInfoContainer) null;
    }

    [SpecialName]
    Point IGaugeLayoutElement.get_RenderTransformOrigin()
    {
      return this.RenderTransformOrigin;
    }
  }
}
