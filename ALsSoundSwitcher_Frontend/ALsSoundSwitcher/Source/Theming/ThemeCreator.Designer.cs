﻿namespace ALsSoundSwitcher
{
  partial class ThemeCreator
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThemeCreator));
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.btn_ActiveSelectionColor = new System.Windows.Forms.Button();
      this.label_ActiveSelectionColor = new System.Windows.Forms.Label();
      this.label_ColorMenuItemSelected = new System.Windows.Forms.Label();
      this.btn_ColorMenuItemSelected = new System.Windows.Forms.Button();
      this.btn_ColorBackground = new System.Windows.Forms.Button();
      this.label_ColorBackground = new System.Windows.Forms.Label();
      this.btn_ColorMenuItemText = new System.Windows.Forms.Button();
      this.label_ColorMenuItemText = new System.Windows.Forms.Label();
      this.btn_ColorMenuArrow = new System.Windows.Forms.Button();
      this.btn_ColorSeparator = new System.Windows.Forms.Button();
      this.label_ColorMenuArrow = new System.Windows.Forms.Label();
      this.btn_ColorMenuBorder = new System.Windows.Forms.Button();
      this.label_ColorSeparator = new System.Windows.Forms.Label();
      this.label_ColorMenuBorder = new System.Windows.Forms.Label();
      this.btn_save = new System.Windows.Forms.Button();
      this.textBox_ThemeName = new System.Windows.Forms.TextBox();
      this.panel2 = new System.Windows.Forms.Panel();
      this.label_errorFileName = new System.Windows.Forms.Label();
      this.panel1 = new System.Windows.Forms.Panel();
      this.checkBox_smoothing = new System.Windows.Forms.CheckBox();
      this.btn_reset = new System.Windows.Forms.Button();
      this.btn_help = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.SuspendLayout();
      // 
      // pictureBox1
      // 
      this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pictureBox1.Image = global::ALsSoundSwitcher.Properties.Resources.mask_border;
      this.pictureBox1.Location = new System.Drawing.Point(13, 33);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(281, 138);
      this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      this.pictureBox1.TabIndex = 0;
      this.pictureBox1.TabStop = false;
      this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
      // 
      // btn_ActiveSelectionColor
      // 
      this.btn_ActiveSelectionColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btn_ActiveSelectionColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btn_ActiveSelectionColor.Location = new System.Drawing.Point(13, 304);
      this.btn_ActiveSelectionColor.Name = "btn_ActiveSelectionColor";
      this.btn_ActiveSelectionColor.Size = new System.Drawing.Size(280, 33);
      this.btn_ActiveSelectionColor.TabIndex = 1;
      this.btn_ActiveSelectionColor.UseVisualStyleBackColor = true;
      this.btn_ActiveSelectionColor.Click += new System.EventHandler(this.HandleButtonClick);
      // 
      // label_ActiveSelectionColor
      // 
      this.label_ActiveSelectionColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label_ActiveSelectionColor.AutoSize = true;
      this.label_ActiveSelectionColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label_ActiveSelectionColor.Location = new System.Drawing.Point(10, 281);
      this.label_ActiveSelectionColor.Name = "label_ActiveSelectionColor";
      this.label_ActiveSelectionColor.Size = new System.Drawing.Size(197, 17);
      this.label_ActiveSelectionColor.TabIndex = 3;
      this.label_ActiveSelectionColor.Text = "Active Device / Slider Selector";
      // 
      // label_ColorMenuItemSelected
      // 
      this.label_ColorMenuItemSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label_ColorMenuItemSelected.AutoSize = true;
      this.label_ColorMenuItemSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label_ColorMenuItemSelected.Location = new System.Drawing.Point(10, 357);
      this.label_ColorMenuItemSelected.Name = "label_ColorMenuItemSelected";
      this.label_ColorMenuItemSelected.Size = new System.Drawing.Size(69, 17);
      this.label_ColorMenuItemSelected.TabIndex = 5;
      this.label_ColorMenuItemSelected.Text = "Hovering ";
      // 
      // btn_ColorMenuItemSelected
      // 
      this.btn_ColorMenuItemSelected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btn_ColorMenuItemSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btn_ColorMenuItemSelected.Location = new System.Drawing.Point(13, 380);
      this.btn_ColorMenuItemSelected.Name = "btn_ColorMenuItemSelected";
      this.btn_ColorMenuItemSelected.Size = new System.Drawing.Size(280, 33);
      this.btn_ColorMenuItemSelected.TabIndex = 2;
      this.btn_ColorMenuItemSelected.UseVisualStyleBackColor = true;
      this.btn_ColorMenuItemSelected.Click += new System.EventHandler(this.HandleButtonClick);
      // 
      // btn_ColorBackground
      // 
      this.btn_ColorBackground.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btn_ColorBackground.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btn_ColorBackground.Location = new System.Drawing.Point(13, 229);
      this.btn_ColorBackground.Name = "btn_ColorBackground";
      this.btn_ColorBackground.Size = new System.Drawing.Size(280, 33);
      this.btn_ColorBackground.TabIndex = 0;
      this.btn_ColorBackground.UseVisualStyleBackColor = true;
      this.btn_ColorBackground.Click += new System.EventHandler(this.HandleButtonClick);
      // 
      // label_ColorBackground
      // 
      this.label_ColorBackground.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label_ColorBackground.AutoSize = true;
      this.label_ColorBackground.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label_ColorBackground.Location = new System.Drawing.Point(10, 206);
      this.label_ColorBackground.Name = "label_ColorBackground";
      this.label_ColorBackground.Size = new System.Drawing.Size(84, 17);
      this.label_ColorBackground.TabIndex = 3;
      this.label_ColorBackground.Text = "Background";
      // 
      // btn_ColorMenuItemText
      // 
      this.btn_ColorMenuItemText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btn_ColorMenuItemText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btn_ColorMenuItemText.Location = new System.Drawing.Point(14, 455);
      this.btn_ColorMenuItemText.Name = "btn_ColorMenuItemText";
      this.btn_ColorMenuItemText.Size = new System.Drawing.Size(280, 33);
      this.btn_ColorMenuItemText.TabIndex = 3;
      this.btn_ColorMenuItemText.UseVisualStyleBackColor = true;
      this.btn_ColorMenuItemText.Click += new System.EventHandler(this.HandleButtonClick);
      // 
      // label_ColorMenuItemText
      // 
      this.label_ColorMenuItemText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label_ColorMenuItemText.AutoSize = true;
      this.label_ColorMenuItemText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label_ColorMenuItemText.Location = new System.Drawing.Point(10, 432);
      this.label_ColorMenuItemText.Name = "label_ColorMenuItemText";
      this.label_ColorMenuItemText.Size = new System.Drawing.Size(35, 17);
      this.label_ColorMenuItemText.TabIndex = 5;
      this.label_ColorMenuItemText.Text = "Text";
      // 
      // btn_ColorMenuArrow
      // 
      this.btn_ColorMenuArrow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btn_ColorMenuArrow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btn_ColorMenuArrow.Location = new System.Drawing.Point(13, 530);
      this.btn_ColorMenuArrow.Name = "btn_ColorMenuArrow";
      this.btn_ColorMenuArrow.Size = new System.Drawing.Size(280, 33);
      this.btn_ColorMenuArrow.TabIndex = 4;
      this.btn_ColorMenuArrow.UseVisualStyleBackColor = true;
      this.btn_ColorMenuArrow.Click += new System.EventHandler(this.HandleButtonClick);
      // 
      // btn_ColorSeparator
      // 
      this.btn_ColorSeparator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btn_ColorSeparator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btn_ColorSeparator.Location = new System.Drawing.Point(13, 606);
      this.btn_ColorSeparator.Name = "btn_ColorSeparator";
      this.btn_ColorSeparator.Size = new System.Drawing.Size(280, 33);
      this.btn_ColorSeparator.TabIndex = 5;
      this.btn_ColorSeparator.UseVisualStyleBackColor = true;
      this.btn_ColorSeparator.Click += new System.EventHandler(this.HandleButtonClick);
      // 
      // label_ColorMenuArrow
      // 
      this.label_ColorMenuArrow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label_ColorMenuArrow.AutoSize = true;
      this.label_ColorMenuArrow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label_ColorMenuArrow.Location = new System.Drawing.Point(10, 507);
      this.label_ColorMenuArrow.Name = "label_ColorMenuArrow";
      this.label_ColorMenuArrow.Size = new System.Drawing.Size(112, 17);
      this.label_ColorMenuArrow.TabIndex = 3;
      this.label_ColorMenuArrow.Text = "Dropdown Arrow";
      // 
      // btn_ColorMenuBorder
      // 
      this.btn_ColorMenuBorder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btn_ColorMenuBorder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btn_ColorMenuBorder.Location = new System.Drawing.Point(13, 681);
      this.btn_ColorMenuBorder.Name = "btn_ColorMenuBorder";
      this.btn_ColorMenuBorder.Size = new System.Drawing.Size(280, 33);
      this.btn_ColorMenuBorder.TabIndex = 6;
      this.btn_ColorMenuBorder.UseVisualStyleBackColor = true;
      this.btn_ColorMenuBorder.Click += new System.EventHandler(this.HandleButtonClick);
      // 
      // label_ColorSeparator
      // 
      this.label_ColorSeparator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label_ColorSeparator.AutoSize = true;
      this.label_ColorSeparator.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label_ColorSeparator.Location = new System.Drawing.Point(10, 583);
      this.label_ColorSeparator.Name = "label_ColorSeparator";
      this.label_ColorSeparator.Size = new System.Drawing.Size(163, 17);
      this.label_ColorSeparator.TabIndex = 5;
      this.label_ColorSeparator.Text = "Separator / Slider Track ";
      // 
      // label_ColorMenuBorder
      // 
      this.label_ColorMenuBorder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label_ColorMenuBorder.AutoSize = true;
      this.label_ColorMenuBorder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label_ColorMenuBorder.Location = new System.Drawing.Point(10, 658);
      this.label_ColorMenuBorder.Name = "label_ColorMenuBorder";
      this.label_ColorMenuBorder.Size = new System.Drawing.Size(90, 17);
      this.label_ColorMenuBorder.TabIndex = 5;
      this.label_ColorMenuBorder.Text = "Menu Border";
      // 
      // btn_save
      // 
      this.btn_save.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btn_save.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btn_save.Location = new System.Drawing.Point(13, 794);
      this.btn_save.Name = "btn_save";
      this.btn_save.Size = new System.Drawing.Size(281, 55);
      this.btn_save.TabIndex = 8;
      this.btn_save.Text = "Save and Apply";
      this.btn_save.UseVisualStyleBackColor = true;
      this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
      // 
      // textBox_ThemeName
      // 
      this.textBox_ThemeName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBox_ThemeName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBox_ThemeName.ForeColor = System.Drawing.Color.Gray;
      this.textBox_ThemeName.Location = new System.Drawing.Point(13, 752);
      this.textBox_ThemeName.Name = "textBox_ThemeName";
      this.textBox_ThemeName.Size = new System.Drawing.Size(281, 26);
      this.textBox_ThemeName.TabIndex = 7;
      this.textBox_ThemeName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.textBox_ThemeName.TextChanged += new System.EventHandler(this.textBox_ThemeName_TextChanged);
      this.textBox_ThemeName.Enter += new System.EventHandler(this.textBox_ThemeName_Enter);
      this.textBox_ThemeName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox_ThemeName_KeyUp);
      this.textBox_ThemeName.Leave += new System.EventHandler(this.textBox_ThemeName_Leave);
      // 
      // panel2
      // 
      this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.panel2.BackColor = System.Drawing.Color.LightGray;
      this.panel2.Location = new System.Drawing.Point(-2, 733);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(323, 2);
      this.panel2.TabIndex = 9;
      // 
      // label_errorFileName
      // 
      this.label_errorFileName.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.label_errorFileName.AutoSize = true;
      this.label_errorFileName.ForeColor = System.Drawing.Color.Red;
      this.label_errorFileName.Location = new System.Drawing.Point(56, 778);
      this.label_errorFileName.Name = "label_errorFileName";
      this.label_errorFileName.Size = new System.Drawing.Size(182, 13);
      this.label_errorFileName.TabIndex = 10;
      this.label_errorFileName.Text = "[Theme with this name already exists]";
      // 
      // panel1
      // 
      this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.panel1.BackColor = System.Drawing.Color.LightGray;
      this.panel1.Location = new System.Drawing.Point(-2, 187);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(323, 2);
      this.panel1.TabIndex = 8;
      // 
      // checkBox_smoothing
      // 
      this.checkBox_smoothing.AutoSize = true;
      this.checkBox_smoothing.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkBox_smoothing.Location = new System.Drawing.Point(14, 8);
      this.checkBox_smoothing.Name = "checkBox_smoothing";
      this.checkBox_smoothing.Size = new System.Drawing.Size(117, 17);
      this.checkBox_smoothing.TabIndex = 12;
      this.checkBox_smoothing.Text = "Preveiw Smoothing";
      this.checkBox_smoothing.UseVisualStyleBackColor = true;
      this.checkBox_smoothing.CheckedChanged += new System.EventHandler(this.checkBox_smoothing_CheckedChanged);
      // 
      // btn_reset
      // 
      this.btn_reset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btn_reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btn_reset.Location = new System.Drawing.Point(187, 4);
      this.btn_reset.Name = "btn_reset";
      this.btn_reset.Size = new System.Drawing.Size(75, 23);
      this.btn_reset.TabIndex = 13;
      this.btn_reset.Text = "Reset";
      this.btn_reset.UseVisualStyleBackColor = true;
      this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
      // 
      // btn_help
      // 
      this.btn_help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btn_help.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btn_help.Location = new System.Drawing.Point(268, 4);
      this.btn_help.Name = "btn_help";
      this.btn_help.Size = new System.Drawing.Size(25, 23);
      this.btn_help.TabIndex = 14;
      this.btn_help.Text = "?";
      this.btn_help.UseVisualStyleBackColor = true;
      this.btn_help.Click += new System.EventHandler(this.btn_help_Click);
      // 
      // ThemeCreator
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.White;
      this.ClientSize = new System.Drawing.Size(307, 861);
      this.Controls.Add(this.btn_help);
      this.Controls.Add(this.btn_reset);
      this.Controls.Add(this.checkBox_smoothing);
      this.Controls.Add(this.label_errorFileName);
      this.Controls.Add(this.panel2);
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.textBox_ThemeName);
      this.Controls.Add(this.btn_save);
      this.Controls.Add(this.label_ColorMenuBorder);
      this.Controls.Add(this.label_ColorMenuItemText);
      this.Controls.Add(this.label_ColorSeparator);
      this.Controls.Add(this.label_ColorMenuItemSelected);
      this.Controls.Add(this.btn_ColorMenuBorder);
      this.Controls.Add(this.btn_ColorMenuItemText);
      this.Controls.Add(this.label_ColorMenuArrow);
      this.Controls.Add(this.label_ColorBackground);
      this.Controls.Add(this.btn_ColorSeparator);
      this.Controls.Add(this.btn_ColorMenuArrow);
      this.Controls.Add(this.btn_ColorMenuItemSelected);
      this.Controls.Add(this.btn_ColorBackground);
      this.Controls.Add(this.label_ActiveSelectionColor);
      this.Controls.Add(this.btn_ActiveSelectionColor);
      this.Controls.Add(this.pictureBox1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(323, 900);
      this.Name = "ThemeCreator";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "AL\'s Sound Switcher - Theme Creator (beta)";
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.Button btn_ActiveSelectionColor;
    private System.Windows.Forms.Label label_ActiveSelectionColor;
    private System.Windows.Forms.Label label_ColorMenuItemSelected;
    private System.Windows.Forms.Button btn_ColorMenuItemSelected;
    private System.Windows.Forms.Button btn_ColorBackground;
    private System.Windows.Forms.Label label_ColorBackground;
    private System.Windows.Forms.Button btn_ColorMenuItemText;
    private System.Windows.Forms.Label label_ColorMenuItemText;
    private System.Windows.Forms.Button btn_ColorMenuArrow;
    private System.Windows.Forms.Button btn_ColorSeparator;
    private System.Windows.Forms.Label label_ColorMenuArrow;
    private System.Windows.Forms.Button btn_ColorMenuBorder;
    private System.Windows.Forms.Label label_ColorSeparator;
    private System.Windows.Forms.Label label_ColorMenuBorder;
    private System.Windows.Forms.Button btn_save;
    private System.Windows.Forms.TextBox textBox_ThemeName;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Label label_errorFileName;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.CheckBox checkBox_smoothing;
    private System.Windows.Forms.Button btn_reset;
    private System.Windows.Forms.Button btn_help;
  }
}