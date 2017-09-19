using System.Windows.Forms;
using System.Drawing;

namespace WaveProject {
    partial class Window {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.encodingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bits441KHZMonoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bits441KHZStereoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bits441KHZMonoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.bits441KHZStereoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.encodingToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(284, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripMenuItem2,
            this.toolStripSeparator1,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(126, 22);
            this.toolStripMenuItem1.Text = "New";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(126, 22);
            this.toolStripMenuItem2.Text = "Save As ...";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(123, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            // 
            // encodingToolStripMenuItem
            // 
            this.encodingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bits441KHZMonoToolStripMenuItem,
            this.bits441KHZStereoToolStripMenuItem,
            this.bits441KHZMonoToolStripMenuItem1,
            this.bits441KHZStereoToolStripMenuItem1});
            this.encodingToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.encodingToolStripMenuItem.Name = "encodingToolStripMenuItem";
            this.encodingToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.encodingToolStripMenuItem.Text = "Encoding";
            // 
            // bits441KHZMonoToolStripMenuItem
            // 
            this.bits441KHZMonoToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.bits441KHZMonoToolStripMenuItem.Name = "bits441KHZMonoToolStripMenuItem";
            this.bits441KHZMonoToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.bits441KHZMonoToolStripMenuItem.Text = "8 bits, 44.1 kHZ, Mono";
            // 
            // bits441KHZStereoToolStripMenuItem
            // 
            this.bits441KHZStereoToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.bits441KHZStereoToolStripMenuItem.Name = "bits441KHZStereoToolStripMenuItem";
            this.bits441KHZStereoToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.bits441KHZStereoToolStripMenuItem.Text = "8 bits, 44.1 kHZ, Stereo";
            // 
            // bits441KHZMonoToolStripMenuItem1
            // 
            this.bits441KHZMonoToolStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.bits441KHZMonoToolStripMenuItem1.Name = "bits441KHZMonoToolStripMenuItem1";
            this.bits441KHZMonoToolStripMenuItem1.Size = new System.Drawing.Size(199, 22);
            this.bits441KHZMonoToolStripMenuItem1.Text = "16 bits, 44.1 kHZ, Mono";
            // 
            // bits441KHZStereoToolStripMenuItem1
            // 
            this.bits441KHZStereoToolStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.bits441KHZStereoToolStripMenuItem1.Name = "bits441KHZStereoToolStripMenuItem1";
            this.bits441KHZStereoToolStripMenuItem1.Size = new System.Drawing.Size(199, 22);
            this.bits441KHZStereoToolStripMenuItem1.Text = "16 bits, 44.1 kHZ, Stereo";
            // 
            // Window
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Window";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Sound Project";
            this.Load += new System.EventHandler(this.Window_Load);
            this.Resize += new System.EventHandler(this.Window_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem quitToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem encodingToolStripMenuItem;
        private ToolStripMenuItem bits441KHZMonoToolStripMenuItem;
        private ToolStripMenuItem bits441KHZStereoToolStripMenuItem;
        private ToolStripMenuItem bits441KHZMonoToolStripMenuItem1;
        private ToolStripMenuItem bits441KHZStereoToolStripMenuItem1;
    }
}

