// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.AnimationProgress
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.Windows;

namespace DevExpress.Xpf.Gauges.Native
{
  public class AnimationProgress : DependencyObject
  {
    public static readonly DependencyProperty ProgressProperty = DependencyPropertyManager.Register("Progress", typeof (double), typeof (AnimationProgress), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(AnimationProgress.ProgressPropertyChanged)));
    private readonly IAnimatableElement element;

    public double Progress
    {
      get
      {
        return (double) this.GetValue(AnimationProgress.ProgressProperty);
      }
      set
      {
        this.SetValue(AnimationProgress.ProgressProperty, (object) value);
      }
    }

    protected IAnimatableElement Element
    {
      get
      {
        return this.element;
      }
    }

    protected bool InProgress
    {
      get
      {
        if (this.element == null)
          return false;
        return this.element.InProgress;
      }
    }

    public double ActualProgress
    {
      get
      {
        if (this.InProgress)
          return this.Progress;
        return 1.0;
      }
    }

    public AnimationProgress(IAnimatableElement element)
    {
      this.element = element;
    }

    private static void ProgressPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      AnimationProgress animationProgress = d as AnimationProgress;
      if (animationProgress == null)
        return;
      animationProgress.OnProgressChanged();
    }

    protected void RaiseProgressChanged()
    {
      if (this.element == null)
        return;
      this.element.ProgressChanged();
    }

    protected virtual void OnProgressChanged()
    {
      this.RaiseProgressChanged();
    }

    public void Start()
    {
      this.Progress = 0.0;
      this.OnProgressChanged();
      this.RaiseProgressChanged();
    }

    public void Finish()
    {
      this.Progress = 1.0;
    }
  }
}
