SoundSwitcher

Copyright (C) Altamish Mahomed - All Rights Reserved  
Unauthorized copying of this code or its contents, via any medium, is strictly prohibited, regardless of intent of use.  
Proprietary and confidential.

Written by Altamish Mahomed  gambit318@gmail.com, Jan 2019. 

___

To use, compile all solutions as respective executables, and copy to a shared folder before running.  

*ALsSoundSwitcher.exe* is the front-end. 

*SetPlaybackDevices.exe* will not perform any function if it is executed on its own.  
The front-end expects this to be located in the same folder so it can be used to change the sound device.  

*GetPlaybackDevices.exe* is in fact the bootstrap and will detect the currently available audio devises before launching the front-end.  
It creates a file "devices.txt" which the front-end uses to populate its device list.  
Ideally, this should be run the very first time, but if not, users are prompted to do so from the front-end if the devices.txt file is not found. 
Note, the "refresh" item in the front-end's tray context menu simply runs this executable and closes the front-end, 'refreshing' the entire program.  
