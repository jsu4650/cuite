﻿using System;
using System.IO;
using CassiniDev;
using CUITe.Controls;
using CUITe.Controls.SilverlightControls;
using CUITe.PageObjects;
using CUITe.SearchConfigurations;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sut.SilverlightTest.PageObjects;

namespace Sut.SilverlightTest
{
    [CodedUITest]
    [DeploymentItem("Sut.Silverlight.html")]
#if DEBUG
    #if VS2010
        [DeploymentItem(@"..\..\..\Sut.Silverlight\Bin\Debug\VS2010\Sut.Silverlight.xap")]
    #elif VS2012
        [DeploymentItem(@"..\..\..\Sut.Silverlight\Bin\Debug\VS2012\Sut.Silverlight.xap")]
    #elif VS2013
        [DeploymentItem(@"..\..\..\Sut.Silverlight\Bin\Debug\VS2013\Sut.Silverlight.xap")]
    #elif VS2015
        [DeploymentItem(@"..\..\..\Sut.Silverlight\Bin\Debug\VS2015\Sut.Silverlight.xap")]
    #endif
#else
    #if VS2010
        [DeploymentItem(@"..\..\..\Sut.Silverlight\Bin\Release\VS2010\Sut.Silverlight.xap")]
    #elif VS2012
        [DeploymentItem(@"..\..\..\Sut.Silverlight\Bin\Release\VS2012\Sut.Silverlight.xap")]
    #elif VS2013
        [DeploymentItem(@"..\..\..\Sut.Silverlight\Bin\Release\VS2013\Sut.Silverlight.xap")]
    #elif VS2015
        [DeploymentItem(@"..\..\..\Sut.Silverlight\Bin\Release\VS2015\Sut.Silverlight.xap")]
    #endif
#endif
    public class SilverlightControlTests
    {
        //TODO: the silverlight control must be hosted on a page served through a web server (ex. iis, cassini \ web dev server) because IE 9 may
        //treat web pages differently between http://localhost and file:// (compatibility mode\view)

        private static readonly CassiniDevServer WebServer = new CassiniDevServer();

        private string PageUrl
        {
            get
            {
                return string.Format("{0}Sut.Silverlight.html", WebServer.RootUrl);
            }
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            WebServer.StartServer(Directory.GetCurrentDirectory());
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            WebServer.StopServer();
        }
        [TestInitialize]
        public void TestInitialize()
        {
#if !VS2010 && !VS2012
            Playback.PlaybackSettings.AlwaysSearchControls = true; // false
#elif !VS2010
            Playback.PlaybackSettings.MaximumRetryCount = 3; // 1
#endif
            Playback.PlaybackSettings.MatchExactHierarchy = false; // false
            Playback.PlaybackSettings.SearchTimeout = 120000; // 120000
            Playback.PlaybackSettings.ShouldSearchFailFast = false; // true
            Playback.PlaybackSettings.SmartMatchOptions = SmartMatchOptions.None; // SmartMatchOptions.Control
            Playback.PlaybackSettings.WaitForReadyLevel = WaitForReadyLevel.UIThreadOnly; // .UIThreadOnly
            Playback.PlaybackSettings.WaitForReadyTimeout = 60000; // 60000
        }

        [TestMethod]
        public void SlButtonAndEditAndDTP_ClickAndSetTextAndSelectedDateAsString_Succeeds()
        {
            BrowserWindow b = BrowserWindow.Launch(PageUrl);
            b.SetFocus();
            b.Find<SilverlightButton>(By.AutomationId("button1")).Click();
            SilverlightEdit oEdit = b.Find<SilverlightEdit>(By.AutomationId("textBox1"));
            oEdit.Text = "asddasdasdasdadasdadasdadadadasd";
            SilverlightDatePicker dp = b.Find<SilverlightDatePicker>(By.AutomationId("datePicker1"));
            dp.SourceControl.SelectedDate = new DateTime(2011, 5, 11);
            b.Close();
        }

        [TestMethod]
        public void SlList_PageObjects_Succeeds()
        {
            var page = Page.Launch<TestPage>(PageUrl);
            page.List.SelectedIndices = new[] { 2 };
            Assert.IsTrue(page.List.SelectedItemsAsString == "Coded UI Test");
        }

        [TestMethod]
        public void SlList_DynamicObjectRecognition_Succeeds()
        {
            BrowserWindow b = BrowserWindow.Launch(PageUrl);
            b.SetFocus();
            SilverlightList oList = b.Find<SilverlightList>(By.AutomationId("listBox1"));
            oList.SelectedIndices = new[] { 2 };
            Assert.IsTrue(oList.SelectedItemsAsString == "Coded UI Test");
            b.Close();
        }

        [TestMethod]
        public void SlComboBox_SelectItem_Succeeds()
        {
            var browserWindow = BrowserWindow.Launch(PageUrl);
            browserWindow.SetFocus();
            var oCombo = browserWindow.Find<SilverlightComboBox>(By.AutomationId("comboBox1"));
            oCombo.SelectIndex(3);
            foreach (UITestControl temp in oCombo.Items)
            {
                Console.WriteLine(temp.Name);
            }
            browserWindow.Close();
        }

        [TestMethod]
        public void SlTab_SelectedIndex_Succeeds()
        {
            BrowserWindow b = BrowserWindow.Launch(PageUrl);
            b.SetFocus();
            SilverlightTab oTab = b.Find<SilverlightTab>(By.AutomationId("tabControl1"));
            oTab.SelectedIndex= 1;
            Assert.IsTrue(oTab.SourceControl.Items[0].Name == "tabItem1");
            b.Close();
        }

        [TestMethod]
        public void SlTab_TraverseSiblingsAndChildren_Succeeds()
        {
            BrowserWindow b = BrowserWindow.Launch(PageUrl);
            b.SetFocus();
            SilverlightTab oTab = b.Find<SilverlightTab>(By.AutomationId("tabControl1"));
            oTab.SelectedIndex = 0;
            var btnOK = b.Find<SilverlightButton>(By.AutomationId("OKButtonInTabItem1"));
            ((SilverlightEdit)(btnOK.PreviousSibling)).Text = "blah blah hurray";
            foreach (ControlBase control in oTab.GetChildren())
            {
                if (control.GetType() == typeof(SilverlightEdit))
                {
                    ((SilverlightEdit)control).Text = "Text Changed";
                    break;
                }
            }
            Assert.IsTrue(((SilverlightTab)btnOK.Parent).SelectedItem == "tabItem1");
            b.Close();
        }

        [TestMethod]
        public void Click_ButtonInChildWindow_Succeeds()
        {
            BrowserWindow browserWindow = BrowserWindow.Launch(PageUrl);
            browserWindow.SetFocus();
            SilverlightButton button = browserWindow.Find<SilverlightButton>(By.AutomationId("displayChildWindowButton"));
            button.Click();

            SilverlightChildWindow childWindow = browserWindow.Find<SilverlightChildWindow>(By.AutomationId("TestChildWindow"));
            SilverlightButton okButton = childWindow.Find<SilverlightButton>(By.AutomationId("OKButton"));
            okButton.Click();

            browserWindow.Close();
        }
    }
}

