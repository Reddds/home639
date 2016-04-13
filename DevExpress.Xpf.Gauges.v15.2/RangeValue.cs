// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.RangeValue
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System.ComponentModel;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// Stores one of the range values.
  /// 
  /// </para>
  /// 
  /// </summary>
  [TypeConverter(typeof (RangeValueConverter))]
  public struct RangeValue
  {
    private double value;
    private RangeValueType type;

    /// <summary>
    /// 
    /// <para>
    /// Returns the value that specifies either the start or end boundary of a range.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:System.Double"/> value that is used to store the range boundaries in either absolute or relative units.
    /// 
    /// </value>
    public double Value
    {
      get
      {
        return this.value;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Indicates whether or not a range boundary's value is set in absolute units.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> if a range value is stored in absolute units; otherwise, <b>false</b>.
    /// 
    /// </value>
    public bool IsAbsolute
    {
      get
      {
        return this.type == RangeValueType.Absolute;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Indicates whether or not a range boundary's value is set as a percent.
    /// 
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// <b>true</b> if a range value is stored as a percent; otherwise, <b>false</b>.
    /// 
    /// 
    /// </value>
    public bool IsPercent
    {
      get
      {
        return this.type == RangeValueType.Percent;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Returns a value specifying in which units to store range boundaries.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// 
    /// <value>
    /// A <see cref="T:DevExpress.Xpf.Gauges.RangeValueType"/> enumeration value that specifies the measurement unit for a range boundary.
    /// 
    /// 
    /// 
    /// </value>
    public RangeValueType RangeValueType
    {
      get
      {
        return this.type;
      }
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the RangeValue class with the specified value.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// <param name="value">A <see cref="T:System.Double"/> object that specifies one of the range values. This value is assigned to the <see cref="P:DevExpress.Xpf.Gauges.RangeValue.Value"/> property.
    /// 
    ///             </param>
    public RangeValue(double value)
    {
      this = new RangeValue(value, RangeValueType.Absolute);
    }

    /// <summary>
    /// 
    /// <para>
    /// Initializes a new instance of the RangeValue class with the specified value and value type.
    /// 
    /// </para>
    /// 
    /// </summary>
    /// <param name="value">A <see cref="T:System.Double"/> object that specifies one of the range values. This value is assigned to the <see cref="P:DevExpress.Xpf.Gauges.RangeValue.Value"/> property.
    /// 
    ///             </param><param name="type">A <see cref="T:DevExpress.Xpf.Gauges.RangeValueType"/> enumeration value that specifies how the assigned value should be treated - as percents or as an absolute value. This parameter is assigned to the <see cref="P:DevExpress.Xpf.Gauges.RangeValue.RangeValueType"/> property.
    /// 
    ///             </param>
    public RangeValue(double value, RangeValueType type)
    {
      this.value = value;
      this.type = type;
    }
  }
}
