// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.PredefinedDigitalGaugeLayerPresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// Contains presentation settings for the digital gauge layer element.
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class PredefinedDigitalGaugeLayerPresentation : DigitalGaugeLayerPresentation
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
    public static readonly DependencyProperty FillProperty = DependencyPropertyManager.Register("Fill", typeof (Brush), typeof (PredefinedDigitalGaugeLayerPresentation), new PropertyMetadata((object) null, new PropertyChangedCallback(PredefinedDigitalGaugeLayerPresentation.FillPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Specifies the fill color of the digital gauge layer element.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A  <see cref="T:System.Windows.Media.Brush"/> object that is fill color of the digital gauge layer.
    /// 
    /// </value>
    [Category("Presentation")]
    public Brush Fill
    {
      get
      {
        return (Brush) this.GetValue(PredefinedDigitalGaugeLayerPresentation.FillProperty);
      }
      set
      {
        this.SetValue(PredefinedDigitalGaugeLayerPresentation.FillProperty, (object) value);
      }
    }

    protected abstract Brush DefaultFill { get; }

    /// <summary>
    /// 
    /// <para>
    /// Gets the actual fill color of the digital gauge layer element.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A  <see cref="T:System.Windows.Media.Brush"/> object that is the actual fill color of the digital gauge layer.
    /// 
    /// </value>
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
      PredefinedDigitalGaugeLayerPresentation layerPresentation = d as PredefinedDigitalGaugeLayerPresentation;
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
