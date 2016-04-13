﻿// Decompiled with JetBrains decompiler
// Type: DevExpress.Xpf.Gauges.CustomFourteenSegmentsPresentation
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
  public class CustomFourteenSegmentsPresentation : FourteenSegmentsPresentation
  {
    public static readonly DependencyProperty SymbolTemplateProperty = DependencyPropertyManager.Register("SymbolTemplate", typeof (ControlTemplate), typeof (CustomFourteenSegmentsPresentation));

    [Category("Common")]
    public ControlTemplate SymbolTemplate
    {
      get
      {
        return (ControlTemplate) this.GetValue(CustomFourteenSegmentsPresentation.SymbolTemplateProperty);
      }
      set
      {
        this.SetValue(CustomFourteenSegmentsPresentation.SymbolTemplateProperty, (object) value);
      }
    }

    public override string PresentationName
    {
      get
      {
        return "Custom";
      }
    }

    protected internal override PresentationControl CreateSymbolPresentationControl()
    {
      CustomPresentationControl presentationControl = new CustomPresentationControl();
      presentationControl.SetBinding(Control.TemplateProperty, (BindingBase) new Binding("SymbolTemplate")
      {
        Source = (object) this
      });
      return (PresentationControl) presentationControl;
    }

    protected override GaugeDependencyObject CreateObject()
    {
      return (GaugeDependencyObject) new CustomFourteenSegmentsPresentation();
    }
  }
}
