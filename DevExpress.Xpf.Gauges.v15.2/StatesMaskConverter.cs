// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.StatesMaskConverter
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System;
using System.ComponentModel;
using System.Globalization;

namespace DevExpress.Xpf.Gauges
{
  public class StatesMaskConverter : TypeConverter
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
      if (str != null)
      {
        string[] strArray = str.Split(new string[3]
        {
          " ",
          ",",
          ", "
        }, StringSplitOptions.RemoveEmptyEntries);
        if (strArray.Length > 0)
        {
          bool[] flagArray = new bool[strArray.Length];
          for (int index = 0; index < strArray.Length; ++index)
          {
            int result;
            if (int.TryParse(strArray[index], out result))
              flagArray[index] = result != 0;
          }
          return (object) new StatesMask(flagArray);
        }
      }
      return (object) null;
    }

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
      if (destinationType == typeof (string))
        return true;
      return base.CanConvertTo(context, destinationType);
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
      if (value is StatesMask)
      {
        StatesMask statesMask = (StatesMask) value;
        if (destinationType == typeof (string))
        {
          string str = "";
          foreach (bool flag in statesMask.States)
            str = str + (str != "" ? " " : "") + (flag ? "1" : "0");
          return (object) str;
        }
      }
      return base.ConvertTo(context, culture, value, destinationType);
    }
  }
}
