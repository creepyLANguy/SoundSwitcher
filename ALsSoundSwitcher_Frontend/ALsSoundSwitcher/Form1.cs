using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace ALsSoundSwitcher
{
  public partial class Form1 : Form
  {
    string getDevicesExe = "GetPlaybackDevices.exe";
    string setDeviceExe = "SetPlaybackDevice.exe";
    int balloonTime = 500;
    string[] ar;

    private ContextMenu contextMenu1;
    private MenuItem menuItemExit;
    private MenuItem menuItemRefresh;

    public Form1()
    {
      InitializeComponent();
    }

    private void Minimize()
    {
      WindowState = FormWindowState.Minimized;
      notifyIcon1.ShowBalloonTip(balloonTime);
      ShowInTaskbar = false;
    }
    private void Maximize()
    {
      WindowState = FormWindowState.Normal;
      ShowInTaskbar = true;
    }

    private void SetupContextMenu()
    {
      string text = "";

      try
      {
        text = System.IO.File.ReadAllText(@"devices.txt");
        Console.WriteLine(text);
      }
      catch (Exception)
      {
        try
        {
          RunExe(getDevicesExe);
          Close();
        }
        catch (Exception)
        {
          MessageBox.Show("Error reading device list.\n\nCould not start " + getDevicesExe);
          Close();
        }

      }

      contextMenu1 = new ContextMenu();

      text = text.Trim();
      ar = text.Split('\n');
      for (int i = 0; i < ar.Length; i += 2)
      {
        listBox1.Items.Add(ar[i]);

        MenuItem menuItem = new MenuItem();
        menuItem.Text = ar[i];
        menuItem.Click += new EventHandler(menuItem_Click);
        contextMenu1.MenuItems.Add(menuItem);
      }

      contextMenu1.MenuItems.Add("-");

      menuItemRefresh = new MenuItem();
      menuItemRefresh.Index = 0;
      menuItemRefresh.Text = "R&efresh";
      menuItemRefresh.Click += new EventHandler(menuItemRefresh_Click);
      contextMenu1.MenuItems.AddRange(new MenuItem[] { menuItemRefresh });

      contextMenu1.MenuItems.Add("-");

      menuItemExit = new MenuItem();
      menuItemExit.Index = 0;
      menuItemExit.Text = "E&xit";
      menuItemExit.Click += new System.EventHandler(menuItemExit_Click);
      contextMenu1.MenuItems.AddRange(new MenuItem[] { menuItemExit });

      notifyIcon1.ContextMenu = contextMenu1;
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      SetupContextMenu();
      Minimize();
    }

    private void listBox1_Click(object sender, EventArgs e)
    {
      int index = ((ListBox)sender).SelectedIndex;
      PerformSwitch(index);
    }

    private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Left)
      {
        return;
      }

      if (WindowState == FormWindowState.Normal)
      {
        Minimize();
      }
      else
      {
        Maximize();
      }
    }

    void RunExe(string exeName, string args = "")
    {
      Process process = new Process();
      process.StartInfo.FileName = exeName;
      process.StartInfo.RedirectStandardOutput = true;
      process.StartInfo.RedirectStandardError = true;
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.CreateNoWindow = true;
      process.StartInfo.Arguments = args;
      process.Start();
    }

    private void menuItem_Click(object Sender, EventArgs e)
    {
      int index = ((MenuItem)Sender).Index;
      PerformSwitch(index);
    }

    private void menuItemExit_Click(object Sender, EventArgs e)
    {
      Close();
    }

    private void menuItemRefresh_Click(object Sender, EventArgs e)
    {
      try
      {
        RunExe(getDevicesExe);
        Close();
      }
      catch (Exception)
      {
        notifyIcon1.ShowBalloonTip(balloonTime, "Error Refreshing Device List", "Could not start " + getDevicesExe, ToolTipIcon.Error);
      }
    }

    void PerformSwitch(int index)
    {
      if(index<0)
      {
        //MessageBox.Show("Ivalid index (-1?) selected.");
        return;
      }

      string id = ar[(index * 2) + 1].TrimEnd();
      Console.WriteLine(id);

      try
      {
        RunExe(setDeviceExe, id);
        notifyIcon1.ShowBalloonTip(balloonTime, "Switched Audio Device", ar[index*2], ToolTipIcon.None);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        notifyIcon1.ShowBalloonTip(balloonTime, "Error Switching Audio Device", "Could not start " + setDeviceExe, ToolTipIcon.Error);
      }

    }

    private void Form1_LocationChanged(object sender, EventArgs e)
    {
      if (WindowState == FormWindowState.Minimized)
      {
        Minimize();
      }
    }

  }
}
