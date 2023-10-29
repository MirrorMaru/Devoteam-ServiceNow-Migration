using System;
using System.Net.Http;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace Devoteam_ServiceNow_Migration;

public class Export
{
    private EdgeDriverService service = EdgeDriverService.CreateDefaultService();
    private EdgeOptions option = new EdgeOptions();
    private EdgeDriver driver;

    private readonly string _address;

    public Export(string address, string username, string pass, MainWindow main)
    {
        _address = address;
        
        //option.AddArgument("--headless=new");
        this.driver = new EdgeDriver(service, option, TimeSpan.FromMinutes(20));
        driver.Navigate().GoToUrl(_address);

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(20);

        IWebElement login = driver.FindElement(By.XPath("//*[@id=\"user_name\"]"));
        IWebElement password = driver.FindElement(By.XPath("//*[@id=\"user_password\"]"));
        
        login.SendKeys(username);
        password.SendKeys(pass);

        IWebElement connect = driver.FindElement(By.XPath("//*[@id=\"sysverb_login\"]"));
        connect.Click();
    }

    private static ShadowRoot getShadowRoot(WebDriver driver, IWebElement shadowHost)
    {
        IJavaScriptExecutor js = (IJavaScriptExecutor) driver;
        return (ShadowRoot)js.ExecuteScript("return arguments[0].shadowRoot", shadowHost);
    }

    public void SendFiles(FilesInitialization files)
    {
        foreach (var VARIABLE in files.files)
        {
            driver.Navigate().GoToUrl(_address+"now/nav/ui/classic/params/target/"+"upload.do%3Fsysparm_referring_url%3D"+VARIABLE.name+"_list.do%26sysparm_target%3D"+VARIABLE.name);

            IWebElement shadowHost =
                driver.FindElement(By.CssSelector("body > macroponent-f51912f4c700201072b211d4d8c26010"));

            ShadowRoot shadowRoot_1 = getShadowRoot(driver, shadowHost);

            IWebElement shadowTreeElement_1 = shadowRoot_1.FindElement(By.CssSelector("#gsft_main"));

            driver.SwitchTo().Frame(shadowTreeElement_1);
            
            driver.FindElement(By.XPath("//*[@id=\"attachFile\"]")).SendKeys(Environment.CurrentDirectory + "/out/" + VARIABLE.folder + "/" + VARIABLE.name + ".xml");
            driver.FindElement(By.XPath("/html/body/div[2]/form/div[3]/div[2]/input")).Click();

            Console.WriteLine(VARIABLE.name + " uploaded !");
            //MainWindow.GetInstance().updateCount(); TODO : Make it works
        }
    }
}