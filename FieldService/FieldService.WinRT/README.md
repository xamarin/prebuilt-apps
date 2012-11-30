# Field Service - WinRT

We are using a Beta of the Bing Maps SDK, install here: http://visualstudiogallery.msdn.microsoft.com/bb764f67-6b2c-4e14-b2d3-17477ae1eaca?SRC=Featured

Instructions here: http://msdn.microsoft.com/en-us/library/hh855146.aspx

You will need a bing maps key here: http://msdn.microsoft.com/en-us/library/ff428642.aspx

One thing to note, is that we were no longer able to put "Any CPU" as our target.  I changed it to x86 for now, for WinRT devices, change it to ARM, and ARM will not work with Debug builds, must run it in Release.