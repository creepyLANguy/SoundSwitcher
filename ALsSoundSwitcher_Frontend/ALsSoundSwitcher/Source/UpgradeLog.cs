using System;
using System.Drawing;
using System.Windows.Forms;

namespace ALsSoundSwitcher
{
  public class UpgradeLog : Form
  {
    private Button _button;
    private RichTextBox _textBox;

    private readonly Color _progressColour = Color.FromArgb(200, 240, 225);
    private readonly Color _failColour = Color.FromArgb(255, 150, 150);


    public UpgradeLog()
    {
      InitializeComponent();
    }

    private void InitializeComponent()
    {
      this._textBox = new System.Windows.Forms.RichTextBox();
      this._button = new System.Windows.Forms.Button();
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
      // _button
      // 
      this._button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this._button.Enabled = false;
      this._button.Location = new System.Drawing.Point(12, 412);
      this._button.Name = "_button";
      this._button.Size = new System.Drawing.Size(360, 37);
      this._button.TabIndex = 1;
      this._button.Text = "Upgrade In Progress...";
      this._button.UseVisualStyleBackColor = true;
      this._button.Click += new System.EventHandler(this.buttonBottom_Click);
      this._button.FlatStyle = FlatStyle.Flat;
      //this._button.BackColor = Color.Lavender;
      // 
      // UpgradeLog
      // 
      this.ClientSize = new System.Drawing.Size(384, 461);
      this.ControlBox = false;
      this.Controls.Add(this._button);
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

    public void MakeDismissible(string buttonMessage, bool hasFailed)
    {
      ControlBox = true;

      Application.DoEvents(); //TODO - revise this bad hack that prevents incorrectly triggering old button clicks. 
      _button.Text = buttonMessage;

      if (hasFailed)
      {
        PaintButton(_failColour, 100);
      }
      
      _button.Enabled = true;
    }

    private void UpgradeLog_FormClosed(object sender, FormClosedEventArgs e) 
      => Application.Exit();

    private void buttonBottom_Click(object sender, EventArgs e)
      => Close();

    public void UpdateProgress(float percentage)
      => PaintButton(_progressColour, percentage);

    private void PaintButton(Color color, float percentage)
    {
      if (_button.BackgroundImage == null || _button.BackgroundImage.Size != _button.ClientSize)
      {
        _button.BackgroundImage = new Bitmap(_button.ClientSize.Width, _button.ClientSize.Height);
      }

      var bm = (Bitmap)_button.BackgroundImage;

      using var graphics = Graphics.FromImage(bm);
      graphics.Clear(Color.Transparent);

      var rect = new RectangleF(0, 0, bm.Width * (percentage / 100), bm.Height);
      graphics.FillRectangle(new SolidBrush(color), rect);

      _button.Refresh();
    }
  }
}
