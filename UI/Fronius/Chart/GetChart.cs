using OpenQA.Selenium;
using Toolkit.Selenium;

namespace UI.Fronius.Chart
{
    class GetChart
    {
        public static string testMain(
        )
        {
            return WebTestObject.FindUntilDisplayed(
                null,
                new By[] {
                    By.TagName("body"),
                    By.XPath("./pre")
                }
            ).GetText();
        }

        
    }
}
