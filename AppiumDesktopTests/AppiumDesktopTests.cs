using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace AppiumDesktopTests
{
    public class AppiumDesktopTests
    {
        private WindowsDriver<WindowsElement> driver;
        private AppiumOptions options;

        private const string appLocation = @"C:\Projects\Exam Preparation\ShortURL-DesktopClient-v1.0.net6\ShortURL-DesktopClient.exe";
        private const string appiumServer = "http://127.0.0.1:4723/wd/hub";
        private const string appServer = "https://shorturl.viktoriyaboneva.repl.co/api";

        [SetUp]

        public void PrepareApp()
        {
            this.options = new AppiumOptions();
            options.AddAdditionalCapability("app", appLocation);
            driver = new WindowsDriver<WindowsElement>(new Uri(appiumServer), options); 
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown]

        public void CloseApp() 
        {
            driver.Quit(); 
        }

        [Test]

        public void Test_AddNewUrl() 
        {
             var newUrl = "https://url" + DateTime.Now.Ticks + ".com";

            //change the URL of the BE
            var inputApiUrl = driver.FindElementByAccessibilityId("textBoxApiUrl");
            inputApiUrl.Clear();   
            inputApiUrl.SendKeys(appServer); 

            //press connect button
            var buttonConnect = driver.FindElementByAccessibilityId("buttonConnect");
            buttonConnect.Click(); 

            //press add button
            var buttonAdd = driver.FindElementByAccessibilityId("buttonAdd");
            buttonAdd.Click();

            //fill the URL field
            var textBoxURL = driver.FindElementByAccessibilityId("textBoxURL");
            textBoxURL.SendKeys(newUrl);

            //press create button
            var buttonCreate = driver.FindElementByAccessibilityId("buttonCreate");
            buttonCreate.Click();

            var resultFrield = driver.FindElementByName(newUrl);

            Assert.IsNotEmpty(resultFrield.Text);
            Assert.That(resultFrield.Text, Is.EqualTo(newUrl)); 
        }
    }
}