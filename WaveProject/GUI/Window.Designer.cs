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
            this.SuspendLayout();
            // 
            // Window
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "Window";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Sound Project";
            this.Load += new System.EventHandler(this.Window_Load);
            this.Resize += new System.EventHandler(this.Window_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}

