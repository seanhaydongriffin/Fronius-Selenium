using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Globalization;
using System.Drawing;

namespace Toolkit.Selenium
{
    public class WebTestObject
    {
        private IWebElement CurrentTO;
        private static WebTestObject OriginTO;
        private static By[] CurrentBy;
        private static int FindImagePointNum = 0;

        public WebTestObject(IWebElement a)
        {
            this.CurrentTO = a;
        }


        //--------------------------------------------------------------------------------
        //	Test Object Retrieval methods
        //--------------------------------------------------------------------------------


        // ===============================================================================
        // Name...........:	FindUntilExistent()
        // Description....:	Find a test object until it exists, or the search times out.
        // Syntax.........:	FindUntilExistent(WebTestObject curr_testobject, By[] args)
        // Parameters.....:	curr_testobject        - a source test object to search from
        //                  args		           - the location of the test object to find, relative to the source above
        // Return values..: The found test object, otherwise null.
        // Remarks........:	Implements two timeouts:
        //                  1. Idle timeout (set in the Test Case Script with call "TestScript.SetIdleRefreshInterval")
        //                      - each time this timeout occurs the current web page is refreshed (in an attempt to improve the search)
        //                  2. Wait timeout (set in the Test Case Script with call "TestScript.SetWaitForObjectTimeout")
        //                      - when this timeout occurs an Exception is raised
        //                  <summary>Find the Test Object until it exists</summary>
        // ==========================================================================================
        public static WebTestObject FindUntilExistent(WebTestObject curr_testobject, By[] args)
        {
            OriginTO = curr_testobject;
            CurrentBy = args;
            Toolkit.Selenium.BrowserTestObject.SetCurrentDriverIfNotSet(false);
            IWebElement curr_webelement = null;
            IWebElement orig_webelement = null;
            int next_idle_refresh_timeout = 10;

            if (curr_testobject != null)
            {
                curr_webelement = curr_testobject.CurrentTO;
                orig_webelement = curr_testobject.CurrentTO;
            }

            "FindUntilExistent".StartTimer();

            for (int loop_num = 0; loop_num < args.Length; loop_num++)
            {
                if ("FindUntilExistent".GetTimer() > next_idle_refresh_timeout)
                {
                    next_idle_refresh_timeout = next_idle_refresh_timeout + 10;
                    ("EXCESSIVE FIND TIME. Initiating page refresh ...").LogDebug();
                    BrowserTestObject.Refresh();
                    BrowserTestObject.WaitUntilReady();
                    curr_webelement = orig_webelement;
                    loop_num = 0;
                }

                if ("FindUntilExistent".GetTimer() > 10)

                    throw new System.Exception("Timed out trying to find a WebTestObject");

                try
                {
                    if (curr_webelement == null)

                        curr_webelement = Toolkit.Selenium.BrowserTestObject.get_current().FindElement(args[loop_num]);
                    else

                        curr_webelement = curr_webelement.FindElement(args[loop_num]);
                }
                catch (Exception e)
                {
                    ("Failed to find " + args[loop_num] + " *** loop_num " + loop_num).LogDebug();
                    curr_webelement = orig_webelement;
                    loop_num = -1;
                    (0.25).Sleep(1);
                }
            }

            WebTestObject tmp = new WebTestObject(curr_webelement);
            return tmp;
        }

        // ===============================================================================
        // Name...........:	FindUntilDisplayed()
        // Description....:	Find a test object until it is displayed, or the search times out.
        // Syntax.........:	FindUntilDisplayed(WebTestObject curr_testobject, By[] args)
        // Parameters.....:	curr_testobject        - a source test object to search from
        //                  args		           - the location of the test object to find, relative to the source above
        // Return values..: The found test object, otherwise null.
        // Remarks........:	Implements two timeouts:
        //                  1. Idle timeout (set in the Test Case Script with call "TestScript.SetIdleRefreshInterval")
        //                      - each time this timeout occurs the current web page is refreshed (in an attempt to improve the search)
        //                  2. Wait timeout (set in the Test Case Script with call "TestScript.SetWaitForObjectTimeout")
        //                      - when this timeout occurs an Exception is raised
        //                  <summary>Find the Test Object until it is displayed</summary>
        // ==========================================================================================
        public static WebTestObject FindUntilDisplayed(WebTestObject curr_testobject, By[] args)
        {
            OriginTO = curr_testobject;
            CurrentBy = args;
            Toolkit.Selenium.BrowserTestObject.SetCurrentDriverIfNotSet(false);
            IWebElement curr_webelement = null;
            IWebElement orig_webelement = null;
            int next_idle_refresh_timeout = 10;

            if (curr_testobject != null)
            {
                curr_webelement = curr_testobject.CurrentTO;
                orig_webelement = curr_testobject.CurrentTO;
            }

            "FindUntilExistent".StartTimer();

            for (int loop_num = 0; loop_num < args.Length; loop_num++)
            {
                if ("FindUntilExistent".GetTimer() > next_idle_refresh_timeout)
                {
                    next_idle_refresh_timeout = next_idle_refresh_timeout + 10;
                    ("EXCESSIVE FIND TIME. Initiating page refresh ...").LogDebug();
                    BrowserTestObject.Refresh();
                    BrowserTestObject.WaitUntilReady();
                    curr_webelement = orig_webelement;
                    loop_num = 0;
                }

                if ("FindUntilExistent".GetTimer() > 10)

                    throw new System.Exception("Timed out trying to find a WebTestObject");

                try
                {
                    if (curr_webelement == null)

                        curr_webelement = Toolkit.Selenium.BrowserTestObject.get_current().FindElement(args[loop_num]);
                    else

                        curr_webelement = curr_webelement.FindElement(args[loop_num]);

                    if (loop_num == (args.Length - 1) && curr_webelement.Displayed == false)
                    {
                        ("Failed to find " + args[loop_num] + " *** loop_num " + loop_num).LogDebug();
                        curr_webelement = orig_webelement;
                        loop_num = -1;
                        (0.25).Sleep(1);
                    }
                }
                catch (Exception e)
                {
                    ("Failed to find " + args[loop_num] + " *** loop_num " + loop_num).LogDebug();
                    curr_webelement = orig_webelement;
                    loop_num = -1;
                    (0.25).Sleep(1);
                }
            }

            WebTestObject tmp = new WebTestObject(curr_webelement);
            return tmp;
        }
        public static WebTestObject FindUntilDisplayed(By by)
        {
            return FindUntilDisplayed(null, new By[] { by });
        }

