// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.GaugeControlBase
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// The base class for all gauge controls shipped with the DXGauges Suite.
  /// 
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class GaugeControlBase : Control, IModelSupported
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
    public static readonly DependencyProperty EnableAnimationProperty = DependencyPropertyManager.Register("EnableAnimation", typeof (bool), typeof (GaugeControlBase), new PropertyMetadata((object) false, new PropertyChangedCallback(GaugeControlBase.EnableAnimationPropertyChanged)));
    internal static readonly DependencyPropertyKey ElementsPropertyKey = DependencyPropertyManager.RegisterReadOnly("Elements", typeof (ObservableCollection<IElementInfo>), typeof (GaugeControlBase), new PropertyMetadata());
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
    public static readonly DependencyProperty ElementsProperty = GaugeControlBase.ElementsPropertyKey.DependencyProperty;
    private const double defaultWidth = 250.0;
    private const double defaultHeight = 250.0;
    private UIElement baseLayoutElement;

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value specifying whether value indicators should be animated when changing their values.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> to enable animation; otherwise, <b>false</b>.
    /// 
    /// 
    /// </value>
    [Category("Animation")]
    public bool EnableAnimation
    {
      get
      {
        return (bool) this.GetValue(GaugeControlBase.EnableAnimationProperty);
      }
      set
      {
        this.SetValue(GaugeControlBase.EnableAnimationProperty, (object) (bool) (value ? 1 : 0));
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// This property is hidden and intended for internal use only. Normally, you won't need to use it.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A collection of elements.
    /// 
    /// </value>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ObservableCollection<IElementInfo> Elements
    {
      get
      {
        return (ObservableCollection<IElementInfo>) this.GetValue(GaugeControlBase.ElementsProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// This property is hidden, because it is not supported in this class.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Windows.HorizontalAlignment"/> value.
    /// 
    /// </value>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public new HorizontalAlignment HorizontalContentAlignment
    {
      get
      {
        return base.HorizontalContentAlignment;
      }
      set
      {
        base.HorizontalContentAlignment = value;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// This property is hidden because it is not supported in this class.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Windows.HorizontalAlignment"/> value.
    /// 
    /// </value>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public new VerticalAlignment VerticalContentAlignment
    {
      get
      {
        return base.VerticalContentAlignment;
      }
      set
      {
        base.VerticalContentAlignment = value;
      }
    }

    internal UIElement BaseLayoutElement
    {
      get
      {
        return this.baseLayoutElement;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the GaugeControlBase class with default settings.
    /// 
    /// </para>
    /// 
    /// </summary>
    public GaugeControlBase()
    {
      this.Loaded += new RoutedEventHandler(this.GaugeLoaded);
      this.Unloaded += new RoutedEventHandler(this.GaugeUnloaded);
      this.SetValue(GaugeControlBase.ElementsPropertyKey, (object) new ObservableCollection<IElementInfo>());
    }

    private static void EnableAnimationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      GaugeControlBase gaugeControlBase = d as GaugeControlBase;
      if (gaugeControlBase == null)
        return;
      gaugeControlBase.Animate();
    }

    void IModelSupported.UpdateModel()
    {
      this.UpdateModel();
    }

    /// <summary>
    /// 
    /// <para>
    /// Called after the template is completely generated and attached to the visual tree.
    /// 
    /// </para>
    /// 
    /// </summary>
    public override void OnApplyTemplate()
    {
      this.baseLayoutElement = this.GetTemplateChild("PART_BaseLayoutElement") as UIElement;
      base.OnApplyTemplate();
    }

    protected virtual void GaugeLoaded(object sender, RoutedEventArgs e)
    {
    }

    protected virtual void GaugeUnloaded(object sender, RoutedEventArgs e)
    {
    }

    protected internal abstract void Animate();

    protected abstract void UpdateModel();

    protected abstract IEnumerable<IElementInfo> GetElements();

    protected override Size MeasureOverride(Size constraint)
    {
      return base.MeasureOverride(new Size(double.IsInfinity(constraint.Width) ? 250.0 : constraint.Width, double.IsInfinity(constraint.Height) ? 250.0 : constraint.Height));
    }

    internal void UpdateElements()
    {
      this.Elements.Clear();
      foreach (IElementInfo elementInfo in this.GetElements())
        this.Elements.Add(elementInfo);
    }
  }
}
