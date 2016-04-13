// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.GaugeElement
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// The base class for other gauge elements, and is intended to hide most properties of the <see cref="T:System.Windows.Controls.Control"/> class.
  /// 
  /// 
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class GaugeElement : Control, IOwnedElement, ILogicalParent
  {
    private readonly List<object> logicalChildren = new List<object>();
    private object owner;

    protected object Owner
    {
      get
      {
        return this.owner;
      }
    }

    protected override IEnumerator LogicalChildren
    {
      get
      {
        return (IEnumerator) this.logicalChildren.GetEnumerator();
      }
    }

    object IOwnedElement.Owner
    {
      get
      {
        return this.owner;
      }
      set
      {
        this.owner = value;
        this.OwnerChanged();
      }
    }

    void ILogicalParent.AddChild(object child)
    {
      if (this.logicalChildren.Contains(child))
        return;
      this.logicalChildren.Add(child);
      this.AddLogicalChild(child);
    }

    void ILogicalParent.RemoveChild(object child)
    {
      if (!this.logicalChildren.Contains(child))
        return;
      this.logicalChildren.Remove(child);
      this.RemoveLogicalChild(child);
    }

    protected virtual void OwnerChanged()
    {
    }
  }
}
