// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Scale
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Utils;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// Serves as the base class for all scales.
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class Scale : GaugeElement, IWeakEventListener, IHitTestableElement, IModelSupported, ILayoutCalculator
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
    public static readonly DependencyProperty StartValueProperty = DependencyPropertyManager.Register("StartValue", typeof (double), typeof (Scale), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(Scale.UpdateElements)));
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
    public static readonly DependencyProperty EndValueProperty = DependencyPropertyManager.Register("EndValue", typeof (double), typeof (Scale), new PropertyMetadata((object) 100.0, new PropertyChangedCallback(Scale.UpdateElements)));
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
    public static readonly DependencyProperty MajorTickmarkOptionsProperty = DependencyPropertyManager.Register("MajorTickmarkOptions", typeof (MajorTickmarkOptions), typeof (Scale), new PropertyMetadata(new PropertyChangedCallback(Scale.TickmarkOptionsPropertyChanged)));
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
    public static readonly DependencyProperty MajorIntervalCountProperty = DependencyPropertyManager.Register("MajorIntervalCount", typeof (int), typeof (Scale), new PropertyMetadata((object) 10, new PropertyChangedCallback(Scale.UpdateElements)), new ValidateValueCallback(Scale.MajorIntervalCountValidation));
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
    public static readonly DependencyProperty MinorTickmarkOptionsProperty = DependencyPropertyManager.Register("MinorTickmarkOptions", typeof (MinorTickmarkOptions), typeof (Scale), new PropertyMetadata(new PropertyChangedCallback(Scale.TickmarkOptionsPropertyChanged)));
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
    public static readonly DependencyProperty MinorIntervalCountProperty = DependencyPropertyManager.Register("MinorIntervalCount", typeof (int), typeof (Scale), new PropertyMetadata((object) 5, new PropertyChangedCallback(Scale.UpdateElements)), new ValidateValueCallback(Scale.MinorIntervalCountValidation));
    internal static readonly DependencyPropertyKey CustomLabelsPropertyKey = DependencyPropertyManager.RegisterReadOnly("CustomLabels", typeof (ScaleCustomLabelCollection), typeof (Scale), new PropertyMetadata(new PropertyChangedCallback(Scale.CustomLabelsPropertyChanged)));
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
    public static readonly DependencyProperty CustomLabelsProperty = Scale.CustomLabelsPropertyKey.DependencyProperty;
    internal static readonly DependencyPropertyKey CustomElementsPropertyKey = DependencyPropertyManager.RegisterReadOnly("CustomElements", typeof (ScaleCustomElementCollection), typeof (Scale), new PropertyMetadata(new PropertyChangedCallback(Scale.CustomElementsPropertyChanged)));
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
    public static readonly DependencyProperty CustomElementsProperty = Scale.CustomElementsPropertyKey.DependencyProperty;
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
    public static readonly DependencyProperty ShowLabelsProperty = DependencyPropertyManager.Register("ShowLabels", typeof (DefaultBoolean), typeof (Scale), new PropertyMetadata((object) DefaultBoolean.Default, new PropertyChangedCallback(Scale.InvalidateLayout)));
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
    public static readonly DependencyProperty ShowMajorTickmarksProperty = DependencyPropertyManager.Register("ShowMajorTickmarks", typeof (DefaultBoolean), typeof (Scale), new PropertyMetadata((object) DefaultBoolean.Default, new PropertyChangedCallback(Scale.InvalidateLayout)));
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
    public static readonly DependencyProperty ShowMinorTickmarksProperty = DependencyPropertyManager.Register("ShowMinorTickmarks", typeof (DefaultBoolean), typeof (Scale), new PropertyMetadata((object) DefaultBoolean.Default, new PropertyChangedCallback(Scale.InvalidateLayout)));
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
    public static readonly DependencyProperty LabelPresentationProperty = DependencyPropertyManager.Register("LabelPresentation", typeof (ScaleLabelPresentation), typeof (Scale), new PropertyMetadata((object) null, new PropertyChangedCallback(Scale.PresentationPropertyChanged)));
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
    public static readonly DependencyProperty TickmarksPresentationProperty = DependencyPropertyManager.Register("TickmarksPresentation", typeof (TickmarksPresentation), typeof (Scale), new PropertyMetadata((object) null, new PropertyChangedCallback(Scale.PresentationPropertyChanged)));
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
    public static readonly DependencyProperty LineOptionsProperty = DependencyPropertyManager.Register("LineOptions", typeof (ScaleLineOptions), typeof (Scale), new PropertyMetadata((object) null, new PropertyChangedCallback(Scale.LineOptionsPropertyChanged)));
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
    public static readonly DependencyProperty ShowLineProperty = DependencyPropertyManager.Register("ShowLine", typeof (DefaultBoolean), typeof (Scale), new PropertyMetadata((object) DefaultBoolean.Default, new PropertyChangedCallback(Scale.InvalidateLayout)));
    private const string StoryboardResourceKey = "storyboard";
    private readonly ScaleElementsInfo minorTickmarksInfo;
    private readonly ScaleElementsInfo majorTickmarksInfo;
    private readonly ScaleElementsInfo labelsInfo;
    private readonly ScaleElementsInfo lineInfo;
    private ScaleLayoutControl layoutControl;
    private ScaleMapping mapping;
    private bool shouldElementsUpdate;

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the start value of the scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value which is a scale start.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ScaleStartValue")]
    [Category("Data")]
    public double StartValue
    {
      get
      {
        return (double) this.GetValue(Scale.StartValueProperty);
      }
      set
      {
        this.SetValue(Scale.StartValueProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the end value of the scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value which is the end of the scale.
    /// 
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ScaleEndValue")]
    [Category("Data")]
    public double EndValue
    {
      get
      {
        return (double) this.GetValue(Scale.EndValueProperty);
      }
      set
      {
        this.SetValue(Scale.EndValueProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to the options that define the appearance, behavior and location of major tickmarks within the current scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.MajorTickmarkOptions"/> object that contains settings for major tickmarks.
    /// 
    /// </value>
    [Category("Presentation")]
    [DevExpressXpfGaugesLocalizedDescription("ScaleMajorTickmarkOptions")]
    public MajorTickmarkOptions MajorTickmarkOptions
    {
      get
      {
        return (MajorTickmarkOptions) this.GetValue(Scale.MajorTickmarkOptionsProperty);
      }
      set
      {
        this.SetValue(Scale.MajorTickmarkOptionsProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value that specifies the number of intervals between major tickmarks on a scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Int32"/> value that is the number of intervals between major tickmarks' on a scale.
    /// 
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ScaleMajorIntervalCount")]
    [Category("Presentation")]
    public int MajorIntervalCount
    {
      get
      {
        return (int) this.GetValue(Scale.MajorIntervalCountProperty);
      }
      set
      {
        this.SetValue(Scale.MajorIntervalCountProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to the options that define the appearance, behavior and location of minor tickmarks within the current scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.MinorTickmarkOptions"/> object that contains settings for minor tickmarks.
    /// 
    /// </value>
    [Category("Presentation")]
    [DevExpressXpfGaugesLocalizedDescription("ScaleMinorTickmarkOptions")]
    public MinorTickmarkOptions MinorTickmarkOptions
    {
      get
      {
        return (MinorTickmarkOptions) this.GetValue(Scale.MinorTickmarkOptionsProperty);
      }
      set
      {
        this.SetValue(Scale.MinorTickmarkOptionsProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value that specifies the number of intervals between minor tickmarks on a scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Int32"/> value that is the number of intervals between minor tickmarks' on a scale.
    /// 
    /// 
    /// </value>
    [Category("Presentation")]
    [DevExpressXpfGaugesLocalizedDescription("ScaleMinorIntervalCount")]
    public int MinorIntervalCount
    {
      get
      {
        return (int) this.GetValue(Scale.MinorIntervalCountProperty);
      }
      set
      {
        this.SetValue(Scale.MinorIntervalCountProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to a collection of custom labels contained in the current scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.ScaleCustomLabelCollection"/> object that contains scale custom labels.
    /// 
    /// </value>
    [Category("Elements")]
    [DevExpressXpfGaugesLocalizedDescription("ScaleCustomLabels")]
    public ScaleCustomLabelCollection CustomLabels
    {
      get
      {
        return (ScaleCustomLabelCollection) this.GetValue(Scale.CustomLabelsProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to a collection of custom elements  contained in the current Circular Scale or Linear Scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.ScaleCustomElementCollection"/> object that contains scale custom elements.
    /// 
    /// </value>
    [Category("Elements")]
    [DevExpressXpfGaugesLocalizedDescription("ScaleCustomElements")]
    public ScaleCustomElementCollection CustomElements
    {
      get
      {
        return (ScaleCustomElementCollection) this.GetValue(Scale.CustomElementsProperty);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value indicating whether or not labels should be displayed on a scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> to show labels on a scale; otherwise <b>false</b>.
    /// 
    /// </value>
    [Category("Behavior")]
    [DevExpressXpfGaugesLocalizedDescription("ScaleShowLabels")]
    public DefaultBoolean ShowLabels
    {
      get
      {
        return (DefaultBoolean) this.GetValue(Scale.ShowLabelsProperty);
      }
      set
      {
        this.SetValue(Scale.ShowLabelsProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets whether or not the major tickmarks should be visible on a scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A  <see cref="T:DevExpress.Utils.DefaultBoolean"/> enumeration value that specifies the visibility of major tickmarks on a scale.
    /// 
    /// 
    /// 
    /// 
    /// </value>
    [Category("Behavior")]
    [DevExpressXpfGaugesLocalizedDescription("ScaleShowMajorTickmarks")]
    public DefaultBoolean ShowMajorTickmarks
    {
      get
      {
        return (DefaultBoolean) this.GetValue(Scale.ShowMajorTickmarksProperty);
      }
      set
      {
        this.SetValue(Scale.ShowMajorTickmarksProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets whether the minor tickmarks should be visible on a scale or not.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Utils.DefaultBoolean"/> enumeration value that specifies the visibility of minor tickmarks on a scale.
    /// 
    /// 
    /// </value>
    [Category("Behavior")]
    [DevExpressXpfGaugesLocalizedDescription("ScaleShowMinorTickmarks")]
    public DefaultBoolean ShowMinorTickmarks
    {
      get
      {
        return (DefaultBoolean) this.GetValue(Scale.ShowMinorTickmarksProperty);
      }
      set
      {
        this.SetValue(Scale.ShowMinorTickmarksProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the current presentation that specifies the appearance of labels.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.ScaleLabelPresentation"/> object.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ScaleLabelPresentation")]
    [Category("Presentation")]
    public ScaleLabelPresentation LabelPresentation
    {
      get
      {
        return (ScaleLabelPresentation) this.GetValue(Scale.LabelPresentationProperty);
      }
      set
      {
        this.SetValue(Scale.LabelPresentationProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns whether the current presentation of scale tickmarks is circular or linear.
    /// 
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.TickmarksPresentation"/> object.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ScaleTickmarksPresentation")]
    [Category("Presentation")]
    public TickmarksPresentation TickmarksPresentation
    {
      get
      {
        return (TickmarksPresentation) this.GetValue(Scale.TickmarksPresentationProperty);
      }
      set
      {
        this.SetValue(Scale.TickmarksPresentationProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Indicates whether or not a line should be displayed on a scale.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A  <see cref="T:DevExpress.Utils.DefaultBoolean"/> enumeration value that specifies a line's visibility on a scale.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ScaleShowLine")]
    [Category("Behavior")]
    public DefaultBoolean ShowLine
    {
      get
      {
        return (DefaultBoolean) this.GetValue(Scale.ShowLineProperty);
      }
      set
      {
        this.SetValue(Scale.ShowLineProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to the options that specify the shape and position of a scale line, either Circular or Linear.
    /// 
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.ScaleLineOptions"/> object that contains line options.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ScaleLineOptions")]
    [Category("Presentation")]
    public ScaleLineOptions LineOptions
    {
      get
      {
        return (ScaleLineOptions) this.GetValue(Scale.LineOptionsProperty);
      }
      set
      {
        this.SetValue(Scale.LineOptionsProperty, (object) value);
      }
    }

    private bool ActualShowLabels
    {
      get
      {
        if (this.ShowLabels == DefaultBoolean.Default && this.Model != null)
          return this.Model.ShowLabels;
        return this.ShowLabels != DefaultBoolean.False;
      }
    }

    private bool ActualShowMajorTickmarks
    {
      get
      {
        if (this.ShowMajorTickmarks == DefaultBoolean.Default && this.Model != null)
          return this.Model.ShowMajorTickmarks;
        return this.ShowMajorTickmarks != DefaultBoolean.False;
      }
    }

    private bool ActualShowMinorTickmarks
    {
      get
      {
        if (this.ShowMinorTickmarks == DefaultBoolean.Default && this.Model != null)
          return this.Model.ShowMinorTickmarks;
        return this.ShowMinorTickmarks != DefaultBoolean.False;
      }
    }

    protected bool ActualShowLine
    {
      get
      {
        if (this.ShowLine == DefaultBoolean.Default && this.Model != null)
          return this.Model.ShowLine;
        return this.ShowLine != DefaultBoolean.False;
      }
    }

    private ScaleLabelPresentation ActualLabelPresentation
    {
      get
      {
        if (this.LabelPresentation != null)
          return this.LabelPresentation;
        if (this.Model != null && this.Model.LabelPresentation != null)
          return this.Model.LabelPresentation;
        return (ScaleLabelPresentation) new DefaultScaleLabelPresentation();
      }
    }

    internal TickmarksPresentation ActualTickmarksPresentation
    {
      get
      {
        if (this.TickmarksPresentation != null)
          return this.TickmarksPresentation;
        if (this.Model != null && this.Model.TickmarksPresentation != null)
          return this.Model.TickmarksPresentation;
        return (TickmarksPresentation) new DefaultTickmarksPresentation();
      }
    }

    internal abstract ScaleLinePresentation ActualLinePresentation { get; }

    protected abstract ScaleModel Model { get; }

    protected internal abstract ScaleLabelOptions ActualLabelOptions { get; }

    protected internal abstract IEnumerable<IElementInfo> Elements { get; }

    protected internal abstract IEnumerable<ValueIndicatorBase> Indicators { get; }

    internal MajorTickmarkOptions ActualMajorTickmarkOptions
    {
      get
      {
        if (this.MajorTickmarkOptions != null)
          return this.MajorTickmarkOptions;
        if (this.Model != null && this.Model.MajorTickmarkOptions != null)
          return this.Model.MajorTickmarkOptions;
        return new MajorTickmarkOptions();
      }
    }

    internal MinorTickmarkOptions ActualMinorTickmarkOptions
    {
      get
      {
        if (this.MinorTickmarkOptions != null)
          return this.MinorTickmarkOptions;
        if (this.Model != null && this.Model.MinorTickmarkOptions != null)
          return this.Model.MinorTickmarkOptions;
        return new MinorTickmarkOptions();
      }
    }

    internal ScaleLineOptions ActualLineOptions
    {
      get
      {
        if (this.LineOptions != null)
          return this.LineOptions;
        if (this.Model != null && this.Model.LineOptions != null)
          return this.Model.LineOptions;
        return new ScaleLineOptions();
      }
    }

    internal ScaleElementsInfo LineInfo
    {
      get
      {
        return this.lineInfo;
      }
    }

    internal IEnumerable<ScaleLineInfo> Lines
    {
      get
      {
        if (this.lineInfo != null)
        {
          foreach (object obj in (Collection<object>) this.lineInfo.Elements)
            yield return (ScaleLineInfo) obj;
        }
      }
    }

    internal double ValuesRange
    {
      get
      {
        return this.EndValue - this.StartValue;
      }
    }

    internal AnalogGaugeControl Gauge
    {
      get
      {
        return this.Owner as AnalogGaugeControl;
      }
    }

    internal ScaleMapping Mapping
    {
      get
      {
        return this.mapping;
      }
    }

    internal ScaleLayoutControl LayoutControl
    {
      get
      {
        return this.layoutControl;
      }
      set
      {
        this.layoutControl = value;
      }
    }

    internal IEnumerable<ScaleLabelInfo> Labels
    {
      get
      {
        if (this.labelsInfo != null)
        {
          foreach (object obj in (Collection<object>) this.labelsInfo.Elements)
          {
            if (obj is ScaleLabelInfo)
              yield return (ScaleLabelInfo) obj;
          }
        }
      }
    }

    internal IEnumerable<MinorTickmarkInfo> MinorTickmarks
    {
      get
      {
        if (this.minorTickmarksInfo != null)
        {
          foreach (object obj in (Collection<object>) this.minorTickmarksInfo.Elements)
            yield return (MinorTickmarkInfo) obj;
        }
      }
    }

    internal IEnumerable<MajorTickmarkInfo> MajorTickmarks
    {
      get
      {
        if (this.majorTickmarksInfo != null)
        {
          foreach (object obj in (Collection<object>) this.majorTickmarksInfo.Elements)
            yield return (MajorTickmarkInfo) obj;
        }
      }
    }

    internal ScaleElementsInfo MinorTickmarksInfo
    {
      get
      {
        return this.minorTickmarksInfo;
      }
    }

    internal ScaleElementsInfo MajorTickmarksInfo
    {
      get
      {
        return this.majorTickmarksInfo;
      }
    }

    internal ScaleElementsInfo LabelsInfo
    {
      get
      {
        return this.labelsInfo;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns a list of predefined presentations for labels.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Collections.Generic.List`1"/> of <see cref="T:DevExpress.Xpf.Gauges.PredefinedElementKind"/> objects.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ScalePredefinedLabelPresentations")]
    public static List<PredefinedElementKind> PredefinedLabelPresentations
    {
      get
      {
        return PredefinedScaleLabelPresentations.PresentationKinds;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Contains the list of predefined presentations for scale tickmarks.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Collections.Generic.List`1"/> of <see cref="T:DevExpress.Xpf.Gauges.PredefinedElementKind"/> objects.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("ScalePredefinedTickmarksPresentations")]
    public static List<PredefinedElementKind> PredefinedTickmarksPresentations
    {
      get
      {
        return DevExpress.Xpf.Gauges.Native.PredefinedTickmarksPresentations.PresentationKinds;
      }
    }

    object IHitTestableElement.Element
    {
      get
      {
        return (object) this;
      }
    }

    object IHitTestableElement.Parent
    {
      get
      {
        return (object) null;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the Scale class with default settings.
    /// 
    /// </para>
    /// 
    /// </summary>
    public Scale()
    {
      this.lineInfo = new ScaleElementsInfo(this, this.ActualLineOptions.ZIndex);
      this.labelsInfo = new ScaleElementsInfo(this, this.ActualLabelOptions.ZIndex);
      this.SetValue(Scale.CustomLabelsPropertyKey, (object) new ScaleCustomLabelCollection(this));
      this.SetValue(Scale.CustomElementsPropertyKey, (object) new ScaleCustomElementCollection(this));
      this.minorTickmarksInfo = new ScaleElementsInfo(this, this.ActualMinorTickmarkOptions.ZIndex);
      this.majorTickmarksInfo = new ScaleElementsInfo(this, this.ActualMajorTickmarkOptions.ZIndex);
      this.LayoutUpdated += new EventHandler(this.ScaleLayoutUpdated);
    }

    private static void CustomLabelsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      Scale scale = d as Scale;
      if (scale == null)
        return;
      ScaleCustomLabelCollection customLabelCollection1 = e.OldValue as ScaleCustomLabelCollection;
      if (customLabelCollection1 != null)
        customLabelCollection1.CollectionChanged -= new NotifyCollectionChangedEventHandler(scale.CustomLabelsCollectionChanged);
      ScaleCustomLabelCollection customLabelCollection2 = e.NewValue as ScaleCustomLabelCollection;
      if (customLabelCollection2 == null)
        return;
      customLabelCollection2.CollectionChanged += new NotifyCollectionChangedEventHandler(scale.CustomLabelsCollectionChanged);
    }

    private static void CustomElementsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      Scale scale = d as Scale;
      if (scale == null)
        return;
      ScaleCustomElementCollection elementCollection1 = e.OldValue as ScaleCustomElementCollection;
      if (elementCollection1 != null)
        elementCollection1.CollectionChanged -= new NotifyCollectionChangedEventHandler(scale.CustomElementsCollectionChanged);
      ScaleCustomElementCollection elementCollection2 = e.NewValue as ScaleCustomElementCollection;
      if (elementCollection2 == null)
        return;
      elementCollection2.CollectionChanged += new NotifyCollectionChangedEventHandler(scale.CustomElementsCollectionChanged);
    }

    private static void TickmarkOptionsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      Scale scale = d as Scale;
      if (scale == null)
        return;
      CommonUtils.SubscribePropertyChangedWeakEvent((INotifyPropertyChanged) (e.OldValue as TickmarkOptions), (INotifyPropertyChanged) (e.NewValue as TickmarkOptions), (IWeakEventListener) scale);
      scale.OnTickmarkChanged();
    }

    private static bool MajorIntervalCountValidation(object value)
    {
      return (int) value > 0;
    }

    private static bool MinorIntervalCountValidation(object value)
    {
      return (int) value > 0;
    }

    protected static void UpdateElements(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      Scale scale = d as Scale;
      if (scale == null)
        return;
      scale.UpdateElementsInfo();
    }

    protected static void InvalidateLayout(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      Scale scale = d as Scale;
      if (scale == null)
        return;
      scale.Invalidate();
    }

    protected static void PresentationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      Scale scale = d as Scale;
      if (scale == null || object.Equals(e.NewValue, e.OldValue))
        return;
      scale.UpdateModel();
    }

    protected static void LabelOptionsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      Scale scale = d as Scale;
      if (scale == null)
        return;
      CommonUtils.SubscribePropertyChangedWeakEvent((INotifyPropertyChanged) (e.OldValue as ScaleLabelOptions), (INotifyPropertyChanged) (e.NewValue as ScaleLabelOptions), (IWeakEventListener) scale);
      scale.OnLabelChanged();
    }

    private static void LineOptionsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      Scale scale = d as Scale;
      if (scale == null)
        return;
      CommonUtils.SubscribePropertyChangedWeakEvent((INotifyPropertyChanged) (e.OldValue as ScaleLineOptions), (INotifyPropertyChanged) (e.NewValue as ScaleLineOptions), (IWeakEventListener) scale);
      scale.OnLineOptionsChanged();
    }

    void IModelSupported.UpdateModel()
    {
      this.UpdateModel();
    }

    bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
    {
      return this.PerformWeakEvent(managerType, sender, e);
    }

    ElementLayout ILayoutCalculator.CreateLayout(Size constraint)
    {
      if (this.Mapping == null)
        return (ElementLayout) null;
      return new ElementLayout(this.Mapping.Layout.InitialBounds.Width, this.Mapping.Layout.InitialBounds.Height);
    }

    void ILayoutCalculator.CompleteLayout(ElementInfoBase elementInfo)
    {
      elementInfo.Layout.CompleteLayout(this.GetLayoutOffset(), (Transform) null, (Geometry) null);
    }

    private void UpdateMajorTickmarksInfo(ScaleElementsInfo tickmarksInfo)
    {
      if (tickmarksInfo == null)
        return;
      tickmarksInfo.Elements.Clear();
      double num1 = this.MajorIntervalCount > 0 ? (this.EndValue - this.StartValue) / (double) this.MajorIntervalCount : this.EndValue - this.StartValue;
      double num2 = this.MajorIntervalCount > 0 ? 1.0 / (double) this.MajorIntervalCount : 0.0;
      for (int index = 0; index <= this.MajorIntervalCount; ++index)
      {
        MajorTickmarkInfo majorTickmarkInfo = new MajorTickmarkInfo(this.ActualTickmarksPresentation.CreateMajorTickPresentationControl(), (PresentationBase) this.ActualTickmarksPresentation, (double) index * num2, this.StartValue + (double) index * num1, index == 0, index == this.MajorIntervalCount);
        tickmarksInfo.Elements.Add((object) majorTickmarkInfo);
      }
    }

    private void UpdateMinorTickmarksInfo(ScaleElementsInfo tickmarksInfo)
    {
      if (tickmarksInfo == null)
        return;
      tickmarksInfo.Elements.Clear();
      int num1 = this.MajorIntervalCount * this.MinorIntervalCount + 1;
      double num2 = num1 > 1 ? 1.0 / (double) (num1 - 1) : 1.0;
      for (int index = 0; index < num1; ++index)
      {
        PresentationControl presentationControl = this.ActualTickmarksPresentation.CreateMinorTickPresentationControl();
        tickmarksInfo.Elements.Add((object) new MinorTickmarkInfo(presentationControl, (PresentationBase) this.ActualTickmarksPresentation, (double) index * num2, index % this.MinorIntervalCount == 0));
      }
    }

    private void UpdateLineInfo()
    {
      if (this.lineInfo == null)
        return;
      this.lineInfo.Elements.Clear();
      this.lineInfo.Elements.Add((object) new ScaleLineInfo(this.ActualLinePresentation.CreateLinePresentationControl(), (PresentationBase) this.ActualLinePresentation));
    }

    private void OnTickmarkChanged()
    {
      if (this.minorTickmarksInfo == null || this.majorTickmarksInfo == null)
        return;
      this.majorTickmarksInfo.ZIndex = this.ActualMajorTickmarkOptions.ZIndex;
      this.Invalidate();
    }

    private void OnLineOptionsChanged()
    {
      if (this.lineInfo == null)
        return;
      this.lineInfo.ZIndex = this.ActualLineOptions.ZIndex;
      this.Invalidate();
    }

    private void UpdateLabelsInfo()
    {
      if (this.labelsInfo == null)
        return;
      this.labelsInfo.Elements.Clear();
      foreach (MajorTickmarkInfo tickmark in this.MajorTickmarks)
      {
        PresentationControl presentationControl = this.ActualLabelPresentation.CreateLabelPresentationControl();
        double num = tickmark.Value * this.ActualLabelOptions.Multiplier + this.ActualLabelOptions.Addend;
        this.labelsInfo.Elements.Add((object) new ScaleLabelInfo(presentationControl, (PresentationBase) this.ActualLabelPresentation, tickmark, string.Format(this.ActualLabelOptions.FormatString, (object) num)));
      }
    }

    private void OnLabelChanged()
    {
      if (this.labelsInfo == null)
        return;
      this.labelsInfo.ZIndex = this.ActualLabelOptions.ZIndex;
      foreach (ScaleLabelInfo scaleLabelInfo in this.Labels)
      {
        double num = scaleLabelInfo.Tickmark.Value * this.ActualLabelOptions.Multiplier + this.ActualLabelOptions.Addend;
        scaleLabelInfo.Text = string.Format(this.ActualLabelOptions.FormatString, (object) num);
      }
      this.Invalidate();
    }

    private void CustomLabelsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (sender is ScaleCustomLabelCollection && (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace) && e.NewItems != null)
      {
        foreach (ScaleCustomLabel scaleCustomLabel in (IEnumerable) e.NewItems)
        {
          if (scaleCustomLabel != null)
            scaleCustomLabel.Owner = (object) this;
        }
      }
      this.UpdateElementsInfo();
    }

    private void CustomElementsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (sender is ScaleCustomElementCollection && (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace) && e.NewItems != null)
      {
        foreach (ScaleCustomElement scaleCustomElement in (IEnumerable) e.NewItems)
        {
          if (scaleCustomElement != null)
            scaleCustomElement.Owner = (object) this;
        }
      }
      this.UpdateElementsInfo();
    }

    private void ScaleLayoutUpdated(object sender, EventArgs e)
    {
      if (!this.shouldElementsUpdate)
        return;
      try
      {
        foreach (IElementInfo elementInfo in this.Elements)
          elementInfo.Invalidate();
      }
      finally
      {
        this.shouldElementsUpdate = false;
      }
    }

    private void CalculateElementsLayout()
    {
      foreach (TickmarkInfo elementInfo in this.MinorTickmarks)
        elementInfo.Layout = this.ActualShowMinorTickmarks ? this.ActualMinorTickmarkOptions.CalculateLayout(elementInfo, this.Mapping) : (ScaleElementLayout) null;
      foreach (TickmarkInfo elementInfo in this.MajorTickmarks)
        elementInfo.Layout = this.ActualShowMajorTickmarks ? this.ActualMajorTickmarkOptions.CalculateLayout(elementInfo, this.Mapping) : (ScaleElementLayout) null;
      foreach (ScaleLabelInfo elementInfo in this.Labels)
        elementInfo.Layout = this.ActualShowLabels ? this.ActualLabelOptions.CalculateLayout(elementInfo, this.Mapping) : (ScaleElementLayout) null;
      foreach (ScaleCustomElement scaleCustomElement in (Collection<ScaleCustomLabel>) this.CustomLabels)
        scaleCustomElement.CalculateLayout(this.Mapping);
      foreach (ScaleCustomElement scaleCustomElement in (Collection<ScaleCustomElement>) this.CustomElements)
        scaleCustomElement.CalculateLayout(this.Mapping);
      foreach (ScaleElementInfoBase scaleElementInfoBase in this.Lines)
        scaleElementInfoBase.Layout = this.CalculateLineLayout();
    }

    protected override void OwnerChanged()
    {
      base.OwnerChanged();
      ((IModelSupported) this).UpdateModel();
    }

    protected override Size ArrangeOverride(Size arrangeBounds)
    {
      this.RequestUpdate();
      return base.ArrangeOverride(arrangeBounds);
    }

    protected virtual bool PerformWeakEvent(Type managerType, object sender, EventArgs e)
    {
      bool flag = false;
      if (managerType == typeof (PropertyChangedWeakEventManager))
      {
        if (sender is TickmarkOptions)
          this.OnTickmarkChanged();
        if (sender is ScaleLabelOptions)
          this.OnLabelChanged();
        if (sender is ScaleLineOptions)
          this.OnLineOptionsChanged();
        flag = true;
      }
      return flag;
    }

    protected abstract ScaleElementLayout CalculateLineLayout();

    protected virtual void UpdateModel()
    {
      ScaleLinePresentation linePresentation = this.ActualLinePresentation;
      foreach (ScaleLineInfo scaleLineInfo in this.Lines)
      {
        scaleLineInfo.Presentation = (PresentationBase) linePresentation;
        scaleLineInfo.PresentationControl = linePresentation.CreateLinePresentationControl();
      }
      TickmarksPresentation tickmarksPresentation = this.ActualTickmarksPresentation;
      foreach (MajorTickmarkInfo majorTickmarkInfo in this.MajorTickmarks)
      {
        majorTickmarkInfo.Presentation = (PresentationBase) tickmarksPresentation;
        majorTickmarkInfo.PresentationControl = tickmarksPresentation.CreateMajorTickPresentationControl();
      }
      foreach (MinorTickmarkInfo minorTickmarkInfo in this.MinorTickmarks)
      {
        minorTickmarkInfo.Presentation = (PresentationBase) tickmarksPresentation;
        minorTickmarkInfo.PresentationControl = tickmarksPresentation.CreateMinorTickPresentationControl();
      }
      this.OnTickmarkChanged();
      this.OnLineOptionsChanged();
      ScaleLabelPresentation labelPresentation = this.ActualLabelPresentation;
      foreach (ScaleElementInfoBase scaleElementInfoBase in this.Labels)
        scaleElementInfoBase.PresentationControl = labelPresentation.CreateLabelPresentationControl();
      this.OnLabelChanged();
      foreach (IModelSupported modelSupported in this.Indicators)
        modelSupported.UpdateModel();
    }

    protected abstract ScaleMapping CalculateMapping(Size constraint);

    protected override AutomationPeer OnCreateAutomationPeer()
    {
      return (AutomationPeer) new ScaleAutomationPeer((FrameworkElement) this);
    }

    protected internal virtual void UpdateElementsInfo()
    {
      this.UpdateMinorTickmarksInfo(this.minorTickmarksInfo);
      this.UpdateMajorTickmarksInfo(this.majorTickmarksInfo);
      this.UpdateLabelsInfo();
      this.UpdateLineInfo();
      this.Invalidate();
    }

    protected internal abstract void CheckIndicatorEnterLeaveRange(ValueIndicatorBase indicator, double oldValue, double newValue);

    internal void AnimateIndicators(bool shouldResetValue)
    {
      foreach (ValueIndicatorBase valueIndicatorBase in this.Indicators)
        valueIndicatorBase.Animate(shouldResetValue);
    }

    internal void AddStoryboard(Storyboard storyboard, int resourceKey)
    {
      if (storyboard == null || this.Resources.Contains((object) resourceKey.ToString()))
        return;
      this.Resources.Add((object) resourceKey.ToString(), (object) storyboard);
    }

    internal void RemoveStoryboard(int resourceKey)
    {
      if (!this.Resources.Contains((object) resourceKey.ToString()))
        return;
      this.Resources.Remove((object) resourceKey.ToString());
    }

    internal double GetValueInPercent(double value)
    {
      double num = value - this.StartValue;
      if (this.ValuesRange == 0.0)
        return 0.0;
      return num / this.ValuesRange;
    }

    internal double GetLimitedValueInPercent(double value)
    {
      return this.GetValueInPercent(this.GetLimitedValue(value));
    }

    internal double GetLimitedValue(double value)
    {
      double num1 = Math.Min(this.StartValue, this.EndValue);
      double num2 = Math.Max(this.StartValue, this.EndValue);
      if (value < num1)
        return num1;
      if (value > num2)
        return num2;
      return value;
    }

    internal ScaleLayout CalculateLayout(Size constraint)
    {
      this.mapping = this.CalculateMapping(constraint);
      this.CalculateElementsLayout();
      this.RequestUpdate();
      return this.mapping.Layout;
    }

    internal Point GetLayoutOffset()
    {
      if (this.LayoutControl == null || this.Gauge == null || this.Gauge.BaseLayoutElement == null)
        return new Point(0.0, 0.0);
      Rect relativeElementRect = LayoutHelper.GetRelativeElementRect((UIElement) this.LayoutControl, this.Gauge.BaseLayoutElement);
      return new Point(relativeElementRect.X, relativeElementRect.Y);
    }

    internal double? GetValueByMousePosition(MouseEventArgs e)
    {
      return this.Mapping.GetValueByPoint(e.GetPosition((IInputElement) this.LayoutControl));
    }

    internal double? GetValueByManipulatorPosition(IManipulator manipulator)
    {
      return this.Mapping.GetValueByPoint(manipulator.GetPosition((IInputElement) this.LayoutControl));
    }

    internal void Invalidate()
    {
      if (this.layoutControl == null)
        return;
      this.layoutControl.InvalidateMeasure();
    }

    internal void RequestUpdate()
    {
      this.shouldElementsUpdate = true;
    }

    internal void ClearAnimation()
    {
      foreach (ValueIndicatorBase valueIndicatorBase in this.Indicators)
        valueIndicatorBase.ClearAnimation();
    }

    [SpecialName]
    bool IHitTestableElement.get_IsHitTestVisible()
    {
      return this.IsHitTestVisible;
    }
  }
}
