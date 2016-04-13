// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.SymbolStateToVisibilityConverter
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DevExpress.Xpf.Gauges
{
  public class SymbolStateToVisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (targetType == typeof (Visibility))
      {
        SymbolState symbolState = value as SymbolState;
        bool flag = false;
        int result;
        if (symbolState != null && int.TryParse(parameter.ToString(), out result))
        {
          if (result < symbolState.Segments.Length)
            flag = symbolState.Segments[result];
          return (object) (Visibility) (flag ? 0 : 2);
        }
      }
      return (object) Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is Visibility && targetType == typeof (bool))
        return (object) (bool) ((Visibility) value == Visibility.Visible ? 1 : 0);
      return (object) null;
    }
  }
}
