// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.SymbolViewBase
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A base class for all symbol view types of a digital gauge control.
  /// 
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class SymbolViewBase : GaugeDependencyObject, IOwnedElement, IWeakEventListener
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
    public static readonly DependencyProperty AnimationProperty = DependencyPropertyManager.Register("Animation", typeof (SymbolsAnimation), typeof (SymbolViewBase), new PropertyMetadata((object) null, new PropertyChangedCallback(SymbolViewBase.AnimationPropertyChanged)));
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
    public static readonly DependencyProperty OptionsProperty = DependencyPropertyManager.Register("Options", typeof (SymbolOptions), typeof (SymbolViewBase), new PropertyMetadata(new PropertyChangedCallback(SymbolViewBase.OptionsPropertyChanged)));
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
    public static readonly DependencyProperty HeightProperty = DependencyPropertyManager.Register("Height", typeof (SymbolLength), typeof (SymbolViewBase), new PropertyMetadata((object) new SymbolLength(SymbolLengthType.Auto), new PropertyChangedCallback(SymbolViewBase.SymbolSizePropertyChanged)));
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
    public static readonly DependencyProperty WidthProperty = DependencyPropertyManager.Register("Width", typeof (SymbolLength), typeof (SymbolViewBase), new PropertyMetadata((object) new SymbolLength(SymbolLengthType.Auto), new PropertyChangedCallback(SymbolViewBase.SymbolSizePropertyChanged)));
    internal static readonly DependencyPropertyKey CustomSymbolMappingPropertyKey = DependencyPropertyManager.RegisterReadOnly("CustomSymbolMapping", typeof (SymbolDictionary), typeof (SymbolViewBase), new PropertyMetadata());
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
    public static readonly DependencyProperty CustomSymbolMappingProperty = SymbolViewBase.CustomSymbolMappingPropertyKey.DependencyProperty;
    private object owner;

    /// <summary>
    /// 
    /// <para>
    /// Provides access to the animation object that allows you to customize animation for the current symbol view type.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.SymbolsAnimation"/> class descendant.
    /// 
    /// 
    /// </value>
    [Category("Presentation")]
    public SymbolsAnimation Animation
    {
      get
      {
        return (SymbolsAnimation) this.GetValue(SymbolViewBase.AnimationProperty);
      }
      set
      {
        this.SetValue(SymbolViewBase.AnimationProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to the settings that specify the symbol view position on the symbols panel.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.SymbolOptions"/> object that contains the settings of the symbol view type.
    /// 
    /// </value>
    [Category("Presentation")]
    public SymbolOptions Options
    {
      get
      {
        return (SymbolOptions) this.GetValue(SymbolViewBase.OptionsProperty);
      }
      set
      {
        this.SetValue(SymbolViewBase.OptionsProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Specifies the symbol's width for the current symbol view.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.SymbolLength"/> object  that is the symbol's width.
    /// 
    /// </value>
    [Category("Layout")]
    public SymbolLength Width
    {
      get
      {
        return (SymbolLength) this.GetValue(SymbolViewBase.WidthProperty);
      }
      set
      {
        this.SetValue(SymbolViewBase.WidthProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Specifies the symbol's height for the current symbol view.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.SymbolLength"/> object that is the symbol's height.
    /// 
    /// </value>
    [Category("Layout")]
    public SymbolLength Height
    {
      get
      {
        return (SymbolLength) this.GetValue(SymbolViewBase.HeightProperty);
      }
      set
      {
        this.SetValue(SymbolViewBase.HeightProperty, (object) value);
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides  elements that are used in custom symbol mapping.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.SymbolDictionary"/> object that stores elements for custom symbol mapping.
    /// 
    /// </value>
    [Category("Data")]
    public SymbolDictionary CustomSymbolMapping
    {
      get
      {
        return (SymbolDictionary) this.GetValue(SymbolViewBase.CustomSymbolMappingProperty);
      }
    }

    internal DigitalGaugeControl Gauge
    {
      get
      {
        return this.owner as DigitalGaugeControl;
      }
    }

    object IOwnedElement.Owner
    {
      get
      {
        return this.owner;
      }
      set
      {
        this.owner = value;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the SymbolViewBase class with default settings.
    /// 
    /// </para>
    /// 
    /// </summary>
    public SymbolViewBase()
    {
      this.SetValue(SymbolViewBase.CustomSymbolMappingPropertyKey, (object) new SymbolDictionary(this));
    }

    private static void AnimationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      SymbolViewBase symbolViewBase = d as SymbolViewBase;
      if (symbolViewBase == null)
        return;
      CommonUtils.SubscribePropertyChangedWeakEvent((INotifyPropertyChanged) (e.OldValue as SymbolsAnimation), (INotifyPropertyChanged) (e.NewValue as SymbolsAnimation), (IWeakEventListener) symbolViewBase);
      symbolViewBase.OnAnimationChanged();
    }

    private static void OptionsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      SymbolViewBase symbolViewBase = d as SymbolViewBase;
      if (symbolViewBase == null)
        return;
      CommonUtils.SubscribePropertyChangedWeakEvent((INotifyPropertyChanged) (e.OldValue as SymbolOptions), (INotifyPropertyChanged) (e.NewValue as SymbolOptions), (IWeakEventListener) symbolViewBase);
      symbolViewBase.OnOptionsChanged();
    }

    private static void SymbolSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      SymbolViewBase symbolViewBase = d as SymbolViewBase;
      if (symbolViewBase == null || symbolViewBase.Gauge == null)
        return;
      symbolViewBase.Gauge.InvalidateLayout();
    }

    protected static void PresentationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      SymbolViewBase symbolViewBase = d as SymbolViewBase;
      if (symbolViewBase == null || symbolViewBase.Gauge == null || object.Equals(e.NewValue, e.OldValue))
        return;
      symbolViewBase.Gauge.UpdateSymbolsModels();
    }

    bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
    {
      return this.PerformWeakEvent(managerType, sender, e);
    }

    private void OnAnimationChanged()
    {
      if (this.Gauge == null)
        return;
      this.Gauge.Animate();
    }

    protected void OnOptionsChanged()
    {
      if (this.Gauge == null)
        return;
      this.Gauge.ViewOptionsChanged();
    }

    protected virtual bool PerformWeakEvent(Type managerType, object sender, EventArgs e)
    {
      bool flag = false;
      if (managerType == typeof (PropertyChangedWeakEventManager))
      {
        if (sender is SymbolsAnimation)
        {
          this.OnAnimationChanged();
          flag = true;
        }
        else if (sender is GaugeDependencyObject)
        {
          this.OnOptionsChanged();
          flag = true;
        }
      }
      return flag;
    }

    protected internal abstract SymbolViewInternal CreateInternalView();
  }
}
