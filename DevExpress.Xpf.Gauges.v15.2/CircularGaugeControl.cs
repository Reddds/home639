// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.CircularGaugeControl
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using DevExpress.Xpf.Utils.About;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A <b>Circular Gauge</b> control shipped with the DXGauges Suite.
  /// 
  /// </para>
  /// 
  /// </summary>
  [DXToolboxBrowsable]
  [LicenseProvider(typeof (DX_WPF_LicenseProvider))]
  public class CircularGaugeControl : AnalogGaugeControl
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
    public static readonly DependencyProperty ModelProperty = DependencyPropertyManager.Register("Model", typeof (CircularGaugeModel), typeof (CircularGaugeControl), new PropertyMetadata((object) null, new PropertyChangedCallback(CircularGaugeControl.ModelProperytChanged)));
    internal static readonly DependencyPropertyKey LayersPropertyKey = DependencyPropertyManager.RegisterReadOnly("Layers", typeof (CircularGaugeLayerCollection), typeof (CircularGaugeControl), new PropertyMetadata());
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
    public static readonly DependencyProperty LayersProperty = CircularGaugeControl.LayersPropertyKey.DependencyProperty;
    internal static readonly DependencyPropertyKey ScalesPropertyKey = DependencyPropertyManager.RegisterReadOnly("Scales", typeof (ArcScaleCollection), typeof (CircularGaugeControl), new PropertyMetadata());
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
    public static readonly DependencyProperty ScalesProperty = CircularGaugeControl.ScalesPropertyKey.DependencyProperty;
    private static readonly DependencyPropertyKey ActualModelPropertyKey = DependencyPropertyManager.RegisterReadOnly("ActualModel", typeof (CircularGaugeModel), typeof (CircularGaugeControl), new PropertyMetadata((object) null, new PropertyChangedCallback(CircularGaugeControl.ActualModelProperytChanged)));
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
    public static readonly DependencyProperty ActualModelProperty = CircularGaugeControl.ActualModelPropertyKey.DependencyProperty;

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a model for the circular gauge control that is used to draw its elements.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.CircularGaugeModel"/> class descendant that is the actual model.
    /// 
    /// </value>
    [Category("Presentation")]
    [DevExpressXpfGaugesLocalizedDescription("CircularGaugeControlModel")]
    public CircularGaugeModel Model
    {
      get
      {
        return (CircularGaugeModel) this.GetValue(CircularGaugeControl.ModelProperty);
      }
      set
      {
        this.SetValue(CircularGaugeControl.ModelProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to a collection of  layers contained in the circular gauge.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.CircularGaugeLayerCollection"/> object that contains circular gauge layers.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("CircularGaugeControlLayers")]
    [Category("Elements")]
    public CircularGaugeLayerCollection Layers
    {
      get
      {
        return (CircularGaugeLayerCollection) this.GetValue(CircularGaugeControl.LayersProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to a collection of  scales contained in the Circular gauge.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An <see cref="T:DevExpress.Xpf.Gauges.ArcScaleCollection"/> object that contains circular gauge scales.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("CircularGaugeControlScales")]
    [Category("Elements")]
    public ArcScaleCollection Scales
    {
      get
      {
        return (ArcScaleCollection) this.GetValue(CircularGaugeControl.ScalesProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets the actual model used to draw elements of a Circular Gauge.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.CircularGaugeModel"/> class descendant that is the actual model.
    /// 
    /// 
    /// </value>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public CircularGaugeModel ActualModel
    {
      get
      {
        return (CircularGaugeModel) this.GetValue(CircularGaugeControl.ActualModelProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns a list of predefined models for a Circular Gauge control.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Collections.Generic.List`1"/> of <see cref="T:DevExpress.Xpf.Gauges.PredefinedElementKind"/> objects.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("CircularGaugeControlPredefinedModels")]
    public static List<PredefinedElementKind> PredefinedModels
    {
      get
      {
        return PredefinedCircularGaugeModels.ModelKinds;
      }
    }

    private UIElement ClipElement
    {
      get
      {
        return this.GetTemplateChild("PART_ClipElement") as UIElement;
      }
    }

    static CircularGaugeControl()
    {
      About.CheckLicenseShowNagScreen(typeof (CircularGaugeControl));
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the CircularGaugeControl class with default settings.
    /// 
    /// </para>
    /// 
    /// </summary>
    public CircularGaugeControl()
    {
      this.DefaultStyleKey = (object) typeof (CircularGaugeControl);
      this.SetValue(CircularGaugeControl.ActualModelPropertyKey, (object) new CircularDefaultModel());
      this.SetValue(CircularGaugeControl.LayersPropertyKey, (object) new CircularGaugeLayerCollection(this));
      this.SetValue(CircularGaugeControl.ScalesPropertyKey, (object) new ArcScaleCollection(this));
    }

    private static void ModelProperytChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      CircularGaugeControl circularGaugeControl = d as CircularGaugeControl;
      if (circularGaugeControl == null)
        return;
      CircularGaugeModel circularGaugeModel = e.NewValue as CircularGaugeModel;
      if (circularGaugeModel == null)
        circularGaugeControl.SetValue(CircularGaugeControl.ActualModelPropertyKey, (object) new CircularDefaultModel());
      else
        circularGaugeControl.SetValue(CircularGaugeControl.ActualModelPropertyKey, (object) circularGaugeModel);
    }

    private static void ActualModelProperytChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      IModelSupported modelSupported = d as IModelSupported;
      IOwnedElement ownedElement = e.NewValue as IOwnedElement;
      if (ownedElement != null)
        ownedElement.Owner = (object) (d as CircularGaugeControl);
      if (modelSupported == null)
        return;
      modelSupported.UpdateModel();
    }

    protected internal override void Animate()
    {
      foreach (Scale scale in (Collection<ArcScale>) this.Scales)
        scale.AnimateIndicators(DesignerProperties.GetIsInDesignMode((DependencyObject) this));
    }

    protected override void UpdateModel()
    {
      if (this.Scales != null)
      {
        foreach (IModelSupported modelSupported in (Collection<ArcScale>) this.Scales)
          modelSupported.UpdateModel();
      }
      if (this.Layers == null)
        return;
      foreach (IModelSupported modelSupported in (FreezableCollection<CircularGaugeLayer>) this.Layers)
        modelSupported.UpdateModel();
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      this.ClipElement.Clip = (Geometry) new RectangleGeometry()
      {
        Rect = new Rect(new Point(0.0, 0.0), finalSize)
      };
      return base.ArrangeOverride(finalSize);
    }

    protected override void GaugeUnloaded(object sender, RoutedEventArgs e)
    {
      base.GaugeUnloaded(sender, e);
      foreach (Scale scale in (Collection<ArcScale>) this.Scales)
        scale.ClearAnimation();
    }

    protected override IEnumerable<IElementInfo> GetElements()
    {
      foreach (CircularGaugeLayer circularGaugeLayer in (FreezableCollection<CircularGaugeLayer>) this.Layers)
        yield return (IElementInfo) circularGaugeLayer.ElementInfo;
      foreach (ArcScale arcScale in (Collection<ArcScale>) this.Scales)
      {
        foreach (IElementInfo elementInfo in arcScale.Elements)
          yield return elementInfo;
      }
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
      return (AutomationPeer) new CircularGaugeControlAutomationPeer((FrameworkElement) this);
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns information on the gauge elements located at the specified point.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// <param name="point">A <see cref="T:System.Drawing.Point"/> structure which specifies the test point coordinates relative to the gauge's top-left corner.
    /// 
    ///             </param>
    /// <returns>
    /// A <see cref="T:DevExpress.Xpf.Gauges.CircularGaugeHitInfo"/> object, which contains information about the gauge elements located at the test point.
    /// 
    /// 
    /// </returns>
    public CircularGaugeHitInfo CalcHitInfo(Point point)
    {
      return new CircularGaugeHitInfo(this, point);
    }
  }
}
