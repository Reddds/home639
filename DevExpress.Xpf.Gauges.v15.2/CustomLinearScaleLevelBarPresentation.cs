// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.CustomLinearScaleLevelBarPresentation
// Assembly: DevExpress.Xpf.Gauges.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a
// MVID: AFCB53C4-1B57-494D-BB49-B7FECB1F23F5
// Assembly location: C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\DevExpress.Xpf.Gauges.v15.2.dll

using DevExpress.Xpf.Utils;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DevExpress.Xpf.Gauges
{
  public class CustomLinearScaleLevelBarPresentation : LinearScaleLevelBarPresentation
  {
    public static readonly DependencyProperty LevelBarBackgroundTemplateProperty = DependencyPropertyManager.Register("LevelBarBackgroundTemplate", typeof (ControlTemplate), typeof (CustomLinearScaleLevelBarPresentation));
    public static readonly DependencyProperty LevelBarForegroundTemplateProperty = DependencyPropertyManager.Register("LevelBarForegroundTemplate", typeof (ControlTemplate), typeof (CustomLinearScaleLevelBarPresentation));

    [Category("Common")]
    public ControlTemplate LevelBarBackgroundTemplate
    {
      get
      {
        return (ControlTemplate) this.GetValue(CustomLinearScaleLevelBarPresentation.LevelBarBackgroundTemplateProperty);
      }
      set
      {
        this.SetValue(CustomLinearScaleLevelBarPresentation.LevelBarBackgroundTemplateProperty, (object) value);
      }
    }

    [Category("Common")]
    public ControlTemplate LevelBarForegroundTemplate
    {
      get
      {
        return (ControlTemplate) this.GetValue(CustomLinearScaleLevelBarPresentation.LevelBarForegroundTemplateProperty);
      }
      set
      {
        this.SetValue(CustomLinearScaleLevelBarPresentation.LevelBarForegroundTemplateProperty, (object) value);
      }
    }

    public override string PresentationName
    {
      get
      {
        return "Custom Level Bar";
      }
    }

    protected internal override PresentationControl CreateIndicatorPresentationControl()
    {
      CustomValueIndicatorPresentationControl presentationControl = new CustomValueIndicatorPresentationControl();
      presentationControl.SetBinding(Control.TemplateProperty, (BindingBase) new Binding("LevelBarBackgroundTemplate")
      {
        Source = (object) this
      });
      return (PresentationControl) presentationControl;
    }

    protected internal override PresentationControl CreateForegroundPresentationControl()
    {
      CustomPresentationControl presentationControl = new CustomPresentationControl();
      presentationControl.SetBinding(Control.TemplateProperty, (BindingBase) new Binding("LevelBarForegroundTemplate")
      {
        Source = (object) this
      });
      return (PresentationControl) presentationControl;
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new CustomLinearScaleLevelBarPresentation();
    }
  }
}
