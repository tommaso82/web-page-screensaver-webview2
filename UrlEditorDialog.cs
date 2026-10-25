using System;
using System.Drawing;
using System.Windows.Forms;

namespace Web_Page_Screensaver
{
    internal sealed class UrlEditorDialog : Form
    {
        private readonly TextBox urlBox = new TextBox();
        private readonly CheckBox useDefault = new CheckBox();
        private readonly NumericUpDown secondsBox = new NumericUpDown();

        public string Url => urlBox.Text.Trim();
        public int? Seconds => useDefault.Checked ? (int?)null : (int)secondsBox.Value;

        public UrlEditorDialog(string url = "", int? seconds = null)
        {
            Text = string.IsNullOrEmpty(url) ? "Add website" : "Edit website";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(410, 174);
            MinimumSize = Size;
            AutoScaleMode = AutoScaleMode.Font;

            var urlLabel = new Label { Text = "Website URL", Location = new Point(12, 14), AutoSize = true };
            urlBox.Location = new Point(12, 36);
            urlBox.Size = new Size(386, 23);
            urlBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            urlBox.Text = url;

            useDefault.Text = "Use screen default";
            useDefault.Location = new Point(12, 74);
            useDefault.AutoSize = true;
            useDefault.Checked = !seconds.HasValue;
            useDefault.CheckedChanged += (s, e) => secondsBox.Enabled = !useDefault.Checked;

            var secondsLabel = new Label { Text = "Time (s)", Location = new Point(218, 76), AutoSize = true };
            secondsBox.Location = new Point(305, 72);
            secondsBox.Size = new Size(93, 23);
            secondsBox.Minimum = 1;
            secondsBox.Maximum = int.MaxValue / 1000;
            secondsBox.Value = seconds.HasValue ? seconds.Value : 30;
            secondsBox.Enabled = seconds.HasValue;

            var ok = new Button
            {
                Text = "OK", DialogResult = DialogResult.OK,
                Location = new Point(222, 130), Size = new Size(82, 28),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            var cancel = new Button
            {
                Text = "Cancel", DialogResult = DialogResult.Cancel,
                Location = new Point(314, 130), Size = new Size(82, 28),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };

            Controls.AddRange(new Control[] { urlLabel, urlBox, useDefault, secondsLabel,
                                              secondsBox, ok, cancel });
            AcceptButton = ok;
            CancelButton = cancel;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                Uri address;
                if (Url.IndexOfAny(new[] { ' ', '\t', '\r', '\n' }) >= 0 ||
                    !Uri.TryCreate(Url, UriKind.Absolute, out address))
                {
                    MessageBox.Show(this, "Enter an absolute URL without spaces.",
                        "Invalid URL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                    urlBox.Focus();
                    urlBox.SelectAll();
                }
            }

            base.OnFormClosing(e);
        }
    }
}