        // ===============================================================================
        // Name...........:	FindUntilEnabled()
        // Description....:	Find a test object until it is enabled, or the search times out.
        // Syntax.........:	FindUntilEnabled(WebTestObject curr_testobject, By[] args)
        // Parameters.....:	curr_testobject        - a source test object to search from
        //                  args		           - the location of the test object to find, relative to the source above
        // Return values..: The found test object, otherwise null.
        // Remarks........:	Implements two timeouts:
        //                  1. Idle timeout (set in the Test Case Script with call "TestScript.SetIdleRefreshInterval")
        //                      - each time this timeout occurs the current web page is refreshed (in an attempt to improve the search)
        //                  2. Wait timeout (set in the Test Case Script with call "TestScript.SetWaitForObjectTimeout")
        //                      - when this timeout occurs an Exception is raised
        //                  <summary>Find the Test Object until it is enabled</summary>
        // ==========================================================================================
        public static WebTestObject FindUntilEnabled(WebTestObject curr_testobject, By[] args)
        {
            OriginTO = curr_testobject;
            CurrentBy = args;
            Toolkit.Selenium.BrowserTestObject.SetCurrentDriverIfNotSet(false);
            IWebElement curr_webelement = null;
            IWebElement orig_webelement = null;

            if (curr_testobject != null)
            {
                curr_webelement = curr_testobject.CurrentTO;
                orig_webelement = curr_testobject.CurrentTO;
            }

            "FindUntilExistent".StartTimer();

            for (int loop_num = 0; loop_num < args.Length; loop_num++)
            {
                try
                {
                    if (curr_webelement == null)
                    {
                        // below two lines for Firefox and C# specifically to avoid unrecoverable Exceptions
                        //                        IWebDriver xxx = Toolkit.Selenium.BrowserTestObject.get_current();
                        //                        (0.25).Sleep();
                        curr_webelement = Toolkit.Selenium.BrowserTestObject.get_current().FindElement(args[loop_num]);
                    }
                    else

                        curr_webelement = curr_webelement.FindElement(args[loop_num]);

                    if (loop_num == (args.Length - 1) && curr_webelement.Enabled == false)
                    {
                        ("Failed to find " + args[loop_num] + " *** loop_num " + loop_num).LogDebug();
                        curr_webelement = orig_webelement;
                        loop_num = -1;
                        (0.25).Sleep(0);
                    }

                }
                catch (Exception e)
                {
                    if ("FindUntilExistent".GetTimer() > 10)

                        throw new System.Exception("Timed out trying to find a WebTestObject");
                    else
                    {
                        ("Failed to find " + args[loop_num] + " *** loop_num " + loop_num).LogDebug();
                        curr_webelement = orig_webelement;
                        loop_num = -1;
                        (0.25).Sleep(0);
                    }
                }
            }

            WebTestObject tmp = new WebTestObject(curr_webelement);
            return tmp;
        }
        public static WebTestObject FindUntilEnabled(By by)
        {
            return FindUntilEnabled(null, new By[] { by });
        }
        // ===============================================================================
        // Name...........:	find()
        // Description....:	Find a test object until it is enabled, or the search times out.
        // Syntax.........:	find(WebTestObject curr_testobject, By[] args)
        // Parameters.....:	curr_testobject        - a source test object to search from
        //                  args		           - the location of the test object to find, relative to the source above
        // Return values..: The found test object, otherwise null.
        // Remarks........:	Implements two timeouts:
        //                  1. Idle timeout (set in the Test Case Script with call "TestScript.SetIdleRefreshInterval")
        //                      - each time this timeout occurs the current web page is refreshed (in an attempt to improve the search)
        //                  2. Wait timeout (set in the Test Case Script with call "TestScript.SetWaitForObjectTimeout")
        //                      - when this timeout occurs an Exception is raised
        //                  <summary>Find the Test Object until it is enabled</summary>
        // ==========================================================================================
        public static WebTestObject find(WebTestObject curr_testobject, By[] args)
        {
            OriginTO = curr_testobject;
            CurrentBy = args;
            Toolkit.Selenium.BrowserTestObject.SetCurrentDriverIfNotSet(false);
            IWebElement curr_webelement = null;
            IWebElement orig_webelement = null;

            if (curr_testobject != null)
            {
                curr_webelement = curr_testobject.CurrentTO;
                orig_webelement = curr_testobject.CurrentTO;
            }

            "FindUntilExistent".StartTimer();

            for (int loop_num = 0; loop_num < args.Length; loop_num++)
            {
                try
                {
                    if (curr_webelement == null)
                    {
                        // below two lines for Firefox and C# specifically to avoid unrecoverable Exceptions
                        //                        IWebDriver xxx = Toolkit.Selenium.BrowserTestObject.get_current();
                        //                        (0.25).Sleep();
                        curr_webelement = Toolkit.Selenium.BrowserTestObject.get_current().FindElement(args[loop_num]);
                    }
                    else

                        curr_webelement = curr_webelement.FindElement(args[loop_num]);
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            WebTestObject tmp = new WebTestObject(curr_webelement);
            return tmp;
        }


        //--------------------------------------------------------------------------------
        //	Attribute Retrieval methods
        //--------------------------------------------------------------------------------


        public static int FindNumberOfElements(WebTestObject curr_testobject, By[] args)
        {
            OriginTO = curr_testobject;
            CurrentBy = args;
            Toolkit.Selenium.BrowserTestObject.SetCurrentDriverIfNotSet(false);
            IWebElement curr_webelement = null;
            IWebElement orig_webelement = null;

            if (curr_testobject != null)
            {
                curr_webelement = curr_testobject.CurrentTO;
                orig_webelement = curr_testobject.CurrentTO;
            }

            "FindUntilExistent".StartTimer();

            for (int loop_num = 0; loop_num < args.Length; loop_num++)
            {
                try
                {
                    if (loop_num == (args.Length - 1))
                    {
                        return curr_webelement.FindElements(args[loop_num]).Count;
                    }
                    else

                        if (curr_webelement == null)
                    {
                        // below two lines for Firefox and C# specifically to avoid unrecoverable Exceptions
                        //                        IWebDriver xxx = Toolkit.Selenium.BrowserTestObject.get_current();
                        //                        (0.25).Sleep();
                        curr_webelement = Toolkit.Selenium.BrowserTestObject.get_current().FindElement(args[loop_num]);
                    }
                    else

                        curr_webelement = curr_webelement.FindElement(args[loop_num]);

                }
                catch (Exception e)
                {

                    if ("FindUntilExistent".GetTimer() > 10)

                        throw new System.Exception("Timed out trying to find a WebTestObject");
                    else
                    {
                        ("Failed to find " + args[loop_num] + " *** loop_num " + loop_num).LogDebug();
                        curr_webelement = orig_webelement;
                        loop_num = -1;
                        (0.25).Sleep(0);
                    }
                }
            }

            return -1;
        }


        public String GetAttribute(String attribute_name)
        {
            return CurrentTO.GetAttribute(attribute_name);
        }

        public String GetTagName()
        {
            return CurrentTO.TagName;
        }

        public int GetWidth()
        {
            return CurrentTO.Size.Width;
        }

        public int GetHeight()
        {
            return CurrentTO.Size.Height;
        }

        // ===============================================================================
        // Name...........:	GetAttributes()
        // Description....:	Gets all attributes and values of the test object.
        // Syntax.........:	GetAttributes()
        // Parameters.....:	None.
        // Return values..: A collection of the attributes and values of the test object.
        // Remarks........:	None.
        // ==========================================================================================
        public System.Collections.ObjectModel.ReadOnlyCollection<Object> GetAttributes()
        {
            System.Collections.ObjectModel.ReadOnlyCollection<Object> result = (System.Collections.ObjectModel.ReadOnlyCollection<Object>)(BrowserTestObject.CurrentDriver).ExecuteScript("var s = []; var attrs = arguments[0].attributes; for (var l = 0; l < attrs.length; ++l) { var a = attrs[l]; s.push(a.name + ':' + a.value); } ; return s;", CurrentTO);

            var sb = new System.Text.StringBuilder();
            sb.Append("TagName " + CurrentTO.TagName + " attributes:- X:" + CurrentTO.Location.X + "; Y:" + CurrentTO.Location.Y + "; Width:" + CurrentTO.Size.Width + "; Height:" + CurrentTO.Size.Height + "; Displayed:" + CurrentTO.Displayed + "; Enabled:" + CurrentTO.Enabled + "; Selected:" + CurrentTO.Selected);
            foreach (var item in result)
            {
                sb.Append("; " + item);
            }
            //sb.ToString().LogDebug();

            return result;
        }


        //--------------------------------------------------------------------------------
        //	Text Entry methods
        //--------------------------------------------------------------------------------


        // ===============================================================================
        // Name...........:	SetAttribute()
        // Description....:	Sets the value of an attribute of the test object.
        // Syntax.........:	SetAttribute(String attribute_name, String attribute_value)
        // Parameters.....:	attribute_name  - the name of the attribute to set
        //                  attribute_value - the value to set
        // Return values..: None.
        // Remarks........:	None.
        // ==========================================================================================
        public String SetAttribute(String attribute_name, String attribute_value)
        {
            return (String)(BrowserTestObject.CurrentDriver).ExecuteScript("arguments[0].setAttribute('" + attribute_name + "', '" + attribute_value + "')", CurrentTO);
        }

        // ===============================================================================
        // Name...........:	SetValue()
        // Description....:	Sets the value of the test object.
        // Syntax.........:	SetValue(String text)
        // Parameters.....:	text        - the value to set
        //                  method		- Optional: A number denoting the method to use to set the value.
        //								    1 = via Selenium SendKeys (default)
        //                                  2 = via Javascript value
        // Return values..: None.
        // Remarks........:	None.
        //                  <summary>Set the value with 'text' using method 'n'</summary>
        //                  From v2.35 of chromedriver there appears to be an intermittent "KeepAliveFailure" exception
        //                  (https://github.com/SeleniumHQ/selenium/issues/5758 and https://github.com/SeleniumHQ/selenium/issues/4162)
        //                  occurring during long running tests.  If this SetValue call is in involved in such a long running
        //                  test (ie. called every 30 seconds over a 2 hour period) then this exception can occur.
        //                  The loop below includes a safeguard to catch this failure and retry the call until regular timeout.
        // ==========================================================================================
        public void SetValue(String text, int method = 1, double delay = 0)
        {
            int next_idle_refresh_timeout = 10;

            // Re-attempt until no Selenium exceptions occur, or timeout occurs
            "SetValue".StartTimer();

            while ("SetValue".GetTimer() < 10)
            {
                try
                {
                    switch (method)
                    {
                        case 1:

                            if (delay > 0)

                                foreach (char c in text)
                                {
                                    CurrentTO.SendKeys(c.ToString());
                                    delay.Sleep(1);
                                }
                            else

                                CurrentTO.SendKeys(text);

                            break;

                        case 2:

                            (BrowserTestObject.CurrentDriver).ExecuteScript("arguments[0].value = arguments[1]", CurrentTO, text);
                            break;
                    }

                    //TestScript.NumberOfCharactersAutomated += text.Length;
                    return;
                }
                catch (ElementNotVisibleException e)
                {
                    "ElementNotVisibleException during SetValue()".LogDebug();
                }
                catch (StaleElementReferenceException e)
                {
                    "StaleElementReferenceException during SetValue()".LogDebug();
                }
                catch (InvalidElementStateException e)
                {
                    "InvalidElementStateException during SetValue()".LogDebug();
                }
                catch (WebDriverException e)
                {
                    if (e.Message.Contains(" is not clickable at point "))
                    {
                        "WebDriverException when element is not clickable during click()".LogDebug();
                    }
                    else

                        if (e.Message.Contains("The status of the exception was KeepAliveFailure"))

                            "KeepAliveFailure exception possibly due to HTTP keep-alive port exhaustion with the remove WebDriver server".LogDebug();
                    else

                        throw;
                }

                3.Sleep(0);

                if ("SetValue".GetTimer() > next_idle_refresh_timeout)
                {
                    next_idle_refresh_timeout = next_idle_refresh_timeout + 10;
                    ("EXCESSIVE FIND TIME. Initiating page refresh ...").LogDebug();
                    BrowserTestObject.Refresh();
                    BrowserTestObject.WaitUntilReady();
                    CurrentTO = find(OriginTO, CurrentBy).CurrentTO;
                }
            }
        }

        // ===============================================================================
        // Name...........:	SetInnerHTML()
        // Description....:	Sets the innerHTML of the test object.
        // Syntax.........:	SetInnerHTML(String text)
        // Parameters.....:	text        - the text to assign
        // Return values..: None.
        // Remarks........:	None.
        ///                 <summary>Set the innetHTML with 'text'</summary>
                // ==========================================================================================
        public void SetInnerHTML(String text)
        {
            (BrowserTestObject.CurrentDriver).ExecuteScript("arguments[0].innerHTML = arguments[1]", CurrentTO, text);
        }

        // ===============================================================================
        // Name...........:	Clear()
        // Description....:	Clears the text of the test object.
        // Syntax.........:	Clear(bool ensure_object_is_visible_first = false)
        // Parameters.....:	ensure_object_is_visible_first        - 
        // Return values..: None.
        // Remarks........:	None.
        ///                 <summary>Set the 'text' using method 'n'</summary>
        // ==========================================================================================
        public void Clear(bool ensure_object_is_visible_first = false)
        {
            int next_idle_refresh_timeout = 10;

            // Re-attempt until no Selenium exceptions occur, or timeout occurs
            "Clear".StartTimer();

            while ("Clear".GetTimer() < 10)
            {
                try
                {
                    if (ensure_object_is_visible_first)

                        (BrowserTestObject.CurrentDriver).ExecuteScript("arguments[0].scrollIntoView(true);", CurrentTO);

                    CurrentTO.Clear();
                    return;
                }
                catch (ElementNotVisibleException e)
                {
                    "ElementNotVisibleException during Clear()".LogDebug();
                }
                catch (StaleElementReferenceException e)
                {
                    "StaleElementReferenceException during Clear()".LogDebug();
                }
                catch (InvalidElementStateException e)
                {
                    "InvalidElementStateException during Clear()".LogDebug();
                }

                3.Sleep(0);

                if ("Clear".GetTimer() > next_idle_refresh_timeout)
                {
                    next_idle_refresh_timeout = next_idle_refresh_timeout + 10;
                    ("EXCESSIVE FIND TIME. Initiating page refresh ...").LogDebug();
                    BrowserTestObject.Refresh();
                    BrowserTestObject.WaitUntilReady();
                    CurrentTO = find(OriginTO, CurrentBy).CurrentTO;
                }
            }
        }

        // ===============================================================================
        // Name...........:	SetText()
        // Description....:	Sets the text of the test object.
        // Syntax.........:	SetText(String text)
        // Parameters.....:	text        - the value to set
        //                  method		- Optional: A number denoting the method to use to set the text.
        //								    1 = via Selenium SendKeys (default)
        //                                  2 = via Selenium Actions and SendKeys
        //                                  3 = Expermental
        // Return values..: None.
        // Remarks........:	None.
        ///                 <summary>Set the 'text' using method 'n'</summary>
        // ==========================================================================================
        public void SetText(string text, int method = 1, bool ensure_object_is_visible_first = false, double delay = 0)
        {
            int next_idle_refresh_timeout = 10;

            // Re-attempt until no Selenium exceptions occur, or timeout occurs
            "SetText".StartTimer();

            while ("SetText".GetTimer() < 10)
            {
                try
                {
                    if (ensure_object_is_visible_first)

                        (BrowserTestObject.CurrentDriver).ExecuteScript("arguments[0].scrollIntoView(true);", CurrentTO);

                    switch (method)
                    {
                        case 1:

                            CurrentTO.Clear();

                            if (delay > 0)

                                foreach (char c in text)
                                {
                                    CurrentTO.SendKeys(c.ToString());
                                    delay.Sleep(1);
                                }
                            else

                                CurrentTO.SendKeys(text);

                            break;

                        case 2:

                            if (delay > 0)
                            {
                                (new Actions(BrowserTestObject.CurrentDriver)).MoveToElement(CurrentTO).Build().Perform();

                                foreach (char c in text)
                                {
                                    (new Actions(BrowserTestObject.CurrentDriver)).SendKeys(c.ToString()).Build().Perform();
                                    delay.Sleep(1);
                                }
                            }
                            else

                                (new Actions(BrowserTestObject.CurrentDriver)).MoveToElement(CurrentTO).SendKeys(text).Build().Perform();

                            break;

                        case 3:

                            Actions actions2 = new Actions(BrowserTestObject.CurrentDriver);
                            actions2.MoveToElement(CurrentTO).SendKeys(Keys.Delete).SendKeys(Keys.Delete).SendKeys(Keys.Delete).Build().Perform();
                            break;

                        case 4:

                            //System.Windows.Forms.Clipboard.SetText(text);
                            Actions actions3 = new Actions(BrowserTestObject.CurrentDriver);
                            actions3.MoveToElement(CurrentTO).SendKeys(Keys.Control + "V").SendKeys("1").SendKeys(Keys.Backspace).Build().Perform();
                            break;

                    }

                    //TestScript.NumberOfCharactersAutomated += text.Length;
                    return;
                }
                catch (ElementNotVisibleException e)
                {
                    "ElementNotVisibleException during SetText()".LogDebug();
                }
                catch (StaleElementReferenceException e)
                {
                    "StaleElementReferenceException during SetText()".LogDebug();
                }
                catch (InvalidElementStateException e)
                {
                    "InvalidElementStateException during SetText()".LogDebug();
                }

                3.Sleep(0);

                if ("SetText".GetTimer() > next_idle_refresh_timeout)
                {
                    next_idle_refresh_timeout = next_idle_refresh_timeout + 10;
                    ("EXCESSIVE FIND TIME. Initiating page refresh ...").LogDebug();
                    BrowserTestObject.Refresh();
                    BrowserTestObject.WaitUntilReady();
                    CurrentTO = find(OriginTO, CurrentBy).CurrentTO;
                }
            }
        }

        // ===============================================================================
        // Name...........:	Focus()
        // Description....:	Focus the test object.
        // Syntax.........:	Focus()
        // Parameters.....:	None.
        // Return values..: None.
        // Remarks........:	None.
        ///                 <summary>Set the innetHTML with 'text'</summary>
        // ==========================================================================================
        public void Focus()
        {
            CurrentTO.SendKeys("");
        }

        public void Hover()
        {
            Actions actions = new Actions(BrowserTestObject.CurrentDriver);
            actions.MoveToElement(CurrentTO).Perform();
        }

        public void click_async()
        {
            (BrowserTestObject.CurrentDriver).ExecuteScript("var elem=arguments[0]; setTimeout(function() {elem.click();}, 100)", CurrentTO);
        }


        // ===============================================================================
        // Name...........:	click()
        // Description....:	Clicks the test object.
        // Syntax.........:	click(bool ensure_object_is_visible_first = false)
        // Parameters.....:	method		- Optional: A number denoting the method to use to click.
        //								    1 = via Selenium (default)
        //                                  2 = via Selenium Actions
        //                                  3 = via Javascript
        //                                  4 = via Windows Mouse Event
        // Return values..: true    = the click was successful
        //                  false   = the click was not successful
        // Remarks........:	This method returns false immediately on a StaleElementReferenceException
        //                  rather than retrying like other exceptions, because a stale element at this point
        //                  cannot be resolved because the test object (element) is already captured.
        //                  The calling method should instead detect the "false" return and respond accordingly
        //                  (ie. re-attempt the Find call above to update the latest test object).
        ///                 <summary>Click using method 'n', 'ensure object is visible first' and x,y offset</summary>
        // ==========================================================================================
        public bool click(int method = 1, bool ensure_object_is_visible_first = false, int x = 0, int y = 0, double speed = 0)
        {
            int next_idle_refresh_timeout = 10;

            // Re-attempt click until no Selenium exceptions occur, or timeout occurs
            "click".StartTimer();

            while ("click".GetTimer() < 10)
            {
                try
                {
                    if (ensure_object_is_visible_first)

                        (BrowserTestObject.CurrentDriver).ExecuteScript("arguments[0].scrollIntoView(true);", CurrentTO);

                    if (x != 0 || y != 0)
                    {
                        ("Clicking at x,y offset " + x + "," + y + " from the center of the Current TO (" + (x + (CurrentTO.Size.Width / 2)) + "," + (y + (CurrentTO.Size.Height / 2)) + " from the top left)").LogDebug();

                        if (speed == 0)
                        {
                            ("Clicking Current TO at top left offset " + x + "," + y).LogDebug();

                            if (method == 1)
                            {
                                //ActionBuilderEx actionbuilder = new ActionBuilderEx();
                                //actionbuilder.MoveToElement(CurrentTO, 2, x, y);
                                //actionbuilder.Click();
                                //actionbuilder.Perform();
                                return true;
                            }

                            if (method == 2)
                            {
                                Actions actions = new Actions(BrowserTestObject.CurrentDriver);
                                actions.MoveToElement(CurrentTO, x, y).Click().Perform();
                                return true;
                            }

                            if (method == 3)
                            {
                                (BrowserTestObject.CurrentDriver).ExecuteScript("arguments[0].click();", CurrentTO);
                                return true;
                            }

                            if (method == 4)
                            {
                                int CurrentTO_screen_x = BrowserTestObject.CurrentViewportLeft + CurrentTO.Location.X + x;
                                int CurrentTO_screen_y = BrowserTestObject.CurrentViewportTop + CurrentTO.Location.Y + y;
                                //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y, 250);
                                //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y, 250);
                                //Mouse.LeftButtonUp(CurrentTO_screen_x, CurrentTO_screen_y, 250);
                                return true;
                            }
                        }
                        else
                        {
                            // below is for debugging only

                            //for (int i=140; i < 340; i=i+20)
                            //{
                            //    ("test 20," + i).LogDebug();
                            //    ActionBuilderEx a1 = new ActionBuilderEx();
                            //    a1.MoveToElement(CurrentTO, 20, i, 2000);
                            //    a1.Perform();
                            //}

                            ("Moving to Current TO at top left offset " + x + "," + y + ", pausing for " + speed + "s, then clicking").LogDebug();

                            if (method == 1)
                            {
                                //ActionBuilderEx actionbuilder = new ActionBuilderEx();
                                //actionbuilder.MoveToElement(CurrentTO, 2, x, y, speed);
                                //actionbuilder.Click();
                                //actionbuilder.Perform();
                                return true;
                            }

                            if (method == 2)
                            {
                                //ActionsEx actions = new ActionsEx(BrowserTestObject.CurrentDriver);
                                //((ActionsEx)actions.MoveToElement(CurrentTO, x, y)).Sleep(speed).Click().Perform();
                                return true;
                            }

                            if (method == 4)
                            {
                                int CurrentTO_screen_x = BrowserTestObject.CurrentViewportLeft + CurrentTO.Location.X + x;
                                int CurrentTO_screen_y = BrowserTestObject.CurrentViewportTop + CurrentTO.Location.Y + y;
                                //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);
                                //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);
                                //Mouse.LeftButtonUp(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);
                                return true;
                            }
                        }

                        return true;
                    }

                    if (speed == 0)
                    {
                        if (method == 1)
                        {
                            CurrentTO.Click();
                            return true;
                        }

                        if (method == 2)
                        {
                            Actions actions = new Actions(BrowserTestObject.CurrentDriver);
                            actions.MoveToElement(CurrentTO).Click().Perform();
                            return true;
                        }

                        if (method == 3)
                        {
                            (BrowserTestObject.CurrentDriver).ExecuteScript("arguments[0].click();", CurrentTO);
                            return true;
                        }

                        if (method == 4)
                        {
                            int CurrentTO_screen_x = GetCenterScreenX();
                            int CurrentTO_screen_y = GetCenterScreenY();
                            //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y);
                            //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y);
                            //Mouse.LeftButtonUp(CurrentTO_screen_x, CurrentTO_screen_y);
                            return true;
                        }
                    }
                    else
                    {
                        if (method == 1)
                        {
                            speed.Sleep();
                            CurrentTO.Click();
                            return true;
                        }

                        if (method == 2)
                        {
                            //ActionsEx actions = new ActionsEx(BrowserTestObject.CurrentDriver);
                            //((ActionsEx)actions.MoveToElement(CurrentTO)).Sleep(speed).Click().Perform();
                            return true;
                        }

                        if (method == 3)
                        {
                            speed.Sleep();
                            (BrowserTestObject.CurrentDriver).ExecuteScript("arguments[0].click();", CurrentTO);
                            return true;
                        }

                        if (method == 4)
                        {
                            int CurrentTO_screen_x = BrowserTestObject.CurrentViewportLeft + CurrentTO.Location.X;
                            int CurrentTO_screen_y = BrowserTestObject.CurrentViewportTop + CurrentTO.Location.Y;
                            //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);
                            //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);
                            //Mouse.LeftButtonUp(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);
                            return true;
                        }

                        if (method == 5)
                        {
                            //ActionBuilderEx actionbuilder = new ActionBuilderEx();
                            //actionbuilder.Click(CurrentTO, speed);
                            //actionbuilder.Perform();
                            return true;
                        }
                    }
                }
                catch (ElementNotInteractableException e)
                {
                    "ElementNotInteractableException during click()".LogDebug();
                }
                //catch (ElementNotVisibleException e)
                //{
                //    "ElementNotVisibleException during click()".LogDebug();
                //}
                catch (StaleElementReferenceException e)
                {
                    "StaleElementReferenceException during click()".LogDebug();
                    return false;
                }
                catch (System.InvalidOperationException e)
                {
                    "System.InvalidOperationException during click()".LogDebug();
                }
                catch (WebDriverException e)
                {
                    if (e.Message.Contains(" is not clickable at point "))

                        "WebDriverException when element is not clickable during click()".LogDebug();
                    else
                        throw;
                }

                (0.25).Sleep(0);

                if ("click".GetTimer() > next_idle_refresh_timeout)
                {
                    next_idle_refresh_timeout = next_idle_refresh_timeout + 10;
                    ("EXCESSIVE FIND TIME. Initiating page refresh ...").LogDebug();
                    BrowserTestObject.Refresh();
                    BrowserTestObject.WaitUntilReady();
                    CurrentTO = find(OriginTO, CurrentBy).CurrentTO;
                }
            }

            throw new System.Exception("Timed out trying to click a WebTestObject");
            return false;
        }
        // ===============================================================================
        // Name...........:	doubleClick()
        // Description....:	Clicks the test object.
        // Syntax.........:	click(bool ensure_object_is_visible_first = false)
        // Parameters.....:	method		- Optional: A number denoting the method to use to click.
        //								    1 = via Selenium (default)
        //                                  2 = via Selenium Actions
        //                                  3 = via Javascript
        //                                  4 = via Windows Mouse Event
        // Return values..: true    = the click was successful
        //                  false   = the click was not successful
        // Remarks........:	This method returns false immediately on a StaleElementReferenceException
        //                  rather than retrying like other exceptions, because a stale element at this point
        //                  cannot be resolved because the test object (element) is already captured.
        //                  The calling method should instead detect the "false" return and respond accordingly
        //                  (ie. re-attempt the Find call above to update the latest test object).
        ///                 <summary>Click using method 'n', 'ensure object is visible first' and x,y offset</summary>
        // ==========================================================================================
        public bool doubleClick(int method = 1, bool ensure_object_is_visible_first = false, int x = 0, int y = 0, double speed = 0)
        {
            int next_idle_refresh_timeout = 10;

            // Re-attempt click until no Selenium exceptions occur, or timeout occurs
            "click".StartTimer();

            while ("click".GetTimer() < 10)
            {
                try
                {
                    if (ensure_object_is_visible_first)

                        (BrowserTestObject.CurrentDriver).ExecuteScript("arguments[0].scrollIntoView(true);", CurrentTO);

                    if (x != 0 || y != 0)
                    {
                        ("Clicking at x,y offset " + x + "," + y + " from the center of the Current TO (" + (x + (CurrentTO.Size.Width / 2)) + "," + (y + (CurrentTO.Size.Height / 2)) + " from the top left)").LogDebug();

                        if (speed == 0)
                        {
                            ("Clicking Current TO at top left offset " + x + "," + y).LogDebug();

                            if (method == 1)
                            {
                                //ActionBuilderEx actionbuilder = new ActionBuilderEx();
                                //actionbuilder.MoveToElement(CurrentTO, 2, x, y);
                                //actionbuilder.Click();
                                //actionbuilder.Perform();
                                return true;
                            }

                            if (method == 2)
                            {
                                Actions actions = new Actions(BrowserTestObject.CurrentDriver);
                                actions.MoveToElement(CurrentTO, x, y).DoubleClick().Perform();
                                return true;
                            }

                            if (method == 3)
                            {
                                (BrowserTestObject.CurrentDriver).ExecuteScript("arguments[0].click();", CurrentTO);
                                return true;
                            }

                            if (method == 4)
                            {
                                int CurrentTO_screen_x = BrowserTestObject.CurrentViewportLeft + CurrentTO.Location.X + x;
                                int CurrentTO_screen_y = BrowserTestObject.CurrentViewportTop + CurrentTO.Location.Y + y;
                                //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y, 250);
                                //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y, 250);
                                //Mouse.LeftButtonUp(CurrentTO_screen_x, CurrentTO_screen_y, 250);
                                return true;
                            }
                        }
                        else
                        {
                            // below is for debugging only

                            //for (int i=140; i < 340; i=i+20)
                            //{
                            //    ("test 20," + i).LogDebug();
                            //    ActionBuilderEx a1 = new ActionBuilderEx();
                            //    a1.MoveToElement(CurrentTO, 20, i, 2000);
                            //    a1.Perform();
                            //}

                            ("Moving to Current TO at top left offset " + x + "," + y + ", pausing for " + speed + "s, then clicking").LogDebug();

                            if (method == 1)
                            {
                                //ActionBuilderEx actionbuilder = new ActionBuilderEx();
                                //actionbuilder.MoveToElement(CurrentTO, 2, x, y, speed);
                                //actionbuilder.Click();
                                //actionbuilder.Perform();
                                return true;
                            }

                            if (method == 2)
                            {
                                //ActionsEx actions = new ActionsEx(BrowserTestObject.CurrentDriver);
                                //((ActionsEx)actions.MoveToElement(CurrentTO, x, y)).Sleep(speed).DoubleClick().Perform();
                                return true;
                            }

                            if (method == 4)
                            {
                                int CurrentTO_screen_x = BrowserTestObject.CurrentViewportLeft + CurrentTO.Location.X + x;
                                int CurrentTO_screen_y = BrowserTestObject.CurrentViewportTop + CurrentTO.Location.Y + y;
                                //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);
                                //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);
                                //Mouse.LeftButtonUp(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);
                                return true;
                            }
                        }

                        return true;
                    }

                    if (speed == 0)
                    {
                        if (method == 1)
                        {
                            CurrentTO.Click();
                            return true;
                        }

                        if (method == 2)
                        {
                            Actions actions = new Actions(BrowserTestObject.CurrentDriver);
                            actions.MoveToElement(CurrentTO).DoubleClick().Perform();
                            return true;
                        }

                        if (method == 3)
                        {
                            (BrowserTestObject.CurrentDriver).ExecuteScript("arguments[0].click();", CurrentTO);
                            return true;
                        }

                        if (method == 4)
                        {
                            int CurrentTO_screen_x = GetCenterScreenX();
                            int CurrentTO_screen_y = GetCenterScreenY();
                            //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y);
                            //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y);
                            //Mouse.LeftButtonUp(CurrentTO_screen_x, CurrentTO_screen_y);
                            return true;
                        }
                    }
                    else
                    {
                        if (method == 1)
                        {
                            speed.Sleep();
                            CurrentTO.Click();
                            return true;
                        }

                        if (method == 2)
                        {
                            //ActionsEx actions = new ActionsEx(BrowserTestObject.CurrentDriver);
                            //((ActionsEx)actions.MoveToElement(CurrentTO)).Sleep(speed).Click().Perform();
                            return true;
                        }

                        if (method == 3)
                        {
                            speed.Sleep();
                            (BrowserTestObject.CurrentDriver).ExecuteScript("arguments[0].click();", CurrentTO);
                            return true;
                        }

                        if (method == 4)
                        {
                            int CurrentTO_screen_x = BrowserTestObject.CurrentViewportLeft + CurrentTO.Location.X;
                            int CurrentTO_screen_y = BrowserTestObject.CurrentViewportTop + CurrentTO.Location.Y;
                            //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);
                            //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);
                            //Mouse.LeftButtonUp(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);
                            return true;
                        }

                        if (method == 5)
                        {
                            //ActionBuilderEx actionbuilder = new ActionBuilderEx();
                            //actionbuilder.Click(CurrentTO, speed);
                            //actionbuilder.Perform();
                            return true;
                        }
                    }
                }
                catch (ElementNotInteractableException e)
                {
                    "ElementNotInteractableException during click()".LogDebug();
                }
                //catch (ElementNotVisibleException e)
                //{
                //    "ElementNotVisibleException during click()".LogDebug();
                //}
                catch (StaleElementReferenceException e)
                {
                    "StaleElementReferenceException during click()".LogDebug();
                    return false;
                }
                catch (System.InvalidOperationException e)
                {
                    "System.InvalidOperationException during click()".LogDebug();
                }
                catch (WebDriverException e)
                {
                    if (e.Message.Contains(" is not clickable at point "))

                        "WebDriverException when element is not clickable during click()".LogDebug();
                    else
                        throw;
                }

                (0.25).Sleep(0);

                if ("click".GetTimer() > next_idle_refresh_timeout)
                {
                    next_idle_refresh_timeout = next_idle_refresh_timeout + 10;
                    ("EXCESSIVE FIND TIME. Initiating page refresh ...").LogDebug();
                    BrowserTestObject.Refresh();
                    BrowserTestObject.WaitUntilReady();
                    CurrentTO = find(OriginTO, CurrentBy).CurrentTO;
                }
            }

            throw new System.Exception("Timed out trying to click a WebTestObject");
            return false;
        }


        // ===============================================================================
        // Name...........:	DragAndDrop()
        // Description....:	Drag and Drop the test object to an optional offset.
        // Syntax.........:	DragAndDrop(int method = 1, int x1 = 0, int y1 = 0, int x2 = 0, int y2 = 0, double speed = 0, WebTestObject scrollable_TO = null)
        // Parameters.....:	method                  - Optional: The method to automate the drag and drop.
        //                                              1 = a method that uses a customised (Selenium) ActionBuilder class (default)
        //                                              2 = a method that uses a customised (Selenium) Actions class (default)
        //                                              4 = a method that uses the Windows MouseEvents API (Windows only)
        //		            x1                      - Optional: A horizontal offset to move to prior to starting the drag.
        //		            y1                      - Optional: A vertical offset to move to prior to starting the drag.
        //		            x2                      - Optional: A horizontal offset to drag to (from x1 above).
        //		            y2                      - Optional: A vertical offset to drag to (from y1 above).
        //		            speed                   - Optional: The speed the drag will occur in seconds (default of 0 is instant).
        //                  scrollable_TO           - Optional: Another test object that can be used to scroll the area being dragged within.
        //                                              This can be the current test object, the target test object, or another test object capable of scrolling
        // Return values..: None.
        // Remarks........:	destination_object and (x1, x2, y1, y2) are mutually exclusive.
        ///                 <summary>Drag and drop either to 'destination object' or to x,y coords</summary>
        // ==========================================================================================
        public void DragAndDrop(int method = 1, int x1 = 0, int y1 = 0, int x2 = 0, int y2 = 0, double speed = 0, WebTestObject scrollable_TO = null)
        {

            if (speed == 0)
            {
                if ((x1 == 0 && y1 == 0) && (x2 == 0 && y2 == 0))
                {
                    ("Drag and Drop Current TO center to Current TO center").LogDebug();

                    if (method == 1)
                    {
                        //ActionBuilderEx actionbuilder = new ActionBuilderEx();
                        //actionbuilder.ClickAndHold(CurrentTO, 0.25);
                        //actionbuilder.Release(0.25);
                        //actionbuilder.Perform();
                        return;
                    }

                    if (method == 2)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        //ActionsEx actions = new ActionsEx(BrowserTestObject.CurrentDriver);
                        //actions.ClickAndHold(CurrentTO).MoveToElement(CurrentTO).Release(CurrentTO).Build().Perform();
                        return;
                    }

                    if (method == 4)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        int CurrentTO_screen_x = GetCenterScreenX();
                        int CurrentTO_screen_y = GetCenterScreenY();
                        //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y);
                        //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y);

                        if (scrollable_TO != null)

                            ScrollIntoView();

                        int TargetTO_screen_x = GetCenterScreenX();
                        int TargetTO_screen_y = GetCenterScreenY();
                        //Mouse.Move(TargetTO_screen_x, TargetTO_screen_y);
                        //Mouse.LeftButtonUp(TargetTO_screen_x, TargetTO_screen_y);
                        return;
                    }
                }
                else

                    if ((x1 != 0 || y1 != 0) && (x2 == 0 && y2 == 0))
                {
                    ("Drag and Drop Current TO top left offset " + x1 + "," + y1 + " to Current TO center").LogDebug();

                    if (method == 1)
                    {
                        //ActionBuilderEx actionbuilder = new ActionBuilderEx();
                        //actionbuilder.ClickAndHold(CurrentTO, 2, x1, y1, 0.25);
                        //actionbuilder.MoveToElement(CurrentTO, 0.25);
                        //actionbuilder.Release(CurrentTO, 0.25);
                        //actionbuilder.Perform();
                        return;
                    }

                    if (method == 2)
                    {
                        if (scrollable_TO != null)
                        {
                            ScrollIntoView();
                            ScrollBy(0, y1, 2);
                        }

                        //ActionsEx actions = new ActionsEx(BrowserTestObject.CurrentDriver);
                        //actions.MoveToElement(CurrentTO, x1, y1).ClickAndHold().MoveToElement(CurrentTO).Release(CurrentTO).Build().Perform();
                        return;
                    }

                    if (method == 4)
                    {
                        if (scrollable_TO != null)
                        {
                            ScrollIntoView();
                            ScrollBy(0, y1, 2);
                        }

                        int CurrentTO_screen_x = GetCenterScreenX() + x1;
                        int CurrentTO_screen_y = GetCenterScreenY() + y1;
                        //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y);
                        //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y);

                        if (scrollable_TO != null)

                            ScrollIntoView();

                        int TargetTO_screen_x = GetCenterScreenX();
                        int TargetTO_screen_y = GetCenterScreenY();
                        //Mouse.Move(TargetTO_screen_x, TargetTO_screen_y);
                        //Mouse.LeftButtonUp(TargetTO_screen_x, TargetTO_screen_y);
                        return;
                    }
                }
                else

                        if ((x1 == 0 && y1 == 0) && (x2 != 0 || y2 != 0))
                {
                    ("Drag and Drop Current TO center to Current TO top left offset " + x2 + "," + y2).LogDebug();

                    if (method == 1)
                    {
                        //ActionBuilderEx actionbuilder = new ActionBuilderEx();
                        //actionbuilder.ClickAndHold(CurrentTO, 0.25);
                        //actionbuilder.MoveToElement(CurrentTO, 2, x2, y2, 0.25);
                        //actionbuilder.Release(0.25);
                        //actionbuilder.Perform();
                        return;
                    }

                    if (method == 2)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        //ActionsEx actions = new ActionsEx(BrowserTestObject.CurrentDriver);
                        //actions.ClickAndHold(CurrentTO).MoveToElement(CurrentTO, x2, y2).Release().Build().Perform();
                        return;
                    }

                    if (method == 4)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        int CurrentTO_screen_x = GetCenterScreenX();
                        int CurrentTO_screen_y = GetCenterScreenY();
                        //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y);
                        //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y);

                        if (scrollable_TO != null)
                        {
                            ScrollIntoView();
                            scrollable_TO.ScrollBy(0, y2, 2);
                        }

                        int TargetTO_screen_x = GetScreenX() + x2;
                        int TargetTO_screen_y = GetScreenY() + y2;
                        //Mouse.Move(TargetTO_screen_x, TargetTO_screen_y);
                        //Mouse.LeftButtonUp(TargetTO_screen_x, TargetTO_screen_y);
                        return;
                    }
                }
                else

