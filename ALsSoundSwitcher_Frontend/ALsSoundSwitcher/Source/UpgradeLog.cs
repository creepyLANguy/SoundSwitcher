using System;
using System.Drawing;
using System.Windows.Forms;

namespace ALsSoundSwitcher
{
  public class UpgradeLog : Form
  {
    private Button _buttonBottom;
    private RichTextBox _textBox;

    public UpgradeLog()
    {
      InitializeComponent();
    }

    private void InitializeComponent()
    {
      this._textBox = new System.Windows.Forms.RichTextBox();
      this._buttonBottom = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // _textBox
      // 
      this._textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this._textBox.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this._textBox.Location = new System.Drawing.Point(0, 0);
      this._textBox.Name = "_textBox";
      this._textBox.ReadOnly = true;
      this._textBox.Size = new System.Drawing.Size(384, 406);
      this._textBox.TabIndex = 0;
      this._textBox.Text = "";
      // 
      // _buttonBottom
      // 
      this._buttonBottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this._buttonBottom.Enabled = false;
      this._buttonBottom.Location = new System.Drawing.Point(12, 412);
      this._buttonBottom.Name = "_buttonBottom";
      this._buttonBottom.Size = new System.Drawing.Size(360, 37);
      this._buttonBottom.TabIndex = 1;
      this._buttonBottom.Text = "Upgrade In Progress...";
      this._buttonBottom.UseVisualStyleBackColor = true;
      this._buttonBottom.Click += new System.EventHandler(this.buttonBottom_Click);
      this._buttonBottom.FlatStyle = FlatStyle.Flat;
      //this._buttonBottom.BackColor = Color.Lavender;
      // 
      // UpgradeLog
      // 
      this.ClientSize = new System.Drawing.Size(384, 461);
      this.ControlBox = false;
      this.Controls.Add(this._buttonBottom);
      this.Controls.Add(this._textBox);
      this.DoubleBuffered = true;
      this.Icon = global::ALsSoundSwitcher.Properties.Resources.Headset;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(300, 300);
      this.Name = "UpgradeLog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "AL\'s Sound Switcher - Upgrade Log (beta)";
      this.TopMost = true;
      this.ResumeLayout(false);
    }

    public void Log(string message, bool showTimestamp)
    {
      var time = showTimestamp ? DateTime.Now.ToString("HH:mm:ss.fff") + Environment.NewLine : "";
      _textBox.Text += time + message + Environment.NewLine + Environment.NewLine;
      _textBox.SelectionStart = _textBox.Text.Length;
      _textBox.ScrollToCaret();
      Refresh();
    }

    public void MakeDismissible(string buttonMessage, Color buttonColor)
    {
      ControlBox = true;

      //TODO - prevents incorrectly triggering old button clicks. 
      _buttonBottom.Text = buttonMessage;
      _buttonBottom.BackColor = buttonColor;
      _buttonBottom.Enabled = true;
    }

    private void UpgradeLog_FormClosed(object sender, FormClosedEventArgs e)
    {
      Application.Exit();
    }

    private void buttonBottom_Click(object sender, EventArgs e)
    {
      Close();
    }
  }
}
