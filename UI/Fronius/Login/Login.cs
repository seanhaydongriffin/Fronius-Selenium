using OpenQA.Selenium;
using Toolkit.Selenium;

namespace UI.Fronius.Login
{
    class Login
    {
        public static void testMain(
            string username,
            string password
        )
        {

            WebTestObject.FindUntilDisplayed(
                null,
                new By[] {
                    By.TagName("body"),
                    By.XPath("//input[@id='usernameUserInput']")
                }
            ).SetText(username);

            WebTestObject.FindUntilDisplayed(
                null,
                new By[] {
                    By.TagName("body"),
                    By.XPath("//input[@id='password']")
                }
            ).SetText(password);

            WebTestObject.FindUntilDisplayed(
                null,
                new By[] {
                    By.TagName("body"),
                    By.XPath("//button[@id='login-button']")
                }
            ).click();

            BrowserTestObject.WaitUntilReady();
        }

        
    }
}
