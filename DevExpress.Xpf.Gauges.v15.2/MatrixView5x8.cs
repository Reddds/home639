// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.MatrixView5x8
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
  /// A matrix5x8 symbols panel type of digital gauge control.
  /// 
  /// 
  /// </para>
  /// 
  /// </summary>
  public class MatrixView5x8 : MatrixView
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
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (Matrix5x8Presentation), typeof (MatrixView5x8), new PropertyMetadata((object) null, new PropertyChangedCallback(SymbolViewBase.PresentationPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets the current presentation that specifies the appearance of the 5x8 matrix.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.Matrix5x8Presentation"/> object.
    /// 
    /// </value>
    [Category("Presentation")]
    public Matrix5x8Presentation Presentation
    {
      get
      {
        return (Matrix5x8Presentation) this.GetValue(MatrixView5x8.PresentationProperty);
      }
      set
      {
        this.SetValue(MatrixView5x8.PresentationProperty, (object) value);
      }
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new MatrixView5x8();
    }

    protected internal override SymbolViewInternal CreateInternalView()
    {
      return (SymbolViewInternal) new MatrixView5x8Internal();
    }
  }
}
