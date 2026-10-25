using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Web_Page_Screensaver
{
    using Web_Page_Screensaver;

    public partial class PrefsByScreenUserControl : UserControl
    {
        public PrefsByScreenUserControl()
        {
            InitializeComponent();
        }

        public void AddStoredUrl(string stored)
        {
            string address = stored;
            int? seconds = null;
            int separator = stored.LastIndexOf(';');
            if (separator >= 0 &&
                int.TryParse(stored.Substring(separator + 1), out int value) &&
                value > 0 && value <= int.MaxValue / 1000)
            {
                address = stored.Substring(0, separator);
                seconds = value;
            }

            AddEntry(address, seconds, stored);
        }

        private void AddEntry(string address, int? seconds, string stored)
        {
            var item = new ListViewItem(address) { Tag = stored };
            item.SubItems.Add(seconds.HasValue ? seconds.Value.ToString() : "Default");
            lvUrls.Items.Add(item);
        }

        private void EditSelectedUrl()
        {
            if (lvUrls.SelectedItems.Count != 1) return;

            var item = lvUrls.SelectedItems[0];
            string stored = item.Tag as string ?? item.Text;
            string address = stored;
            int? seconds = null;
            int separator = stored.LastIndexOf(';');
            if (separator >= 0 &&
                int.TryParse(stored.Substring(separator + 1), out int value) &&
                value > 0 && value <= int.MaxValue / 1000)
            {
                address = stored.Substring(0, separator);
                seconds = value;
            }

            using (var editor = new UrlEditorDialog(address, seconds))
            {
                if (editor.ShowDialog(this) != DialogResult.OK) return;

                item.Text = editor.Url;
                item.SubItems[1].Text = editor.Seconds.HasValue
                    ? editor.Seconds.Value.ToString() : "Default";
                item.Tag = editor.Url +
                    (editor.Seconds.HasValue ? ";" + editor.Seconds.Value : "");
            }
        }

        private void editUrlButton_Click(object sender, EventArgs e)
        {
            EditSelectedUrl();
        }

        private void lvUrls_ItemActivate(object sender, EventArgs e)
        {
            EditSelectedUrl();
        }

        private void lvUrls_SelectedIndexChanged(object sender, EventArgs e)
        {
            editButton.Enabled = lvUrls.SelectedItems.Count == 1;
        }

        private void lvUrls_Resize(object sender, EventArgs e)
        {
            if (lvUrls.Columns.Count == 2)
                lvUrls.Columns[0].Width = Math.Max(100,
                    lvUrls.ClientSize.Width - lvUrls.Columns[1].Width - 4);
        }

        private void MoveAllSelectedUrlsDown_Click(object sender, EventArgs e)
        {
            // TODO: make button grey out when none selected OR when all selected ones are in a bunch at the bottom.
            bool gapFound = false;

            // scan from the bottom up, but don't start moving selected items down until
            // we find an unselected gap to move items(s) down in to.
            for (int i = lvUrls.Items.Count - 1; i >= 0; i--)
            {
                if (lvUrls.Items[i].Selected)
                {
                    if (gapFound)
                    {
                        Swap(lvUrls.Items, i, i + 1);
                    }
                }
                else
                {
                    gapFound = true;
                }
            }

            // 'select'ing the list makes sure the selections of items within it are being displayed.
            // Otherwise the button becomes the 'selected' control and the selections within the list
            // are invisible or hard to see.
            lvUrls.Select();
        }

        private void MoveAllSelectedUrlsUp_Click(object sender, EventArgs e)
        {
            // TODO: make button grey out when none selected OR when all selected ones are in a bunch at the top.
            bool gapFound = false;

            // scan through for all selected, but don't start moving selected items up until
            // we find an unselected gap to move items(s) up in to.
            for (int i = 0; i < lvUrls.Items.Count; i++)
            {
                if (lvUrls.Items[i].Selected)
                {
                    if (gapFound)
                    {
                        Swap(lvUrls.Items, i, i - 1);
                    }
                }
                else
                {
                    gapFound = true;
                }
            }

            // 'select'ing the list makes sure the selections of items within it are being displayed.
            // Otherwise the button becomes the 'selected' control and the selections within the list
            // are invisible or hard to see.
            lvUrls.Select();
        }

        private void DeleteAllSelectedUrls_Click(object sender, EventArgs e)
        {
            // TODO: undo capability?
            // TODO: make button grey out when no selection.
            // work from the bottom up, deleting any we find
            for (int i = lvUrls.Items.Count - 1; i >= 0; i--)
            {
                if (lvUrls.Items[i].Selected)
                {
                    lvUrls.Items[i].Remove();
                }
            }
        }

        private void addUrlButton_Click(object sender, EventArgs e)
        {
            using (var editor = new UrlEditorDialog())
            {
                if (editor.ShowDialog(this) == DialogResult.OK)
                {
                    string stored = editor.Url +
                        (editor.Seconds.HasValue ? ";" + editor.Seconds.Value : "");
                    AddEntry(editor.Url, editor.Seconds, stored);
                }
            }
        }

        /// <summary>
        /// Swap the positions of 2 items in a ListViewItemCollection, maintaining Selected status.
        /// Any indexes given outside the bounds of the list are treated as if they point to
        /// the nearest end of the list.
        /// </summary>
        private static void Swap(ListView.ListViewItemCollection itemsList, int indexA, int indexB)
        {
            var a = Math.Min(itemsList.Count - 1, Math.Max(0, indexA));
            var b = Math.Min(itemsList.Count - 1, Math.Max(0, indexB));
            if (a != b)
            {
                var itemA = (ListViewItem)itemsList[a].Clone();
                itemA.Tag = itemsList[a].Tag;
                bool itemASelected = itemsList[a].Selected;
                var itemB = (ListViewItem)itemsList[b].Clone();
                itemB.Tag = itemsList[b].Tag;
                bool itemBSelected = itemsList[b].Selected;
                itemsList[a] = itemB;
                itemsList[a].Selected = itemBSelected;
                itemsList[b] = itemA;
                itemsList[b].Selected = itemASelected;
            }
        }
    }
}
