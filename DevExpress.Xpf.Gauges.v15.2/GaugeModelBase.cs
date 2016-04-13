// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.GaugeModelBase
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  public abstract class GaugeModelBase : GaugeElement, IWeakEventListener, INamedElement
  {
    public static readonly DependencyProperty InnerPaddingProperty = DependencyPropertyManager.Register("InnerPadding", typeof (Thickness), typeof (GaugeModelBase), new PropertyMetadata((object) new Thickness(0.0)));

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Thickness InnerPadding
    {
      get
      {
        return (Thickness) this.GetValue(GaugeModelBase.InnerPaddingProperty);
      }
      set
      {
        this.SetValue(GaugeModelBase.InnerPaddingProperty, (object) value);
      }
    }

    private IModelSupported ModelHolder
    {
      get
      {
        return this.Owner as IModelSupported;
      }
    }

    public abstract string ModelName { get; }

    string INamedElement.Name
    {
      get
      {
        return this.ModelName;
      }
    }

    protected static void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      GaugeModelBase gaugeModelBase = d as GaugeModelBase;
      if (gaugeModelBase == null || gaugeModelBase.ModelHolder == null)
        return;
      CommonUtils.SubscribePropertyChangedWeakEvent(e.OldValue as INotifyPropertyChanged, e.NewValue as INotifyPropertyChanged, d as IWeakEventListener);
      gaugeModelBase.ModelHolder.UpdateModel();
    }

    protected static void CollectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      GaugeModelBase gaugeModelBase = d as GaugeModelBase;
      if (gaugeModelBase == null)
        return;
      CommonUtils.SubscribePropertyChangedWeakEvent(e.OldValue as INotifyPropertyChanged, e.NewValue as INotifyPropertyChanged, d as IWeakEventListener);
      if (gaugeModelBase.ModelHolder == null)
        return;
      gaugeModelBase.ModelHolder.UpdateModel();
    }

    bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
    {
      return this.PerformWeakEvent(managerType, sender, e);
    }

    private bool PerformWeakEvent(Type managerType, object sender, EventArgs e)
    {
      bool flag = false;
      if (managerType == typeof (PropertyChangedWeakEventManager))
      {
        if (this.ModelHolder != null)
          this.ModelHolder.UpdateModel();
        flag = true;
      }
      return flag;
    }

    protected ModelBase GetModel(DependencyProperty dp, int index)
    {
      IList list = this.GetValue(dp) as IList;
      if (list == null || list.Count <= 0)
        return (ModelBase) null;
      if (index < list.Count)
        return list[index] as ModelBase;
      return list[list.Count - 1] as ModelBase;
    }
  }
}
