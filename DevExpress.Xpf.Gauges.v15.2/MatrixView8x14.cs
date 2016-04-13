// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.MatrixView8x14
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A matrix8x14 symbols panel type of a digital gauge control.
  /// 
  /// 
  /// </para>
  /// 
  /// </summary>
  public class MatrixView8x14 : MatrixView
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
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (Matrix8x14Presentation), typeof (MatrixView8x14), new PropertyMetadata((object) null, new PropertyChangedCallback(SymbolViewBase.PresentationPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the current presentation that specifies the appearance of the 8x14 matrix.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.Matrix8x14Presentation"/> object.
    /// 
    /// </value>
    [Category("Presentation")]
    public Matrix8x14Presentation Presentation
    {
      get
      {
        return (Matrix8x14Presentation) this.GetValue(MatrixView8x14.PresentationProperty);
      }
      set
      {
        this.SetValue(MatrixView8x14.PresentationProperty, (object) value);
      }
    }

    private static void OptionsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      MatrixView8x14 matrixView8x14 = d as MatrixView8x14;
      if (matrixView8x14 == null)
        return;
      CommonUtils.SubscribePropertyChangedWeakEvent((INotifyPropertyChanged) (e.OldValue as SymbolOptions), (INotifyPropertyChanged) (e.NewValue as SymbolOptions), (IWeakEventListener) matrixView8x14);
      matrixView8x14.OnOptionsChanged();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new MatrixView8x14();
    }

    protected internal override SymbolViewInternal CreateInternalView()
    {
      return (SymbolViewInternal) new MatrixView8x14Internal();
    }
  }
}
