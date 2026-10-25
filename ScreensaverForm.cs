using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace Web_Page_Screensaver
{
    public partial class ScreensaverForm : Form
    {
        private Timer timer;
        private Timer retryTimer;
        private string currentUrl;
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
            webView2.CoreWebView2InitializationCompleted += (s, e) =>
            {
                if (!e.IsSuccess)
                    Trace.TraceError("WebView2 initialization failed: " + e.InitializationException);
            };
            webView2.NavigationCompleted += (s, e) =>
            {
                if (e.IsSuccess)
                {
                    retryTimer?.Stop();
                }
                else if (e.WebErrorStatus != CoreWebView2WebErrorStatus.OperationCanceled)
                {
                    Trace.TraceWarning("WebView2 navigation failed: " + e.WebErrorStatus);
                    ScheduleRetry();
                }
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
            if (string.IsNullOrWhiteSpace(url))
            {
                webView2.Visible = false;
                return;
            }

            Uri address;
            if (!Uri.TryCreate(url, UriKind.Absolute, out address))
            {
                Trace.TraceWarning("Invalid screensaver URL: " + url);
                webView2.Visible = false;
                return;
            }

            webView2.Visible = true;
            try
            {
                webView2.Source = address;
            }
            catch (Exception ex)
            {
                Trace.TraceError("WebView2 navigation error: " + ex);
                ScheduleRetry();
            }
        }

        private void ScheduleRetry()
        {
            if (Urls.Count != 1 || IsDisposed || Disposing) return;

            if (retryTimer == null)
            {
                retryTimer = new Timer { Interval = 30000 };
                retryTimer.Tick += (s, e) =>
                {
                    retryTimer.Stop();
                    BrowseTo(currentUrl);
                };
            }
            retryTimer.Start();
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

            currentUrl = url;
            BrowseTo(url);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            timer?.Dispose();
            retryTimer?.Dispose();
            base.OnFormClosed(e);
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