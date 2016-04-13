// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LinearGaugeControl
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

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A <b>Linear Gauge</b> control shipped with the DXGauges Suite.
  /// 
  /// </para>
  /// 
  /// </summary>
  [LicenseProvider(typeof (DX_WPF_LicenseProvider))]
  [DXToolboxBrowsable]
  public class LinearGaugeControl : AnalogGaugeControl
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
    public static readonly DependencyProperty ModelProperty = DependencyPropertyManager.Register("Model", typeof (LinearGaugeModel), typeof (LinearGaugeControl), new PropertyMetadata((object) null, new PropertyChangedCallback(LinearGaugeControl.ModelProperytChanged)));
    internal static readonly DependencyPropertyKey LayersPropertyKey = DependencyPropertyManager.RegisterReadOnly("Layers", typeof (LinearGaugeLayerCollection), typeof (LinearGaugeControl), new PropertyMetadata());
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
    public static readonly DependencyProperty LayersProperty = LinearGaugeControl.LayersPropertyKey.DependencyProperty;
    internal static readonly DependencyPropertyKey ScalesPropertyKey = DependencyPropertyManager.RegisterReadOnly("Scales", typeof (LinearScaleCollection), typeof (LinearGaugeControl), new PropertyMetadata());
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
    public static readonly DependencyProperty ScalesProperty = LinearGaugeControl.ScalesPropertyKey.DependencyProperty;
    private static readonly DependencyPropertyKey ActualModelPropertyKey = DependencyPropertyManager.RegisterReadOnly("ActualModel", typeof (LinearGaugeModel), typeof (LinearGaugeControl), new PropertyMetadata((object) null, new PropertyChangedCallback(LinearGaugeControl.ActualModelProperytChanged)));
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
    public static readonly DependencyProperty ActualModelProperty = LinearGaugeControl.ActualModelPropertyKey.DependencyProperty;

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a model for the linear gauge control that is used to draw its elements.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearGaugeModel"/> class descendant that is the actual model.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("LinearGaugeControlModel")]
    [Category("Presentation")]
    public LinearGaugeModel Model
    {
      get
      {
        return (LinearGaugeModel) this.GetValue(LinearGaugeControl.ModelProperty);
      }
      set
      {
        this.SetValue(LinearGaugeControl.ModelProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to a collection of  layers contained in the linear gauge.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearGaugeLayerCollection"/> object that contains linear gauge layers.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("LinearGaugeControlLayers")]
    [Category("Elements")]
    public LinearGaugeLayerCollection Layers
    {
      get
      {
        return (LinearGaugeLayerCollection) this.GetValue(LinearGaugeControl.LayersProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to a collection of scales contained in the linear gauge.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearScaleCollection"/> object that contains linear gauge scales.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("LinearGaugeControlScales")]
    [Category("Elements")]
    public LinearScaleCollection Scales
    {
      get
      {
        return (LinearScaleCollection) this.GetValue(LinearGaugeControl.ScalesProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets the actual model used to draw elements of a Linear Gauge.
    /// 
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearGaugeModel"/> class descendant that is the actual model.
    /// 
    /// 
    /// </value>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public LinearGaugeModel ActualModel
    {
      get
      {
        return (LinearGaugeModel) this.GetValue(LinearGaugeControl.ActualModelProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns a list of predefined models for a Linear Gauge control.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Collections.Generic.List`1"/> of <see cref="T:DevExpress.Xpf.Gauges.PredefinedElementKind"/> objects.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("LinearGaugeControlPredefinedModels")]
    public static List<PredefinedElementKind> PredefinedModels
    {
      get
      {
        return PredefinedLinearGaugeModels.ModelKinds;
      }
    }

    static LinearGaugeControl()
    {
      About.CheckLicenseShowNagScreen(typeof (LinearGaugeControl));
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the LinearGaugeControl class with default settings.
    /// 
    /// </para>
    /// 
    /// </summary>
    public LinearGaugeControl()
    {
      this.DefaultStyleKey = (object) typeof (LinearGaugeControl);
      this.SetValue(LinearGaugeControl.ActualModelPropertyKey, (object) new LinearDefaultModel());
      this.SetValue(LinearGaugeControl.LayersPropertyKey, (object) new LinearGaugeLayerCollection(this));
      this.SetValue(LinearGaugeControl.ScalesPropertyKey, (object) new LinearScaleCollection(this));
    }

    private static void ModelProperytChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      LinearGaugeControl linearGaugeControl = d as LinearGaugeControl;
      if (linearGaugeControl == null)
        return;
      LinearGaugeModel linearGaugeModel = e.NewValue as LinearGaugeModel;
      if (linearGaugeModel == null)
        linearGaugeControl.SetValue(LinearGaugeControl.ActualModelPropertyKey, (object) new LinearDefaultModel());
      else
        linearGaugeControl.SetValue(LinearGaugeControl.ActualModelPropertyKey, (object) linearGaugeModel);
    }

    private static void ActualModelProperytChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      IModelSupported modelSupported = d as IModelSupported;
      IOwnedElement ownedElement = e.NewValue as IOwnedElement;
      if (ownedElement != null)
        ownedElement.Owner = (object) (d as LinearGaugeControl);
      if (modelSupported == null)
        return;
      modelSupported.UpdateModel();
    }

    protected internal override void Animate()
    {
      foreach (Scale scale in (Collection<LinearScale>) this.Scales)
        scale.AnimateIndicators(DesignerProperties.GetIsInDesignMode((DependencyObject) this));
    }

    protected override void UpdateModel()
    {
      if (this.Scales != null)
      {
        foreach (IModelSupported modelSupported in (Collection<LinearScale>) this.Scales)
          modelSupported.UpdateModel();
      }
      if (this.Layers == null)
        return;
      foreach (IModelSupported modelSupported in (FreezableCollection<LinearGaugeLayer>) this.Layers)
        modelSupported.UpdateModel();
    }

    protected override void GaugeUnloaded(object sender, RoutedEventArgs e)
    {
      base.GaugeUnloaded(sender, e);
      foreach (Scale scale in (Collection<LinearScale>) this.Scales)
        scale.ClearAnimation();
    }

    protected override IEnumerable<IElementInfo> GetElements()
    {
      foreach (LinearGaugeLayer linearGaugeLayer in (FreezableCollection<LinearGaugeLayer>) this.Layers)
        yield return (IElementInfo) linearGaugeLayer.ElementInfo;
      foreach (LinearScale linearScale in (Collection<LinearScale>) this.Scales)
      {
        foreach (IElementInfo elementInfo in linearScale.Elements)
          yield return elementInfo;
      }
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
      return (AutomationPeer) new LinearGaugeControlAutomationPeer((FrameworkElement) this);
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
    /// A <see cref="T:DevExpress.Xpf.Gauges.LinearGaugeHitInfo"/> object, which contains information about the gauge elements located at the test point.
    /// 
    /// 
    /// </returns>
    public LinearGaugeHitInfo CalcHitInfo(Point point)
    {
      return new LinearGaugeHitInfo(this, point);
    }
  }
}
