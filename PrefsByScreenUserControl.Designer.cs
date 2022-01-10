namespace Web_Page_Screensaver
{
    partial class PrefsByScreenUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.nudRotationInterval = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.cbRandomize = new System.Windows.Forms.CheckBox();
            this.lvUrls = new System.Windows.Forms.ListView();
            this.addUrlButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.urlButtonsTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.downButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.nudRotationInterval)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // nudRotationInterval
            // 
            this.nudRotationInterval.Location = new System.Drawing.Point(15, 88);
            this.nudRotationInterval.Margin = new System.Windows.Forms.Padding(4);
            this.nudRotationInterval.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudRotationInterval.Name = "nudRotationInterval";
            this.nudRotationInterval.Size = new System.Drawing.Size(53, 22);
            this.nudRotationInterval.TabIndex = 6;
            this.nudRotationInterval.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(76, 90);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(188, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Seconds to display each site";
            // 
            // cbRandomize
            // 
            this.cbRandomize.AutoSize = true;
            this.cbRandomize.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbRandomize.Location = new System.Drawing.Point(14, 59);
            this.cbRandomize.Margin = new System.Windows.Forms.Padding(4);
            this.cbRandomize.Name = "cbRandomize";
            this.cbRandomize.Size = new System.Drawing.Size(169, 22);
            this.cbRandomize.TabIndex = 5;
            this.cbRandomize.Text = "Shuffle display order";
            this.cbRandomize.UseVisualStyleBackColor = true;
            // 
            // lvUrls
            // 
            this.lvUrls.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvUrls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvUrls.HideSelection = false;
            this.lvUrls.LabelEdit = true;
            this.lvUrls.Location = new System.Drawing.Point(0, 0);
            this.lvUrls.Name = "lvUrls";
            this.lvUrls.Size = new System.Drawing.Size(300, 400);
            this.lvUrls.TabIndex = 0;
            this.lvUrls.UseCompatibleStateImageBehavior = false;
            this.lvUrls.View = System.Windows.Forms.View.List;
            // 
            // addUrlButton
            // 
            this.addUrlButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.addUrlButton.Location = new System.Drawing.Point(13, 13);
            this.addUrlButton.Margin = new System.Windows.Forms.Padding(4);
            this.addUrlButton.Name = "addUrlButton";
            this.addUrlButton.Size = new System.Drawing.Size(77, 24);
            this.addUrlButton.TabIndex = 11;
            this.addUrlButton.Text = "Add";
            this.addUrlButton.UseVisualStyleBackColor = true;
            this.addUrlButton.Click += new System.EventHandler(this.addUrlButton_Click);
            // 
            // upButton
            // 
            this.upButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.upButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.upButton.Location = new System.Drawing.Point(140, 13);
            this.upButton.Margin = new System.Windows.Forms.Padding(0);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(24, 24);
            this.upButton.TabIndex = 3;
            this.upButton.Text = "▲";
            this.urlButtonsTooltip.SetToolTip(this.upButton, "Move selected URLs up");
            this.upButton.UseVisualStyleBackColor = true;
            this.upButton.Click += new System.EventHandler(this.MoveAllSelectedUrlsUp_Click);
            // 
            // downButton
            // 
            this.downButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.downButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downButton.Location = new System.Drawing.Point(164, 13);
            this.downButton.Margin = new System.Windows.Forms.Padding(0);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(24, 24);
            this.downButton.TabIndex = 12;
            this.downButton.Text = "▼";
            this.urlButtonsTooltip.SetToolTip(this.downButton, "Move selected URLs down");
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.Click += new System.EventHandler(this.MoveAllSelectedUrlsDown_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.deleteButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteButton.ForeColor = System.Drawing.Color.Red;
            this.deleteButton.Location = new System.Drawing.Point(94, 13);
            this.deleteButton.Margin = new System.Windows.Forms.Padding(0);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(24, 24);
            this.deleteButton.TabIndex = 13;
            this.deleteButton.Text = "X";
            this.urlButtonsTooltip.SetToolTip(this.deleteButton, "DELETE selected URLs");
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.DeleteAllSelectedUrls_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.addUrlButton);
            this.panel1.Controls.Add(this.deleteButton);
            this.panel1.Controls.Add(this.cbRandomize);
            this.panel1.Controls.Add(this.downButton);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.upButton);
            this.panel1.Controls.Add(this.nudRotationInterval);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 253);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 147);
            this.panel1.TabIndex = 14;
            // 
            // PrefsByScreenUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lvUrls);
            this.MinimumSize = new System.Drawing.Size(300, 0);
            this.Name = "PrefsByScreenUserControl";
            this.Size = new System.Drawing.Size(300, 400);
            ((System.ComponentModel.ISupportInitialize)(this.nudRotationInterval)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.NumericUpDown nudRotationInterval;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.CheckBox cbRandomize;
        public System.Windows.Forms.ListView lvUrls;
        private System.Windows.Forms.ToolTip urlButtonsTooltip;
        private System.Windows.Forms.Button addUrlButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Panel panel1;
    }
}
