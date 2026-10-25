using System;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;

namespace Web_Page_Screensaver
{
    using System.Collections.Generic;
    using System.Drawing;

    static class Program
    {
        public static readonly string KEY = "Software\\Web-Page-Screensaver";

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string mode = args.Length == 0 ? "/c" : args[0].ToLowerInvariant();
            if (mode == "/p" || mode.StartsWith("/p:") || mode == "-p")
                return;

            if (mode == "/c" || mode.StartsWith("/c:") || mode == "-c")
            {
                Application.Run(new PreferencesForm());
            }
            else if (mode == "/s" || mode == "-s")
            {
                var formsList = new List<ScreensaverForm>();
                var screens = (new PreferencesManager()).EffectiveScreensList;
                foreach (var screen in screens)
                {
                    var screensaverForm = new ScreensaverForm(screen.ScreenNum)
                    {
                        Location = new Point(screen.Bounds.Left, screen.Bounds.Top),
                        Size = new Size(screen.Bounds.Width, screen.Bounds.Height)
                    };

                    formsList.Add(screensaverForm);
                }

                Application.Run(new MultiFormContext(formsList));
            }
        }
    }

    public class MultiFormContext : ApplicationContext
    {
        private readonly List<ScreensaverForm> forms;
        private readonly Timer controlsTimer = new Timer { Interval = 3500 };
        private IKeyboardMouseEvents globalHook;
        private Point lastMousePosition;
        private bool controlsVisible;
        private bool cursorHidden;
        private bool exiting;

        public MultiFormContext(List<ScreensaverForm> screensaverForms)
        {
            forms = screensaverForms;
            foreach (var form in forms)
            {
                form.FormClosed += (s, e) => ExitThread();
                form.Show();
            }

            controlsTimer.Tick += (s, e) => HideControls();
            lastMousePosition = Cursor.Position;
            Cursor.Hide();
            cursorHidden = true;

            try
            {
                globalHook = Hook.GlobalEvents();
                globalHook.KeyDown += OnKeyDown;
                globalHook.MouseMoveExt += OnMouseMove;
                globalHook.MouseDown += OnMouseInput;
                globalHook.MouseWheel += OnMouseInput;
            }
            catch
            {
                globalHook?.Dispose();
                SetCursorVisible(true);
                throw;
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            OnActivity();
        }

        private void OnMouseInput(object sender, MouseEventArgs e)
        {
            OnActivity();
        }

        private void OnMouseMove(object sender, MouseEventExtArgs e)
        {
            Point position = Cursor.Position;
            if (Math.Abs(position.X - lastMousePosition.X) < 3 &&
                Math.Abs(position.Y - lastMousePosition.Y) < 3)
                return;

            lastMousePosition = position;
            OnActivity();
        }

        private void OnActivity()
        {
            if (exiting || controlsVisible) return;
            if (forms.Count == 0 || forms[0].CloseOnActivity)
            {
                ExitThread();
                return;
            }

            controlsVisible = true;
            foreach (var form in forms) form.ShowControls();
            SetCursorVisible(true);
            controlsTimer.Start();
        }

        private void HideControls()
        {
            controlsTimer.Stop();
            foreach (var form in forms) form.HideControls();
            SetCursorVisible(false);
            lastMousePosition = Cursor.Position;
            controlsVisible = false;
        }

        private void SetCursorVisible(bool visible)
        {
            if (visible && cursorHidden)
            {
                Cursor.Show();
                cursorHidden = false;
            }
            else if (!visible && !cursorHidden)
            {
                Cursor.Hide();
                cursorHidden = true;
            }
        }

        protected override void ExitThreadCore()
        {
            if (exiting) return;
            exiting = true;
            controlsTimer.Stop();
            controlsTimer.Dispose();

            if (globalHook != null)
            {
                globalHook.KeyDown -= OnKeyDown;
                globalHook.MouseMoveExt -= OnMouseMove;
                globalHook.MouseDown -= OnMouseInput;
                globalHook.MouseWheel -= OnMouseInput;
                globalHook.Dispose();
                globalHook = null;
            }

            SetCursorVisible(true);
            foreach (var form in forms)
                if (!form.IsDisposed && !form.Disposing && form.Visible) form.Close();

            base.ExitThreadCore();
        }
    }
}
