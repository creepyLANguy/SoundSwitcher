**SoundSwitcher**

![](https://i.imgur.com/EYEprd7.png)

___

 ### **[Download Latest Release Here](https://github.com/creepyLANguy/SoundSwitcher/releases)** ###  

Alternatively, compile all solutions and copy the resulting executables to a common folder before running *ALsSoundSwitcher.exe*  

___


**ALsSoundSwitcher.exe** is the front-end.  
You should be able to simply run this as-is.  
 - Left click on the tray icon to automatically switch to the next audio device.
 - Right Click on the tray icon to view a list of available audio devices and select a specific one to switch to. 

**SetPlaybackDevices.exe** is a helper program that will not perform any function if it is executed on its own.  
The front-end expects this to be located in the same folder so it can be used to change the sound device.  

**GetPlaybackDevices.exe** is a helper program as well as an additional bootstrap.  
It can detect the currently available audio devices before launching the front-end.  
Importantly, it creates the *devices.txt* file which the front-end uses to populate its device list.
The front-end also calls into this on startup if it cannot find *devices.txt*. 

___

You may also modify *devices.txt* by selecting the "Edit" option to only contain the audio devices you use.  
This is helpful when using the double-click feature to easily toggle between a small number of devices, eg. speakers and headphones.
Select "Restart" after editing to enable your file changes. 
___

Copyright (C) Altamish Mahomed - All Rights Reserved  
Unauthorized copying of this code or its contents, via any medium, is strictly prohibited, regardless of intent of use.  
Proprietary and confidential.

Written by Altamish Mahomed  gambit318@gmail.com, Jan 2019. 
