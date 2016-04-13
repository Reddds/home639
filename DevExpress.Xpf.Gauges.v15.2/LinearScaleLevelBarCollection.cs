﻿// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.LinearScaleLevelBarCollection
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// A collection that stores the level bars of a particular linear scale.
  /// 
  /// 
  /// 
  /// </para>
  /// 
  /// </summary>
  public class LinearScaleLevelBarCollection : LinearScaleIndicatorCollection<LinearScaleLevelBar>
  {
    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the LinearScaleLevelBarCollection class with the specified owner.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// <param name="scale">A <see cref="T:DevExpress.Xpf.Gauges.LinearScale"/> object that should be the owner of the created collection.
    /// 
    ///             </param>
    public LinearScaleLevelBarCollection(LinearScale scale)
      : base(scale)
    {
    }
  }
}
