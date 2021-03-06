﻿// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Matrix8x14Model
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// For internal use.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class Matrix8x14Model : SymbolsModelBase
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
    public static readonly DependencyProperty PresentationProperty = DependencyPropertyManager.Register("Presentation", typeof (Matrix8x14Presentation), typeof (Matrix8x14Model), new PropertyMetadata((object) null, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// For internal use.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value/>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Matrix8x14Presentation Presentation
    {
      get
      {
        return (Matrix8x14Presentation) this.GetValue(Matrix8x14Model.PresentationProperty);
      }
      set
      {
        this.SetValue(Matrix8x14Model.PresentationProperty, (object) value);
      }
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new Matrix8x14Model();
    }
  }
}
