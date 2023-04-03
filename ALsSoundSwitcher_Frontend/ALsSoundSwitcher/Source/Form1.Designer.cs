namespace ALsSoundSwitcher
{
  partial class Form1
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
      textBox1 = new System.Windows.Forms.TextBox();
      SuspendLayout();
      // 
      // notifyIcon1
      // 
      notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
      notifyIcon1.BalloonTipText = "Minimised to tray.";
      notifyIcon1.BalloonTipTitle = "AL\'s Sound Switcher ";
      notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
      notifyIcon1.Text = "AL\'s Sound Switcher has been minimised.";
      notifyIcon1.Visible = true;
      notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
      // 
      // textBox1
      // 
      textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
      textBox1.Location = new System.Drawing.Point(13, 13);
      textBox1.Multiline = true;
      textBox1.Name = "textBox1";
      textBox1.ReadOnly = true;
      textBox1.Size = new System.Drawing.Size(259, 129);
      textBox1.TabIndex = 0;
      textBox1.Text = "This window should not show. \r\nPerhaps something failed on startup?\r\n";
      // 
      // Form1
      // 
      AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      ClientSize = new System.Drawing.Size(284, 154);
      Controls.Add(textBox1);
      FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      MaximizeBox = false;
      Name = "Form1";
      SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      Text = "AL\'s Sound Switcher";
      Load += new System.EventHandler(this.Form1_Load);
      ResumeLayout(false);
      PerformLayout();

    }

    #endregion

    private static System.Windows.Forms.TextBox textBox1;
    private static System.Windows.Forms.NotifyIcon notifyIcon1;
  }
}

