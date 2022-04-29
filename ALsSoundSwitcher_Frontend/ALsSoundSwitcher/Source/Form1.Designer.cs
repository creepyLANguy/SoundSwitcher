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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      this.listBox1 = new System.Windows.Forms.ListBox();
      this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
      this.SuspendLayout();
      // 
      // listBox1
      // 
      this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listBox1.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.listBox1.FormattingEnabled = true;
      this.listBox1.HorizontalScrollbar = true;
      this.listBox1.ItemHeight = 15;
      this.listBox1.Location = new System.Drawing.Point(0, 0);
      this.listBox1.Name = "listBox1";
      this.listBox1.Size = new System.Drawing.Size(284, 154);
      this.listBox1.TabIndex = 0;
      this.listBox1.Click += new System.EventHandler(this.listBox1_Click);
      // 
      // notifyIcon1
      // 
      this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
      this.notifyIcon1.BalloonTipText = "Minimised to tray.";
      this.notifyIcon1.BalloonTipTitle = "AL\'s Sound Switcher ";
      this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
      this.notifyIcon1.Text = "AL\'s Sound Switcher has been minimised.";
      this.notifyIcon1.Visible = true;
      this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
      this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(284, 154);
      this.Controls.Add(this.listBox1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.Name = "Form1";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.Text = "AL\'s Sound Switcher";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.LocationChanged += new System.EventHandler(this.Form1_LocationChanged);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListBox listBox1;
    private System.Windows.Forms.NotifyIcon notifyIcon1;
  }
}

