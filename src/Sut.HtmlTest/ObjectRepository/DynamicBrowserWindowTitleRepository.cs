﻿using CUITe.Controls.HtmlControls;
using CUITe.SearchConfigurations;

namespace Sut.HtmlTest.ObjectRepository
{
    public class DynamicBrowserWindowTitleRepository : DynamicBrowserWindowUnderTest
    {
        public DynamicBrowserWindowTitleRepository(string title)
            : base(title)
        {
        }

        public DynamicBrowserWindowTitleRepository()
            : this("Clicking the buttons changes the window title")
        {
        }

        public HtmlButton btnGoToHomePage
        {
            get { return Find<HtmlButton>(By.Id("Home")); }
        }

        public HtmlButton btnGoToPage1
        {
            get { return Find<HtmlButton>(By.Id("1")); }
        }

        public HtmlButton btnGoToPage2
        {
            get { return Find<HtmlButton>(By.Id("2")); }
        }

        public HtmlButton btnChangeWindowTitle
        {
            get { return Find<HtmlButton>(By.Id("Change Window Title")); }
        }
    }
}