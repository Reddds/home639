// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.CustomTickmarksPresentation
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
  public class CustomTickmarksPresentation : TickmarksPresentation
  {
    public static readonly DependencyProperty MinorTickmarkTemplateProperty = DependencyPropertyManager.Register("MinorTickmarkTemplate", typeof (ControlTemplate), typeof (CustomTickmarksPresentation));
    public static readonly DependencyProperty MajorTickmarkTemplateProperty = DependencyPropertyManager.Register("MajorTickmarkTemplate", typeof (ControlTemplate), typeof (CustomTickmarksPresentation));

    [Category("Common")]
    public ControlTemplate MinorTickmarkTemplate
    {
      get
      {
        return (ControlTemplate) this.GetValue(CustomTickmarksPresentation.MinorTickmarkTemplateProperty);
      }
      set
      {
        this.SetValue(CustomTickmarksPresentation.MinorTickmarkTemplateProperty, (object) value);
      }
    }

    [Category("Common")]
    public ControlTemplate MajorTickmarkTemplate
    {
      get
      {
        return (ControlTemplate) this.GetValue(CustomTickmarksPresentation.MajorTickmarkTemplateProperty);
      }
      set
      {
        this.SetValue(CustomTickmarksPresentation.MajorTickmarkTemplateProperty, (object) value);
      }
    }

    public override string PresentationName
    {
      get
      {
        return "Custom Tickmarks";
      }
    }

    protected internal override PresentationControl CreateMajorTickPresentationControl()
    {
      CustomPresentationControl presentationControl = new CustomPresentationControl();
      presentationControl.SetBinding(Control.TemplateProperty, (BindingBase) new Binding("MajorTickmarkTemplate")
      {
        Source = (object) this
      });
      return (PresentationControl) presentationControl;
    }

    protected internal override PresentationControl CreateMinorTickPresentationControl()
    {
      CustomPresentationControl presentationControl = new CustomPresentationControl();
      presentationControl.SetBinding(Control.TemplateProperty, (BindingBase) new Binding("MinorTickmarkTemplate")
      {
        Source = (object) this
      });
      return (PresentationControl) presentationControl;
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new CustomTickmarksPresentation();
    }
  }
}
