// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.DigitalGaugeControl
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using DevExpress.Xpf.Utils.About;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A digital gauge control shipped with the DXGauge Suite.
  /// 
  /// </para>
  /// 
  /// </summary>
  [LicenseProvider(typeof (DX_WPF_LicenseProvider))]
  [DXToolboxBrowsable]
  public class DigitalGaugeControl : GaugeControlBase
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
    public static readonly DependencyProperty ModelProperty = DependencyPropertyManager.Register("Model", typeof (DigitalGaugeModel), typeof (DigitalGaugeControl), new PropertyMetadata((object) null, new PropertyChangedCallback(DigitalGaugeControl.ModelProperytChanged)));
    internal static readonly DependencyPropertyKey LayersPropertyKey = DependencyPropertyManager.RegisterReadOnly("Layers", typeof (DigitalGaugeLayerCollection), typeof (DigitalGaugeControl), new PropertyMetadata());
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
    public static readonly DependencyProperty LayersProperty = DigitalGaugeControl.LayersPropertyKey.DependencyProperty;
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
    public static readonly DependencyProperty TextProperty = DependencyPropertyManager.Register("Text", typeof (string), typeof (DigitalGaugeControl), new PropertyMetadata((object) "00.000", new PropertyChangedCallback(DigitalGaugeControl.TextPropertyChanged)));
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
    public static readonly DependencyProperty SymbolCountProperty = DependencyPropertyManager.Register("SymbolCount", typeof (int), typeof (DigitalGaugeControl), new PropertyMetadata((object) 0, new PropertyChangedCallback(DigitalGaugeControl.SymbolCountPropertyChanged)), new ValidateValueCallback(DigitalGaugeControl.CountValidation));
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
    public static readonly DependencyProperty SymbolViewProperty = DependencyPropertyManager.Register("SymbolView", typeof (SymbolViewBase), typeof (DigitalGaugeControl), new PropertyMetadata((object) null, new PropertyChangedCallback(DigitalGaugeControl.SymbolViewPropertyChanged)));
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
    public static readonly DependencyProperty TextHorizontalAlignmentProperty = DependencyPropertyManager.Register("TextHorizontalAlignment", typeof (TextHorizontalAlignment), typeof (DigitalGaugeControl), new PropertyMetadata((object) TextHorizontalAlignment.Center, new PropertyChangedCallback(DigitalGaugeControl.TextAlignmentPropertyChanged)));
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
    public static readonly DependencyProperty TextVerticalAlignmentProperty = DependencyPropertyManager.Register("TextVerticalAlignment", typeof (TextVerticalAlignment), typeof (DigitalGaugeControl), new PropertyMetadata((object) TextVerticalAlignment.Center, new PropertyChangedCallback(DigitalGaugeControl.TextAlignmentPropertyChanged)));
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
    public static readonly DependencyProperty TextDirectionProperty = DependencyPropertyManager.Register("TextDirection", typeof (TextDirection), typeof (DigitalGaugeControl), new PropertyMetadata((object) TextDirection.LeftToRight, new PropertyChangedCallback(DigitalGaugeControl.TextDirectionPropertyChanged)));
    private static readonly DependencyPropertyKey ActualModelPropertyKey = DependencyPropertyManager.RegisterReadOnly("ActualModel", typeof (DigitalGaugeModel), typeof (DigitalGaugeControl), new PropertyMetadata((object) null, new PropertyChangedCallback(DigitalGaugeControl.ActualModelProperytChanged)));
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
    public static readonly DependencyProperty ActualModelProperty = DigitalGaugeControl.ActualModelPropertyKey.DependencyProperty;
    private static readonly DependencyPropertyKey ActualSymbolViewPropertyKey = DependencyPropertyManager.RegisterReadOnly("ActualSymbolView", typeof (SymbolViewBase), typeof (DigitalGaugeControl), new PropertyMetadata((object) null, new PropertyChangedCallback(DigitalGaugeControl.ActualSymbolViewPropertyChanged)));
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
    public static readonly DependencyProperty ActualSymbolViewProperty = DigitalGaugeControl.ActualSymbolViewPropertyKey.DependencyProperty;
    private const string StoryboardResourceKey = "Storyboard";
    private SymbolsLayout symbolslayout;
    private Storyboard storyboard;
    private SymbolViewInternal symbolViewInternal;

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a model for the digital gauge control that is used to draw its elements.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.DigitalGaugeModel"/> class descendant that is the actual model.
    /// 
    /// </value>
    [Category("Presentation")]
    public DigitalGaugeModel Model
    {
      get
      {
        return (DigitalGaugeModel) this.GetValue(DigitalGaugeControl.ModelProperty);
      }
      set
      {
        this.SetValue(DigitalGaugeControl.ModelProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to a collection of  layers contained in the digital gauge.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.DigitalGaugeLayerCollection"/> object that contains digital gauge layers.
    /// 
    /// </value>
    [Category("Elements")]
    public DigitalGaugeLayerCollection Layers
    {
      get
      {
        return (DigitalGaugeLayerCollection) this.GetValue(DigitalGaugeControl.LayersProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a text that is displayed on the symbols panel of the digital gauge control.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.String"/> object that is the text displayed on the digital gauge control.
    /// 
    /// 
    /// </value>
    [Category("Data")]
    public string Text
    {
      get
      {
        return (string) this.GetValue(DigitalGaugeControl.TextProperty);
      }
      set
      {
        this.SetValue(DigitalGaugeControl.TextProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Specifies the total number of symbols (both containing a text and empty or only empty) that should be displayed on the symbols panel.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// An integer value that is the symbols number.
    /// 
    /// </value>
    [Category("Presentation")]
    public int SymbolCount
    {
      get
      {
        return (int) this.GetValue(DigitalGaugeControl.SymbolCountProperty);
      }
      set
      {
        this.SetValue(DigitalGaugeControl.SymbolCountProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to the settings of the current symbol view of the DigitalGaugeControl.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.SymbolViewBase"/> class descendant that is the current symbol view.
    /// 
    /// </value>
    [Category("Presentation")]
    public SymbolViewBase SymbolView
    {
      get
      {
        return (SymbolViewBase) this.GetValue(DigitalGaugeControl.SymbolViewProperty);
      }
      set
      {
        this.SetValue(DigitalGaugeControl.SymbolViewProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Specifies which part of a text limited by the <see cref="P:DevExpress.Xpf.Gauges.DigitalGaugeControl.SymbolCount"/> property should be shown on the symbols panel (either the initial or final).
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.TextDirection"/> enumeration value.
    /// 
    /// </value>
    [Category("Behavior")]
    public TextDirection TextDirection
    {
      get
      {
        return (TextDirection) this.GetValue(DigitalGaugeControl.TextDirectionProperty);
      }
      set
      {
        this.SetValue(DigitalGaugeControl.TextDirectionProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the horizontal alignment of a text for the digital gauge control.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.TextHorizontalAlignment"/> enumeration value.
    /// 
    /// </value>
    [Category("Layout")]
    public TextHorizontalAlignment TextHorizontalAlignment
    {
      get
      {
        return (TextHorizontalAlignment) this.GetValue(DigitalGaugeControl.TextHorizontalAlignmentProperty);
      }
      set
      {
        this.SetValue(DigitalGaugeControl.TextHorizontalAlignmentProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the text vertical alignment for the digital gauge control.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.TextVerticalAlignment"/> enumeration value.
    /// 
    /// </value>
    [Category("Layout")]
    public TextVerticalAlignment TextVerticalAlignment
    {
      get
      {
        return (TextVerticalAlignment) this.GetValue(DigitalGaugeControl.TextVerticalAlignmentProperty);
      }
      set
      {
        this.SetValue(DigitalGaugeControl.TextVerticalAlignmentProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets the actual symbol view of the <see cref="T:DevExpress.Xpf.Gauges.DigitalGaugeControl"/>.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.SymbolViewBase"/> class descendant that is the actual symbol view.
    /// 
    /// </value>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public SymbolViewBase ActualSymbolView
    {
      get
      {
        return (SymbolViewBase) this.GetValue(DigitalGaugeControl.ActualSymbolViewProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets the actual model used to draw elements of a Digital Gauge.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.DigitalGaugeModel"/> class descendant that is the actual model.
    /// 
    /// </value>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DigitalGaugeModel ActualModel
    {
      get
      {
        return (DigitalGaugeModel) this.GetValue(DigitalGaugeControl.ActualModelProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns a list of predefined models for a Digital Gauge control.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Collections.Generic.List`1"/> of <see cref="T:DevExpress.Xpf.Gauges.PredefinedElementKind"/> objects.
    /// 
    /// </value>
    public static List<PredefinedElementKind> PredefinedModels
    {
      get
      {
        return PredefinedDigitalGaugeModels.ModelKinds;
      }
    }

    private Panel ElementsPanel
    {
      get
      {
        return CommonUtils.GetChildPanel(this.GetTemplateChild("PART_Elements") as ItemsControl);
      }
    }

    internal bool AutoSymbolsGeniration
    {
      get
      {
        return this.SymbolCount < 1;
      }
    }

    internal List<string> TextBySymbols
    {
      get
      {
        return this.SymbolViewInternal.SeparateTextToSymbols(this.Text);
      }
    }

    internal SymbolViewInternal SymbolViewInternal
    {
      get
      {
        return this.symbolViewInternal;
      }
      private set
      {
        this.symbolViewInternal = value;
      }
    }

    internal int ActualSymbolCount
    {
      get
      {
        if (!this.AutoSymbolsGeniration)
          return this.SymbolCount;
        int count = this.TextBySymbols.Count;
        if (count <= 0)
          return 1;
        return count;
      }
    }

    internal SymbolsLayout SymbolsLayout
    {
      get
      {
        return this.symbolslayout;
      }
      set
      {
        this.symbolslayout = value;
        this.InvalidateLayout();
      }
    }

    internal Storyboard Storyboard
    {
      get
      {
        if (this.storyboard == null)
        {
          this.storyboard = new Storyboard();
          this.storyboard.Completed += new EventHandler(this.OnStoryboardCompleted);
          this.Resources.Add((object) "Storyboard", (object) this.storyboard);
        }
        return this.storyboard;
      }
    }

    static DigitalGaugeControl()
    {
      About.CheckLicenseShowNagScreen(typeof (DigitalGaugeControl));
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the DigitalGaugeControl class with default settings.
    /// 
    /// </para>
    /// 
    /// </summary>
    public DigitalGaugeControl()
    {
      this.DefaultStyleKey = (object) typeof (DigitalGaugeControl);
      this.SetValue(DigitalGaugeControl.ActualModelPropertyKey, (object) new DigitalDefaultModel());
      this.SetValue(DigitalGaugeControl.ActualSymbolViewPropertyKey, (object) new SevenSegmentsView());
      this.SetValue(DigitalGaugeControl.LayersPropertyKey, (object) new DigitalGaugeLayerCollection(this));
      this.UpdateSymbols();
    }

    private static void TextAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      DigitalGaugeControl digitalGaugeControl = d as DigitalGaugeControl;
      if (digitalGaugeControl == null)
        return;
      digitalGaugeControl.InvalidateLayout();
    }

    private static void ModelProperytChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      DigitalGaugeControl digitalGaugeControl = d as DigitalGaugeControl;
      if (digitalGaugeControl == null)
        return;
      DigitalGaugeModel digitalGaugeModel = e.NewValue as DigitalGaugeModel;
      if (digitalGaugeModel == null)
        digitalGaugeControl.SetValue(DigitalGaugeControl.ActualModelPropertyKey, (object) new DigitalDefaultModel());
      else
        digitalGaugeControl.SetValue(DigitalGaugeControl.ActualModelPropertyKey, (object) digitalGaugeModel);
    }

    private static void ActualModelProperytChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      IModelSupported modelSupported = d as IModelSupported;
      IOwnedElement ownedElement = e.NewValue as IOwnedElement;
      if (ownedElement != null)
        ownedElement.Owner = (object) (d as DigitalGaugeControl);
      if (modelSupported == null)
        return;
      modelSupported.UpdateModel();
    }

    private static void TextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      DigitalGaugeControl digitalGaugeControl = d as DigitalGaugeControl;
      if (digitalGaugeControl == null)
        return;
      if (digitalGaugeControl.AutoSymbolsGeniration)
      {
        digitalGaugeControl.UpdateSymbols();
      }
      else
      {
        digitalGaugeControl.SymbolViewInternal.UpdateSymbols();
        digitalGaugeControl.Animate();
      }
    }

    private static void TextDirectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      DigitalGaugeControl digitalGaugeControl = d as DigitalGaugeControl;
      if (digitalGaugeControl == null)
        return;
      digitalGaugeControl.UpdateSymbols();
    }

    private static void SymbolCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      DigitalGaugeControl digitalGaugeControl = d as DigitalGaugeControl;
      if (digitalGaugeControl == null)
        return;
      digitalGaugeControl.UpdateSymbols();
    }

    private static void SymbolViewPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      DigitalGaugeControl digitalGaugeControl = d as DigitalGaugeControl;
      if (digitalGaugeControl == null)
        return;
      SymbolViewBase symbolViewBase = e.NewValue as SymbolViewBase;
      if (symbolViewBase == null)
        digitalGaugeControl.SetValue(DigitalGaugeControl.ActualSymbolViewPropertyKey, (object) new SevenSegmentsView());
      else
        digitalGaugeControl.SetValue(DigitalGaugeControl.ActualSymbolViewPropertyKey, (object) symbolViewBase);
    }

    private static void ActualSymbolViewPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      DigitalGaugeControl digitalGaugeControl = d as DigitalGaugeControl;
      if (digitalGaugeControl == null || object.ReferenceEquals(e.NewValue, e.OldValue))
        return;
      SymbolViewBase symbolViewBase = e.NewValue as SymbolViewBase;
      if (symbolViewBase != null)
      {
        digitalGaugeControl.SymbolViewInternal = symbolViewBase.CreateInternalView();
        ((IOwnedElement) digitalGaugeControl.SymbolViewInternal).Owner = (object) digitalGaugeControl;
        ((IOwnedElement) symbolViewBase).Owner = (object) digitalGaugeControl;
      }
      digitalGaugeControl.UpdateSymbols();
    }

    private static bool CountValidation(object value)
    {
      return (int) value > -1;
    }

    private void OnStoryboardCompleted(object sender, EventArgs e)
    {
      this.SymbolViewInternal.Replay();
    }

    private void UpdateSymbols()
    {
      this.SymbolViewInternal.RecreateSymbols(this.ActualSymbolCount);
      this.UpdateElements();
      this.InvalidateLayout();
      this.Animate();
    }

    internal void InvalidateLayout()
    {
      foreach (IElementInfo elementInfo in (Collection<IElementInfo>) this.Elements)
        elementInfo.Invalidate();
      if (this.SymbolsLayout != null)
        this.SymbolsLayout.Invalidate();
      if (this.ElementsPanel == null)
        return;
      this.ElementsPanel.InvalidateMeasure();
    }

    internal void ViewOptionsChanged()
    {
      this.SymbolViewInternal.OnOptionsChanged();
    }

    internal void UpdateViewSymbols()
    {
      this.SymbolViewInternal.UpdateSymbols();
    }

    internal void UpdateSymbolsModels()
    {
      this.SymbolViewInternal.UpdateModel();
    }

    protected internal override void Animate()
    {
      this.SymbolViewInternal.Animate();
    }

    protected override IEnumerable<IElementInfo> GetElements()
    {
      if (this.Layers != null)
      {
        foreach (DigitalGaugeLayer digitalGaugeLayer in (FreezableCollection<DigitalGaugeLayer>) this.Layers)
          yield return (IElementInfo) digitalGaugeLayer.ElementInfo;
      }
      foreach (SymbolInfo symbolInfo in this.SymbolViewInternal.Symbols)
        yield return (IElementInfo) symbolInfo;
    }

    protected override void UpdateModel()
    {
      if (this.SymbolViewInternal != null)
        ((IModelSupported) this.SymbolViewInternal).UpdateModel();
      if (this.Layers == null)
        return;
      foreach (IModelSupported modelSupported in (FreezableCollection<DigitalGaugeLayer>) this.Layers)
        modelSupported.UpdateModel();
    }

    protected override void GaugeLoaded(object sender, RoutedEventArgs e)
    {
      this.Dispatcher.BeginInvoke((Delegate) (() => this.Animate()));
      base.GaugeLoaded(sender, e);
    }

    protected override void GaugeUnloaded(object sender, RoutedEventArgs e)
    {
      base.GaugeUnloaded(sender, e);
      if (this.storyboard == null)
        return;
      this.storyboard.Stop();
      this.storyboard.Completed -= new EventHandler(this.OnStoryboardCompleted);
      this.Resources.Remove((object) "Storyboard");
      this.storyboard = (Storyboard) null;
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
      return (AutomationPeer) new DigitalGaugeControlAutomationPeer((FrameworkElement) this);
    }
  }
}
