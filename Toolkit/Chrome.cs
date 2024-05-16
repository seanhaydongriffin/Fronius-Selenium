using System;
using Toolkit.Selenium;
using OpenQA.Selenium;
using System.Drawing;

namespace Toolkit
{
    public static class Chrome
    {

        // Clear the browser data in Chrome

        public static void ClearBrowserData()
        {
            BrowserTestObject.CurrentDriver.Navigate().GoToUrl("chrome://settings/clearBrowserData");

            WebTestObject.FindUntilDisplayed(
                null,
                new By[] {
                    By.CssSelector("* /deep/ #clearBrowsingDataConfirm")
                }
            ).click();

            //(new WebDriverWait(BrowserTestObject.CurrentDriver, TimeSpan.FromSeconds(60))).Until(ExpectedConditions.UrlToBe("chrome://settings/"));
        }

        public static Point GetRenderWidgetPoint(IntPtr MainWindowHandle)
        {
            var handles = Window.GetAllChildHandles(MainWindowHandle);

            foreach (IntPtr handle in handles)
            {
                var class_name = Window.GetClassName(handle);

                if (class_name.Equals("Chrome_RenderWidgetHostHWND"))

                    return Window.GetPoint(handle);
            }

            return Point.Empty;
        }

        public static Window.RECT GetRenderWidgetRect(IntPtr MainWindowHandle)
        {
            var handles = Window.GetAllChildHandles(MainWindowHandle);

            foreach (IntPtr handle in handles)
            {
                var class_name = Window.GetClassName(handle);

                if (class_name.Equals("Chrome_RenderWidgetHostHWND"))

                    return Window.GetRect(handle);
            }

            return new Window.RECT();
        }



        public static Window.RECT GetRenderWidgetRect(int pid)
        {
            foreach (var pidhandle in Window.EnumerateProcessWindowHandles(pid))
            {
                var handles = Window.GetAllChildHandles(pidhandle);

                foreach (IntPtr handle in handles)
                {
                    var class_name = Window.GetClassName(handle);

                    if (class_name.Equals("Chrome_RenderWidgetHostHWND"))

                        return Window.GetRect(handle);
                }
            }

            return new Window.RECT();
        }

        //public static void GetBrowserPIDHwnd(out int pid, out IntPtr hwnd)
        //{
        //    pid = 0;
        //    hwnd = IntPtr.Zero;

        //    // Get the PID and HWND details for a chrome browser

        //    System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName("chrome");
        //    for (int p = 0; p < processes.Length; p++)
        //    {
        //        ManagementObjectSearcher commandLineSearcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + processes[p].Id);
        //        String commandLine = "";
        //        foreach (ManagementObject commandLineObject in commandLineSearcher.Get())
        //        {
        //            commandLine += (String)commandLineObject["CommandLine"];
        //        }

        //        String script_pid_str = (new Regex("--scriptpid-(.+?) ")).Match(commandLine).Groups[1].Value;

        //        if (!script_pid_str.Equals("") && Convert.ToInt32(script_pid_str).Equals(System.Diagnostics.Process.GetCurrentProcess().Id))
        //        {
        //            pid = processes[p].Id;
        //            hwnd = processes[p].MainWindowHandle;
        //            return;
        //        }
        //    }

        //    return;
        //}


        public static void GetBrowserViewportPosition(int pid, IntPtr hwnd, out int browser_left, out int browser_top, out int browser_right, out int browser_bottom, out int browser_width, out int browser_height, out int viewport_left, out int viewport_top, out int viewport_right, out int viewport_bottom, out int viewport_width, out int viewport_height)
        {
            var browser_rect = Window.GetRect(hwnd);
            browser_left = browser_rect.Left;
            browser_top = browser_rect.Top;
            browser_right = browser_rect.Right;
            browser_bottom = browser_rect.Bottom;
            browser_width = browser_right - browser_left;
            browser_height = browser_bottom - browser_top;

            var viewport_rect = Chrome.GetRenderWidgetRect(pid);
            viewport_left = viewport_rect.Left;
            viewport_top = viewport_rect.Top;
            viewport_right = viewport_rect.Right;
            viewport_bottom = viewport_rect.Bottom;
            viewport_width = viewport_right - viewport_left;
            viewport_height = viewport_bottom - viewport_top;
        }

        public static void Print()
        {
            //Switch to Print dialog
            var windowHandles = BrowserTestObject.CurrentDriver.WindowHandles;

            if (windowHandles != null)
            {
                BrowserTestObject.CurrentDriver.SwitchTo().Window(windowHandles[windowHandles.Count - 1]);
            }

            //Now work with the dialog as with an ordinary page:  
            //driver.findElement(By.className("cancel")).click();

            var div = WebTestObject.find(
                null,
                new By[] {
                    By.TagName("body")
                }
            );

            BrowserTestObject.SwitchToDefaultContent();
        }



    }
}
