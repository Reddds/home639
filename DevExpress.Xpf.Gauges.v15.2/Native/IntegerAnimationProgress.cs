// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.IntegerAnimationProgress
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System;
using System.Windows;

namespace DevExpress.Xpf.Gauges.Native
{
  public class IntegerAnimationProgress : AnimationProgress
  {
    public static readonly DependencyProperty InitialOffsetProperty = DependencyPropertyManager.Register("InitialOffset", typeof (int), typeof (IntegerAnimationProgress), new PropertyMetadata((object) 0));
    public static readonly DependencyProperty IntervalCountProperty = DependencyPropertyManager.Register("IntervalCount", typeof (int), typeof (IntegerAnimationProgress), new PropertyMetadata((object) 1));
    public static readonly DependencyProperty IntegerProgressProperty = DependencyPropertyManager.Register("IntegerProgress", typeof (int), typeof (IntegerAnimationProgress), new PropertyMetadata((object) -1, new PropertyChangedCallback(IntegerAnimationProgress.ProgressPropertyChanged)));

    public int InitialOffset
    {
      get
      {
        return (int) this.GetValue(IntegerAnimationProgress.InitialOffsetProperty);
      }
      set
      {
        this.SetValue(IntegerAnimationProgress.InitialOffsetProperty, (object) value);
      }
    }

    public int IntervalCount
    {
      get
      {
        return (int) this.GetValue(IntegerAnimationProgress.IntervalCountProperty);
      }
      set
      {
        this.SetValue(IntegerAnimationProgress.IntervalCountProperty, (object) value);
      }
    }

    public int IntegerProgress
    {
      get
      {
        return (int) this.GetValue(IntegerAnimationProgress.IntegerProgressProperty);
      }
      set
      {
        this.SetValue(IntegerAnimationProgress.IntegerProgressProperty, (object) value);
      }
    }

    public int ActualIntegerProgress
    {
      get
      {
        return this.InitialOffset + this.IntegerProgress;
      }
    }

    public IntegerAnimationProgress(IAnimatableElement element)
      : base(element)
    {
    }

    private static void ProgressPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue)
        return;
      IntegerAnimationProgress animationProgress = d as IntegerAnimationProgress;
      if (animationProgress == null)
        return;
      animationProgress.RaiseProgressChanged();
    }

    protected override void OnProgressChanged()
    {
      this.IntegerProgress = (int) Math.Floor(this.ActualProgress * (double) this.IntervalCount);
    }
  }
}
