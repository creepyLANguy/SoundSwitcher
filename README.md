**SoundSwitcher**

![](https://i.imgur.com/DZk1ILg.png)

___ 

 ### **[Download Latest Release Here](https://github.com/creepyLANguy/SoundSwitcher/releases)** ###  

Alternatively, compile all solutions and copy the resulting executables to a common folder before running `ALsSoundSwitcher.exe`  

___

**Custom Icons (v1.8)** - *New!*

Apply custom icons for each sound device.  

Simply include `.ico` files with similar names to sound devices in the same folder as the executables (or in any subfolder).

Your icons do not need to be named the exact same as your sound device - the software will try its best to find a match.

eg. If the sound device is called *Speakers (Razer BlackShark V2 Pro)*, the icon called `Razer BlackShark.ico` will display when selecting this device. 

![](https://i.imgur.com/TSdmrhI.png)

___

#### What do each of the executables do? 

##### ALsSoundSwitcher.exe
This is the front-end. Simply run it as-is.
 - **Left** click on the tray icon to automatically switch to the next audio device.
 - **Middle** click to toggle the Windows volume mixer window.
 - **Right** click on the tray icon to view a list of available audio devices and select a specific one to switch to. 

##### SetPlaybackDevices.exe
A helper program that will not perform any function if it is executed on its own.  
The front-end expects this to be located in the same folder so it can be used to change the sound device.  

##### GetPlaybackDevices.exe
A helper program as well as an additional bootstrap.  
It can detect the currently available audio devices before launching the front-end.  
Importantly, it creates the *devices.txt* file which the front-end uses to populate its device list.
The front-end also calls into this on startup if it cannot find *devices.txt*. 

___

You may also modify `devices.txt` by selecting the **Edit** option to only contain the audio devices you use.  
This is helpful when using the left-click feature to easily toggle between a small number of devices, eg. speakers and headphones.  
Select **Restart** after editing to enable your file changes. 

___

*Copyright (C) Altamish Mahomed. Unauthorized copying of this code or its contents, via any medium, is strictly prohibited, regardless of intent of use. Proprietary and confidential.*
