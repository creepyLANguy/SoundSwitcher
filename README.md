**SoundSwitcher**

![](https://i.imgur.com/Rk5t4vq.png)

___

To use, extract the file found in the **Sample_Build** folder. 

Alternatively, compile all solutions as their respective executables and copy to a shared folder before running *ALsSoundSwitcher.exe*  

**ALsSoundSwitcher.exe** is the front-end.  
You should be able to simply run this.  
 - Left or Right Click on the tray icon to view a list of available audio devices and select one to switch to. 
 - Double-click the tray icon to automatically switch to the next audio device.

**SetPlaybackDevices.exe** will not perform any function if it is executed on its own.  
The front-end expects this to be located in the same folder so it can be used to change the sound device.  

**GetPlaybackDevices.exe** is in fact the bootstrap and can detect the currently available audio devices before launching the front-end.  
It creates a file *devices.txt* which the front-end uses to populate its device list.
The front-end also calls into this on startup if it cannot find *devices.txt*. 

___

You may also modify *devices.txt* to only contain the audio devices you use.  
This is helpful when using the double-click feature to easily toggle between a small number of devices, eg. speakers and headphones.
___

Copyright (C) Altamish Mahomed - All Rights Reserved  
Unauthorized copying of this code or its contents, via any medium, is strictly prohibited, regardless of intent of use.  
Proprietary and confidential.

Written by Altamish Mahomed  gambit318@gmail.com, Jan 2019. 
