// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.AnimationBase
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
  /// A base class for all animation available in the DXGauge Suite.
  /// 
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class AnimationBase : GaugeDependencyObject
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
    public static readonly DependencyProperty EnableProperty = DependencyPropertyManager.Register("Enable", typeof (bool), typeof (AnimationBase), new PropertyMetadata((object) true, new PropertyChangedCallback(GaugeDependencyObject.NotifyPropertyChanged)));

    /// <summary>
    /// 
    /// <para>
    /// Gets or sets a value specifying whether the digital gauge control should be animated using creeping line or blinking animation.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> to enable animation; otherwise, <b>false</b>.
    /// 
    /// </value>
    [Category("Behavior")]
    public bool Enable
    {
      get
      {
        return (bool) this.GetValue(AnimationBase.EnableProperty);
      }
      set
      {
        this.SetValue(AnimationBase.EnableProperty, (object) (bool) (value ? 1 : 0));
      }
    }
  }
}
