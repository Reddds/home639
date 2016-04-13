// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.Native.SymbolViewInternal
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Gauges;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace DevExpress.Xpf.Gauges.Native
{
  public abstract class SymbolViewInternal : IOwnedElement, IModelSupported, IAnimatableElement, ILayoutCalculator
  {
    private readonly List<SymbolInfo> symbols = new List<SymbolInfo>();
    private object owner;
    private bool animationInProgress;
    private IntegerAnimationProgress progress;
    private List<SymbolState> actualSymbolsState;

    private SymbolsAnimation ActualAnimation
    {
      get
      {
        if (this.Animation != null)
          return this.Animation;
        if (this.Gauge == null || !this.Gauge.EnableAnimation)
          return (SymbolsAnimation) null;
        return this.GetDefaultAnimation();
      }
    }

    protected SymbolViewBase View
    {
      get
      {
        return this.Gauge.ActualSymbolView;
      }
    }

    protected SymbolDictionary CustomSymbolMapping
    {
      get
      {
        return this.View.CustomSymbolMapping;
      }
    }

    protected SymbolOptions Options
    {
      get
      {
        return this.View.Options;
      }
    }

    protected SymbolsAnimation Animation
    {
      get
      {
        return this.View.Animation;
      }
    }

    internal bool ShouldAnimate
    {
      get
      {
        if (this.Gauge == null)
          return false;
        if (this.Animation == null)
          return this.Gauge.EnableAnimation;
        return this.Animation.Enable;
      }
    }

    internal IntegerAnimationProgress Progress
    {
      get
      {
        return this.progress;
      }
    }

    internal SymbolOptions ActualOptions
    {
      get
      {
        if (this.Options != null)
          return this.Options;
        if (this.ModelBase != null && this.ModelBase.Options != null)
          return this.ModelBase.Options;
        return new SymbolOptions();
      }
    }

    internal DigitalGaugeControl Gauge
    {
      get
      {
        return this.owner as DigitalGaugeControl;
      }
    }

    internal List<SymbolInfo> Symbols
    {
      get
      {
        return this.symbols;
      }
    }

    protected abstract SymbolPresentation ActualPresentation { get; }

    protected abstract SymbolsModelBase ModelBase { get; }

    internal abstract double DefaultHeightToWidthRatio { get; }

    object IOwnedElement.Owner
    {
      get
      {
        return this.owner;
      }
      set
      {
        this.owner = value;
      }
    }

    bool IAnimatableElement.InProgress
    {
      get
      {
        return this.animationInProgress;
      }
    }

    protected SymbolViewInternal()
    {
      this.progress = new IntegerAnimationProgress((IAnimatableElement) this);
    }

    void IModelSupported.UpdateModel()
    {
      this.UpdateModel();
    }

    ElementLayout ILayoutCalculator.CreateLayout(Size constraint)
    {
      Size size = this.Gauge == null || this.Gauge.SymbolsLayout == null ? new Size(0.0, 0.0) : this.Gauge.SymbolsLayout.SymbolSize;
      return new ElementLayout(size.Width, size.Height);
    }

    void ILayoutCalculator.CompleteLayout(ElementInfoBase elementInfo)
    {
      SymbolInfo symbolInfo = elementInfo as SymbolInfo;
      if (symbolInfo == null || symbolInfo.Layout == null)
        return;
      Point location = this.Gauge == null || this.Gauge.SymbolsLayout == null ? new Point(0.0, 0.0) : this.Gauge.SymbolsLayout.GetSymbolLocation(this.Gauge.BaseLayoutElement, symbolInfo);
      Rect rect = this.Gauge == null || this.Gauge.SymbolsLayout == null ? new Rect(0.0, 0.0, 0.0, 0.0) : this.Gauge.SymbolsLayout.GetClipBounds(this.Gauge.BaseLayoutElement);
      symbolInfo.Layout.CompleteLayout(location, (Transform) null, (Geometry) new RectangleGeometry()
      {
        Rect = new Rect(rect.Left - location.X, rect.Top - location.Y, rect.Width, rect.Height)
      });
    }

    void IAnimatableElement.ProgressChanged()
    {
      this.ApplyStatesToSymbols();
    }

    private void Animate(bool firstPlay)
    {
      this.Gauge.Storyboard.Stop();
      this.Gauge.Storyboard.Children.Clear();
      this.ActualAnimation.Prepare(this.Gauge.Storyboard, this, this.actualSymbolsState, firstPlay);
      this.animationInProgress = true;
      this.progress.Start();
      if (!this.Gauge.IsLoaded)
        return;
      this.Gauge.Storyboard.Begin();
    }

    private void ApplyStatesToSymbols()
    {
      if (this.actualSymbolsState == null)
        return;
      SymbolsAnimation actualAnimation = this.ActualAnimation;
      List<SymbolState> list = actualAnimation == null || !actualAnimation.Enable ? this.actualSymbolsState : actualAnimation.AnimateSymbolsStates(this.actualSymbolsState, this);
      int num1 = 0;
      int num2 = list.Count - 1;
      while (num1 < this.symbols.Count)
      {
        int index = this.Gauge.TextDirection == TextDirection.LeftToRight ? num1 : this.symbols.Count - 1 - num1;
        int num3 = this.Gauge.TextDirection == TextDirection.LeftToRight ? num1 : num2;
        this.symbols[index].SymbolState = list[num3 % list.Count];
        this.symbols[index].DisplayText = list[num3 % list.Count].Symbol;
        ++num1;
        --num2;
      }
    }

    private List<SymbolState> GetSymbolsStateByDisplayText(int symbolCount, List<string> textBySymbols)
    {
      return this.GetSymbolsStateByDisplayText(this.Gauge.ActualSymbolCount, textBySymbols);
    }

    private void UpdateSymbolsStates()
    {
      this.actualSymbolsState = this.GetSymbolsStateByDisplayText(this.SeparateTextToSymbols(this.Gauge.Text));
      while (this.actualSymbolsState.Count < this.symbols.Count)
      {
        if (this.Gauge.TextDirection == TextDirection.LeftToRight)
          this.actualSymbolsState.Add(this.GetEmptySymbolState());
        else
          this.actualSymbolsState.Insert(0, this.GetEmptySymbolState());
      }
    }

    internal void UpdateModel()
    {
      foreach (SymbolInfo symbolInfo in this.Symbols)
      {
        SymbolPresentation actualPresentation = this.ActualPresentation;
        symbolInfo.Presentation = (PresentationBase) actualPresentation;
        symbolInfo.PresentationControl = actualPresentation.CreateSymbolPresentationControl();
      }
      this.OnOptionsChanged();
    }

    internal void OnOptionsChanged()
    {
      for (int index = 0; index < this.Symbols.Count; ++index)
      {
        SymbolInfo symbolInfo = this.Symbols[index];
        symbolInfo.Margin = this.ActualOptions.Margin;
        symbolInfo.RenderTransform = (Transform) new SkewTransform()
        {
          AngleX = this.ActualOptions.SkewAngleX,
          AngleY = this.ActualOptions.SkewAngleY
        };
      }
    }

    protected SymbolSegmentsMapping GetCustomSegmentsMapping(char symbol)
    {
      foreach (SymbolSegmentsMapping symbolSegmentsMapping in (FreezableCollection<SymbolSegmentsMapping>) this.CustomSymbolMapping)
      {
        if ((int) symbolSegmentsMapping.Symbol == (int) symbol)
          return symbolSegmentsMapping;
      }
      return (SymbolSegmentsMapping) null;
    }

    internal void Animate()
    {
      if (this.Gauge == null)
        return;
      this.UpdateSymbolsStates();
      this.StopAnimation();
      if (!this.ShouldAnimate)
        return;
      this.Animate(true);
    }

    internal void Replay()
    {
      if (this.Gauge == null)
        return;
      this.StopAnimation();
      if (this.ShouldAnimate && this.ActualAnimation.ShouldReplay)
      {
        this.UpdateSymbolsStates();
        this.Animate(false);
      }
      else
      {
        if (this.ActualAnimation == null)
          return;
        this.Gauge.Dispatcher.BeginInvoke((Delegate) (() => this.ActualAnimation.RaiseCompletedEvent()));
      }
    }

    internal void StopAnimation()
    {
      this.animationInProgress = false;
    }

    internal void RecreateSymbols(int symbolCount)
    {
      if (this.actualSymbolsState != null)
        this.actualSymbolsState.Clear();
      foreach (SymbolInfo symbolInfo in this.Symbols)
      {
        symbolInfo.Presentation = (PresentationBase) null;
        symbolInfo.SymbolState = (SymbolState) null;
        symbolInfo.PresentationControl = (PresentationControl) null;
      }
      this.Symbols.Clear();
      SymbolPresentation actualPresentation = this.ActualPresentation;
      for (int symbolIndex = 0; symbolIndex < symbolCount; ++symbolIndex)
        this.Symbols.Add(new SymbolInfo((ILayoutCalculator) this, actualPresentation.CreateSymbolPresentationControl(), (PresentationBase) actualPresentation, symbolIndex));
      this.UpdateSymbols();
    }

    internal void UpdateSymbols()
    {
      if (this.Gauge == null)
        return;
      this.UpdateSymbolsStates();
      this.ApplyStatesToSymbols();
      this.OnOptionsChanged();
    }

    protected abstract List<SymbolState> GetSymbolsStateByDisplayText(List<string> textBySymbols);

    protected abstract SymbolsAnimation GetDefaultAnimation();

    protected internal virtual SymbolState GetEmptySymbolState()
    {
      return new SymbolState(new bool[1]);
    }

    protected internal abstract List<string> SeparateTextToSymbols(string text);
  }
}
