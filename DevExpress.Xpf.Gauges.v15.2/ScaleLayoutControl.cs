// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ScaleLayoutControl
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  [NonCategorized]
  public class ScaleLayoutControl : Control
  {
    public static readonly DependencyProperty ScaleProperty = DependencyPropertyManager.Register("Scale", typeof (Scale), typeof (ScaleLayoutControl), new PropertyMetadata(new PropertyChangedCallback(ScaleLayoutControl.ScalePropertyChanged)));
    private const double defaultWidth = 300.0;
    private const double defaultHeight = 300.0;

    public Scale Scale
    {
      get
      {
        return (Scale) this.GetValue(ScaleLayoutControl.ScaleProperty);
      }
      set
      {
        this.SetValue(ScaleLayoutControl.ScaleProperty, (object) value);
      }
    }

    public ScaleLayoutControl()
    {
      this.DefaultStyleKey = (object) typeof (ScaleLayoutControl);
    }

    private static void ScalePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      Scale scale1 = e.NewValue as Scale;
      if (scale1 != null)
        scale1.LayoutControl = d as ScaleLayoutControl;
      Scale scale2 = (Scale) (e.OldValue as ArcScale);
      if (scale2 == null)
        return;
      scale2.LayoutControl = (ScaleLayoutControl) null;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
      Size constraint = new Size(double.IsInfinity(availableSize.Width) ? 300.0 : availableSize.Width, double.IsInfinity(availableSize.Height) ? 300.0 : availableSize.Height);
      if (this.Scale != null)
      {
        ScaleLayout scaleLayout = this.Scale.CalculateLayout(constraint);
        this.Clip = scaleLayout.Clip;
        constraint = new Size(scaleLayout.InitialBounds.Width, scaleLayout.InitialBounds.Height);
      }
      else
        this.Clip = (Geometry) null;
      return constraint;
    }
  }
}
