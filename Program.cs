#region Dependencies
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
#endregion

namespace Kahoot_Bomb
{
    class Program
    {
        /*
         Kahoot Bomb - Built on Selenium Framework
         Developed by Centos
         I recommend you use premium HTTPS proxies otherwise the bot will be very slow
        */

        static List<string> Proxies = new List<string>();
        static string[] Usernames = { "Laz", "Centos", "TrinitySeal", "Bot1337",  new Random().Next().ToString() };
        static int GamePIN { get; set; } // This will be set at runtime

        static void Main(string[] args)
        {
            #region Start up

            // If proxies haven't been added
            if (args.Length <= 0)
            {
                Console.WriteLine(" You must drag your proxy list onto the exe!");
                Console.Read();
            }

            // Populate dictionary with proxies
            foreach(string line in File.ReadAllLines(args[0]))
            {
                Proxies.Add(line);
            }

            // Get the game pin
            Console.Write("Enter your game pin: ");
            GamePIN = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Starting bot...");
            #endregion

            Task.Run(() => StartBot());
        }

        static void StartBot()
        {
            // Loop the bot for however many proxies the user has added
            foreach (string line in Proxies)
            {
                #region Config
                // Hide command prompt window
                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;

                // Hide chrome pop-up and setup proxies
                // Incognito so I don't explode your history
                var options = new ChromeOptions();
                options.AddArguments(new string[] { "headless", "--incognito", "--log-level=3", $"--proxy-server={line}" });
                #endregion

                // Create new driver variable with our config
                IWebDriver driver = new ChromeDriver(service, options);

                // Navigate to kahoot website
                driver.Navigate().GoToUrl("https://kahoot.it");

                // Give the game pin textbox time to pop up
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

                // Find the 'Game PIN' textbox
                IWebElement textbox = driver.FindElement(By.Id("game-input"));

                // Clear the text box for new input
                textbox.Clear();

                // Populate text box with game pin
                textbox.SendKeys(GamePIN.ToString());

                // Hit the enter key to make the nickname box pop up
                textbox.SendKeys(Keys.Return);

                // Give the nickname box time to pop up
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

                // Find the 'Nickname' textbox
                IWebElement nickname = driver.FindElement(By.Id("nickname"));

                // Clear the nickname box for new input
                nickname.Clear();

                // Selects a random username from the array (6 because there are 6 values in the array)
                int index = new Random().Next(6);

                // Populate nickname text box with a random nickname
                nickname.SendKeys(Usernames[index]);

                // Hit the enter key to join the game
                // Voila :D
                nickname.SendKeys(Keys.Return);
            }
        }
    }
}
