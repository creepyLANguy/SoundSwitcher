﻿using ALsSoundSwitcher.Properties;
using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ALsSoundSwitcher
{
  public class UpgradeLog : Form
  {
    private RichTextBox textBox;

    public UpgradeLog()
    {
      InitializeComponent();
    }

    private void InitializeComponent()
    {
      this.textBox = new System.Windows.Forms.RichTextBox();
      this.SuspendLayout();
      // 
      // textBox
      // 
      this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.textBox.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBox.Location = new System.Drawing.Point(0, 0);
      this.textBox.Name = "textBox";
      this.textBox.ReadOnly = true;
      this.textBox.Size = new System.Drawing.Size(384, 461);
      this.textBox.TabIndex = 0;
      this.textBox.Text = "";
      // 
      // UpgradeLog
      // 
      this.ClientSize = new System.Drawing.Size(384, 461);
      this.ControlBox = false;
      this.Controls.Add(this.textBox);
      this.DoubleBuffered = true;
      this.Icon = global::ALsSoundSwitcher.Properties.Resources.Headset;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(300, 300);
      this.Name = "UpgradeLog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "AL\'s Sound Switcher - Upgrade Log (beta)";
      this.TopMost = true;
      this.FormClosed += UpgradeLog_FormClosed;
      this.ResumeLayout(false);

    }

    public void Log(string message, bool showTimestamp)
    {
      var time = showTimestamp ? DateTime.Now.ToString("HH:mm:ss.fff") + Environment.NewLine : "";
      textBox.Text += time + message + Environment.NewLine + Environment.NewLine;
      textBox.SelectionStart = textBox.Text.Length;
      textBox.ScrollToCaret();
      Refresh();
    }

    public void ShowControlBox()
    {
      ControlBox = true;
    }

    private void UpgradeLog_FormClosed(object sender, FormClosedEventArgs e)
    {
      Application.Exit();
    }
  }
}