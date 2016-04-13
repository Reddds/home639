// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ScaleCustomElement
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A custom element on a scale.
  /// 
  /// </para>
  /// 
  /// </summary>
  [System.Windows.Markup.ContentProperty("Content")]
  public class ScaleCustomElement : GaugeElement, IGaugeLayoutElement, IElementInfo
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
    public static readonly DependencyProperty ContentProperty = DependencyPropertyManager.Register("Content", typeof (object), typeof (ScaleCustomElement), new PropertyMetadata(new PropertyChangedCallback(ScaleCustomElement.ContentPropertyChanged)));
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
    public static readonly DependencyProperty ContentTemplateProperty = DependencyPropertyManager.Register("ContentTemplate", typeof (DataTemplate), typeof (ScaleCustomElement));
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
    public static readonly DependencyProperty VisibleProperty = DependencyPropertyManager.Register("Visible", typeof (bool), typeof (ScaleCustomElement), new PropertyMetadata((object) true, new PropertyChangedCallback(ScaleCustomElement.PropertyChanged)));
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
    public static readonly DependencyProperty ZIndexProperty = DependencyPropertyManager.Register("ZIndex", typeof (int), typeof (ScaleCustomElement), new PropertyMetadata((object) 30, new PropertyChangedCallback(ScaleCustomElement.ZIndexPropertyChanged)));
    private ElementLayout layout;

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the scale custom element's content. This is a dependency property.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Object"/> value that is the custom element's content.
    /// 
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ScaleCustomElementContent")]
    [Category("Data")]
    [TypeConverter(typeof (StringConverter))]
    public object Content
    {
      get
      {
        return this.GetValue(ScaleCustomElement.ContentProperty);
      }
      set
      {
        this.SetValue(ScaleCustomElement.ContentProperty, value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the template  that defines the presentation of the custom element's content represented by the <see cref="P:DevExpress.Xpf.Gauges.ScaleCustomElement.Content"/>  property. This is a dependency property.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Windows.DataTemplate"/> object, representing the template which defines the presentation of the custom element's content.
    /// 
    /// </value>
    [Category("Presentation")]
    [DevExpressXpfGaugesLocalizedDescription("ScaleCustomElementContentTemplate")]
    public DataTemplate ContentTemplate
    {
      get
      {
        return (DataTemplate) this.GetValue(ScaleCustomElement.ContentTemplateProperty);
      }
      set
      {
        this.SetValue(ScaleCustomElement.ContentTemplateProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets whether the scale custom element is visible.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> if the custom element is visible on the scale; otherwise, <b>false</b>.
    /// 
    /// </value>
    [Category("Behavior")]
    [DevExpressXpfGaugesLocalizedDescription("ScaleCustomElementVisible")]
    public bool Visible
    {
      get
      {
        return (bool) this.GetValue(ScaleCustomElement.VisibleProperty);
      }
      set
      {
        this.SetValue(ScaleCustomElement.VisibleProperty, (object) (bool) (value ? 1 : 0));
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the z-index of a scale custom element.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An integer value that is the z-index.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ScaleCustomElementZIndex")]
    [Category("Layout")]
    public int ZIndex
    {
      get
      {
        return (int) this.GetValue(ScaleCustomElement.ZIndexProperty);
      }
      set
      {
        this.SetValue(ScaleCustomElement.ZIndexProperty, (object) value);
      }
    }

    internal Scale Scale
    {
      get
      {
        return this.Owner as Scale;
      }
    }

    Point IGaugeLayoutElement.Offset
    {
      get
      {
        if (this.Scale == null)
          return new Point(0.0, 0.0);
        return this.Scale.GetLayoutOffset();
      }
    }

    bool IGaugeLayoutElement.InfluenceOnGaugeSize
    {
      get
      {
        return false;
      }
    }

    ElementLayout IGaugeLayoutElement.Layout
    {
      get
      {
        return this.layout;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the ScaleCustomElement class with default settings.
    /// 
    /// </para>
    /// 
    /// </summary>
    public ScaleCustomElement()
    {
      this.DefaultStyleKey = (object) typeof (ScaleCustomElement);
    }

    private static void ZIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ScaleCustomElement scaleCustomElement = d as ScaleCustomElement;
      if (scaleCustomElement == null)
        return;
      Panel.SetZIndex((UIElement) scaleCustomElement, (int) e.NewValue);
    }

    private static void ContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ILogicalParent logicalParent = d as ILogicalParent;
      if (logicalParent == null)
        return;
      if (e.OldValue != null)
        logicalParent.RemoveChild(e.OldValue);
      if (e.NewValue == null)
        return;
      logicalParent.AddChild(e.NewValue);
    }

    protected static void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ScaleCustomElement scaleCustomElement = d as ScaleCustomElement;
      if (scaleCustomElement == null || scaleCustomElement.Scale == null)
        return;
      scaleCustomElement.Scale.Invalidate();
    }

    void IElementInfo.Invalidate()
    {
      UIElement uiElement = LayoutHelper.GetParent((DependencyObject) this, false) as UIElement;
      if (uiElement == null)
        return;
      uiElement.InvalidateMeasure();
    }

    protected virtual ElementLayout CreateLayout(ScaleMapping mapping)
    {
      ElementLayout elementLayout = new ElementLayout(mapping.Layout.InitialBounds.Width, mapping.Layout.InitialBounds.Height);
      elementLayout.CompleteLayout(new Point(0.0, 0.0), (Transform) null, (Geometry) null);
      return elementLayout;
    }

    internal void CalculateLayout(ScaleMapping mapping)
    {
      this.layout = this.Visible ? this.CreateLayout(mapping) : (ElementLayout) null;
    }

    [SpecialName]
    Point IGaugeLayoutElement.get_RenderTransformOrigin()
    {
      return this.RenderTransformOrigin;
    }
  }
}
