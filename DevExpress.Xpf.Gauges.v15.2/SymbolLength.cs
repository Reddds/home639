// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.SymbolLength
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.ComponentModel;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// Contains the values used to specify the length (width or height) of a symbol on the symbols panel.
  /// 
  /// </para>
  /// 
  /// </summary>
  [TypeConverter(typeof (SymbolLengthConverter))]
  public struct SymbolLength
  {
    private SymbolLengthType type;
    private double fixedLength;
    private double proportionalLength;

    /// <summary>
    /// 
    /// <para>
    /// Returns the type of the SymbolLength object.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.SymbolLengthType"/> enumeration value that specifies is the type of the SymbolLength object.
    /// 
    /// </value>
    public SymbolLengthType Type
    {
      get
      {
        return this.type;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to the width and height of symbols that are set in absolute values.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A double value that is the symbol length specified in absolute value.
    /// 
    /// </value>
    public double FixedLength
    {
      get
      {
        return this.fixedLength;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Provides access to the width and height of symbols that are specified in proportional values.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that is the symbols length specified in proportional values.
    /// 
    /// </value>
    public double ProportionalLength
    {
      get
      {
        return this.proportionalLength;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the SymbolLength class with the specified symbol length type and symbol length value.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// <param name="type">A <see cref="T:DevExpress.Xpf.Gauges.SymbolLengthType"/> enumeration value that specifies the type of a symbol length. This value is assigned to the <see cref="P:DevExpress.Xpf.Gauges.SymbolLength.Type"/> property.
    /// 
    /// 
    ///             </param><param name="length">A <see cref="T:System.Double"/> value that specifies the symbol length. This value is assigned to either the <see cref="P:DevExpress.Xpf.Gauges.SymbolLength.FixedLength"/> or <see cref="P:DevExpress.Xpf.Gauges.SymbolLength.ProportionalLength"/> property, depending on the value passed as the <i>type</i> parameter.
    /// 
    /// 
    ///             </param>
    public SymbolLength(SymbolLengthType type, double length)
    {
      this.type = type;
      this.fixedLength = 0.0;
      this.proportionalLength = 0.0;
      switch (type)
      {
        case SymbolLengthType.Fixed:
          this.fixedLength = length;
          break;
        case SymbolLengthType.Proportional:
          this.proportionalLength = length;
          break;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the SymbolLength class with the specified owner.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// <param name="type">A <see cref="T:DevExpress.Xpf.Gauges.SymbolLengthType"/> enumeration value that specifies one of the possible symbol length types. This value is assigned to the <see cref="P:DevExpress.Xpf.Gauges.SymbolLength.Type"/> property.
    /// 
    /// 
    ///             </param>
    public SymbolLength(SymbolLengthType type)
    {
      this = new SymbolLength(type, 0.0);
    }
  }
}
