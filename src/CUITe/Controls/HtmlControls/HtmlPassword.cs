﻿using CUITControls = Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;

namespace CUITe.Controls.HtmlControls
{
    public class HtmlPassword : HtmlEdit
    {
        public HtmlPassword(CUITControls.HtmlEdit sourceControl = null, string searchProperties = null)
            : base(sourceControl, searchProperties)
        {
            SetSearchProperty(CUITControls.HtmlControl.PropertyNames.Type, "PASSWORD");
        }
    }
}