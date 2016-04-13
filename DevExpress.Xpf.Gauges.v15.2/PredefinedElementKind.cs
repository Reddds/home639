// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.PredefinedElementKind
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// Defines the kind of a predefined element.
  /// 
  /// </para>
  /// 
  /// </summary>
  public class PredefinedElementKind
  {
    private Type type;
    private string name;

    /// <summary>
    /// 
    /// <para>
    /// Returns the type of the predefined element.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Type"/> that is the element type.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("PredefinedElementKindType")]
    public Type Type
    {
      get
      {
        return this.type;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns the name of the predefined element.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.String"/> that is the element name.
    /// 
    /// </value>
    [DevExpressXpfGaugesLocalizedDescription("PredefinedElementKindName")]
    public string Name
    {
      get
      {
        return this.name;
      }
    }

    internal PredefinedElementKind(Type type, string name)
    {
      this.type = type;
      this.name = name;
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns the textual representation of the PredefinedElementKind object.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <returns>
    /// A <see cref="T:System.String"/> value, which is the textual representation of the element kind.
    /// 
    /// </returns>
    public override string ToString()
    {
      return this.name;
    }
  }
}
