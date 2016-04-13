// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.PredefinedElementKinds
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges;
using System;
using System.Collections.Generic;

namespace DevExpress.Xpf.Gauges.Native
{
  public class PredefinedElementKinds
  {
    private static void Add(List<PredefinedElementKind> kindList, Type type)
    {
      INamedElement namedElement = Activator.CreateInstance(type) as INamedElement;
      if (namedElement == null)
        return;
      kindList.Add(new PredefinedElementKind(type, namedElement.Name));
    }

    protected static void FillKindList(List<PredefinedElementKind> kindList, Type baseType)
    {
      foreach (Type type in baseType.Assembly.GetTypes())
      {
        if (!type.IsAbstract && type.IsSubclassOf(baseType))
          PredefinedElementKinds.Add(kindList, type);
      }
    }
  }
}
