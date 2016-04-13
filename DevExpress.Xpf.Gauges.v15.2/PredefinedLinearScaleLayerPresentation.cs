// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.PredefinedLinearScaleLayerPresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public abstract class PredefinedLinearScaleLayerPresentation : LinearScaleLayerPresentation
  {
    public static readonly DependencyProperty FillProperty = DependencyPropertyManager.Register("Fill", typeof (Brush), typeof (PredefinedLinearScaleLayerPresentation), new PropertyMetadata((object) null, new PropertyChangedCallback(PredefinedLinearScaleLayerPresentation.FillPropertyChanged)));

    [Category("Presentation")]
    public Brush Fill
    {
      get
      {
        return (Brush) this.GetValue(PredefinedLinearScaleLayerPresentation.FillProperty);
      }
      set
      {
        this.SetValue(PredefinedLinearScaleLayerPresentation.FillProperty, (object) value);
      }
    }

    protected abstract Brush DefaultFill { get; }

    [Category("Presentation")]
    public Brush ActualFill
    {
      get
      {
        if (this.Fill == null)
          return this.DefaultFill;
        return this.Fill;
      }
    }

    private static void FillPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      PredefinedLinearScaleLayerPresentation layerPresentation = d as PredefinedLinearScaleLayerPresentation;
      if (layerPresentation == null)
        return;
      layerPresentation.ActualFillChanged();
    }

    private void ActualFillChanged()
    {
      this.NotifyPropertyChanged("ActualFill");
    }
  }
}
