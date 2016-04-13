// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.ILayoutCalculator
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges;
using System.Windows;

namespace DevExpress.Xpf.Gauges.Native
{
  public interface ILayoutCalculator
  {
    ElementLayout CreateLayout(Size constraint);

    void CompleteLayout(ElementInfoBase elementInfo);
  }
}
