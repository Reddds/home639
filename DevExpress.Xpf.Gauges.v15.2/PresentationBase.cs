// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.PresentationBase
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A base for all classes that contain presentation settings.
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class PresentationBase : GaugeDependencyObject, INamedElement
  {
    /// <summary>
    /// 
    /// <para>
    /// Returns the human-readable name of the current presentation.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.String"/> value which is the presentation name.
    /// 
    /// </value>
    public abstract string PresentationName { get; }

    string INamedElement.Name
    {
      get
      {
        return this.PresentationName;
      }
    }
  }
}
