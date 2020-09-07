using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using ALsSoundSwitcher.Properties;

namespace ALsSoundSwitcher
{
  public partial class Form1 : Form
  {
    string getDevicesExe = "GetPlaybackDevices.exe";
    string setDeviceExe = "SetPlaybackDevice.exe";
    int balloonTime = 1500;
    string[] ar;

    private int lastIndex = 0;

    private ContextMenu contextMenu1;
    private MenuItem menuItemExit;
    private MenuItem menuItemRefresh;

    private string activeMarker = " *";

    public Form1()
    {
      InitializeComponent();
    }

    private void Minimize()
    {
      WindowState = FormWindowState.Minimized;
      notifyIcon1.ShowBalloonTip(balloonTime);
      ShowInTaskbar = false;
      Visible = false;
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

    /*
    private void InvokeRightClick()
    {
      MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu",
        BindingFlags.Instance | BindingFlags.NonPublic);
      mi.Invoke(notifyIcon1, null);
    }
    */

    private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
    {
      if (((MouseEventArgs)e).Button == MouseButtons.Left)
      {
        //InvokeRightClick();
        Toggle();
        return;
      }

      /*
      if (WindowState == FormWindowState.Normal)
      {
        Minimize();
      }
      else
      {
        Maximize();
      }
      */
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
      if (index < 0)
      {
        MessageBox.Show("Invalid index (-1?) selected.");
        return;
      }

      Console.WriteLine(ar[index * 2]);

      string id = ar[(index * 2) + 1].TrimEnd();


      try
      {
        RunExe(setDeviceExe, id);

        SetIcon(ar[index * 2]);

        notifyIcon1.ShowBalloonTip(balloonTime, "Switched Audio Device", ar[index*2], ToolTipIcon.None);
        notifyIcon1.Text = ar[index * 2];

        foreach (MenuItem item in notifyIcon1.ContextMenu.MenuItems)
        {
          int i = item.Text.IndexOf(activeMarker);
          if (i >= 0)
          {
            item.Text = item.Text.Substring(0, i);
          }
        }
        notifyIcon1.ContextMenu.MenuItems[index].Text += activeMarker;
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

    private void Toggle()
    {
      ++lastIndex;
      if (lastIndex == ar.Length / 2)
      {
        lastIndex = 0;
      }
      PerformSwitch(lastIndex);
    }

    private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      //Toggle();
    }

    private void SetIcon(string iconName)
    {
      iconName = iconName.Trim();
      var icon = ResourceExtended.GetIconByRawName(iconName);
      if (icon == null)
      {
        icon = Properties.Resources.Icon;
      }

      notifyIcon1.Icon = icon;
    }
  }

  public class ResourceExtended
    {
      public static Icon GetIconByRawName(string iconName)
      {
        var obj = Resources.ResourceManager.GetObject(iconName, Resources.Culture);
        return (Icon)obj;
      }
    }
}
