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
      notifyIcon1 = new System.Windows.Forms.NotifyIcon(components);
      label1 = new System.Windows.Forms.Label();
      linkLabel1 = new System.Windows.Forms.LinkLabel();
      btn_restart = new System.Windows.Forms.Button();
      SuspendLayout();
      // 
      // notifyIcon1
      // 
      notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
      notifyIcon1.BalloonTipText = "Minimised to tray.";
      notifyIcon1.BalloonTipTitle = "AL\'s Sound Switcher ";
      notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
      notifyIcon1.Text = "AL\'s Sound Switcher has been minimised.";
      notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon1_MouseClick);
      // 
      // label1
      // 
      label1.Location = new System.Drawing.Point(13, 13);
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size(259, 152);
      label1.TabIndex = 0;
      label1.Text = resources.GetString("label1.Text");
      // 
      // linkLabel1
      // 
      linkLabel1.AutoSize = true;
      linkLabel1.Location = new System.Drawing.Point(13, 165);
      linkLabel1.Name = "linkLabel1";
      linkLabel1.Size = new System.Drawing.Size(168, 13);
      linkLabel1.TabIndex = 1;
      linkLabel1.TabStop = true;
      linkLabel1.Text = "AL\'s Sound Switcher on GitHub 🡵";
      linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
      // 
      // btn_restart
      // 
      btn_restart.Location = new System.Drawing.Point(12, 197);
      btn_restart.Name = "btn_restart";
      btn_restart.Size = new System.Drawing.Size(259, 23);
      btn_restart.TabIndex = 2;
      btn_restart.Text = "Attempt Restart";
      btn_restart.UseVisualStyleBackColor = true;
      btn_restart.Click += new System.EventHandler(btn_restart_Click);
      // 
      // Form1
      // 
      AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      ClientSize = new System.Drawing.Size(284, 232);
      Controls.Add(btn_restart);
      Controls.Add(linkLabel1);
      Controls.Add(label1);
      FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "Form1";
      SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      Text = "AL\'s Sound Switcher";
      Load += new System.EventHandler(Form1_Load);
      ResumeLayout(false);
      PerformLayout();

    }

    #endregion

    private static System.Windows.Forms.Button btn_restart;
    private static System.Windows.Forms.LinkLabel linkLabel1;
    private static System.Windows.Forms.Label label1;
    private static System.Windows.Forms.NotifyIcon notifyIcon1;
  }
}

