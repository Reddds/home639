// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.PresentationToFillConverter
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class PresentationToFillConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      IDefaultSymbolPresentation symbolPresentation = value as IDefaultSymbolPresentation;
      PresentationToFillConverter.SegmentType result;
      if (!(targetType == typeof (Brush)) || parameter == null || (symbolPresentation == null || !Enum.TryParse<PresentationToFillConverter.SegmentType>(parameter.ToString(), true, out result)))
        return (object) null;
      if (result != PresentationToFillConverter.SegmentType.Active)
        return (object) symbolPresentation.ActualFillInactive;
      return (object) symbolPresentation.ActualFillActive;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    private enum SegmentType
    {
      Active,
      Inactive,
    }
  }
}
