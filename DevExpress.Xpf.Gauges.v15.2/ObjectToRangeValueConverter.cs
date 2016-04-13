// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ObjectToRangeValueConverter
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System;
using System.Globalization;
using System.Windows.Data;

namespace DevExpress.Xpf.Gauges
{
  public class ObjectToRangeValueConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (!(targetType == typeof (RangeValue)))
        return (object) null;
      string str = value as string;
      if (str == null)
        return (object) new RangeValue(Convert.ToDouble(value, (IFormatProvider) culture));
      if (str.EndsWith("%"))
        return (object) new RangeValue(Convert.ToDouble(str.Substring(0, str.Length - 1), (IFormatProvider) culture), RangeValueType.Percent);
      return (object) new RangeValue(Convert.ToDouble(str, (IFormatProvider) culture));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (!(value is RangeValue))
        return (object) null;
      if (!(targetType == typeof (string)))
        return (object) ((RangeValue) value).Value;
      string str = ((RangeValue) value).Value.ToString();
      if (!((RangeValue) value).IsPercent)
        return (object) str;
      return (object) (str + "%");
    }
  }
}
