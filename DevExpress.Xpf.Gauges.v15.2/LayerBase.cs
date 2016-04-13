// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LayerBase
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
  /// Serves as the base class for all layers.
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class LayerBase : GaugeDependencyObject, IOwnedElement, IModelSupported, IWeakEventListener, ILayoutCalculator
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
    public static readonly DependencyProperty VisibleProperty = DependencyPropertyManager.Register("Visible", typeof (bool), typeof (LayerBase), new PropertyMetadata((object) true, new PropertyChangedCallback(LayerBase.VisiblePropertyChanged)));
    private readonly LayerInfo info;
    private object owner;

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets whether the layer is visible.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> if the layer is visible; otherwise, <b>false</b>.
    /// 
    /// </value>
    [Category("Behavior")]
    [DevExpressXpfGaugesLocalizedDescription("LayerBaseVisible")]
    public bool Visible
    {
      get
      {
        return (bool) this.GetValue(LayerBase.VisibleProperty);
      }
      set
      {
        this.SetValue(LayerBase.VisibleProperty, (object) (bool) (value ? 1 : 0));
      }
    }

    protected object Owner
    {
      get
      {
        return this.owner;
      }
    }

    protected abstract int ActualZIndex { get; }

    protected abstract LayerPresentation ActualPresentation { get; }

    internal virtual LayerInfo ElementInfo
    {
      get
      {
        return this.info;
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
        this.ChangeOwner();
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the LayerBase class with default settings.
    /// 
    /// </para>
    /// 
    /// </summary>
    public LayerBase()
    {
      this.info = new LayerInfo((ILayoutCalculator) this, this.ActualZIndex, this.ActualPresentation.CreateLayerPresentationControl(), (PresentationBase) this.ActualPresentation);
    }

    private static void VisiblePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      LayerBase layerBase = d as LayerBase;
      if (layerBase == null)
        return;
      layerBase.Invalidate();
    }

    protected static void PresentationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      LayerBase layerBase = d as LayerBase;
      if (layerBase == null || object.Equals(e.NewValue, e.OldValue))
        return;
      ((IModelSupported) layerBase).UpdateModel();
    }

    protected static void OptionsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      LayerBase layerBase = d as LayerBase;
      if (layerBase == null)
        return;
      CommonUtils.SubscribePropertyChangedWeakEvent((INotifyPropertyChanged) (e.OldValue as GaugeDependencyObject), (INotifyPropertyChanged) (e.NewValue as GaugeDependencyObject), (IWeakEventListener) layerBase);
      layerBase.OnOptionsChanged();
    }

    private void ChangeOwner()
    {
      ((IModelSupported) this).UpdateModel();
    }

    void IModelSupported.UpdateModel()
    {
      this.UpdatePresentation();
      this.OnOptionsChanged();
    }

    bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
    {
      bool flag = false;
      if (managerType == typeof (PropertyChangedWeakEventManager))
      {
        if (sender is GaugeDependencyObject)
          this.OnOptionsChanged();
        flag = true;
      }
      return flag;
    }

    ElementLayout ILayoutCalculator.CreateLayout(Size constraint)
    {
      if (!this.Visible)
        return (ElementLayout) null;
      return this.CreateLayout(constraint);
    }

    void ILayoutCalculator.CompleteLayout(ElementInfoBase elementInfo)
    {
      this.CompleteLayout(elementInfo);
    }

    protected void Invalidate()
    {
      if (this.ElementInfo == null)
        return;
      this.ElementInfo.Invalidate();
    }

    protected void UpdatePresentation()
    {
      if (this.ElementInfo == null)
        return;
      this.ElementInfo.Presentation = (PresentationBase) this.ActualPresentation;
      this.ElementInfo.PresentationControl = this.ActualPresentation.CreateLayerPresentationControl();
    }

    protected virtual void OnOptionsChanged()
    {
      if (this.ElementInfo != null)
        this.ElementInfo.ZIndex = this.ActualZIndex;
      this.Invalidate();
    }

    protected abstract ElementLayout CreateLayout(Size constraint);

    protected abstract void CompleteLayout(ElementInfoBase elementInfo);
  }
}