                            if ((x1 != 0 || y1 != 0) && (x2 != 0 || y2 != 0))
                {
                    ("Drag and Drop Current TO top left offset " + x1 + "," + y1 + " to Current TO top left offset " + x2 + "," + y2).LogDebug();

                    if (method == 1)
                    {
                        //ActionBuilderEx actionbuilder = new ActionBuilderEx();
                        //actionbuilder.ClickAndHold(CurrentTO, 2, x1, y1, 0.25);
                        //actionbuilder.MoveToElement(CurrentTO, 2, x2, y2, 0.25);
                        //actionbuilder.Release(0.25);
                        //actionbuilder.Perform();
                        return;
                    }

                    if (method == 2)
                    {
                        if (scrollable_TO != null)
                        {
                            ScrollIntoView();
                            ScrollBy(0, y1, 2);
                        }

                        //ActionsEx actions = new ActionsEx(BrowserTestObject.CurrentDriver);
                        //actions.MoveToElement(CurrentTO, x1, y1).ClickAndHold().MoveToElement(CurrentTO, x2, y2).Release().Build().Perform();
                        return;
                    }

                    if (method == 4)
                    {
                        if (scrollable_TO != null)
                        {
                            ScrollIntoView();
                            ScrollBy(0, y1, 2);
                        }

                        int CurrentTO_screen_x = GetScreenX() + x1;
                        int CurrentTO_screen_y = GetScreenY() + y1;
                        //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y);
                        //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y);

                        if (scrollable_TO != null)
                        {
                            ScrollIntoView();
                            scrollable_TO.ScrollBy(0, y2, 2);
                        }

                        int TargetTO_screen_x = GetScreenX() + x2;
                        int TargetTO_screen_y = GetScreenY() + y2;
                        //Mouse.Move(TargetTO_screen_x, TargetTO_screen_y);
                        //Mouse.LeftButtonUp(TargetTO_screen_x, TargetTO_screen_y);
                        return;
                    }
                }
            }
            else
            {
                if ((x1 == 0 && y1 == 0) && (x2 == 0 && y2 == 0))
                {
                    ("Drag and Drop Current TO center to Current TO center at speed " + speed).LogDebug();

                    if (method == 1)
                    {
                        //ActionBuilderEx actionbuilder = new ActionBuilderEx();
                        //actionbuilder.ClickAndHold(CurrentTO, speed);
                        //actionbuilder.MoveToElement(CurrentTO, speed);
                        //actionbuilder.Release(CurrentTO, speed);
                        //actionbuilder.Perform();
                        return;
                    }

                    if (method == 2)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        //ActionsEx actions = new ActionsEx(BrowserTestObject.CurrentDriver);
                        //((ActionsEx)((ActionsEx)((ActionsEx)actions.ClickAndHold(CurrentTO)).Sleep(speed).MoveToElement(CurrentTO)).Sleep(speed).Release(CurrentTO)).Sleep(speed).Build().Perform();
                        return;
                    }

                    if (method == 4)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        int CurrentTO_screen_x = GetCenterScreenX();
                        int CurrentTO_screen_y = GetCenterScreenY();
                        //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);
                        //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);

                        if (scrollable_TO != null)

                            ScrollIntoView();

                        int TargetTO_screen_x = GetCenterScreenX();
                        int TargetTO_screen_y = GetCenterScreenY();
                        //Mouse.Move(TargetTO_screen_x, TargetTO_screen_y, speed * 1000);
                        //Mouse.LeftButtonUp(TargetTO_screen_x, TargetTO_screen_y, speed * 1000);
                        return;
                    }
                }
                else

                    if ((x1 != 0 || y1 != 0) && (x2 == 0 && y2 == 0))
                {
                    ("Drag and Drop Current TO top left offset " + x1 + "," + y1 + " to Current TO center at speed " + speed).LogDebug();

                    if (method == 1)
                    {
                        //ActionBuilderEx actionbuilder = new ActionBuilderEx();
                        //actionbuilder.ClickAndHold(CurrentTO, 2, x1, y1, speed);
                        //actionbuilder.MoveToElement(CurrentTO, speed);
                        //actionbuilder.Release(CurrentTO, speed);
                        //actionbuilder.Perform();
                        return;
                    }

                    if (method == 2)
                    {
                        if (scrollable_TO != null)
                        {
                            ScrollIntoView();
                            ScrollBy(0, y1, 2);
                        }

                        //ActionsEx actions = new ActionsEx(BrowserTestObject.CurrentDriver);
                        //((ActionsEx)((ActionsEx)((ActionsEx)((ActionsEx)actions.MoveToElement(CurrentTO, x1, y1)).Sleep(speed).ClickAndHold()).Sleep(speed).MoveToElement(CurrentTO)).Sleep(speed).Release(CurrentTO)).Sleep(speed).Build().Perform();
                        return;
                    }

                    if (method == 4)
                    {
                        if (scrollable_TO != null)
                        {
                            ScrollIntoView();
                            ScrollBy(0, y1, 2);
                        }

                        int CurrentTO_screen_x = GetCenterScreenX() + x1;
                        int CurrentTO_screen_y = GetCenterScreenY() + y1;
                        //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);
                        //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);

                        if (scrollable_TO != null)

                            ScrollIntoView();

                        int TargetTO_screen_x = GetCenterScreenX();
                        int TargetTO_screen_y = GetCenterScreenY();
                        //Mouse.Move(TargetTO_screen_x, TargetTO_screen_y, speed * 1000);
                        //Mouse.LeftButtonUp(TargetTO_screen_x, TargetTO_screen_y, speed * 1000);
                        return;
                    }
                }
                else

                        if ((x1 == 0 && y1 == 0) && (x2 != 0 || y2 != 0))
                {
                    ("Drag and Drop Current TO center to Current TO top left offset " + x2 + "," + y2 + " at speed " + speed).LogDebug();

                    if (method == 1)
                    {
                        //ActionBuilderEx actionbuilder = new ActionBuilderEx();
                        //actionbuilder.ClickAndHold(CurrentTO, speed);
                        //actionbuilder.MoveToElement(CurrentTO, 2, x2, y2, speed);
                        //actionbuilder.Release(0.25);
                        //actionbuilder.Perform();
                        return;
                    }

                    if (method == 2)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        //ActionsEx actions = new ActionsEx(BrowserTestObject.CurrentDriver);
                        //((ActionsEx)((ActionsEx)((ActionsEx)actions.ClickAndHold(CurrentTO)).Sleep(speed).MoveToElement(CurrentTO, x2, y2)).Sleep(speed).Release()).Sleep(speed).Build().Perform();
                        return;
                    }

                    if (method == 4)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        int CurrentTO_screen_x = GetCenterScreenX();
                        int CurrentTO_screen_y = GetCenterScreenY();
                        //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);
                        //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);

                        if (scrollable_TO != null)
                        {
                            ScrollIntoView();
                            scrollable_TO.ScrollBy(0, y2, 2);
                        }

                        int TargetTO_screen_x = GetScreenX() + x2;
                        int TargetTO_screen_y = GetScreenY() + y2;
                        //Mouse.Move(TargetTO_screen_x, TargetTO_screen_y, speed * 1000);
                        //Mouse.LeftButtonUp(TargetTO_screen_x, TargetTO_screen_y, speed * 1000);
                        return;
                    }
                }
                else

                            if ((x1 != 0 || y1 != 0) && (x2 != 0 || y2 != 0))
                {
                    ("Drag and Drop Current TO top left offset " + x1 + "," + y1 + " to Current TO top left offset " + x2 + "," + y2 + " at speed " + speed).LogDebug();

                    if (method == 1)
                    {
                        //ActionBuilderEx actionbuilder = new ActionBuilderEx();
                        //actionbuilder.ClickAndHold(CurrentTO, 2, x1, y1, speed);
                        //actionbuilder.MoveToElement(CurrentTO, 2, x2, y2, speed);
                        //actionbuilder.Release(speed);
                        //actionbuilder.Perform();
                        return;
                    }

                    if (method == 2)
                    {
                        if (scrollable_TO != null)
                        {
                            ScrollIntoView();
                            ScrollBy(0, y1, 2);
                        }

                        //ActionsEx actions = new ActionsEx(BrowserTestObject.CurrentDriver);
                        //((ActionsEx)((ActionsEx)((ActionsEx)((ActionsEx)actions.MoveToElement(CurrentTO, x1, y1)).Sleep(speed).ClickAndHold()).Sleep(speed).MoveToElement(CurrentTO, x2, y2)).Sleep(speed).Release()).Sleep(speed).Build().Perform();
                        return;
                    }

                    if (method == 4)
                    {
                        if (scrollable_TO != null)
                        {
                            ScrollIntoView();
                            ScrollBy(0, y1, 2);
                        }

                        int CurrentTO_screen_x = GetScreenX() + x1;
                        int CurrentTO_screen_y = GetScreenY() + y1;
                        //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);
                        //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);

                        if (scrollable_TO != null)
                        {
                            ScrollIntoView();
                            scrollable_TO.ScrollBy(0, y2, 2);
                        }

                        int TargetTO_screen_x = GetScreenX() + x2;
                        int TargetTO_screen_y = GetScreenY() + y2;
                        //Mouse.Move(TargetTO_screen_x, TargetTO_screen_y, speed * 1000);
                        //Mouse.LeftButtonUp(TargetTO_screen_x, TargetTO_screen_y, speed * 1000);
                        return;
                    }
                }
            }

            // Sean G - 10 Aug - additional screenshot added to investigate intermittent drag and drop failures, particularly
            //  across different platforms
            //"screenshot browser".LogDebug();

        }

        // ===============================================================================
        // Name...........:	DragAndDrop()
        // Description....:	Drag and Drop the current test object to a target test object with optional offsets.
        // Syntax.........:	DragAndDrop(bool ensure_object_is_visible_first = false)
        // Parameters.....:	TargetTO        		- The target test object to drag and drop to.
        //                  method                  - Optional: The method to automate the drag and drop.
        //                                              1 = a method that uses a customised (Selenium) ActionBuilder class (default)
        //                                              2 = a method that uses a customised (Selenium) Actions class (default)
        //                                              4 = a method that uses the Windows MouseEvents API (Windows only)
        //		            x1                      - Optional: A horizontal offset to move to prior to starting the drag.
        //		            y1                      - Optional: A vertical offset to move to prior to starting the drag.
        //		            x2                      - Optional: A horizontal offset to drag to (from x1 above).
        //		            y2                      - Optional: A vertical offset to drag to (from y1 above).
        //		            speed                   - Optional: The speed the drag will occur in seconds (default of 0 is instant).
        //                  scrollable_TO           - Optional: Another test object that can be used to scroll the area being dragged within.
        //                                              This can be the current test object, the target test object, or another test object capable of scrolling
        // Return values..: None.
        // Remarks........:	destination_object and (x1, x2, y1, y2) are mutually exclusive.
        ///                 <summary>Drag and drop either to 'destination object' or to x,y coords</summary>
        // ==========================================================================================
        public void DragAndDrop(WebTestObject TargetTO, int method = 1, int x1 = 0, int y1 = 0, int x2 = 0, int y2 = 0, double speed = 0, WebTestObject scrollable_TO = null)
        {
            if (speed == 0)
            {
                if ((x1 == 0 && y1 == 0) && (x2 == 0 && y2 == 0))
                {
                    ("Drag and Drop Current TO center to Target TO center").LogDebug();

                    if (method == 1)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        //ActionBuilderEx actionbuilder = new ActionBuilderEx();
                        //actionbuilder.ClickAndHold(CurrentTO, 0.25);
                        //actionbuilder.Perform();

                        if (scrollable_TO != null)

                            TargetTO.ScrollIntoView();

                        //ActionBuilderEx actionbuilder2 = new ActionBuilderEx();
                        //actionbuilder2.MoveToElement(TargetTO.CurrentTO, 0.25);
                        //actionbuilder2.Release(TargetTO.CurrentTO, 0.25);
                        //actionbuilder2.Perform();
                        return;
                    }

                    if (method == 2)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        //ActionsEx actions = new ActionsEx(BrowserTestObject.CurrentDriver);
                        //actions.ClickAndHold(CurrentTO).MoveToElement(TargetTO.CurrentTO).Release(TargetTO.CurrentTO).Build().Perform();
                        return;
                    }

                    if (method == 4)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        int CurrentTO_screen_x = GetCenterScreenX();
                        int CurrentTO_screen_y = GetCenterScreenY();
                        //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y);
                        //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y);

                        if (scrollable_TO != null)
                        {
                            // The 3 lines below are specifically for IE only.
                            //  In IE objects being dragged can shift away from the mouse if scrolled off screen at the same time
                            //  Below is a workaround to first drag the object towards it's destination, to avoid being scrolled off screen.
                            //  The normal scroll and drag below then repositions the object in the correct place (as normal)
                            int TargetTO_screen_x2 = TargetTO.GetCenterScreenX();
                            int TargetTO_screen_y2 = TargetTO.GetCenterScreenY();
                            //Mouse.Move(TargetTO_screen_x2, TargetTO_screen_y2);

                            TargetTO.ScrollIntoView();
                        }

                        int TargetTO_screen_x = TargetTO.GetCenterScreenX();
                        int TargetTO_screen_y = TargetTO.GetCenterScreenY();
                        //Mouse.Move(TargetTO_screen_x, TargetTO_screen_y);
                        //Mouse.LeftButtonUp(TargetTO_screen_x, TargetTO_screen_y);
                        return;
                    }
                }
                else

                    if ((x1 != 0 || y1 != 0) && (x2 == 0 && y2 == 0))
                {
                    ("Drag and Drop Current TO top left offset " + x1 + "," + y1 + " to Target TO center").LogDebug();

                    if (method == 1)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        //ActionBuilderEx actionbuilder = new ActionBuilderEx();
                        //actionbuilder.ClickAndHold(CurrentTO, 2, x1, y1, 0.25);
                        //actionbuilder.Perform();

                        if (scrollable_TO != null)

                            TargetTO.ScrollIntoView();

                        //ActionBuilderEx actionbuilder2 = new ActionBuilderEx();
                        //actionbuilder2.MoveToElement(TargetTO.CurrentTO, 0.25);
                        //actionbuilder2.Release(TargetTO.CurrentTO, 0.25);
                        //actionbuilder2.Perform();
                        return;
                    }

                    if (method == 2)
                    {
                        if (scrollable_TO != null)
                        {
                            ScrollIntoView();
                            ScrollBy(0, y1, 2);
                        }

                        //ActionsEx actions = new ActionsEx(BrowserTestObject.CurrentDriver);
                        //actions.MoveToElement(CurrentTO, x1, y1).ClickAndHold().MoveToElement(TargetTO.CurrentTO).Release(TargetTO.CurrentTO).Build().Perform();
                        return;
                    }

                    if (method == 4)
                    {
                        if (scrollable_TO != null)
                        {
                            ScrollIntoView();
                            ScrollBy(0, y1, 2);
                        }

                        int CurrentTO_screen_x = GetCenterScreenX() + x1;
                        int CurrentTO_screen_y = GetCenterScreenY() + y1;
                        //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y);
                        //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y);

                        if (scrollable_TO != null)
                        {
                            // The 3 lines below are specifically for IE only.
                            //  In IE objects being dragged can shift away from the mouse if scrolled off screen at the same time
                            //  Below is a workaround to first drag the object towards it's destination, to avoid being scrolled off screen.
                            //  The normal scroll and drag below then repositions the object in the correct place (as normal)
                            int TargetTO_screen_x2 = TargetTO.GetCenterScreenX();
                            int TargetTO_screen_y2 = TargetTO.GetCenterScreenY();
                            //Mouse.Move(TargetTO_screen_x2, TargetTO_screen_y2);

                            TargetTO.ScrollIntoView();
                        }

                        int TargetTO_screen_x = TargetTO.GetCenterScreenX();
                        int TargetTO_screen_y = TargetTO.GetCenterScreenY();
                        //Mouse.Move(TargetTO_screen_x, TargetTO_screen_y);
                        //Mouse.LeftButtonUp(TargetTO_screen_x, TargetTO_screen_y);
                        return;
                    }
                }
                else

                        if ((x1 == 0 && y1 == 0) && (x2 != 0 || y2 != 0))
                {
                    ("Drag and Drop Current TO center to Target TO top left offset " + x2 + "," + y2).LogDebug();

                    if (method == 1)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        //ActionBuilderEx actionbuilder = new ActionBuilderEx();
                        //actionbuilder.ClickAndHold(CurrentTO, 0.25);
                        //actionbuilder.Perform();

                        if (scrollable_TO != null)

                            TargetTO.ScrollIntoView();

                        //ActionBuilderEx actionbuilder2 = new ActionBuilderEx();
                        //actionbuilder2.MoveToElement(TargetTO.CurrentTO, 2, x2, y2, 0.25);
                        //actionbuilder2.Release(0.25);
                        //actionbuilder2.Perform();
                        return;
                    }

                    if (method == 2)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        //ActionsEx actions = new ActionsEx(BrowserTestObject.CurrentDriver);
                        //actions.ClickAndHold(CurrentTO).MoveToElement(TargetTO.CurrentTO, x2, y2).Release().Build().Perform();
                        return;
                    }

                    if (method == 4)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        int CurrentTO_screen_x = GetCenterScreenX();
                        int CurrentTO_screen_y = GetCenterScreenY();
                        //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y);
                        //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y);

                        if (scrollable_TO != null)
                        {
                            // The 3 lines below are specifically for IE only.
                            //  In IE objects being dragged can shift away from the mouse if scrolled off screen at the same time
                            //  Below is a workaround to first drag the object towards it's destination, to avoid being scrolled off screen.
                            //  The normal scroll and drag below then repositions the object in the correct place (as normal)
                            int TargetTO_screen_x2 = TargetTO.GetScreenX() + x2;
                            int TargetTO_screen_y2 = TargetTO.GetScreenY() + y2;
                            //Mouse.Move(TargetTO_screen_x2, TargetTO_screen_y2);

                            TargetTO.ScrollIntoView();
                            scrollable_TO.ScrollBy(0, y2, 2);
                        }

                        int TargetTO_screen_x = TargetTO.GetScreenX() + x2;
                        int TargetTO_screen_y = TargetTO.GetScreenY() + y2;
                        //Mouse.Move(TargetTO_screen_x, TargetTO_screen_y);
                        //Mouse.LeftButtonUp(TargetTO_screen_x, TargetTO_screen_y);
                        return;
                    }
                }
                else

                            if ((x1 != 0 || y1 != 0) && (x2 != 0 || y2 != 0))
                {
                    ("Drag and Drop Current TO top left offset " + x1 + "," + y1 + " to Target TO top left offset " + x2 + "," + y2).LogDebug();

                    if (method == 1)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        //ActionBuilderEx actionbuilder = new ActionBuilderEx();
                        //actionbuilder.ClickAndHold(CurrentTO, 2, x1, y1, 0.25);
                        //actionbuilder.Perform();

                        if (scrollable_TO != null)

                            TargetTO.ScrollIntoView();

                        //ActionBuilderEx actionbuilder2 = new ActionBuilderEx();
                        //actionbuilder2.MoveToElement(TargetTO.CurrentTO, 2, x2, y2, 0.25);
                        //actionbuilder2.Release(0.25);
                        //actionbuilder2.Perform();
                        return;
                    }

                    if (method == 2)
                    {
                        if (scrollable_TO != null)
                        {
                            ScrollIntoView();
                            ScrollBy(0, y1, 2);
                        }

                        //ActionsEx actions = new ActionsEx(BrowserTestObject.CurrentDriver);
                        //actions.MoveToElement(CurrentTO, x1, y1).ClickAndHold().MoveToElement(TargetTO.CurrentTO, x2, y2).Release().Build().Perform();
                        return;
                    }

                    if (method == 4)
                    {
                        if (scrollable_TO != null)
                        {
                            ScrollIntoView();
                            ScrollBy(0, y1, 2);
                        }

                        int CurrentTO_screen_x = GetCenterScreenX() + x1;
                        int CurrentTO_screen_y = GetCenterScreenY() + y1;
                        //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y);
                        //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y);

                        if (scrollable_TO != null)
                        {
                            // The 3 lines below are specifically for IE only.
                            //  In IE objects being dragged can shift away from the mouse if scrolled off screen at the same time
                            //  Below is a workaround to first drag the object towards it's destination, to avoid being scrolled off screen.
                            //  The normal scroll and drag below then repositions the object in the correct place (as normal)
                            int TargetTO_screen_x2 = TargetTO.GetScreenX() + x2;
                            int TargetTO_screen_y2 = TargetTO.GetScreenY() + y2;
                            //Mouse.Move(TargetTO_screen_x2, TargetTO_screen_y2);

                            TargetTO.ScrollIntoView();
                            scrollable_TO.ScrollBy(0, y2, 2);
                        }

                        int TargetTO_screen_x = TargetTO.GetScreenX() + x2;
                        int TargetTO_screen_y = TargetTO.GetScreenY() + y2;
                        //Mouse.Move(TargetTO_screen_x, TargetTO_screen_y);
                        //Mouse.LeftButtonUp(TargetTO_screen_x, TargetTO_screen_y);
                        return;
                    }
                }
            }
            else
            {
                if ((x1 == 0 && y1 == 0) && (x2 == 0 && y2 == 0))
                {
                    // debugging ...
                    //for (int y = 10; y > -1000; y = y - 5)
                    //{
                    //    actionbuilder.MoveToElement(CurrentTO, 20, y, 250);
                    //    actionbuilder.Click();
                    //    actionbuilder.Perform();
                    //}

                    ("Drag and Drop Current TO center to Target TO center at speed " + speed).LogDebug();

                    if (method == 1)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        //ActionBuilderEx actionbuilder = new ActionBuilderEx();
                        //actionbuilder.ClickAndHold(CurrentTO, speed);
                        //actionbuilder.Perform();

                        if (scrollable_TO != null)

                            TargetTO.ScrollIntoView();

                        //ActionBuilderEx actionbuilder2 = new ActionBuilderEx();
                        //actionbuilder2.MoveToElement(TargetTO.CurrentTO, speed);
                        //actionbuilder2.Release(TargetTO.CurrentTO, speed);
                        //actionbuilder2.Perform();
                        return;
                    }

                    if (method == 2)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        //ActionsEx actions = new ActionsEx(BrowserTestObject.CurrentDriver);
                        //((ActionsEx)((ActionsEx)((ActionsEx)actions.ClickAndHold(CurrentTO)).Sleep(speed).MoveToElement(TargetTO.CurrentTO)).Sleep(speed).Release(TargetTO.CurrentTO)).Sleep(speed).Build().Perform();
                        return;
                    }

                    if (method == 4)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        int CurrentTO_screen_x = GetCenterScreenX();
                        int CurrentTO_screen_y = GetCenterScreenY();
                        //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);
                        //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);

                        if (scrollable_TO != null)
                        {
                            // The 3 lines below are specifically for IE only.
                            //  In IE objects being dragged can shift away from the mouse if scrolled off screen at the same time
                            //  Below is a workaround to first drag the object towards it's destination, to avoid being scrolled off screen.
                            //  The normal scroll and drag below then repositions the object in the correct place (as normal)
                            int TargetTO_screen_x2 = TargetTO.GetCenterScreenX();
                            int TargetTO_screen_y2 = TargetTO.GetCenterScreenY();
                            //Mouse.Move(TargetTO_screen_x2, TargetTO_screen_y2, speed * 1000);

                            TargetTO.ScrollIntoView();
                        }

                        int TargetTO_screen_x = TargetTO.GetCenterScreenX();
                        int TargetTO_screen_y = TargetTO.GetCenterScreenY();
                        //Mouse.Move(TargetTO_screen_x, TargetTO_screen_y, speed * 1000);
                        //Mouse.LeftButtonUp(TargetTO_screen_x, TargetTO_screen_y, speed * 1000);
                        return;
                    }
                }
                else

                    if ((x1 != 0 || y1 != 0) && (x2 == 0 && y2 == 0))
                {
                    ("Drag and Drop Current TO top left offset " + x1 + "," + y1 + " to Target TO center at speed " + speed).LogDebug();

                    if (method == 1)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        //ActionBuilderEx actionbuilder = new ActionBuilderEx();
                        //actionbuilder.ClickAndHold(CurrentTO, 2, x1, y1, speed);
                        //actionbuilder.Perform();

                        if (scrollable_TO != null)

                            TargetTO.ScrollIntoView();

                        //ActionBuilderEx actionbuilder2 = new ActionBuilderEx();
                        //actionbuilder2.MoveToElement(TargetTO.CurrentTO, speed);
                        //actionbuilder2.Release(TargetTO.CurrentTO, speed);
                        //actionbuilder2.Perform();
                        return;
                    }

                    if (method == 2)
                    {
                        if (scrollable_TO != null)
                        {
                            ScrollIntoView();
                            ScrollBy(0, y1, 2);
                        }

                        //ActionsEx actions = new ActionsEx(BrowserTestObject.CurrentDriver);
                        //((ActionsEx)((ActionsEx)((ActionsEx)((ActionsEx)actions.MoveToElement(CurrentTO, x1, y1)).Sleep(speed).ClickAndHold()).Sleep(speed).MoveToElement(TargetTO.CurrentTO)).Sleep(speed).Release(TargetTO.CurrentTO)).Sleep(speed).Build().Perform();
                        return;
                    }

                    if (method == 4)
                    {
                        if (scrollable_TO != null)
                        {
                            ScrollIntoView();
                            ScrollBy(0, y1, 2);
                        }

                        int CurrentTO_screen_x = GetCenterScreenX() + x1;
                        int CurrentTO_screen_y = GetCenterScreenY() + y1;
                        //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);
                        //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);

                        if (scrollable_TO != null)
                        {
                            // The 3 lines below are specifically for IE only.
                            //  In IE objects being dragged can shift away from the mouse if scrolled off screen at the same time
                            //  Below is a workaround to first drag the object towards it's destination, to avoid being scrolled off screen.
                            //  The normal scroll and drag below then repositions the object in the correct place (as normal)
                            int TargetTO_screen_x2 = TargetTO.GetCenterScreenX();
                            int TargetTO_screen_y2 = TargetTO.GetCenterScreenY();
                            //Mouse.Move(TargetTO_screen_x2, TargetTO_screen_y2, speed * 1000);

                            TargetTO.ScrollIntoView();
                        }

                        int TargetTO_screen_x = TargetTO.GetCenterScreenX();
                        int TargetTO_screen_y = TargetTO.GetCenterScreenY();
                        //Mouse.Move(TargetTO_screen_x, TargetTO_screen_y, speed * 1000);
                        //Mouse.LeftButtonUp(TargetTO_screen_x, TargetTO_screen_y, speed * 1000);
                        return;
                    }
                }
                else

                        if ((x1 == 0 && y1 == 0) && (x2 != 0 || y2 != 0))
                {
                    ("Drag and Drop Current TO center to Target TO top left offset " + x2 + "," + y2 + " at speed " + speed).LogDebug();

                    if (method == 1)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        //ActionBuilderEx actionbuilder = new ActionBuilderEx();
                        //actionbuilder.ClickAndHold(CurrentTO, speed);
                        //actionbuilder.Perform();

                        if (scrollable_TO != null)

                            TargetTO.ScrollIntoView();

                        //ActionBuilderEx actionbuilder2 = new ActionBuilderEx();
                        //actionbuilder2.MoveToElement(TargetTO.CurrentTO, 2, x2, y2, speed);
                        //actionbuilder2.Release(speed);
                        //actionbuilder2.Perform();
                        return;
                    }

                    if (method == 2)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        //ActionsEx actions = new ActionsEx(BrowserTestObject.CurrentDriver);
                        //((ActionsEx)((ActionsEx)((ActionsEx)actions.ClickAndHold(CurrentTO)).Sleep(speed).MoveToElement(TargetTO.CurrentTO, x2, y2)).Sleep(speed).Release()).Sleep(speed).Build().Perform();
                        return;
                    }

                    if (method == 4)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        int CurrentTO_screen_x = GetCenterScreenX();
                        int CurrentTO_screen_y = GetCenterScreenY();
                        //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);
                        //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);

                        if (scrollable_TO != null)
                        {
                            // The 3 lines below are specifically for IE only.
                            //  In IE objects being dragged can shift away from the mouse if scrolled off screen at the same time
                            //  Below is a workaround to first drag the object towards it's destination, to avoid being scrolled off screen.
                            //  The normal scroll and drag below then repositions the object in the correct place (as normal)
                            int TargetTO_screen_x2 = TargetTO.GetScreenX() + x2;
                            int TargetTO_screen_y2 = TargetTO.GetScreenY() + y2;
                            //Mouse.Move(TargetTO_screen_x2, TargetTO_screen_y2, speed * 1000);

                            TargetTO.ScrollIntoView();
                            scrollable_TO.ScrollBy(0, y2, 2);
                        }

                        int TargetTO_screen_x = TargetTO.GetScreenX() + x2;
                        int TargetTO_screen_y = TargetTO.GetScreenY() + y2;
                        //Mouse.Move(TargetTO_screen_x, TargetTO_screen_y, speed * 1000);
                        //Mouse.LeftButtonUp(TargetTO_screen_x, TargetTO_screen_y, speed * 1000);
                        return;
                    }
                }
                else

                            if ((x1 != 0 || y1 != 0) && (x2 != 0 || y2 != 0))
                {
                    ("Drag and Drop Current TO top left offset " + x1 + "," + y1 + " to Target TO top left offset " + x2 + "," + y2 + " at speed " + speed).LogDebug();

                    if (method == 1)
                    {
                        if (scrollable_TO != null)

                            ScrollIntoView();

                        //ActionBuilderEx actionbuilder = new ActionBuilderEx();
                        //actionbuilder.ClickAndHold(CurrentTO, 2, x1, y1, speed);
                        //actionbuilder.Perform();

                        if (scrollable_TO != null)

                            TargetTO.ScrollIntoView();

                        //ActionBuilderEx actionbuilder2 = new ActionBuilderEx();
                        //actionbuilder2.MoveToElement(TargetTO.CurrentTO, 2, x2, y2, speed);
                        //actionbuilder2.Release();
                        //actionbuilder2.Perform();
                        return;
                    }

                    if (method == 2)
                    {
                        if (scrollable_TO != null)
                        {
                            ScrollIntoView();
                            ScrollBy(0, y1, 2);
                        }

                        //ActionsEx actions = new ActionsEx(BrowserTestObject.CurrentDriver);
                        //((ActionsEx)((ActionsEx)((ActionsEx)((ActionsEx)actions.MoveToElement(CurrentTO, x1, y1)).Sleep(speed).ClickAndHold()).Sleep(speed).MoveToElement(TargetTO.CurrentTO, x2, y2)).Sleep(speed).Release()).Sleep(speed).Build().Perform();
                        return;
                    }

                    if (method == 4)
                    {
                        if (scrollable_TO != null)
                        {
                            ScrollIntoView();
                            ScrollBy(0, y1, 2);
                        }

                        int CurrentTO_screen_x = GetCenterScreenX() + x1;
                        int CurrentTO_screen_y = GetCenterScreenY() + y1;
                        //Mouse.Move(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);
                        //Mouse.LeftButtonDown(CurrentTO_screen_x, CurrentTO_screen_y, speed * 1000);

                        if (scrollable_TO != null)
                        {
                            // The 3 lines below are specifically for IE only.
                            //  In IE objects being dragged can shift away from the mouse if scrolled off screen at the same time
                            //  Below is a workaround to first drag the object towards it's destination, to avoid being scrolled off screen.
                            //  The normal scroll and drag below then repositions the object in the correct place (as normal)
                            int TargetTO_screen_x2 = TargetTO.GetScreenX() + x2;
                            int TargetTO_screen_y2 = TargetTO.GetScreenY() + y2;
                            //Mouse.Move(TargetTO_screen_x2, TargetTO_screen_y2, speed * 1000);

                            TargetTO.ScrollIntoView();
                            scrollable_TO.ScrollBy(0, y2, 2);
                        }

                        int TargetTO_screen_x = TargetTO.GetScreenX() + x2;
                        int TargetTO_screen_y = TargetTO.GetScreenY() + y2;
                        //Mouse.Move(TargetTO_screen_x, TargetTO_screen_y, speed * 1000);
                        //Mouse.LeftButtonUp(TargetTO_screen_x, TargetTO_screen_y, speed * 1000);
                        return;
                    }
                }
            }

            // Sean G - 10 Aug - additional screenshot added to investigate intermittent drag and drop failures, particularly
            //  across different platforms
            //"screenshot browser".LogDebug();

        }


        public String ExecuteScript(String javascript)
        {
            return (String)(BrowserTestObject.CurrentDriver).ExecuteScript(javascript, CurrentTO);
        }




        public void DragAndDropJS(WebTestObject TargetTO, int x1 = 0, int y1 = 0, int x2 = 0, int y2 = 0, double speed = 0, bool scroll_TO = false)
        {
            (BrowserTestObject.CurrentDriver).ExecuteScript(
                "function createEvent(typeOfEvent) {\n" +
                "var event =document.createEvent(\"CustomEvent\");\n" +
                "event.initCustomEvent(typeOfEvent,true, true, null);\n" +
                "event.dataTransfer = {\n" +
                "data: {},\n" +
                "setData: function (key, value) {\n" +
                "this.data[key] = value;\n" +
                "},\n" +
                "getData: function (key) {\n" +
                "return this.data[key];\n" +
                "}\n" +
                "};\n" +
                "return event;\n" +
                "}\n" +
                "\n" +
                "function dispatchEvent(element, event,transferData) {\n" +
                "if (transferData !== undefined) {\n" +
                "event.dataTransfer = transferData;\n" +
                "}\n" +
                "if (element.dispatchEvent) {\n" +
                "element.dispatchEvent(event);\n" +
                "} else if (element.fireEvent) {\n" +
                "element.fireEvent(\"on\" + event.type, event);\n" +
                "}\n" +
                "}\n" +
                "\n" +
                "function simulateHTML5DragAndDrop(element, destination) {\n" +
                "var dragStartEvent =createEvent('dragstart');\n" +
                "dispatchEvent(element, dragStartEvent);\n" +
                "var dropEvent = createEvent('drop');\n" +
                "dispatchEvent(destination, dropEvent,dragStartEvent.dataTransfer);\n" +
                "var dragEndEvent = createEvent('dragend');\n" +
                "dispatchEvent(element, dragEndEvent,dropEvent.dataTransfer);\n" +
                "}\n" +
                "\n" +
                "var source = arguments[0];\n" +
                "var destination = arguments[1];\n" +
                "simulateHTML5DragAndDrop(source,destination);", CurrentTO, TargetTO.CurrentTO);



        }





        //--------------------------------------------------------------------------------
        //	State Retrieval methods
        //--------------------------------------------------------------------------------


        // ===============================================================================
        // Name...........:	Displayed()
        // Description....:	Is the test object displayed.
        // Syntax.........:	Displayed()
        // Parameters.....:	None.
        // Return values..: True    = the test object is displayed.
        //                  False   = the test object is not displayed.
        // Remarks........:	None.
        ///                 <summary>Is the test object displayed</summary>
        // ==========================================================================================
        public bool Displayed()
        {
            return CurrentTO.Displayed;
        }

        public bool Enabled()
        {
            return CurrentTO.Enabled;
        }


        //--------------------------------------------------------------------------------
        //	Wait methods
        //--------------------------------------------------------------------------------


        // ===============================================================================
        // Name...........:	WaitUntilClickable()
        // Description....:	Waits until the test object is clickable.
        // Syntax.........:	WaitUntilClickable()
        // Parameters.....:	None.
        // Return values..: None.
        // Remarks........:	None.
        ///                 <summary>Waits for the test object to be clickable</summary>
        // ==========================================================================================
        public void WaitUntilClickable()
        {
            // Re-attempt clickable check until no Selenium exceptions occur, or timeout occurs
            "clickable".StartTimer();

            while ("clickable".GetTimer() < 10)
            {
                try
                {
                    //new WebDriverWait(BrowserTestObject.CurrentDriver, TimeSpan.FromSeconds(10)).Until(ExpectedConditions.ElementToBeClickable(CurrentTO));
                    return;
                }
                catch (WebDriverTimeoutException)
                {
                    ("EXCESSIVE FIND TIME. Initiating page refresh ...").LogDebug();
                    BrowserTestObject.Refresh();
                    BrowserTestObject.WaitUntilReady();
                }
            }

            throw new System.Exception("Timed out waiting for the WebTestObject to be clickable");
        }

        // ===============================================================================
        // Name...........:	GetScreenX()
        // Description....:	Gets the X location of the top left corner of the test object relative to the screen.
        // Syntax.........:	GetScreenX()
        // Parameters.....:	None.
        // Return values..: None.
        // Remarks........:	None.
        //                  <summary>Waits for the test object to be clickable</summary>
        // ==========================================================================================

        public int GetScreenX()
        {
            // the following is for Chrome only

            //return BrowserTestObject.CurrentViewportLeft + CurrentTO.Location.X + CurrentTO.Size.Width / 2;

            // the following is for IE (maybe Chrome???)

            return BrowserTestObject.CurrentViewportLeft + GetViewportX();
        }

        // ===============================================================================
        // Name...........:	GetScreenY()
        // Description....:	Gets the Y location of the top left corner of the test object relative to the screen.
        // Syntax.........:	GetScreenY()
        // Parameters.....:	None.
        // Return values..: None.
        // Remarks........:	None.
        //                  <summary>Waits for the test object to be clickable</summary>
        // ==========================================================================================

        public int GetScreenY()
        {
            // the following is for Chrome only

            //            return BrowserTestObject.CurrentViewportTop + CurrentTO.Location.Y + CurrentTO.Size.Height / 2;

            // the following is for IE (maybe Chrome???)

            return BrowserTestObject.CurrentViewportTop + GetViewportY();
        }

        // ===============================================================================
        // Name...........:	GetCenterScreenX()
        // Description....:	Gets the X location of the center of the test object relative to the screen.
        // Syntax.........:	GetCenterScreenX()
        // Parameters.....:	None.
        // Return values..: None.
        // Remarks........:	None.
        //                  <summary>Waits for the test object to be clickable</summary>
        // ==========================================================================================

        public int GetCenterScreenX()
        {
            // the following is for Chrome only

            //return BrowserTestObject.CurrentViewportLeft + CurrentTO.Location.X + CurrentTO.Size.Width / 2;

            // the following is for IE (maybe Chrome???)

            return BrowserTestObject.CurrentViewportLeft + GetViewportX() + GetWidth() / 2;
        }

        // ===============================================================================
        // Name...........:	GetCenterScreenY()
        // Description....:	Gets the Y location of the center of the test object relative to the screen.
        // Syntax.........:	GetCenterScreenY()
        // Parameters.....:	None.
        // Return values..: None.
        // Remarks........:	None.
        //                  <summary>Waits for the test object to be clickable</summary>
        // ==========================================================================================

        public int GetCenterScreenY()
        {
            // the following is for Chrome only

            //            return BrowserTestObject.CurrentViewportTop + CurrentTO.Location.Y + CurrentTO.Size.Height / 2;

            // the following is for IE (maybe Chrome???)

            return BrowserTestObject.CurrentViewportTop + GetViewportY() + GetHeight() / 2;
        }

        public int GetDomX()
        {
            return ((ILocatable)CurrentTO).Coordinates.LocationInDom.X;
        }

        public int GetDomY()
        {
            return ((ILocatable)CurrentTO).Coordinates.LocationInDom.Y;
        }

        public int GetViewportX()
        {
            return ((ILocatable)CurrentTO).Coordinates.LocationInViewport.X;
        }

        public int GetViewportY()
        {
            return ((ILocatable)CurrentTO).Coordinates.LocationInViewport.Y;
        }





        // ===============================================================================
        // Name...........:	GetX()
        // Description....:	Get the horizontal (X) position of the test object.
        // Syntax.........:	GetX()
        // Parameters.....:	None.
        // Return values..: the horizontal (X) position of the test object.
        // Remarks........:	None.
        ///                 <summary>the horizontal position of the test object</summary>
        // ==========================================================================================
        public int GetX()
        {
            return CurrentTO.Location.X;
        }

        // ===============================================================================
        // Name...........:	GetY()
        // Description....:	Get the vertical (Y) position of the test object.
        // Syntax.........:	GetY()
        // Parameters.....:	None.
        // Return values..: the vertical (Y) position of the test object.
        // Remarks........:	None.
        ///                 <summary>the vertical position of the test object</summary>
        // ==========================================================================================
        public int GetY()
        {
            return CurrentTO.Location.Y;
        }


        //--------------------------------------------------------------------------------
        //	Text Retrieval methods
        //--------------------------------------------------------------------------------


        // ===============================================================================
        // Name...........:	GetText()
        // Description....:	Get the text of the test object.
        // Syntax.........:	GetText()
        // Parameters.....:	retry                               - Optional: if an exception occurs then keep retrying until timeout.
        //                  ensure_object_is_visible_first      - Optional: should the test object be scrolled into view first.
        // Return values..: The text of the test object.
        // Remarks........:	This method returns null immediately on a StaleElementReferenceException
        //                  rather than retrying like other exceptions, because a stale element at this point
        //                  cannot be resolved because the test object (element) is already captured.
        //                  The calling method should instead detect the "null" return and respond accordingly
        //                  (ie. re-attempt the Find call above to update the latest test object).
        ///                 <summary>Get the text of the test object, and 'retry' if exception</summary>
        // ==========================================================================================
        public string GetText(bool retry = true, bool ensure_object_is_visible_first = false)
        {
            int next_idle_refresh_timeout = 10;

            // Re-attempt until no Selenium exceptions occur, or timeout occurs
            "GetText".StartTimer();

            while ("GetText".GetTimer() < 10)
            {
                try
                {
                    if (ensure_object_is_visible_first)

                        (BrowserTestObject.CurrentDriver).ExecuteScript("arguments[0].scrollIntoView(true);", CurrentTO);

                    return CurrentTO.Text;
                }
                catch (ElementNotVisibleException e)
                {
                    "ElementNotVisibleException during GetText()".LogDebug();
                }
                catch (StaleElementReferenceException e)
                {
                    "StaleElementReferenceException during GetText()".LogDebug();
                    return null;
                }

                (0.25).Sleep(0);

                if (retry == false)

                    break;

                if ("GetText".GetTimer() > next_idle_refresh_timeout)
                {
                    next_idle_refresh_timeout = next_idle_refresh_timeout + 10;
                    ("EXCESSIVE FIND TIME. Initiating page refresh ...").LogDebug();
                    BrowserTestObject.Refresh();
                    BrowserTestObject.WaitUntilReady();
                    CurrentTO = find(OriginTO, CurrentBy).CurrentTO;
                }
            }

            return null;
        }

        // ===============================================================================
        // Name...........:	GetValue()
        // Description....:	Gets the value of the test object.
        // Syntax.........:	GetValue(int method = 1)
        // Parameters.....:	method		- Optional: A number denoting the method to use to get the value.
        //								    1 = via JQuery value attribute (default)
        //                                  2 = via Javascript value attribute
        // Return values..: The value of the test object.
        // Remarks........:	None.
        // ==========================================================================================
        public Object GetValue(int method = 1)
        {
            switch (method)
            {
                case 1:

                    return (Object)(BrowserTestObject.CurrentDriver).ExecuteScript("return $(arguments[0]).value", CurrentTO);

                case 2:

                    return (Object)(BrowserTestObject.CurrentDriver).ExecuteScript("return arguments[0].value;", CurrentTO);
            }

            return null;
        }

        // ===============================================================================
        // Name...........:	Selected()
        // Description....:	Is the test object selected.
        // Syntax.........:	Selected()
        // Parameters.....:	None.
        // Return values..: True    = the test object is selected.
        //                  False   = the test object is not selected.
        // Remarks........:	None.
        ///                 <summary>Is the test object selected</summary>
        // ==========================================================================================
        public bool Selected()
        {
            return CurrentTO.Selected;
        }

        // ===============================================================================
        // Name...........:	Select()
        // Description....:	Select the test object.
        // Syntax.........:	Select()
        // Parameters.....:	None.
        // Return values..: None.
        // Remarks........:	None.
        ///                 <summary>Select the test object</summary>
        // ==========================================================================================
        public void Select(int method = 1, bool ensure_object_is_visible_first = false)
        {
            int next_idle_refresh_timeout = 10;

            // Re-attempt until no Selenium exceptions occur, or timeout occurs
            "Select".StartTimer();

            while ("Select".GetTimer() < 10)
            {
                try
                {
                    if (ensure_object_is_visible_first)

                        (BrowserTestObject.CurrentDriver).ExecuteScript("arguments[0].scrollIntoView(true);", CurrentTO);

                    if (!CurrentTO.Selected)
                    {
                        CurrentTO.Click();
                    }

                    return;
                }
                catch (ElementNotVisibleException e)
                {
                    "ElementNotVisibleException during Select()".LogDebug();
                }
                catch (StaleElementReferenceException e)
                {
                    "StaleElementReferenceException during Select()".LogDebug();
                }

                (0.25).Sleep(0);

                if ("Select".GetTimer() > next_idle_refresh_timeout)
                {
                    next_idle_refresh_timeout = next_idle_refresh_timeout + 10;
                    ("EXCESSIVE FIND TIME. Initiating page refresh ...").LogDebug();
                    BrowserTestObject.Refresh();
                    BrowserTestObject.WaitUntilReady();
                    CurrentTO = find(OriginTO, CurrentBy).CurrentTO;
                }
            }

            throw new System.Exception("Timed out trying to select a WebTestObject");
        }

        // ===============================================================================
        // Name...........:	DeSelect()
        // Description....:	De-select the test object.
        // Syntax.........:	DeSelect()
        // Parameters.....:	None.
        // Return values..: None.
        // Remarks........:	None.
        ///                 <summary>De-select the test object</summary>
        // ==========================================================================================
        public void DeSelect(int method = 1, bool ensure_object_is_visible_first = false)
        {
            int next_idle_refresh_timeout = 10;

            // Re-attempt until no Selenium exceptions occur, or timeout occurs
            "DeSelect".StartTimer();

            while ("DeSelect".GetTimer() < 10)
            {
                try
                {
                    if (ensure_object_is_visible_first)

                        (BrowserTestObject.CurrentDriver).ExecuteScript("arguments[0].scrollIntoView(true);", CurrentTO);

                    if (CurrentTO.Selected)
                    {
                        CurrentTO.Click();
                    }

                    return;
                }
                catch (ElementNotVisibleException e)
                {
                    "ElementNotVisibleException during DeSelect()".LogDebug();
                }
                catch (StaleElementReferenceException e)
                {
                    "StaleElementReferenceException during DeSelect()".LogDebug();
                }

                (0.25).Sleep(0);

                if ("DeSelect".GetTimer() > next_idle_refresh_timeout)
                {
                    next_idle_refresh_timeout = next_idle_refresh_timeout + 10;
                    ("EXCESSIVE FIND TIME. Initiating page refresh ...").LogDebug();
                    BrowserTestObject.Refresh();
                    BrowserTestObject.WaitUntilReady();
                    CurrentTO = find(OriginTO, CurrentBy).CurrentTO;
                }
            }

            throw new System.Exception("Timed out trying to deselect a WebTestObject");
        }

        // ===============================================================================
        // Name...........:	ScrollIntoView()
        // Description....:	Scroll the test object into view.
        // Syntax.........:	ScrollIntoView()
        // Parameters.....:	None.
        // Return values..: None.
        // Remarks........:	None.
        ///                 <summary>Scroll the test object into view</summary>
        // ==========================================================================================
        public void ScrollIntoView()
        {
            int next_idle_refresh_timeout = 10;

            // Re-attempt scroll until no Selenium exceptions occur, or timeout occurs
            "ScrollIntoView".StartTimer();

            while ("ScrollIntoView".GetTimer() < 10)
            {
                try
                {
                    (BrowserTestObject.CurrentDriver).ExecuteScript("arguments[0].scrollIntoView(true);", CurrentTO);
                    return;
                }
                catch (ElementNotVisibleException e)
                {
                    "ElementNotVisibleException during ScrollIntoView()".LogDebug();
                }
                catch (StaleElementReferenceException e)
                {
                    "StaleElementReferenceException during ScrollIntoView()".LogDebug();
                }

                (0.25).Sleep(0);

                if ("ScrollIntoView".GetTimer() > next_idle_refresh_timeout)
                {
                    next_idle_refresh_timeout = next_idle_refresh_timeout + 10;
                    ("EXCESSIVE FIND TIME. Initiating page refresh ...").LogDebug();
                    BrowserTestObject.Refresh();
                    BrowserTestObject.WaitUntilReady();
                    CurrentTO = find(OriginTO, CurrentBy).CurrentTO;
                }
            }

            throw new System.Exception("Timed out trying to scroll into view a WebTestObject");
        }

        public void ScrollTop(int y)
        {
            (BrowserTestObject.CurrentDriver).ExecuteScript("arguments[0].scrollTop = " + y + ";", CurrentTO);
        }


        public int GetScrollTop()
        {
            return Convert.ToInt32((BrowserTestObject.CurrentDriver).ExecuteScript("return arguments[0].scrollTop;", CurrentTO));
        }


        public void ScrollBy(int x = 0, int y = 0, int method = 1)
        {
            switch (method)
            {
                case 1:

                    (BrowserTestObject.CurrentDriver).ExecuteScript("arguments[0].scrollBy(" + x + "," + y + ");", CurrentTO);
                    break;

                case 2:

                    if (x != 0)

                        (BrowserTestObject.CurrentDriver).ExecuteScript("arguments[0].scrollLeft = arguments[0].scrollLeft + " + x + ";", CurrentTO);

                    if (y != 0)

                        (BrowserTestObject.CurrentDriver).ExecuteScript("arguments[0].scrollTop = arguments[0].scrollTop + " + y + ";", CurrentTO);

                    break;
            }
        }



        /*
                public void SelectByText(String text)
                {

                    SelectElement sb = new SelectElement(a);

                    sb.SelectByValue("AssociateInteraction");

                    if (sb.Options.Count == 1)

                        a.Click();
                    else

                        for (int i = 0; i < sb.Options.Count; i++)
                        {
                            if (sb.Options[i].Text.Equals(text))
                            {
                                // If the list is scrolling the following will scroll the list to make the option visible
                                (BrowserTestObject.CurrentDriver).ExecuteScript("arguments[0].selectedIndex=" + i + ";", a);

                                // The following clicks on the option
                                sb.Options[i].Click();
                                (0.25).Sleep(0);
                                break;
                            }
                        }
                }
                */

        // ===============================================================================
        // Name...........:	SelectByValue()
        // Description....:	Select a test object based on it's value.
        // Syntax.........:	SelectByValue()
        // Parameters.....:	value		- the value to select.
        // Return values..: None.
        // Remarks........:	None.
        ///                 <summary>Select a test object based on it's 'value'</summary>
        // ==========================================================================================
        public void SelectByValue(String value)
        {
            (BrowserTestObject.CurrentDriver).ExecuteScript("arguments[0].scrollIntoView(true);", CurrentTO);
            //(new SelectElement(CurrentTO)).SelectByValue(value);
        }

        // The following "SelectByText" method is a re-creation of the same method that already exists in the C# WebDriver.Support
        //  class.  The reason I am re-creating it here is because the Selenium Grid (selenium-server-standalone-*.jar)
        //  has no knowledge of the C# WebDriver.Support classes.  By re-posting the WebDriver.Support source code below I give
        //  the Selenium Grid these features.

        public void SelectByText(string text, bool partialMatch = false)
        {
            if (text == null)

                throw new ArgumentNullException("text", "text must not be null");

            bool matched = false;
            IList<IWebElement> options;

            if (!partialMatch)
            {
                // try to find the option via XPATH ...
                options = CurrentTO.FindElements(By.XPath(".//option[normalize-space(.) = " + EscapeQuotes(text) + "]"));
            }
            else
            {
                options = CurrentTO.FindElements(By.XPath(".//option[contains(normalize-space(.),  " + EscapeQuotes(text) + ")]"));
            }

            foreach (IWebElement option in options)
            {
                SetSelected(option, true);
                if (!this.IsMultiple)
                {
                    return;
                }

                matched = true;
            }

            if (options.Count == 0 && text.Contains(" "))
            {
                string substringWithoutSpace = GetLongestSubstringWithoutSpace(text);
                IList<IWebElement> candidates;
                if (string.IsNullOrEmpty(substringWithoutSpace))
                {
                    // hmm, text is either empty or contains only spaces - get all options ...
                    candidates = CurrentTO.FindElements(By.TagName("option"));
                }
                else
                {
                    // get candidates via XPATH ...
                    candidates = CurrentTO.FindElements(By.XPath(".//option[contains(., " + EscapeQuotes(substringWithoutSpace) + ")]"));
                }

                foreach (IWebElement option in candidates)
                {
                    if (text == option.Text)
                    {
                        SetSelected(option, true);
                        if (!this.IsMultiple)
                        {
                            return;
                        }

                        matched = true;
                    }
                }
            }

            if (!matched)

                throw new NoSuchElementException("Cannot locate element with text: " + text);
        }


        private static string EscapeQuotes(string toEscape)
        {
            // Convert strings with both quotes and ticks into: foo'"bar -> concat("foo'", '"', "bar")
            if (toEscape.IndexOf("\"", StringComparison.OrdinalIgnoreCase) > -1 && toEscape.IndexOf("'", StringComparison.OrdinalIgnoreCase) > -1)
            {
                bool quoteIsLast = false;
                if (toEscape.LastIndexOf("\"", StringComparison.OrdinalIgnoreCase) == toEscape.Length - 1)
                {
                    quoteIsLast = true;
                }

                List<string> substrings = new List<string>(toEscape.Split('\"'));
                if (quoteIsLast && string.IsNullOrEmpty(substrings[substrings.Count - 1]))
                {
                    // If the last character is a quote ('"'), we end up with an empty entry
                    // at the end of the list, which is unnecessary. We don't want to split
                    // ignoring *all* empty entries, since that might mask legitimate empty
                    // strings. Instead, just remove the empty ending entry.
                    substrings.RemoveAt(substrings.Count - 1);
                }

                StringBuilder quoted = new StringBuilder("concat(");
                for (int i = 0; i < substrings.Count; i++)
                {
                    quoted.Append("\"").Append(substrings[i]).Append("\"");
                    if (i == substrings.Count - 1)
                    {
                        if (quoteIsLast)
                        {
                            quoted.Append(", '\"')");
                        }
                        else
                        {
                            quoted.Append(")");
                        }
                    }
                    else
                    {
                        quoted.Append(", '\"', ");
                    }
                }

                return quoted.ToString();
            }

            // Escape string with just a quote into being single quoted: f"oo -> 'f"oo'
            if (toEscape.IndexOf("\"", StringComparison.OrdinalIgnoreCase) > -1)
            {
                return string.Format(CultureInfo.InvariantCulture, "'{0}'", toEscape);
            }

            // Otherwise return the quoted string
            return string.Format(CultureInfo.InvariantCulture, "\"{0}\"", toEscape);
        }


        private static void SetSelected(IWebElement option, bool select)
        {
            bool isSelected = option.Selected;
            if ((!isSelected && select) || (isSelected && !select))
            {
                option.Click();
            }
        }

        public bool IsMultiple { get; private set; }

        private static string GetLongestSubstringWithoutSpace(string s)
        {
            string result = string.Empty;
            string[] substrings = s.Split(' ');
            foreach (string substring in substrings)
            {
                if (substring.Length > result.Length)
                {
                    result = substring;
                }
            }

            return result;
        }


        //--------------------------------------------------------------------------------
        //	Frame related methods
        //--------------------------------------------------------------------------------

        // Switch control back to the main browser window

        public void SwitchToFrame()
        {
            BrowserTestObject.SetCurrentDriverIfNotSet(true);
            BrowserTestObject.CurrentDriver.SwitchTo().Frame(CurrentTO);
        }
        public void SwitchTab()
        {
            var browserTabs = BrowserTestObject.CurrentDriver.WindowHandles;
            BrowserTestObject.CurrentDriver.SwitchTo().Window(browserTabs[1]);
        }
        public System.Drawing.Bitmap Screenshot(int x_offset = 0, int y_offset = 0, int width = -1, int height = -1, int zero_width_override = -1, int zero_height_override = -1)
        {
            Bitmap target = null;

            try
            {
                var result = BrowserTestObject.CurrentDriver.ExecuteScript(
                     "var elm = arguments[0];" +
                     "elm.scrollIntoView(true);" +
                     "var rect = elm.getBoundingClientRect();" +
                     "return [rect.left, rect.top, rect.width, rect.height];"
                     , CurrentTO);

                int[] pts = Array.ConvertAll(((IReadOnlyCollection<object>)result).ToArray(), Convert.ToInt32);

                int x = pts[0] + x_offset;
                int y = pts[1] + y_offset;

                if (width < 0)

                    width = pts[2];

                if (height < 0)

                    height = pts[3];

                if (width <= 0 && zero_width_override > -1)

                    width = zero_width_override;

                if (height <= 0 && zero_height_override > -1)

                    height = zero_height_override;

                var rect = new Rectangle(x, y, width, height);
                Screenshot screenshot = BrowserTestObject.CurrentDriver.GetScreenshot();

                using (var mstream = new MemoryStream(screenshot.AsByteArray))
                using (var bitmap = (Bitmap)System.Drawing.Image.FromStream(mstream, false, false))
                {
                    rect.Intersect(new Rectangle(0, 0, bitmap.Width, bitmap.Height));

                    if (rect.IsEmpty)

                        throw new ArgumentOutOfRangeException("Cropping rectangle is out of range.");

                    target = bitmap.Clone(rect, bitmap.PixelFormat);
                }
            }
            catch (Exception e)
            {
                ("Screenshot of the current test object failed.").LogDebug();
                return null;
            }

            return target;
        }

        /*
        public Rectangle FindImageRectangle(String ImageFilePath)
        {
            System.Drawing.Bitmap screenshot = Screenshot(0, 0, -1, -1);
            Rectangle result = (Rectangle)Toolkit.Image.InImage(screenshot, ImageFilePath, 1);
            return result;
        }

        // ===============================================================================
        // Name...........:	FindImagePoint()
        // Description....:	Gets the value of the test object.
        // Syntax.........:	FindImagePoint(String ImageFilePath, int screenshot_x_offset = 0, int screenshot_y_offset = 0, int width = -1, int height = -1, int zero_width_override = -1, int zero_height_override = -1, String SecondImageFilePath = "")
        // Parameters.....:	ImageFilePath		    - The path to an image (file) to find within the test object
        //                  screenshot_x_offset     - Optional: an offset (in pixels) from the left of the test object to search from
        //                                              0 = ignore the offset and search from the left of the test object
        //                  screenshot_y_offset     - Optional: an offset (in pixels) from the top of the test object to search from
        //                                              0 = ignore the offset and search from the top of the test object
        //                  width                   - Optional: a width (in pixels) inside the test object to search
        //                                              0 = ignore the offset and search the full width of the test object
        //                  height                  - Optional: a width (in pixels) inside the test object to search
        //                                              0 = ignore the offset and search the full height of the test object
        //                  zero_width_override     - Optional: 
        //                  zero_height_override    - Optional: 
        //                  AlternateImageFilePath  - Optional: the path to an additional image (file) to find within the test object .
        //                                              If provided this image will be also be searched for and if either
        //                                              "ImageFilePath" or "AlternateImageFilePath" are found this will suceed
        // Return values..: The point at where the image is found in the test object.
        // Remarks........:	None.
        // ==========================================================================================

        public Point FindImagePoint(String ImageFilePath, int screenshot_x_offset = 0, int screenshot_y_offset = 0, int width = -1, int height = -1, int zero_width_override = -1, int zero_height_override = -1, String AdditionalImageFilePath = "")
        {
            // Re-attempt find until the image is found, or timeout occurs
            "FindImagePoint".StartTimer();

            while ("FindImagePoint".GetTimer() < 10)
            {
                FindImagePointNum++;
                var screenshot_file_path = "R:\\" + Test_Environment.test_case_script_name + " FindImagePoint " + FindImagePointNum + ".jpg";
                System.Drawing.Bitmap screenshot = Screenshot(screenshot_x_offset, screenshot_y_offset, width, height, zero_width_override, zero_height_override);

                if (screenshot != null)
                {
                    Point result = (Point)Toolkit.Image.InImage(screenshot, ImageFilePath, 2);

                    // If the point is populated (ie. it is not the default of Empty or 0,0), then return that
                    if (!result.Equals(Point.Empty))
                    {
                        // Debug
                        ("Found " + ImageFilePath + " in screenshot. See screenshot at " + screenshot_file_path).LogDebug();
                        screenshot.Save(screenshot_file_path, ImageFormat.Jpeg);
                        return result;
                    }

                    ("Failed to find " + ImageFilePath + " in screenshot. See screenshot at " + screenshot_file_path).LogDebug();
                    screenshot.Save(screenshot_file_path, ImageFormat.Jpeg);

                    // If an additional image path is provided then repeat the find for that image ...

                    if (!AdditionalImageFilePath.Equals(""))
                    {
                        FindImagePointNum++;
                        screenshot_file_path = "R:\\" + Test_Environment.test_case_script_name + " FindImagePoint " + FindImagePointNum + ".jpg";
                        result = (Point)Toolkit.Image.InImage(screenshot, AdditionalImageFilePath, 2);

                        // If the point is populated (ie. it is not the default of Empty or 0,0), then return that
                        if (!result.Equals(Point.Empty))
                        {
                            // Debug
                            ("Found " + AdditionalImageFilePath + " in screenshot. See screenshot at " + screenshot_file_path).LogDebug();
                            screenshot.Save(screenshot_file_path, ImageFormat.Jpeg);
                            return result;
                        }

                        ("Failed to find " + AdditionalImageFilePath + " in screenshot. See screenshot at " + screenshot_file_path).LogDebug();
                        screenshot.Save(screenshot_file_path, ImageFormat.Jpeg);
                    }
                }

                // Otherwise continually loop until it's populated
                1.Sleep(0);
            }

            return Point.Empty;
        }

        // Find an image within this test object from it's center and outwards a "width" and "height" in pixels, and optional
        //  offset from it's center provided

        public Point FindImagePoint2(String ImageFilePath, int width, int height, int x_center_offset = 0, int y_center_offset = 0)
        {
            // Re-attempt find until the image is found, or timeout occurs
            "FindImagePoint".StartTimer();

            while ("FindImagePoint".GetTimer() < 10)
            {

                int x_center = CurrentTO.Location.X + (CurrentTO.Size.Width / 2);
                int y_center = CurrentTO.Location.Y + (CurrentTO.Size.Height / 2);

                // Take a screenshot of a test object with dimensions extending outwards a given "width" and "height" from it's center

                System.Drawing.Bitmap screenshot = BrowserTestObject.Screenshot((x_center + x_center_offset) - (width / 2), (y_center + y_center_offset) - (height / 2), width, height);
                //                screenshot.Save("R:\\fred.jpg", ImageFormat.Jpeg);

                Point result = (Point)Toolkit.Image.InImage(screenshot, ImageFilePath, 2);

                // If the point is populated (ie. it is not the default of Empty or 0,0), then return that
                if (!result.Equals(Point.Empty))

                    return result;

                // Otherwise continually loop until it's populated
                1.Sleep(0);
            }

            return Point.Empty;
        }

        public Point FindImagePointUntilDisplayed(String ImageFilePath, int screenshot_x_offset = 0, int screenshot_y_offset = 0, int width = -1, int height = -1)
        {
            // Re-attempt find until the image is found, or timeout occurs
            "FindImagePoint".StartTimer();

            while ("FindImagePoint".GetTimer() < 10)
            {
                System.Drawing.Bitmap screenshot = Screenshot(screenshot_x_offset, screenshot_y_offset, width, height);
                //                screenshot.Save("R:\\fred.jpg", ImageFormat.Jpeg);
                //                screenshot.Save(ImageFilePath, ImageFormat.Jpeg);

                Point result = (Point)Toolkit.Image.InImage(screenshot, ImageFilePath, 2);

                // If the point is populated (ie. it is not the default of Empty or 0,0), then return that
                if (!result.Equals(Point.Empty))

                    return result;

                // Otherwise continually loop until it's populated
                screenshot.Save("R:\\FindImagePoint failed screenshot.jpg", ImageFormat.Jpeg);
                ("FindImagePoint failed to find " + ImageFilePath + " in R:\\FindImagePoint failed screenshot.jpg").LogDebug();
                2.Sleep(0);
            }

            return Point.Empty;
        }
        */



    }
}
