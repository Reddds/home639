// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.DefaultMatrix5x8Presentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges.Native;
using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges
{
  public class DefaultMatrix5x8Presentation : Matrix5x8Presentation, IDefaultSymbolPresentation
  {
    public static readonly DependencyProperty FillActiveProperty = DependencyPropertyManager.Register("FillActive", typeof (Brush), typeof (DefaultMatrix5x8Presentation), new PropertyMetadata((object) null, new PropertyChangedCallback(DefaultMatrix5x8Presentation.FillActivePropertyChanged)));
    public static readonly DependencyProperty FillInactiveProperty = DependencyPropertyManager.Register("FillInactive", typeof (Brush), typeof (DefaultMatrix5x8Presentation), new PropertyMetadata((object) null, new PropertyChangedCallback(DefaultMatrix5x8Presentation.FillInactivePropertyChanged)));

    [Category("Presentation")]
    public Brush FillActive
    {
      get
      {
        return (Brush) this.GetValue(DefaultMatrix5x8Presentation.FillActiveProperty);
      }
      set
      {
        this.SetValue(DefaultMatrix5x8Presentation.FillActiveProperty, (object) value);
      }
    }

    [Category("Presentation")]
    public Brush FillInactive
    {
      get
      {
        return (Brush) this.GetValue(DefaultMatrix5x8Presentation.FillInactiveProperty);
      }
      set
      {
        this.SetValue(DefaultMatrix5x8Presentation.FillInactiveProperty, (object) value);
      }
    }

    private Brush DefaultFillActive
    {
      get
      {
        return (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 85, (byte) 85, (byte) 85));
      }
    }

    private Brush DefaultFillInactive
    {
      get
      {
        return (Brush) new SolidColorBrush(Color.FromArgb((byte) 15, (byte) 85, (byte) 85, (byte) 85));
      }
    }

    public override string PresentationName
    {
      get
      {
        return "Default";
      }
    }

    [Category("Presentation")]
    public Brush ActualFillActive
    {
      get
      {
        if (this.FillActive == null)
          return this.DefaultFillActive;
        return this.FillActive;
      }
    }

    [Category("Presentation")]
    public Brush ActualFillInactive
    {
      get
      {
        if (this.FillInactive == null)
          return this.DefaultFillInactive;
        return this.FillInactive;
      }
    }

    private static void FillActivePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      DefaultMatrix5x8Presentation matrix5x8Presentation = d as DefaultMatrix5x8Presentation;
      if (matrix5x8Presentation == null)
        return;
      matrix5x8Presentation.ActualFillActiveChanged();
    }

    private static void FillInactivePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      DefaultMatrix5x8Presentation matrix5x8Presentation = d as DefaultMatrix5x8Presentation;
      if (matrix5x8Presentation == null)
        return;
      matrix5x8Presentation.ActualFillInactiveChanged();
    }

    private void ActualFillActiveChanged()
    {
      this.NotifyPropertyChanged("ActualFillActive");
    }

    private void ActualFillInactiveChanged()
    {
      this.NotifyPropertyChanged("ActualFillInactive");
    }

    protected internal override PresentationControl CreateSymbolPresentationControl()
    {
      return (PresentationControl) new DefaultMatrix5x8Control();
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new DefaultMatrix5x8Presentation();
    }
  }
}
