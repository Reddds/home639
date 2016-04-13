// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.ScaleLabelInfo
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Core.Native;

namespace DevExpress.Xpf.Gauges
{
  [NonCategorized]
  public class ScaleLabelInfo : ScaleElementInfoBase
  {
    private readonly MajorTickmarkInfo tickmark;
    private string text;

    internal MajorTickmarkInfo Tickmark
    {
      get
      {
        return this.tickmark;
      }
    }

    public string Text
    {
      get
      {
        return this.text;
      }
      set
      {
        if (!(this.text != value))
          return;
        this.text = value;
        this.RaisePropertyChanged("Text");
      }
    }

    internal ScaleLabelInfo(PresentationControl presentationControl, PresentationBase presentation, MajorTickmarkInfo tickmark, string text)
      : base(presentationControl, presentation)
    {
      this.Text = text;
      this.tickmark = tickmark;
      if (presentationControl == null)
        return;
      presentationControl.DataContext = (object) this;
    }
  }
}
