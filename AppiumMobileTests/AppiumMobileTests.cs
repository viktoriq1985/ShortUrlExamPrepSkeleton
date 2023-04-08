using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace AppiumMobileTests
{
    public class AppiumMobileTests
    {
        private AndroidDriver<AndroidElement> driver;
        private AppiumOptions options;

        private const string appLocation = @"C:\Projects\Apk\com.android.example.github.apk";
        private const string appiumServer = "http://127.0.0.1:4723/wd/hub";

        [SetUp]
        public void prepareApp()
        {
            this.options = new AppiumOptions() { PlatformName = "Android" };
            options.AddAdditionalCapability("app", appLocation); 
            driver = new AndroidDriver<AndroidElement> (new Uri(appiumServer), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);  
        }

        [TearDown]

        public void closeApp() 
        {
            driver.Quit();
        }

        [Test]
        public void Test_VerifyBarancevName()
        {
            var inputSearcField = driver.FindElementById("com.android.example.github:id/input");
            inputSearcField.Click();
            inputSearcField.SendKeys("Selenium");

            //hit enter
            driver.PressKeyCode(AndroidKeyCode.Keycode_ENTER);

            //same as
            //driver.PressKeyCode(66);
            //inputSearcField.SendKeys("Selenium\\n");

            var textSelenium = driver.FindElementByXPath("//android.view.ViewGroup/android.widget.TextView[2]");
            Assert.That(textSelenium.Text, Is.EqualTo("SeleniumHQ/selenium"));

            textSelenium.Click();

            var listTextPerson = driver.FindElementByXPath("//android.widget.FrameLayout[2]/android.view.ViewGroup/android.widget.TextView");
            Assert.That(listTextPerson.Text, Is.EqualTo("barancev"));
            listTextPerson.Click();

            var personName = driver.FindElementByXPath("//android.widget.TextView[@content-desc=\'user name\']");
            Assert.That(personName.Text, Is.EqualTo("Alexei Barantsev")); 
        }
    }
}