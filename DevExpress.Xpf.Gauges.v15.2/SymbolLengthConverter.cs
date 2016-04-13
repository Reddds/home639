// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.SymbolLengthConverter
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System;
using System.ComponentModel;
using System.Globalization;

namespace DevExpress.Xpf.Gauges
{
  public class SymbolLengthConverter : TypeConverter
  {
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
      if (sourceType == typeof (string))
        return true;
      return base.CanConvertFrom(context, sourceType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
      string str = value as string;
      if (str == null)
        return (object) null;
      if (str == "Auto")
        return (object) new SymbolLength(SymbolLengthType.Auto);
      if (str == "Stretch")
        return (object) new SymbolLength(SymbolLengthType.Stretch);
      if (str == "*")
        return (object) new SymbolLength(SymbolLengthType.Proportional, 1.0);
      if (str.EndsWith("*"))
        return (object) new SymbolLength(SymbolLengthType.Proportional, Convert.ToDouble(str.Substring(0, str.Length - 1), (IFormatProvider) culture));
      return (object) new SymbolLength(SymbolLengthType.Fixed, Convert.ToDouble(str, (IFormatProvider) culture));
    }

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
      if (destinationType == typeof (string))
        return true;
      return base.CanConvertTo(context, destinationType);
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
      if (value is SymbolLength)
      {
        SymbolLength symbolLength = (SymbolLength) value;
        if (destinationType == typeof (string))
        {
          string str = "";
          switch (symbolLength.Type)
          {
            case SymbolLengthType.Auto:
              str = "Auto";
              break;
            case SymbolLengthType.Stretch:
              str = "Stretch";
              break;
            case SymbolLengthType.Fixed:
              str = symbolLength.FixedLength.ToString((IFormatProvider) culture);
              break;
            case SymbolLengthType.Proportional:
              str = symbolLength.ProportionalLength == 1.0 ? "*" : symbolLength.ProportionalLength.ToString((IFormatProvider) culture) + "*";
              break;
          }
          return (object) str;
        }
      }
      return base.ConvertTo(context, culture, value, destinationType);
    }
  }
}
