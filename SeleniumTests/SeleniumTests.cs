using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using static System.Net.WebRequestMethods;

namespace SeleniumTests
{
    public class SeleniumTests
    {

        private WebDriver driver;
        private const string baseUrl = "https://shorturl.viktoriyaboneva.repl.co/";
        //private const string baseUrl = "http://localhost:8080/";

        [SetUp]
        public void OpenWebApp()
        {
            this.driver = new ChromeDriver();
            driver.Manage().Window.Maximize(); 
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Url = baseUrl;
        }

        [TearDown]
        public void CloseWebApp() 
        { 
            driver.Quit();
        }

        [Test]
        public void Test_VerifyTableTopLeftCell()
        {
            //Navigate to ShortURL page
            var linkShortUrl = driver.FindElement(By.LinkText("Short URLs"));
            linkShortUrl.Click();

            //same as driver.FindElement(By.LinkText("Short URLs")).Click();

            var tableHearderLeftCell = driver.FindElement(By.CssSelector("th:nth-child(1)"));
            Assert.That(tableHearderLeftCell.Text, Is.EqualTo("Original URL"));
        }

        [Test]
        public void Test_AddNewUrlValid()
        {
            //var exampleUrl = "https://www.wikipedia.org/";
            var urlToAdd = "http://url" + DateTime.Now.Ticks + ".com";

            //Navigate to AddURL page
            var linkAddUrl = driver.FindElement(By.XPath("//a[contains(@href, 'add')]"));
            linkAddUrl.Click();

            var inputAddUrl = driver.FindElement(By.Id("url"));
            inputAddUrl.SendKeys(urlToAdd);

            var buttonSubmit = driver.FindElement(By.XPath("//button[contains(@type, 'submit')]"));
            buttonSubmit.Click();

            //Assertion for the URL in the page source
            Assert.That(driver.PageSource.Contains(urlToAdd));

            var tableLastRow = driver.FindElements(By.CssSelector("table > tbody > tr")).Last();
            
            var tableLastRowFirstCell = tableLastRow.FindElements(By.CssSelector("td")).First(); 
            // same as var tableLastRowFirstCell = tableLastRow.FindElements(By.CssSelector("td"))[0]; 

            Assert.That(tableLastRowFirstCell.Text, Is.EqualTo(urlToAdd));
        }
        [Test]
        public void Test_AddNewUrlIvalid()
        {
            //Navigate to AddURL page
            var linkAddUrl = driver.FindElement(By.XPath("//a[contains(@href, 'add')]"));
            linkAddUrl.Click();

            var inputUrl = driver.FindElement(By.Id("url"));
            inputUrl.SendKeys("wikipedia");

            var buttonSubmit = driver.FindElement(By.XPath("//button[contains(@type, 'submit')]"));
            buttonSubmit.Click();

            var labelError = driver.FindElement(By.XPath("//div[contains(@class, 'err')]"));
            Assert.That(labelError.Text, Is.EqualTo("Invalid URL!"));
            Assert.True(labelError.Displayed); 
        }
        [Test]
        public void Test_NavigateToInvalidUrl()
        {
            driver.Navigate().GoToUrl("http://shorturl.nakov.repl.co/go/invalid536524");
            //same as driver.Url = "http://shorturl.nakov.repl.co/go/invalid536524";

            var labelError = driver.FindElement(By.XPath("//div[contains(@class, 'err')]"));
            Assert.That(labelError.Text, Is.EqualTo("Cannot navigate to given short URL"));
            Assert.True(labelError.Displayed);
        }
        [Test]
        public void Test_VerifyCounterIncrease()
        {
            //Navigate to ShortURL page
            var linkShortUrl = driver.FindElement(By.LinkText("Short URLs"));
            linkShortUrl.Click();

            var tableFirstRow = driver.FindElements(By.CssSelector("table > tbody > tr")).First();
            var oldCounter = int.Parse(tableFirstRow.FindElements(By.CssSelector("td")).Last().Text);

            var linkToClickCell = tableFirstRow.FindElements(By.CssSelector("td"))[1];

            var linkToClick = linkToClickCell.FindElement(By.TagName("a")); 
            linkToClick.Click();

            //switch to first tab
            driver.SwitchTo().Window(driver.WindowHandles[0]);

            driver.Navigate().Refresh();

            tableFirstRow = driver.FindElements(By.CssSelector("table > tbody > tr")).First();
            var newCounter = int.Parse(tableFirstRow.FindElements(By.CssSelector("td")).Last().Text);

            Assert.That(newCounter, Is.EqualTo(oldCounter + 1));
        }

    }
}