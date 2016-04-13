// Decompiled with JetBrains decompiler
// Type: DevExpressXpfGaugesLocalizedDescriptionAttribute
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using System;
using System.ComponentModel;
using System.Resources;

[AttributeUsage(AttributeTargets.All)]
internal class DevExpressXpfGaugesLocalizedDescriptionAttribute : DescriptionAttribute
{
  private static ResourceManager rm;
  private bool loaded;

  public override string Description
  {
    get
    {
      if (!this.loaded)
      {
        this.loaded = true;
        if (DevExpressXpfGaugesLocalizedDescriptionAttribute.rm == null)
        {
          lock (typeof (DevExpressXpfGaugesLocalizedDescriptionAttribute))
          {
            if (DevExpressXpfGaugesLocalizedDescriptionAttribute.rm == null)
              DevExpressXpfGaugesLocalizedDescriptionAttribute.rm = new ResourceManager("DevExpress.Xpf.Gauges.Descriptions", typeof (DevExpressXpfGaugesLocalizedDescriptionAttribute).Assembly);
          }
        }
        this.DescriptionValue = DevExpressXpfGaugesLocalizedDescriptionAttribute.rm.GetString(base.Description);
      }
      return base.Description;
    }
  }

  public DevExpressXpfGaugesLocalizedDescriptionAttribute(string name)
    : base(name)
  {
  }
}
