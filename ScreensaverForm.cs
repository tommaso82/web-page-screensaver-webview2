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
using Microsoft.Web.WebView2.Core;
using Gma.System.MouseKeyHook;

namespace Web_Page_Screensaver
{
    public partial class ScreensaverForm : Form
    {
        private DateTime StartTime;
        private Timer timer;
        private int currentSiteIndex = -1;
        private bool shuffleOrder;
        private List<string> urls;
        private IKeyboardMouseEvents m_GlobalHook;
        private Timer ctrl_timer = new Timer();
        private PreferencesManager prefsManager = new PreferencesManager();

        private int screenNum;

        [ThreadStatic]
        private static Random random;

        public ScreensaverForm(int? screenNumber = null)
        {
            //userEventHandler = new GlobalUserEventHandler();
            //serEventHandler.Event += new GlobalUserEventHandler.UserEvent(HandleUserActivity);            
            if (screenNumber == null) screenNum = prefsManager.EffectiveScreensList.FindIndex(s => s.IsPrimary);
            else screenNum = (int)screenNumber;

            InitializeComponent();
            Subscribe();
            ctrl_timer.Interval = 3500;
            ctrl_timer.Tick += new EventHandler(ctrl_timer_Tick);
            Cursor.Hide();
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

        public void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyDown += KeyHook;
            m_GlobalHook.MouseMoveExt += MouseHook;
        }
        public void Unsubscribe()
        {
            m_GlobalHook.KeyDown -= KeyHook;
            m_GlobalHook.MouseMoveExt -= MouseHook;

            //It is recommened to dispose it
            //m_GlobalHook.Dispose();
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
                    timer.Interval = prefsManager.GetRotationIntervalByScreen(screenNum) * 1000;
                    timer.Tick += (s, ee) => RotateSite();
                    timer.Start();
                }

                // Display the first site in the list
                RotateSite();

                StartTime = DateTime.Now;
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

            BrowseTo(url);
        }
        private void KeyHook(object sender, KeyEventArgs e)
        {
            UserEvent();
        }
        private void MouseHook(object sender, MouseEventExtArgs e)
        {
            UserEvent();
        }
        private void UserEvent()
        {
            if (StartTime.AddMilliseconds(20) > DateTime.Now) return;

            if (prefsManager.CloseOnActivity)
            {
                Close();
            }
            else
            {
                closeButton.Visible = true;
                Cursor.Show();
                webView2.Enabled = true;
                Unsubscribe();
                ctrl_timer.Start();
            }
        }

        private void ctrl_timer_Tick(object sender, EventArgs e)
        {
            // Hide the close button and cursor when the timer ticks
            closeButton.Visible = false;
            Cursor.Hide();
            webView2.Enabled = false;

            // Stop the timer
            ctrl_timer.Stop();
            Subscribe();

        }
        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
    public class AutoClosingMessageBox
    {
        System.Threading.Timer _timeoutTimer;
        string _caption;
        AutoClosingMessageBox(string text, string caption, int timeout)
        {
            _caption = caption;
            _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                null, timeout, System.Threading.Timeout.Infinite);
            using (_timeoutTimer)
                MessageBox.Show(text, caption);
        }
        public static void Show(string text, string caption, int timeout)
        {
            new AutoClosingMessageBox(text, caption, timeout);
        }
        void OnTimerElapsed(object state)
        {
            IntPtr mbWnd = FindWindow("#32770", _caption); // lpClassName is #32770 for MessageBox
            if (mbWnd != IntPtr.Zero)
                SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            _timeoutTimer.Dispose();
        }
        const int WM_CLOSE = 0x0010;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
    }
}