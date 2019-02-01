**SoundSwitcher**

___

To use, extract the file found in the **Sample_Build** folder. 

Alternatively, compile all solutions as their respective executables and copy to a shared folder before running *ALsSoundSwitcher.exe*  

**ALsSoundSwitcher.exe** is the front-end. 

**SetPlaybackDevices.exe** will not perform any function if it is executed on its own.  
The front-end expects this to be located in the same folder so it can be used to change the sound device.  

**GetPlaybackDevices.exe** is in fact the bootstrap and can detect the currently available audio devices before launching the front-end.  
It creates a file *devices.txt* which the front-end uses to populate its device list.
The front-end also calls into this on startup if it cannot find devices.txt. 

___

Suggest using the context menu from your system tray by right-clicking the icon, as the traditional window is rather minimal in its functionality. 
___

Copyright (C) Altamish Mahomed - All Rights Reserved  
Unauthorized copying of this code or its contents, via any medium, is strictly prohibited, regardless of intent of use.  
Proprietary and confidential.

Written by Altamish Mahomed  gambit318@gmail.com, Jan 2019. 
