// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.GaugeDependencyObject
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;

namespace DevExpress.Xpf.Gauges
{
  /// <summary>
  /// 
  /// <para>
  /// Enables Windows Presentation Foundation (WPF) property system services for its derived model classes.
  /// 
  /// </para>
  /// 
  /// </summary>
  public abstract class GaugeDependencyObject : Freezable, INotifyPropertyChanged
  {
    /// <summary>
    /// 
    /// <para>
    /// Occurs every time any of the GaugeDependencyObject class properties has changed its value.
    /// 
    /// </para>
    /// 
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    protected static void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      GaugeDependencyObject dependencyObject = d as GaugeDependencyObject;
      if (dependencyObject == null)
        return;
      dependencyObject.NotifyPropertyChanged(DependencyPropertyExtensions.GetName(e.Property));
    }

    protected abstract GaugeDependencyObject CreateObject();

    protected void NotifyPropertyChanged(string propertyName)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    protected void NotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged(sender, e);
    }

    protected override Freezable CreateInstanceCore()
    {
      GaugeDependencyObject @object = this.CreateObject();
      if (@object == null)
        DebugHelper.Fail("GaudgeDependencyObject is expected");
      return (Freezable) @object;
    }
  }
}
