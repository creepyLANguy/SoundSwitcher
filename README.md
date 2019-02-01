SoundSwitcher

Copyright (C) Altamish Mahomed - All Rights Reserved  
Unauthorized copying of this code or its contents, via any medium, is strictly prohibited, regardless of intent of use.  
Proprietary and confidential.

Written by Altamish Mahomed  gambit318@gmail.com, Jan 2019. 

___

To use, compile all solutions as respective executables, and copy to a shared folder before running.  

ALsSoundSwitcher.exe is the front-end. 

SetPlaybackDevices.exe will not perform any function if it it executed on its own.  
The front-end expects this to be located in the same folder so it can be used to change the sound device.  

GetPlaybackDevices is in fact the bootstrap and will detect currenttly available audio devises before launching the front-end.  
It creates a file "devices.txt" which the front-end uses to populate a list of available audo devices.  
Ideally, this should be run the very first time, but if not, users are prompted to from the front-end if the devices.txt file is not found. 