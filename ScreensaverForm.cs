using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using Microsoft.Web.WebView2.WinForms;

namespace Web_Page_Screensaver
{
    public partial class ScreensaverForm : Form
    {
        private Timer timer;
        private int currentSiteIndex = -1;
        private bool shuffleOrder;
        private List<string> urls;
        private PreferencesManager prefsManager = new PreferencesManager();

        private int screenNum;

        [ThreadStatic]
        private static Random random;

        public ScreensaverForm(int? screenNumber = null)
        {
            if (screenNumber == null) screenNum = prefsManager.EffectiveScreensList.FindIndex(s => s.IsPrimary);
            else screenNum = (int)screenNumber;

            InitializeComponent();
            webView2.CreationProperties = new CoreWebView2CreationProperties
            {
                UserDataFolder = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Web-Page-Screensaver", "WebView2")
            };
        }

        public List<string> Urls
        {
            get
            {
                if (urls == null)
                {
                    urls = prefsManager.GetUrlsByScreen(screenNum);
                }

                return urls;
            }
        }

        private void ScreensaverForm_Load(object sender, EventArgs e)
        {
            if (Urls.Any())
            {
                if (Urls.Count > 1)
                {
                    // Shuffle the URLs if necessary
                    shuffleOrder = prefsManager.GetRandomizeFlagByScreen(screenNum);
                    if (shuffleOrder)
                    {
                        random = new Random();

                        int n = urls.Count;
                        while (n > 1)
                        {
                            n--;
                            int k = random.Next(n + 1);
                            var value = urls[k];
                            urls[k] = urls[n];
                            urls[n] = value;
                        }
                    }

                    // Set up timer to rotate to the next URL
                    timer = new Timer();
                    timer.Tick += (s, ee) => RotateSite();
                }

                // Display the first site in the list
                RotateSite();
                if (timer != null) timer.Start();
            }
            else
            {
                webView2.Visible = false;
            }
        }

        private void BrowseTo(string url)
        {
            // Disable the user event handler while navigating

            if (string.IsNullOrWhiteSpace(url))
            {
                webView2.Visible = false;
            }
            else
            {
                webView2.Visible = true;
                try
                {
                    Debug.WriteLine($"Navigating: {url}");

                    webView2.Source = new Uri(url);
                }
                catch
                {
                    // This can happen if IE pops up a window that isn't closed before the next call to Navigate()
                }
            }
        }

        private void RotateSite()
        {
            currentSiteIndex++;

            if (currentSiteIndex >= Urls.Count)
            {
                currentSiteIndex = 0;
            }

            var url = Urls[currentSiteIndex];
            const int maxSeconds = int.MaxValue / 1000;
            int configuredSeconds = prefsManager.GetRotationIntervalByScreen(screenNum);
            int seconds = configuredSeconds > 0 && configuredSeconds <= maxSeconds
                ? configuredSeconds : 30;

            int separator = url.LastIndexOf(';');
            if (separator >= 0 && long.TryParse(url.Substring(separator + 1), out long customSeconds))
            {
                if (customSeconds > 0 && customSeconds <= maxSeconds)
                    seconds = (int)customSeconds;
                url = url.Substring(0, separator);
            }

            if (timer != null && timer.Interval != seconds * 1000)
                timer.Interval = seconds * 1000;

            BrowseTo(url);
        }

        public bool CloseOnActivity => prefsManager.CloseOnActivity;

        public void ShowControls()
        {
            closeButton.Visible = true;
            webView2.Enabled = true;
        }

        public void HideControls()
        {
            closeButton.Visible = false;
            webView2.Enabled = false;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }    
}